using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class MovementBehaviour : MonoBehaviour {

    public MovementLogic MovementLogic { get { return _movementLogic; } set { _movementLogic = value; } }
    
    private GameObject controllerLogic;

    //Might wanna make this a list later on, deping if we want multiple previous decisions effecting dialog
    private DeterminDialog dialogScript;

    //Possible dialogs depending on previous decisions
    private List<string> _dialogText;

    //Yes dialogs
    private List<string> _yesText;

    //No dialogs
    private List<string> _noText;

    [Header("Character Movement Options")]
    public float enterSpeed;
    public float stoppingPointX = 400f;
    public float endingPointX = 1200f;

    public float startingPointX = 1200f;
    public float startingPointY = 1200f;

    public float exitSpeed;

    [Header("Animation Options")]
    public iTween.EaseType moveInAnimationType = iTween.EaseType.easeOutQuart;
    public iTween.EaseType moveOutAnimationType = iTween.EaseType.easeInSine;

    public IdleAnimationType idleAnimationType = IdleAnimationType.Bob;

    [Header("Sine Wave Options")]
    public float amplitudeY;
    public float omegaY;

    private bool enter;
    private bool exit;

    private float index;
    private int currentTextIndex = 0;

    private GameObject speechInstance;

    private bool choicesOpen = false;
    private bool gameIsNowOver = false;

    private bool activated = false;

    private bool answeredYes = false;
    private bool answeredNo = false;

    private bool disableSpeech = false;

    //Which yes dialog are we currently on?
    private int currentYesTextIndex = 0;
    //Which no dialog are we currently on?
    private int currentNoTextIndex = 0;

    //Sound script
    private SoundDef soundScript;

    private SpeechBehaviour _speechBehaviour;
    private MovementLogic _movementLogic;

    void Awake()
    {
        _speechBehaviour = GetComponent<SpeechBehaviour>();
        _movementLogic = new MovementLogic();
    }

    void Start()
    {
        SetUpLogic();
        
        dialogScript = GetComponent<DeterminDialog>();

        //Set up sound player
        soundScript = GameObject.FindGameObjectWithTag("SoundPlayer").GetComponent<SoundDef>();
    }

    private void SetUpLogic()
    {
        _movementLogic.ThisTransform = this.transform;
        _movementLogic.SpeechBehaviour = _speechBehaviour;
        _movementLogic.SpeechInstance = GlobalReferencesBehaviour.instance.SceneData.speechBubble;
        _movementLogic.ControllerLogic = GlobalReferencesBehaviour.instance.SceneData.controller;
        _movementLogic.GameOver = GlobalReferencesBehaviour.instance.SceneData.gameOver;
        _movementLogic.EnterSpeed = enterSpeed;
        _movementLogic.StoppingPointX = stoppingPointX;
        _movementLogic.EndingPointX = endingPointX;
        _movementLogic.StartingPointX = startingPointX;
        _movementLogic.StartingPointY = startingPointY;
        _movementLogic.ExitSpeed = exitSpeed;
        _movementLogic.MoveInEaseType = moveInAnimationType;
        _movementLogic.MoveOutEaseType = moveOutAnimationType;

        _movementLogic.IdleAnimationType = idleAnimationType;

        _movementLogic.AmplitudeY = amplitudeY;
        _movementLogic.OmegaY = omegaY;

        _movementLogic.Setup();
    }

    public void Update()
    {
        _movementLogic.UpdateSineWave();
        _movementLogic.HandleMovement();

        if (Input.GetKeyUp(KeyCode.Space))
            _movementLogic.HandleInput();
    }

    private void itweenCallback_FinishedMovingOntoScreen()
    {
        _movementLogic.FinishedMovingOntoScreen();
    }

    private void itweenCallback_FinishedMovingOffScreen()
    {
        _movementLogic.FinishedMovingOffScreen();
    }

}
