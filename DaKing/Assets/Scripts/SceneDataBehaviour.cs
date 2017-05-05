﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneDataBehaviour : MonoBehaviour {

    public List<MovementBehaviour> Characters { get { return characters; } set { characters = value; } }
    
    public GameObject gameOver;
    public PlayerAttributes playerAttributes;
    public GameMaster gameMaster;
    public MenuController menuController;
    public ChoicesScript choices;
    public ResourceManager resourceManager;
    public GameObject speechBubble;
    public SoundDef soundScript;
    public GameObject controller;
    public GameObject nextDay;
    public GameObject musicController;
    public GameObject gameCameraPosition;

    private List<MovementBehaviour> characters;

}