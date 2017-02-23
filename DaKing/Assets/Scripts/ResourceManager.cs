using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    public string jsonDataPath;

    GameObject mainCamera;
    PlayerAttributes playerAttributes;
    GameMaster gameMaster;
    GameObject treasureChest;
    MenuController menuController;

    int jsonLoadCount = 0;
    int jsonCount = 9999;

    public List<string> lstJsonData = new List<string>();

    Dictionary<string, GameObject> dicCharacterByName;

    public static ResourceManager instance;

    void Awake()
    {
        Debug.Log("Resource manager awake");

        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);
        }
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance.
            DestroyImmediate(gameObject);

        instance.dicCharacterByName = new Dictionary<string, GameObject>();

        StartLoadingCharacterTextFiles();
    }

    void OnLevelWasLoaded()
    {
        StartLoadingCharacterTextFiles();
    }

    private void StartLoadingCharacterTextFiles()
    {
        //Get all characters
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Character");
        this.jsonCount = characters.Length;

        //If already loaded, dont do it again
        if (hasJsonLoaded())
        {
            LoadingFinished();
            return;
        }
        
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, jsonDataPath);
        foreach (GameObject character in characters)
        {
            //Debug.Log("loading characters...");
            StartCoroutine(LoadWWW(filePath + "/" + character.name + "Text.json"));
        }

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        gameMaster = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameMaster>();
        treasureChest = GameObject.Find("TreasureChest");
        playerAttributes = GameObject.Find("king").GetComponent<PlayerAttributes>();
        menuController = GameObject.Find("MenuController").GetComponent<MenuController>();
    }

    public bool hasJsonLoaded()
    {
        return this.jsonLoadCount >= this.jsonCount;
    }


    public GameObject getTreasureChest()
    {
        return treasureChest;
    }

    public GameMaster getGameMaster()
    {
        return gameMaster;
    }

    public PlayerAttributes getPlayerAttributes()
    {
        if(!playerAttributes)
            playerAttributes = GameObject.FindGameObjectWithTag("King").GetComponent<PlayerAttributes>();

        return playerAttributes;
    }

    public GameObject getMainCamera()
    {
        return mainCamera;
    }

    public GameObject getCharacterByName(string charName)
    {
        GameObject theChar;
        dicCharacterByName.TryGetValue(charName, out theChar);
        return theChar;
    }

    private IEnumerator LoadWWW(string filePath)
    {
        //Debug.Log("filePath is: " + filePath);
        if (filePath.Contains("://"))
        {
            WWW www = new WWW(filePath);
            yield return www;
            //Debug.Log(www.text);
            //Debug.Log("On a remote machine, loading the file via WWW");

            lstJsonData.Add(www.text);
            ++this.jsonLoadCount;
        }
        else {
            Debug.Log("On a local machine, loading the file via System.IO");

            lstJsonData.Add(System.IO.File.ReadAllText(filePath));
            ++this.jsonLoadCount;
        }

        if (hasJsonLoaded())
            LoadingFinished();
    }

    private void LoadingFinished()
    {
        GameObject.Find("LoadingUI").SetActive(false);
        GameObject.Find("MenuController").GetComponent<MenuController>().FinishedLoading();
    }

}
