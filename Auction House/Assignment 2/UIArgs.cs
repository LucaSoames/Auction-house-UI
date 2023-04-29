using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    internal class UIArgs
    {
        //options dictionary used for running different menu interfaces in the program
        private IDictionary<string, string> stringArgs = new Dictionary<string, string>();

        public IDictionary<string, string> StringArgs { get; set; }

        private string[] options;

        public string[] Options;

        public UIArgs(IDictionary<string, string> strings, string[] options)
        {
            this.StringArgs = strings;
            this.Options = options;
        }
    }
}
