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

    static ResourceManager instance;
    void Awake()
    {
        if (ResourceManager.instance == null)
        {
            DontDestroyOnLoad(transform.gameObject);
            ResourceManager.instance = this;
            load();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static ResourceManager getInstance()
    {
        return instance;
    }

    void load()
    {
        Debug.Log("loading...!");
        ResourceManager.getInstance().dicCharacterByName = new Dictionary<string, GameObject>();

        lstJsonData = new List<TextAsset>();
        DirectoryInfo dir = new DirectoryInfo(@"Assets/Resources/"+jsonDataPath);
        FileInfo[] dirInfo = dir.GetFiles("*.json");
        foreach (FileInfo file in dirInfo)
        {
            Debug.Log(@jsonDataPath + "/" + file.Name.Split( new char[] {'.'} )[0] );
            lstJsonData.Add(Resources.Load<TextAsset>(@jsonDataPath+"/"+file.Name.Split( new char[] {'.'} )[0]));
        }


        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerAttributes = GameObject.FindGameObjectWithTag("King").GetComponent<PlayerAttributes>();

        Debug.Log("loading fin!");
    }

    public static PlayerAttributes getPlayerAttributes()
    {
        return ResourceManager.getInstance().playerAttributes;
    }

    public static GameObject getMainCamera()
    {
        return ResourceManager.getInstance().mainCamera;
    }
    public static GameObject getCharacterByName(string charName)
    {
        GameObject theChar;
        ResourceManager.getInstance().dicCharacterByName.TryGetValue(charName, out theChar);
        return theChar;
    }
}
