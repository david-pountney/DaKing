using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ControllerBehaviour : MonoBehaviour {

    public ControllerLogic ControllerLogic { get { return _controllerLogic; } set { _controllerLogic = value; } }

    //public List<GameObject> _listOfCharacters;

    private ControllerLogic _controllerLogic;

    // Use this for initialization
    void Awake()
    {
        _controllerLogic = new ControllerLogic();

    }

    void Start()
    {
        Setup();

    }

    private void Setup()
    {
        _controllerLogic.MusicController = GlobalReferencesBehaviour.instance.SceneData.musicController;
        _controllerLogic.MoodDisplay = GlobalReferencesBehaviour.instance.SceneData.controller.GetComponent<MoodDisplayScript>();
        _controllerLogic.GetAllCharacters();

    }
}
