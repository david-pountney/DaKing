using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JSONManagerLogic {

    public List<string> LstJsonData { get { return _lstJsonData; } set { _lstJsonData = value; } }
    public Dictionary<string, GameObject> DicCharacterByName { get { return _dicCharacterByName; } set { _dicCharacterByName = value; } }
    public string JsonDataPath { get { return _jsonDataPath; } set { _jsonDataPath = value; } }
    public JSONManagerBehaviour ResourceManager { get { return _resourceManager; } set { _resourceManager = value; } }

    private List<string> _lstJsonData = new List<string>();
    private Dictionary<string, GameObject> _dicCharacterByName;
    
    private int _jsonLoadCount = 0;
    private int _jsonCount = 9999;

    private string _jsonDataPath;

    private JSONManagerBehaviour _resourceManager;

    public void StartLoadingCharacterTextFiles()
    {
        GameObject characterContainer = GameObject.Find("Characters");
        List<MovementBehaviour> characters = new List<MovementBehaviour>();
        characterContainer.GetComponentsInChildren<MovementBehaviour>(true, characters);

        //Store characters
        GlobalReferencesBehaviour.instance.SceneData.Characters = characters;

        //Get all characters
        _jsonCount = characters.Count;

        //If already loaded, dont do it again
        if (hasJsonLoaded())
        {
            LoadingFinished();
            return;
        }

        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, _jsonDataPath);
        foreach (MovementBehaviour character in characters)
        {
            //Debug.Log("loading characters...");
            _resourceManager.CreateCoroutine(filePath + "/" + character.gameObject.name + "Text.json");
            character.gameObject.SetActive(false);
        }

    }

    public bool hasJsonLoaded()
    {
        return _jsonLoadCount >= _jsonCount;
    }

    public GameObject getCharacterByName(string charName)
    {
        GameObject theChar;
        _dicCharacterByName.TryGetValue(charName, out theChar);
        return theChar;
    }

    public IEnumerator LoadWWW(string filePath)
    {
        //Debug.Log("filePath is: " + filePath);
        if (filePath.Contains("://"))
        {
            WWW www = new WWW(filePath);
            yield return www;
            //Debug.Log(www.text);
            //Debug.Log("On a remote machine, loading the file via WWW");

            _lstJsonData.Add(www.text);
            ++_jsonLoadCount;
        }
        else {
            //Debug.Log("On a local machine, loading the file via System.IO");

            _lstJsonData.Add(System.IO.File.ReadAllText(filePath));
            ++_jsonLoadCount;
        }

        if (hasJsonLoaded())
            LoadingFinished();
    }

    private void LoadingFinished()
    {
        if (_resourceManager.onLoaded != null)
            _resourceManager.onLoaded.Invoke();
    }

}
