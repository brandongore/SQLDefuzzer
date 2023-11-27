using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDefuzzer
{
    internal class GeneralOptions : DialogPage
    {
        [Category("General")]
        [DisplayName("Use SQLFluff Container?")]
        [Description("When enabled will call the SQLFluff container endpoint to format/lint")]
        public bool UseSQLFluffContainer { get; set; } = true;

        [Category("General")]
        [DisplayName("SQLFluff Container Endpoint url")]
        [Description("Color of the greeting text")]
        public string SQLFluffContainerEndpoint { get; set; } = "http://127.0.0.1:5000";
    }
}
