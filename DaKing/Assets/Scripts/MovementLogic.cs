using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public enum IdleAnimationType
{
    Bob,
    Swift
}

public class MovementLogic {
    public Transform ThisTransform { get { return _thisTransform; } set { _thisTransform = value; } }
    public SpeechBehaviour SpeechBehaviour { get { return _speechBehaviour; } set { _speechBehaviour = value; } }
    public GameObject SpeechInstance { get { return _speechInstance; } set { _speechInstance = value; } }
    public GameObject Choices { get { return _choices; } set { _choices = value; } }
    public GameObject GameOver { get { return _gameover; } set { _gameover = value; } }
    public GameObject ControllerLogic { get { return _controllerLogic; } set { _controllerLogic = value; } }
    public float EnterSpeed { get { return _enterSpeed; } set { _enterSpeed = value; } }
    public float StoppingPointX { get { return _stoppingPointX; } set { _stoppingPointX = value; } }
    public float EndingPointX { get { return _endingPointX; } set { _endingPointX = value; } }
    public float StartingPointX { get { return _startingPointX; } set { _startingPointX = value; } }
    public float StartingPointY { get { return _startingPointY; } set { _startingPointY = value; } }
    public float ExitSpeed { get { return _exitSpeed; } set { _exitSpeed = value; } }
    public float AmplitudeY { get { return _amplitudeY; } set { _amplitudeY = value; } }
    public float OmegaY { get { return _omegaY; } set { _omegaY = value; } }
    public bool GameIsNowOver { get { return _gameIsNowOver; } set { _gameIsNowOver = value; } }
    public bool Enter { get { return _enter; } set { _enter = value; } }
    public bool Exit { get { return _exit; } set { _exit = value; } }
    public iTween.EaseType MoveInEaseType { get { return _moveInAnimationType; } set { _moveInAnimationType = value; } }
    public iTween.EaseType MoveOutEaseType { get { return _moveOutAnimationType; } set { _moveOutAnimationType = value; } }
    public IdleAnimationType IdleAnimationType { get { return _idleAnimationType; } set { _idleAnimationType = value; } }

    private Transform _thisTransform;

    private GameObject _choices;
    private GameObject _gameover;
    private GameObject _controllerLogic;

    //Might wanna make this a list later on, deping if we want multiple previous decisions effecting dialog
    private DeterminDialog _dialogScript;

    //Possible dialogs depending on previous decisions
    private List<string> _dialogText;

    //Yes dialogs
    private List<string> _yesText;

    //No dialogs
    private List<string> _noText;

    private float _enterSpeed;
    private float _stoppingPointX = 400f;
    private float _endingPointX = 1200f;

    private float _startingPointX = 1200f;
    private float _startingPointY = 1200f;

    private float _exitSpeed;

    private bool _enter;
    private bool _exit;

    //Sine wave
    private float _sineWaveIndex;
    private float _amplitudeY;
    private float _omegaY;
    
    private int _currentTextIndex = 0;

    private GameObject _speechInstance;

    private bool _choicesOpen = false;
    private bool _gameIsNowOver = false;
    
    private bool _answeredYes = false;
    private bool _answeredNo = false;

    private bool _disableSpeech = false;

    //Which yes dialog are we currently on?
    private int _currentYesTextIndex = 0;
    //Which no dialog are we currently on?
    private int _currentNoTextIndex = 0;

    //Sound script
    private SoundDef _soundScript;

    private SpeechBehaviour _speechBehaviour;

    //Animation
    private iTween.EaseType _moveInAnimationType;
    private iTween.EaseType _moveOutAnimationType;

    private IdleAnimationType _idleAnimationType;

    public void UpdateSineWave()
    {
        //Sine wave
        _sineWaveIndex += Time.deltaTime;
        float y = _amplitudeY * Mathf.Sin(_omegaY * _sineWaveIndex);

        if(_idleAnimationType == IdleAnimationType.Bob)
            _thisTransform.localPosition = new Vector2(_thisTransform.localPosition.x, _thisTransform.localPosition.y + y);
        if (_idleAnimationType == IdleAnimationType.Swift)
            _thisTransform.localRotation = new Quaternion(_thisTransform.localRotation.x, _thisTransform.localRotation.y, y, 1f);
    }

    public void HandleMovement()
    {
        //Enter/exit
        if (_enter && _thisTransform.localPosition.x > _stoppingPointX)
        {
            MoveLeft();
        }
        else if (_enter)
        {

        }
        else if (_exit)
        {
            MoveRight();
        }
    }

    public void HandleInput()
    {
        
    }

    public void Setup()
    {
        _thisTransform.localPosition = new Vector2(_startingPointX, _startingPointY);
        _choicesOpen = false;
        //_enter = true;
        MoveLeft();
    }

    /// <summary>
    /// Flips character around when leaving the room
    /// </summary>
    private void FlipCharacter()
    {
        _thisTransform.localScale = new Vector2(_thisTransform.localScale.x * -1, _thisTransform.localScale.y);
    }

    private bool CheckForTags(string tag)
    {
        if (_speechInstance.transform.GetComponentInChildren<Text>().text.Contains(tag))
        {
            return true;
        }
        return false;
    }

    private void RemoveTagFromText(int length)
    {
        //Ch string
        string temp = _speechInstance.transform.GetComponentInChildren<Text>().text;
        string temp2 = temp.Remove(temp.Length - length, length);
        _speechInstance.transform.GetComponentInChildren<Text>().text = temp2;
    }

    public void StartMovingCharacterOut()
    {
        MoveRight();
    }

    private void MoveLeft()
    {
        iTween.MoveTo(_thisTransform.gameObject, iTween.Hash("position", new Vector3(_stoppingPointX, _thisTransform.position.y, 0f),
                                                             "time", 3f,
                                                             "easeType", _moveInAnimationType,
                                                             "oncomplete", "itweenCallback_FinishedMovingOntoScreen"));
    }

    public void FinishedMovingOntoScreen()
    {
        _speechBehaviour.SpeechLogic.StartSpeak();
    }

    private void MoveRight()
    {
        _thisTransform.position = new Vector2(_thisTransform.position.x + (_exitSpeed * Time.deltaTime), _thisTransform.position.y);
        //_speechInstance.transform.localPosition = new Vector3(999f, 999f, 999f);

        iTween.MoveTo(_thisTransform.gameObject, iTween.Hash("position", new Vector3(_endingPointX, _thisTransform.position.y, 0f),
                                                     "time", 3f,
                                                     "easeType", _moveOutAnimationType,
                                                     "oncomplete", "itweenCallback_FinishedMovingOffScreen"));
    }

    public void FinishedMovingOffScreen()
    {
        killSelf();
    }

    private void killSpeechAndChoices()
    {
        _choices.SetActive(false);

        //Play the page turning sound
        _soundScript.fire();
    }

    private void killSelf()
    {
        _thisTransform.localPosition = new Vector2(9999, 9999);
        _currentTextIndex = 0;
        _controllerLogic.GetComponent<ControllerBehaviour>().ControllerLogic.nextCharacter();
    }
}
