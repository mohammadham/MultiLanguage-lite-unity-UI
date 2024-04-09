using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

namespace Resources.multiLanguage
{
    public class LanguageManager : MonoBehaviour
    {
        // Singleton instance
        private static LanguageManager instance;

        // Get the singleton instance
        public static LanguageManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<LanguageManager>();
                }

                return instance;
            }
        }
        //active laanguage
        private string ActiveLanguage ;
        //language file name => address file
        private Dictionary<string, string> _languageAddress;
        private List<Language> _languages = new List<Language>();
        private Language _defaultLanguage = new Language();
        private Language _ActiveLanguage = new Language();
        private List<string> _baseSubNameLanguages = new List<string>( ){ "en", "pr", "ru","Ar" };
        
        public delegate void ActiveLanguageEventHandler(string __activeLanguage);
        public event ActiveLanguageEventHandler ActiveLanguageChanged;

    private void Awake()
        {
            
                if (Instance != this)
                {
                    Destroy(gameObject);
                }
                //load main file of languages config
            string writer = Application.dataPath + "/lan/language_config.json";
            if (!(File.Exists(writer.ToString()) && Directory.Exists(Application.dataPath + "/log")))
            {
                if (!Directory.Exists(Application.dataPath + "/lan"))
                {
                    Directory.CreateDirectory(Application.dataPath + "/lan");
                }
                if(!File.Exists(Application.dataPath + "/lan/language_config.json"))
                {
                    File.Create(Application.dataPath + "/lan/language_config.json");
                }

                LoadConfigFileData();
                LoadFileData();
                if (!(this._languages.Count > 0))
                {
                    CreateNewLanguageSection("english");
                }
            }
            
        }

    ///<summary>
    /// return active language
    /// </summary>
    public Language GetActiveLanguage()
    {
        
        if (!string.IsNullOrEmpty(this.ActiveLanguage))
        {
            return this.LoadFileData(this.ActiveLanguage);
        }

        return null;
    }
    ///<summary>
    /// return word from active language or set to new word
    /// </summary>
    public string GetWordFromActiveLanguage(string word)
    {
        
        if (!string.IsNullOrEmpty(this.ActiveLanguage) && !string.IsNullOrEmpty(word))
        {
            string rWord = CheckWordIsInALanguage(this.ActiveLanguage,word)?(this._ActiveLanguage.Words.ContainsKey(word)? this._ActiveLanguage.Words[word]:word):"" ;

            if (string.IsNullOrEmpty(rWord))
            {
                rWord = CheckWordIsInALanguage(this._defaultLanguage.Name,word)?(this._defaultLanguage.Words.ContainsKey(word)? this._defaultLanguage.Words[word]:word):"" ;
            }
            if (string.IsNullOrEmpty(rWord))
            {
                
                UpdateWordsInALanguage(word,this.ActiveLanguage,this.SpellingAutoCreate(word));
                return word;
            }
            return rWord;
        }

        return null;
    }
    ///<summary>
    /// return word from active language or set to new word
    /// </summary>
    public string GetWordFromActiveLanguage(string word,string spel)
    {
        
        if (!string.IsNullOrEmpty(this.ActiveLanguage) && !string.IsNullOrEmpty(word) && !string.IsNullOrEmpty(spel))
        {
            string rWord = CheckWordIsInALanguage(this.ActiveLanguage,spel)?(this._ActiveLanguage.Words.ContainsKey(spel)? this._ActiveLanguage.Words[word]:word):"" ;

            if (string.IsNullOrEmpty(rWord))
            {
                rWord = CheckWordIsInALanguage(this._defaultLanguage.Name,spel)?(this._defaultLanguage.Words.ContainsKey(spel)? this._defaultLanguage.Words[word]:word):"" ;
            }
            if (string.IsNullOrEmpty(rWord))
            {
                
                UpdateWordsInALanguage(word,this.ActiveLanguage,this.SpellingAutoCreate(spel));
                return word;
            }
            return rWord;
        }

        return null;
    }
