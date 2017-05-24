using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class JSONManagerLogic {

    public List<string> LstJsonCharacterData { get { return _lstJsonCharacterData; } set { _lstJsonCharacterData = value; } }
    public List<string> LstJsonCharacterOptionsData { get { return _lstJsonCharacterOptionsData; } set { _lstJsonCharacterOptionsData = value; } }
    public string JsonCharacterDataPath { get { return _jsonCharacterDataPath; } set { _jsonCharacterDataPath = value; } }
    public string JsonCharacterOptionDataPath { get { return _jsonCharacterOptionDataPath; } set { _jsonCharacterOptionDataPath = value; } }
    public JSONManagerBehaviour JsonManagerBehaviour { get { return _jsonManagerBehaviour; } set { _jsonManagerBehaviour = value; } }

    private List<string> _lstJsonCharacterData = new List<string>();
    private List<string> _lstJsonCharacterOptionsData = new List<string>();
    private Dictionary<string, GameObject> _dicCharacterByName;
    
    //Character files
    private int _jsonCharacterLoadCount = 0;
    private int _jsonCharacterCount = 9999;

    //Character Option files
    private int _jsonCharacterOptionsLoadCount = 0;
    private int _jsonCharacterOptionsCount = 9999;

    private string _jsonCharacterDataPath;
    private string _jsonCharacterOptionDataPath;

    private bool _loadedCharacters = false;
    private bool _loadedCharacterOptions = false;

    private JSONManagerBehaviour _jsonManagerBehaviour;

    public void StartLoadingCharacterTextFiles()
    {
        GameObject characterContainer = GlobalReferencesBehaviour.instance.SceneData.characterContainer;
        List<MovementBehaviour> characters = new List<MovementBehaviour>();
        characterContainer.GetComponentsInChildren<MovementBehaviour>(true, characters);

        //Store characters
        GlobalReferencesBehaviour.instance.SceneData.Characters = characters;

        //Get all characters
        _jsonCharacterCount = characters.Count;

        //If already loaded, dont do it again
        if (hasJsonCharactersLoaded())
        {
            LoadingFinished();
            return;
        }

        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, _jsonCharacterDataPath);
        foreach (MovementBehaviour character in characters)
        {
            //Debug.Log("loading characters...");
            _jsonManagerBehaviour.LoadCharacterTextCoroutine(filePath + "/" + character.gameObject.name + "Text.json");
            character.gameObject.SetActive(false);
        }
    }

    public void StartLoadingCharacterOptionFiles()
    {
        List<ChooseCharacterScript> chooseCharacterScripts = new List<ChooseCharacterScript>();

        GameObject characterContainer = GlobalReferencesBehaviour.instance.SceneData.characterContainer;

        //If already loaded, dont do it again
        if (hasJsonCharacterOptionsLoaded())
        {
            LoadingFinished();
            return;
        }

        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, _jsonCharacterOptionDataPath);
        //Get all character options
        characterContainer.GetComponentsInChildren<ChooseCharacterScript>(false, chooseCharacterScripts);

        //Count all character options
        _jsonCharacterOptionsCount = chooseCharacterScripts.Count;

        foreach (ChooseCharacterScript characterOption in chooseCharacterScripts)
        {
            Debug.Log("loading character options...");
            _jsonManagerBehaviour.LoadCharacterOptionsTextCoroutine(filePath + "/" + characterOption.gameObject.name.ToLower() + "OptionsText.json");
            //characterOption.gameObject.SetActive(false);
        }
    }

    public bool hasJsonCharactersLoaded()
    {
        return _jsonCharacterLoadCount >= _jsonCharacterCount;
    }

    public bool hasJsonCharacterOptionsLoaded()
    {
        return _jsonCharacterOptionsLoadCount >= _jsonCharacterOptionsCount;
    }

    public GameObject getCharacterByName(string charName)
    {
        GameObject theChar;
        _dicCharacterByName.TryGetValue(charName, out theChar);
        return theChar;
    }

    public IEnumerator LoadCharacterWWW(string filePath)
    {
        //Debug.Log("filePath is: " + filePath);
        if (filePath.Contains("://"))
        {
            WWW www = new WWW(filePath);
            yield return www;
            //Debug.Log(www.text);
            //Debug.Log("On a remote machine, loading the file via WWW");

            _lstJsonCharacterData.Add(www.text);
            ++_jsonCharacterLoadCount;
        }
        else {
            //Debug.Log("On a local machine, loading the file via System.IO");
            _lstJsonCharacterData.Add(System.IO.File.ReadAllText(filePath));
            ++_jsonCharacterLoadCount;
        }
    }

    public IEnumerator LoadCharacterOptionsWWW(string filePath)
    {
        //Debug.Log("filePath is: " + filePath);
        if (filePath.Contains("://"))
        {
            WWW www = new WWW(filePath);

            yield return www;

            _lstJsonCharacterOptionsData.Add(www.text);
            ++_jsonCharacterOptionsLoadCount;
        }
        else {
            //Debug.Log("On a local machine, loading the file via System.IO");

            if (File.Exists(filePath))
            {
                _lstJsonCharacterOptionsData.Add(System.IO.File.ReadAllText(filePath));
                ++_jsonCharacterOptionsLoadCount;
            }
        }

        if (hasJsonCharacterOptionsLoaded())
            LoadingFinished();
    }

    private void LoadingFinished()
    {
        if (_jsonManagerBehaviour.onLoaded != null)
            _jsonManagerBehaviour.onLoaded.Invoke();
    }

}
