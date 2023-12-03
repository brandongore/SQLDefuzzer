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

                if (activeWindow == null)
                {
                    //return for now, can display a dialog in future
                    return;
                }

                //get text from window
                TextDocument textDoc = (TextDocument)activeWindow.Document.Object("TextDocument");
                EditPoint editPoint = textDoc.StartPoint.CreateEditPoint();
                string sqlQuery = editPoint.GetText(textDoc.EndPoint);

                // setup parameters for calling Flask app
                string flaskAppUrl = "http://localhost:5000/execute";

                var options = (SQLFLUFFOptions)package.GetDialogPage(typeof(SQLFLUFFOptions));
                var config = options.BuildConfiguration();

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
                    using (HttpResponseMessage response = await client.PostAsync(flaskAppUrl, content))
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        response.EnsureSuccessStatusCode(); // Ensure the HTTP response indicates success

                        // read the API response
                        string apiResponse = await reader.ReadToEndAsync();

                        // Deserialize the JSON response
                        var jsonResponse = JsonConvert.DeserializeObject<SqlDefuzzerFlaskResponse>(apiResponse);

                        // Extract the formatted SQL code from the response
                        string formattedSqlQuery = jsonResponse.result;

                        // update text within query
                        editPoint.ReplaceText(textDoc.EndPoint, formattedSqlQuery, (int)vsEPReplaceTextOptions.vsEPReplaceTextAutoformat);
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Handle HTTP request-related exceptions
                    Debug.WriteLine($"HTTP request failed: {ex.Message}");
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    Debug.WriteLine($"An error occurred: {ex.Message}");
                }
            });
        }
    }
}