/// <summary>
/// auto spelling creator
/// </summary>
/// <param name="word"></param>
/// <returns></returns>
    public string SpellingAutoCreate(string word)
    {
         return (string.Join(" ",
            word.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()))).Replace(" ","_");
    }
    ///<summary>
    /// set active language
    /// </summary>
    public bool SetActiveLanguage(string languageName)
    {
        
        if (!string.IsNullOrEmpty(languageName))
        {
            if (this.ActiveLanguage != languageName)
            {
                if (this.LoadFileData(this.ActiveLanguage) != null)
                {
                    if (ActiveLanguageChanged != null)
                    {
                        

                        ActiveLanguageChanged(this.ActiveLanguage);
                    }
                    return true;
                }
            }
            else
            {
                //update too last state of active language
                //if (this.LoadFileData(this.ActiveLanguage) != null)
                //{
                    return true;
                //}
            }

            return false;
        }

        return false;
    }
/// <summary>
/// create a new language file
/// </summary>
/// <param name="languageName"></param>
/// <returns></returns>
    public bool CreateNewLanguageSection(string languageName)
    {
        try
        {
            if (!CheckLanguageIsInALanguages(languageName))
            {
                if (!Directory.Exists(Application.dataPath + "/lan"))
                {
                    Directory.CreateDirectory(Application.dataPath + "/lan");
                }

                if (!File.Exists(Application.dataPath + "/lan/" + languageName + ".json"))
                {
                    File.Create(Application.dataPath + "/lan/" + languageName + ".json");
                    this._languageAddress.Add(languageName, Application.dataPath + "/lan/" + languageName + ".json");
                    if (languageName == "english")
                    {
                        this._defaultLanguage = new Language();
                    }

                    return true;
                }
            }

            return false;
                    

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return false;
    }
/// <summary>
/// Delete a old language file
/// </summary>
/// <param name="languageName"></param>
/// <returns></returns>
public bool DeleteOldLanguageSection(string languageName)
{
    try
    {
        if (CheckLanguageIsInALanguages(languageName))
        {

            if (File.Exists(this._languageAddress[languageName]))
            {
                File.Delete(this._languageAddress[languageName]);
                this._languageAddress.Remove(languageName);
                if (languageName == "english")
                {
                    CreateNewLanguageSection("english");
                }

                return true;
            }
        }

        return false;
                    

    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
    return false;
}
/// <summary>
/// add a new word or update/edit to a language and if is not set in main Language add to it
/// </summary>
/// <param name="word"> show word</param>
/// <param name="language"> language to save to it</param>
/// <param name="spel"> pointer to word</param>
/// <returns> save or not </returns>
        public bool UpdateWordsInALanguage(string word , string language , string spel = "")
        {
            try
            {
                if (CheckLanguageIsInALanguages(language))
                {
                    if (ActiveLanguage != this._defaultLanguage.Name)
                    {
                        if (!string.IsNullOrEmpty(spel))
                        {
                            //update default language
                            UpdateLanguageFile("english", word, spel);
                        }
                        else
                        {
                            //update default language
                            UpdateLanguageFile("english", word, "");
                        }
                    }

                    return UpdateLanguageFile(language, word, spel);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return false;
        }
/// <summary>
/// add a new word or update/edit to a language and if is not set in main Language at to it
/// </summary>
/// <param name="word"> show word</param>
/// <param name="language"> language to save to it</param>
/// <param name="spel"> pointer to word</param>
/// <returns> save or not </returns>
public bool RemoveWordInWordsInALanguage(string word , string language , string spel = "")
{
    try
    {
        if (CheckLanguageIsInALanguages(language))
        {
            if (!string.IsNullOrEmpty(spel))
            {
                //update default language
                UpdateLanguageFile("english", "", spel);
                return UpdateLanguageFile(language, "", spel);
            }
            else
            {
                //update default language
                UpdateLanguageFile("english", "",word);
                return UpdateLanguageFile(language, "", word);
            }
            

        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }

    return false;
}

///<summary>
/// update name of a language
/// </summary>
///<param name="NewName">new name of language</param>
///<param name="OldName">old name of language</param>

private bool UpdateLanguageFile(string NewName,string OldName)
{
    try
    {
        if (!string.IsNullOrEmpty(NewName) && !string.IsNullOrEmpty(OldName))
        {
            Language Temp = GetLanguageIsInALanguages(OldName);
            if (Temp != null)
            {
                Temp.Name = NewName;
                // save update file json
                if (UpdateJsonFile(Application.dataPath + "/lan/" + OldName + ".json", Temp))
                {
                    return true;
                }
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
    return false;
}
///<summary>
/// update dictionary words of a language
/// </summary>
///<param name="languageName">name language</param>
///<param name="words">dictionary of words </param>

private bool UpdateLanguageFile(string languageName,Dictionary<string, string> words )
{
    try
    {
        if (words.Count>0)
        {
            Language Temp = GetLanguageIsInALanguages(languageName);
            if (Temp != null)
            {
                Temp.Words = words;
                // save update file json
                if (UpdateJsonFile(Application.dataPath + "/lan/" + languageName + ".json", Temp))
                {
                    return true;
                }
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
    return false;
}
///<summary>
/// update word of a language with word and spelling and remove a word when spelling is set and word is empty
/// </summary>
///<param name="languageName">language Name</param>
///<param name="word">word</param>
/// <param name="spel">spelling set it with "" for not chack</param>

private bool UpdateLanguageFile(string languageName,string word , string spel  )
{
    try
    {
        if (CheckLanguageIsInALanguages(languageName))
        {
            
            Language Temp = GetLanguageIsInALanguages(languageName);
            if (Temp != null)
            {
                if (!string.IsNullOrEmpty(spel)  )
                {
                    if (!CheckWordIsInALanguage(languageName, spel))
                    {
                        //isRemoved or create
                        if (string.IsNullOrEmpty(word))
                        {
                            return true; 
                        }
                        else
                        {
                            Temp.Words.Add(spel,word);
                        }
                    }
                    else
                    {
                        // remove or update
                        if (string.IsNullOrEmpty(word))
                        {
                            Temp.Words.Remove(spel);
                        }
                        else
                        {
                            Temp.Words[spel] = word;
                        }
                    }
                }
                else
                {
                    if (!CheckWordIsInALanguage(languageName, word))
                    {
                        Temp.Words.Add(word,word);
                    }
                    else
                    {
                        Temp.Words[word] = word;
                    }                    
                }
                // save update file json
                if (UpdateJsonFile(Application.dataPath + "/lan/" + languageName + ".json", Temp))
                {
                    return true;
                }
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
    return false;
}
/// <summary>
/// save a object to a json file
/// </summary>
/// <param name="JsonFileAddress">address</param>
/// <param name="Value">object</param>
/// <typeparam name="T">object type</typeparam>
/// <returns></returns>
public bool UpdateJsonFile<T>(string JsonFileAddress, T Value) 
{
    try
    {
        if (!File.Exists(JsonFileAddress))
        {
            File.Create(JsonFileAddress);
        }
        
        {
            string json = JsonConvert.SerializeObject(Value);
            json = json.EnCoderBaseUTF8();
            File.WriteAllText(JsonFileAddress,json);
            return true;
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
    return false;
}
/// <summary>
/// check if that language have this word or spel
/// </summary>
/// <param name="languageName"> Language Name</param>
/// <param name="word"> word </param>
/// <returns></returns>
public bool CheckWordIsInALanguage(string languageName , string word)
{
    if (_baseSubNameLanguages.Contains(languageName)  )
    {
        if (languageName == "en")
        {
            languageName = "english";
        }

        if (languageName == "pr")
        {
            languageName = "persian";
        }
        if (languageName == "ru")
        {
            languageName = "russian";
        }
        if (languageName == "Ar")
        {
            languageName = "arabic";
        }
        
    }

    if (_languages.Count > 0)
    {
        Language temp = _languages.Find(languageT => languageT.Name == languageName);
        if (temp != null)
        {
            if (temp.Words.ContainsKey(word) || temp.Words.ContainsValue(word))
            {
                return true;
            }
        }
    }

    return false;
}
/// <summary>
/// check if that language is in languages list
/// </summary>
/// <param name="languageName"> Language Name </param>
/// <returns></returns>
public bool CheckLanguageIsInALanguages(string languageName )
{
    if (_baseSubNameLanguages.Contains(languageName)  )
    {
        if (languageName == "en")
        {
            languageName = "english";
        }

        if (languageName == "pr")
        {
            languageName = "persian";
        }
        if (languageName == "ru")
        {
            languageName = "russian";
        }
        if (languageName == "Ar")
        {
            languageName = "arabic";
        }
        
    }

    if (_languages.Count > 0)
    {
        Language temp = _languages.Find(languageT => languageT.Name == languageName);
        if (temp != null)
        {
            
                return true;
            
        }
    }

    return false;
}
/// <summary>
/// get a language if that languageName is in languages list
/// </summary>
/// <param name="LanguageName"> Language Name</param>
/// <returns></returns>
public Language GetLanguageIsInALanguages(string languageName )
{
    if (_baseSubNameLanguages.Contains(languageName)  )
    {
        if (languageName == "en")
        {
            languageName = "english";
        }

        if (languageName == "pr")
        {
            languageName = "persian";
        }
        if (languageName == "ru")
        {
            languageName = "russian";
        }
        if (languageName == "Ar")
        {
            languageName = "arabic";
        }
        
    }

    if (_languages.Count > 0)
    {
        if (languageName == this.ActiveLanguage)
            return this._ActiveLanguage;
        if (languageName == this._defaultLanguage.Name)
            return this._defaultLanguage;
        Language temp = _languages.Find(languageT => languageT.Name == languageName);
        if (temp != null)
        {
            
            return temp;
            
        }
    }

    return null;
}
/// <summary>
/// load address of language files
/// </summary>
        public void LoadConfigFileData()
        {
            if (!(this._languageAddress.Count > 0))
            {
                string jsonPath = Path.Combine(Application.dataPath, "/lan/language_config.json");
                if (File.Exists(jsonPath))
                {
                    string json = File.ReadAllText(jsonPath);
                    json = json.DeCoderBaseUTF8();
                    this._languageAddress = JsonConvert.DeserializeObject<ConfigLanguages>(json).LanguageAddress;
                    this.ActiveLanguage = JsonConvert.DeserializeObject<ConfigLanguages>(json).ActiveLanguageName;
                }
                else
                {
                    this._languageAddress = new System.Collections.Generic.Dictionary<string, string>();
                    this.ActiveLanguage = "en";
                }
            }
        }
/// <summary>
/// load data of language file
/// </summary>
/// <param name="fileName">file Name</param>
/// <returns></returns>
        public Language LoadFileData(string fileName="")
        {
            if (_baseSubNameLanguages.Contains(fileName)  )
            {
                if (fileName == "en")
                {
                    fileName = "english.json";
                }

                if (fileName == "pr")
                {
                    fileName = "persian.json";
                }
                if (fileName == "ru")
                {
                    fileName = "russian.json";
                }
                if (fileName == "Ar")
                {
                    fileName = "arabic.json";
                }
        
            }
            if (fileName == "")
            {
                fileName = "english.json";
            }
            else
            {
                fileName = this._languageAddress[fileName];
            }
            
            if (!(this._languageAddress.Count > 0))
            {
                string jsonPath = Path.Combine(Application.dataPath, "/lan/"+fileName);
                if (File.Exists(jsonPath))
                {
                    string json = File.ReadAllText(jsonPath);
                    json = json.DeCoderBaseUTF8();
                    this._languages.Add(JsonConvert.DeserializeObject<Language>(json));
                    if (fileName == "english.json")
                    {
                        this._defaultLanguage = JsonConvert.DeserializeObject<Language>(json); 
                        
                    }

                    if (fileName == this.ActiveLanguage)
                    {
                        this._ActiveLanguage = JsonConvert.DeserializeObject<Language>(json);
                        this.ActiveLanguage = this._ActiveLanguage.Name;
                    }
                    return JsonConvert.DeserializeObject<Language>(json);
                }
                else
                {
                    return null;
                }
            }

            return null;
        }
    }
}