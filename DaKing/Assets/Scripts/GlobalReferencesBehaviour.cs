using UnityEngine;
using System.Collections;

public class GlobalReferencesBehaviour : MonoBehaviour {

    public SceneDataBehaviour SceneData { get { return sceneData; } set { sceneData = value; } }

    public static GlobalReferencesBehaviour instance = null;
    public SceneDataBehaviour sceneData;
    
    // Use this for initialization
    void Awake()
    {
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
    }

    void OnLevelWasLoaded()
    {
        // If a new level is loaded, find it's "SceneData" object in the scene
        sceneData = FindObjectOfType<SceneDataBehaviour>();
    }
}
