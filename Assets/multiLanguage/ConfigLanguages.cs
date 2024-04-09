using System.Collections.Generic;

namespace Resources.multiLanguage
{
    public class ConfigLanguages
    {
        public string ActiveLanguageName { set; get; }

        public Dictionary<string, string> LanguageAddress{
            set;
            get;
        }
    }
}