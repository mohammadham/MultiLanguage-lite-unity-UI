using System;
using UnityEngine;
using UnityEngine.UI;

namespace Resources.multiLanguage
{
    public class LanguageTextUi :MonoBehaviour
    {
        [HideInInspector] public Text Handler;
        [HideInInspector] public string TextView;
        private void OnEnable()
        {
            if (Handler == null)
            {
                gameObject.TryGetComponent(out Handler);
            }

            if (string.IsNullOrEmpty(TextView) && Handler)
            {
                TextView = Handler.text;
            }
        }

        public void ChangeText(string text)
        {
            this.Handler.text = text;
            this.TextView = text;
            
        }
    }
}