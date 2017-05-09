using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GenerateCharactersByJSONBehaviour : MonoBehaviour {

    public GenerateCharactersByJSONLogic GameMasterLogic { get { return _gameMaster; } set { _gameMaster = value; } }

    [SerializeField]
    private List<CharacterData> lstCharData;

    [SerializeField]
    private int superSoldiersCount;

    public int superSoldiersNeeded;

    [Header("Dev Options")]
    public bool devMode = false;

    private GenerateCharactersByJSONLogic _gameMaster;

    // Use this for initialization
    void Awake () {
        _gameMaster = new GenerateCharactersByJSONLogic();
	}
	
    void Start()
    {
        Setup();
    }

    private void Setup()
    {
        _gameMaster.SuperSoldierNeeded = superSoldiersNeeded;
        _gameMaster.DevMode = devMode;
        _gameMaster.ResourceManager = GlobalReferencesBehaviour.instance.SceneData.resourceManager;
    }

}
