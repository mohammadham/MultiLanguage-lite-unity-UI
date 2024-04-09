using System.Collections.Generic;

namespace Resources.multiLanguage
{
    public class Language
    {
        public string Name { set; get; }

        public Dictionary<string, string> Words{
            set;
            get;
        }
    }
}