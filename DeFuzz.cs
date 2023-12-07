using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Newtonsoft.Json;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace SQLDefuzzer
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class DeFuzz
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("5c1ee3a0-8397-4e49-a267-12787833bbfd");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeFuzz"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private DeFuzz(Package package)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static DeFuzz Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            // Switch to the main thread - the call to AddCommand in DeFuzz's constructor requires
            // the UI thread.
            //await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
            ThreadHelper.ThrowIfNotOnUIThread();

            Instance = new DeFuzz(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            ThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                EnvDTE.DTE dte = this.ServiceProvider.GetService(typeof(Microsoft.VisualStudio.Shell.Interop.SDTE)) as EnvDTE.DTE;

                //get currently active window
                Window activeWindow = ((DTE2)dte).ActiveWindow;

                if (activeWindow == null || activeWindow.Document == null)
                {
                    MessageBox.Show("No active window. Please open a query window.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //get text from window
                TextDocument textDoc = (TextDocument)activeWindow.Document.Object("TextDocument");
                if (textDoc == null)
                {
                    MessageBox.Show("No text document. Please open a query window.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                EditPoint editPoint = textDoc.StartPoint.CreateEditPoint();
                string sqlQuery = editPoint.GetText(textDoc.EndPoint);

                string formattedSqlQuery = String.Empty;

                var generalOptions = (GeneralOptions)package.GetDialogPage(typeof(GeneralOptions));
               
                var options = (SQLFLUFFOptions)package.GetDialogPage(typeof(SQLFLUFFOptions));

                var config = options.BuildConfiguration();
                if (generalOptions.UseSQLFluffContainer)
                {
                    // setup parameters for calling Flask api
                    string sqlFluffFixEndpoint = $"{generalOptions.SQLFluffContainerEndpoint}/fix";

                    // create JSON payload
                    var payload = new
                    {
                        code = sqlQuery,
                        configuration = config
                    };

                    string jsonPayload = JsonConvert.SerializeObject(payload);

                    try
                    {
                        using (HttpClient client = new HttpClient())
                        using (HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json"))
                        using (HttpResponseMessage response = await client.PostAsync(sqlFluffFixEndpoint, content))
                        using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            response.EnsureSuccessStatusCode(); // Ensure the HTTP response indicates success

                            // read the API response
                            string apiResponse = await reader.ReadToEndAsync();

                            // Deserialize the JSON response
                            var jsonResponse = JsonConvert.DeserializeObject<SqlDefuzzerFlaskResponse>(apiResponse);

                            // Extract the formatted SQL code from the response
                            formattedSqlQuery = jsonResponse.result;

                            // update text within query
                            editPoint.ReplaceText(textDoc.EndPoint, formattedSqlQuery, (int)vsEPReplaceTextOptions.vsEPReplaceTextAutoformat);
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        // Handle HTTP request-related exceptions
                        MessageBox.Show("Container Request Error: " + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        // Handle other exceptions
                        MessageBox.Show("Extension Error: " + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    //setup parameters for calling sqlfluff
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "python";

                    var sqlFluffConfig = options.ConvertToCfgFormat(config);

                    var tempPath = Path.Combine(Path.GetTempPath(), ".sqlfluff");
                    System.IO.File.WriteAllText(tempPath, sqlFluffConfig);

                    startInfo.Arguments = $"-m sqlfluff fix - --dialect={SQLFLUFFOptions.MapEnumOption(options.Dialect)} --config={tempPath}";
                    startInfo.RedirectStandardInput = true;
                    startInfo.RedirectStandardOutput = true;
                    startInfo.RedirectStandardError = true;
                    startInfo.UseShellExecute = false;

                    //start new process to format sql query
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo = startInfo;
                    process.Start();

                    //send query to process
                    process.StandardInput.WriteLine(sqlQuery);
                    process.StandardInput.Close();

                    //read output from process
                    formattedSqlQuery = process.StandardOutput.ReadToEnd();
                    var sqlFluffError = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(sqlFluffError))
                    {
                        // Handle the error
                        MessageBox.Show("Error: " + sqlFluffError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        editPoint.ReplaceText(textDoc.EndPoint, formattedSqlQuery, (int)vsEPReplaceTextOptions.vsEPReplaceTextAutoformat);
                    }
                }
            });
        }
    }
}
