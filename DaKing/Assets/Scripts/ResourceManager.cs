using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    public string jsonDataPath;

    GameObject mainCamera;
    PlayerAttributes playerAttributes;
    public GameObject sadParticles;

    public List<string> lstJsonData;

    Dictionary<string, GameObject> dicCharacterByName;

    public static ResourceManager instance;

    void Awake()
    {
        if (ResourceManager.instance == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

            DontDestroyOnLoad(transform.gameObject);
            ResourceManager.instance = this;
            Debug.Log("Awake called, going to load");

        }
        else
        {
            Debug.Log("destroying resource manager");
            Destroy(this);
        }

        load();
    }

    void load()
    {
        Debug.Log("loading...!");
        ResourceManager.instance.dicCharacterByName = new Dictionary<string, GameObject>();

        lstJsonData = new List<string>();

        //Get all characters
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Character");

        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, jsonDataPath);
        //filePath = "file:///E:/DaKing/DaKing/Build/StreamingAssets/Characters";

        Debug.Log("hgot here");
        string result = "";

        foreach (GameObject character in characters)
        {
            Debug.Log("loading characters...");
            StartCoroutine(LoadWWW(filePath + "/" + character.name + "Text.json"));

        }

        playerAttributes = GameObject.FindGameObjectWithTag("King").GetComponent<PlayerAttributes>();
        sadParticles = GameObject.FindGameObjectWithTag("SadParticles");
        Debug.Log("loading fin!");
    }

    public PlayerAttributes getPlayerAttributes()
    {
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
            Debug.Log("On a remote machine, loading the file via WWW");

            lstJsonData.Add(www.text);
        }
        else {
            Debug.Log("On a local machine, loading the file via System.IO");

            lstJsonData.Add(System.IO.File.ReadAllText(filePath));
        }
    }

}
