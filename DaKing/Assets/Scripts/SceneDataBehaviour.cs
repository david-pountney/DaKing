using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneDataBehaviour : MonoBehaviour {

    public List<MovementBehaviour> Characters { get { return characters; } set { characters = value; } }
    
    public GameObject gameOver;
    public PlayerAttributes playerAttributes;
    public GenerateCharactersByJSONBehaviour gameMaster;
    public UIControllerBehaviour menuController;
    public ChoicesScript choices;
    public JSONManagerBehaviour resourceManager;
    public GameObject speechBubble;
    public SoundDef soundScript;
    public GameObject controller;
    public GameObject nextDay;
    public GameObject musicController;
    public GameObject gameCameraPosition;
    public GameObject loadingui;
    public GameObject gameui;
    public GameObject menuui;


    private List<MovementBehaviour> characters;

}
