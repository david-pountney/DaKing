using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GenerateCharactersByJSONBehaviour : MonoBehaviour {

    public GenerateCharactersByJSONLogic GenerateCharactersByJSONLogic { get { return _generateCharactersByJSONLogic; } set { _generateCharactersByJSONLogic = value; } }

    [SerializeField]
    private int superSoldiersCount;

    public int superSoldiersNeeded;

    [Header("Dev Options")]
    public bool devMode = false;

    private GenerateCharactersByJSONLogic _generateCharactersByJSONLogic;

    // Use this for initialization
    void Awake () {
        _generateCharactersByJSONLogic = new GenerateCharactersByJSONLogic();
	}
	
    void Start()
    {
        Setup();
    }

    private void Setup()
    {
        _generateCharactersByJSONLogic.SuperSoldierNeeded = superSoldiersNeeded;
        _generateCharactersByJSONLogic.DevMode = devMode;
        _generateCharactersByJSONLogic.ResourceManager = GlobalReferencesBehaviour.instance.SceneData.resourceManager;
    }

}
