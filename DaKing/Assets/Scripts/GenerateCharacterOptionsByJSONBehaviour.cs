using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateCharacterOptionsByJSONBehaviour : MonoBehaviour {

    public GenerateCharacterOptionsByJSONLogic GenerateCharacterOptionsByJSONLogic { get { return _generateCharacterOptionsByJSONLogic; } set { _generateCharacterOptionsByJSONLogic = value; } }

    [Header("Dev Options")]
    public bool devMode = false;

    private GenerateCharacterOptionsByJSONLogic _generateCharacterOptionsByJSONLogic;

    // Use this for initialization
    void Awake()
    {
        _generateCharacterOptionsByJSONLogic = new GenerateCharacterOptionsByJSONLogic();
    }

    void Start()
    {
        Setup();
    }

    private void Setup()
    {
        _generateCharacterOptionsByJSONLogic.DevMode = devMode;
        _generateCharacterOptionsByJSONLogic.ResourceManager = GlobalReferencesBehaviour.instance.SceneData.resourceManager;
    }
}
