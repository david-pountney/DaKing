using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    public string jsonDataPath;

    GameObject mainCamera;
    PlayerAttributes playerAttributes;

    public List<TextAsset> lstJsonData;

    Dictionary<string, GameObject> dicCharacterByName;

    public static ResourceManager instance;

    void Awake()
    {
        if (ResourceManager.instance == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

            DontDestroyOnLoad(transform.gameObject);
            ResourceManager.instance = this;
            load();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void load()
    {
        Debug.Log("loading...!");
        ResourceManager.instance.dicCharacterByName = new Dictionary<string, GameObject>();

        lstJsonData = new List<TextAsset>();
        DirectoryInfo dir = new DirectoryInfo(@"Assets/Resources/"+jsonDataPath);
        FileInfo[] dirInfo = dir.GetFiles("*.json");
        foreach (FileInfo file in dirInfo)
        {
            Debug.Log(@jsonDataPath + "/" + file.Name.Split( new char[] {'.'} )[0] );
            lstJsonData.Add(Resources.Load<TextAsset>(@jsonDataPath+"/"+file.Name.Split( new char[] {'.'} )[0]));
        }


        playerAttributes = GameObject.FindGameObjectWithTag("King").GetComponent<PlayerAttributes>();

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
}
