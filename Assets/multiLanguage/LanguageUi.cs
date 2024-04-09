using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Resources.multiLanguage
{
    // [RequireComponent(Text,TextMeshPro)]
    public class LanguageUi :MonoBehaviour
    {
        [SerializeField] private Text _textUi;
        [SerializeField] private TextMeshPro _textMeshProUi;

        [SerializeField] [Header("this parameter use for save in data base(can be null)")]

        public string Spelling;

        [SerializeField] [Header("this parameter use for show in view")]

        public string Word;

        private LanguageTextUi _handlerLanguageTextUi;
        private LanguageTextMeshProUi _handlerLanguageTextMeshProUi;

        private Theme.Language _handlerActiveLanguage;
        private Theme.LanguageManager _handlerLanguageManager;
        private void Awake()
        {
            if (Theme.LanguageManager.Instance)
            {
                this._handlerActiveLanguage = Theme.LanguageManager.Instance.GetActiveLanguage();
                if (this._handlerActiveLanguage != null)
                {
                    //check have weach one of type of text is set
                    if (!(gameObject.TryGetComponent(out this._textUi) ||
                          gameObject.TryGetComponent(out this._textMeshProUi)))
                    {
                        this._textUi = gameObject.AddComponent<Text>();
                        this._handlerLanguageTextUi = gameObject.AddComponent<LanguageTextUi>();
                    }
                    else
                    {
                        if (this._textUi)
                        {
                            this._handlerLanguageTextUi = gameObject.AddComponent<LanguageTextUi>();
                        }
                        else if (this._textMeshProUi)
                        {
                            this._handlerLanguageTextMeshProUi = gameObject.AddComponent<LanguageTextMeshProUi>();
                        }
                    }

                    if (this._textUi || this._textMeshProUi)
                    {
                        ActiveLanguageChanged("");
                    }
                }
            }

        }

        private void OnEnable()
        {
            if (Theme.LanguageManager.Instance)
            {
                _handlerLanguageManager = Theme.LanguageManager.Instance;
                //active language changed
                _handlerLanguageManager.ActiveLanguageChanged += ActiveLanguageChanged;
            }
        }

        private void OnDisable()
        {
            if (Theme.LanguageManager.Instance)
            {
                _handlerLanguageManager = Theme.LanguageManager.Instance;
                //active language changed
                _handlerLanguageManager.ActiveLanguageChanged -= ActiveLanguageChanged;
            }
        }

        private void ActiveLanguageChanged(string act)
        {
            if (Theme.LanguageManager.Instance)
            {
                _handlerLanguageManager = Theme.LanguageManager.Instance;
            }
            if (this._textUi || this._textMeshProUi)
            {
                if (!string.IsNullOrEmpty(Spelling) || !string.IsNullOrEmpty(Word))
                {
                    //get data from language ui manager
                    if (!string.IsNullOrEmpty(Spelling)  && !string.IsNullOrEmpty(Word))
                    {
                        if (this._handlerLanguageTextUi && _handlerLanguageManager)
                        {
                            this._handlerLanguageTextUi.ChangeText(_handlerLanguageManager.GetWordFromActiveLanguage(this.Word,this.Spelling));
                        }
                        if (this._handlerLanguageTextMeshProUi && _handlerLanguageManager)
                        {
                            this._handlerLanguageTextMeshProUi.ChangeText(_handlerLanguageManager.GetWordFromActiveLanguage(this.Word,this.Spelling));
                        }
                    }else if (!string.IsNullOrEmpty(Spelling)  && string.IsNullOrEmpty(Word))
                    {
                        if (this._handlerLanguageTextUi && _handlerLanguageManager)
                        {
                            this._handlerLanguageTextUi.ChangeText(_handlerLanguageManager.GetWordFromActiveLanguage(this.Spelling));
                        }  
                        if (this._handlerLanguageTextMeshProUi && _handlerLanguageManager)
                        {
                            this._handlerLanguageTextMeshProUi.ChangeText(_handlerLanguageManager.GetWordFromActiveLanguage(this.Word,this.Spelling));
                        }
                    }else if (string.IsNullOrEmpty(Spelling)  && !string.IsNullOrEmpty(Word))
                    {
                        if (this._handlerLanguageTextUi && _handlerLanguageManager)
                        {
                            this._handlerLanguageTextUi.ChangeText(_handlerLanguageManager.GetWordFromActiveLanguage(this.Word));
                        }  
                        if (this._handlerLanguageTextMeshProUi && _handlerLanguageManager)
                        {
                            this._handlerLanguageTextMeshProUi.ChangeText(_handlerLanguageManager.GetWordFromActiveLanguage(this.Word,this.Spelling));
                        }
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (Theme.LanguageManager.Instance)
            {
                _handlerLanguageManager = Theme.LanguageManager.Instance;
                //active language changed
                _handlerLanguageManager.ActiveLanguageChanged -= ActiveLanguageChanged;
            }
            if (this._handlerLanguageTextUi)
            {
                Destroy(this._handlerLanguageTextUi);
            }
            if (this._handlerLanguageTextMeshProUi)
            {
                Destroy(this._handlerLanguageTextMeshProUi);
            }
        }
    }
}