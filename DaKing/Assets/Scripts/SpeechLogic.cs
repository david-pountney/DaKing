using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Text.RegularExpressions;

public class SpeechLogic {

    public Transform ThisTransform { get { return _thisTransform; } set { _thisTransform = value; } }
    public GameObject SpeechInstance { get { return _speechInstance; } set { _speechInstance = value; } }
    public MovementBehaviour MovementLogic{ get { return _movementLogic; } set { _movementLogic = value; } }
    public GameObject Choices { get { return _choices; } set { _choices = value; } }
    public GameObject ControllerLogic { get { return _controllerLogic; } set { _controllerLogic = value; } }
    public GameObject GameOver { get { return _gameover; } set { _gameover = value; } }
    public DeterminDialog DialogScript { get { return _dialogScript; } set { _dialogScript = value; } }
    public List<string> DialogText { get { return _dialogText; } set { _dialogText = value; } }
    public List<string> YesText { get { return _yesText; } set { _yesText = value; } }
    public List<string> NoText { get { return _noText; } set { _noText = value; } }
    public float SpeechBubbleX { get { return _speechBubbleX; } set { _speechBubbleX = value; } }
    public float SpeechBubbleY { get { return _speechBubbleY; } set { _speechBubbleY = value; } }
    public Vector3 NormalSpeechBubbleScale { get { return _normalSpeechBubbleScale; } set { _normalSpeechBubbleScale = value; } }
    public SoundDef SoundScript { get { return _soundScript; } set { _soundScript = value; } }
    public bool GameIsNowOver { get { return _gameIsNowOver; } set { _gameIsNowOver = value; } }

    private Transform _thisTransform;

    private GameObject _speechInstance;
    private MovementBehaviour _movementLogic;

    private GameObject _choices;
    private GameObject _controllerLogic;
    private GameObject _gameover;

    //Might wanna make this a list later on, deping if we want multiple previous decisions effecting dialog
    private DeterminDialog _dialogScript;

    //Possible dialogs depending on previous decisions
    private List<string> _dialogText;

    //Yes dialogs
    private List<string> _yesText;

    //No dialogs
    private List<string> _noText;

    private float _speechBubbleX;
    private float _speechBubbleY;

    private Vector3 _normalSpeechBubbleScale;

    private bool _choicesOpen = false;
    private bool _gameIsNowOver = false;

    private bool _activated = false;

    private bool _answeredYes = false;
    private bool _answeredNo = false;

    private bool _disableSpeech = false;

    private float _index;
    private int _currentTextIndex = 0;

    //Which yes dialog are we currently on?
    private int _currentYesTextIndex = 0;
    //Which no dialog are we currently on?
    private int _currentNoTextIndex = 0;

    //Sound script
    private SoundDef _soundScript;
    
    //How long the current speech should be delayed for before it is shown
    private int _speechDelay = 0;
    private bool _speechShowing = false;

    public void HandleInput()
    {
        if (!_speechInstance || _disableSpeech) return;

        if (CheckForGameOver())
            //Check if the game is over
            GameOverLogic();

        else if (_choices && !_choices.activeSelf && !_movementLogic.MovementLogic.Enter && !_movementLogic.MovementLogic.Exit && _speechInstance.activeSelf)
            ChangeSpeak();
        else if (!_choices.activeSelf && !_movementLogic.MovementLogic.Enter && !_movementLogic.MovementLogic.Exit && _speechInstance.activeSelf)
            ChangeSpeak();
    }

    private void Init()
    {
        _speechInstance.transform.localPosition = new Vector2(_speechBubbleX, _speechBubbleY);

        //Get dialog
        _dialogText = _dialogScript.GetDialog();

        //We can now show the speech bubble
        _speechShowing = true;
    }

    public void StartSpeak()
    {
        Init();
        ShowSpeechBubble();
        ChangeSpeak();
    }

    public void ChangeSpeak()
    {
        //Check if speech is currently accepting input
        if (!CheckIfSpeechEnabled()) return;

        //Check if we are currently going through a characters answer dialog, if so..
        //Flip to the next answer speech bubble or end the speech bubble if we at the end
        else if (!AnsweredLogic())
        //Otherwise just run the normal logic
            NormalDialogLogic();

        PlayPageTurnSound();
    }

    public bool CheckForGameOver()
    {
        if (_gameIsNowOver) return true;

        return false;
    }

    public void ShowYesSpeech()
    {
        //Get yes/no dialog
        _yesText = _dialogScript.SpeechYes;

        _answeredYes = true;

        string nextSpeech = _dialogScript.SpeechYes[_currentYesTextIndex];

        //Show next speech bubble
        _speechInstance.transform.GetComponentInChildren<Text>().text = ParseAllTags(nextSpeech);

        PlayPageTurnSound();
    }

    private void RemoveSpeechBubble()
    {
        Text speechText = _speechInstance.GetComponentInChildren<Text>();
        speechText.enabled = false;

        iTween.ScaleTo(_speechInstance.gameObject, iTween.Hash("scale", Vector3.zero,
                                                              "time", .25f,
                                                              "easetype", iTween.EaseType.linear,
                                                              "oncomplete", "itweenCallback_FinishedRemovingSpeechBubble",
                                                              "oncompletetarget", _thisTransform.gameObject));

        //We are now hiding the speech bubble
        _speechShowing = false;
    }

    private void HideSpeechBubble()
    {
        Text speechText = _speechInstance.GetComponentInChildren<Text>();
        speechText.enabled = false;

        iTween.ScaleTo(_speechInstance.gameObject, iTween.Hash("scale", Vector3.zero,
                                                              "time", .25f,
                                                              "easetype", iTween.EaseType.linear));

        //We are now hiding the speech bubble
        _speechShowing = false;
    }

    public void ShowSpeechBubble()
    {
        Text speechText = _speechInstance.GetComponentInChildren<Text>();
        speechText.enabled = true;

        _speechInstance.transform.localScale = Vector3.zero;

        iTween.ScaleTo(_speechInstance.gameObject, iTween.Hash("scale", _normalSpeechBubbleScale,
                                                      "time", .25f,
                                                      "easetype", iTween.EaseType.linear));

        //We are now showing the speech bubble 
        _speechShowing = true;
    }

    public void ShowNoSpeech()
    {
        //Get yes/no dialog
        _noText = _dialogScript.SpeechNo;

        _answeredNo = true;

        string nextSpeech = _dialogScript.SpeechNo[_currentNoTextIndex];

        //Show next speech bubble
        _speechInstance.transform.GetComponentInChildren<Text>().text = ParseAllTags(nextSpeech);

        //Play the page turning sound
        PlayPageTurnSound();
    }

    private void NormalDialogLogic()
    {
        if (CheckIfFinishedDialog())
            return;
        
        string nextSpeech = _dialogText[_currentTextIndex];

        //Show next speech bubble
        _speechInstance.transform.GetComponentInChildren<Text>().text = ParseAllTags(nextSpeech);

        _currentTextIndex++;
    }

    private bool CheckIfFinishedDialog()
    {
        if (_currentTextIndex > DialogText.Count - 1)
        {
            KillSpeechAndChoices();
            

            return true;
        }

        return false;
    }

    private void PlayPageTurnSound()
    {
        //Play the page turning sound
        _soundScript.fire();
    }

    private bool CheckIfSpeechEnabled()
    {
        if ((_choices.activeSelf && _choicesOpen) || !_speechShowing) return false;

        return true;
    }

    private string ParseAllTags(string newSpeech)
    {
        //Check if we show choices now
        if (checkForTags(newSpeech, @"|c"))
        {
            removeTagFromEndOfText(ref newSpeech, 2);
            createChoices();
        }

        //Check if we exexcute passives now
        if (checkForTags(newSpeech, @"|p1"))
        {
            removeTagFromEndOfText(ref newSpeech, 3);
            executePassiveOne();
        }

        //Check if we exexcute passives now
        if (checkForTags(newSpeech, @"|p2"))
        {
            removeTagFromEndOfText(ref newSpeech, 3);
            executePassiveTwo();
        }

        //Check if the player has died
        if (checkForTags(newSpeech, @"|d"))
        {
            removeTagFromEndOfText(ref newSpeech, 2);
            ChangeSpeechBubbleSprite();
            _gameIsNowOver = true;
        }

        //Adds to the super soldier count
        if (checkForTags(newSpeech, @"|super"))
        {
            GlobalReferencesBehaviour.instance.SceneData.gameMaster.GameMasterLogic.SuperSoldierCount++;
            removeTagFromEndOfText(ref newSpeech, 6);
        }

        //Triggers the check for whether the player has enough super soldiers
        if (checkForTags(newSpeech, @"|superlast"))
        {
            removeTagFromEndOfText(ref newSpeech, 10);
            GenerateCharactersByJSONBehaviour gm = GlobalReferencesBehaviour.instance.SceneData.gameMaster;
            _thisTransform.GetComponent<ExecuteChoices>().outcomeChoice = gm.GameMasterLogic.SuperSoldierCount >= gm.GameMasterLogic.SuperSoldierNeeded;
        }

        if (checkForTags(newSpeech, "("))
        {
            ParseSpeechDelay(newSpeech);

            removeTagFromBeginningOfText(ref newSpeech, 3);
        }

        return newSpeech;
    }

    private void ParseSpeechDelay(string newSpeech)
    {
        string text = newSpeech.Substring(0, 3);
        string stringLength = String.Empty;
        var isMatch = Regex.Match(text, @"^(\([0-9-]+\))+$");

        if (isMatch.Value != String.Empty)
        {
            stringLength = isMatch.ToString().Substring(1, 1);
            _speechDelay = int.Parse(stringLength);

            if (_speechDelay > 0)
            {
                HideSpeechBubble();
                WaitForSpeechDelay();
            }
        }
    }

    private void WaitForSpeechDelay()
    {
        iTween.ValueTo(_thisTransform.gameObject, iTween.Hash("from", 0f, "to", 1f,
                                                              "time", _speechDelay, 
                                                              "onupdate", "null",
                                                              "oncomplete", "itweenCallback_FinishedWaitingForSpeechDelay"));

        //Reset SpeechDelay
        _speechDelay = 0;
    }

    private void ChangeSpeechBubbleSprite()
    {
        GlobalReferencesBehaviour.instance.SceneData.speechBubble.GetComponent<Image>().sprite =
            Resources.Load<Sprite>("speechInverted");

        GlobalReferencesBehaviour.instance.SceneData.speechBubble.GetComponentInChildren<Text>().color = Color.white;
    }
    
    private bool AnsweredLogic()
    {
        if (_answeredYes)
        {
            //Are we at the end of the yes logic?
            if (_currentYesTextIndex >= _yesText.Count - 1)
            {
                //Get rid of the speech bubble
                KillSpeechAndChoices();
                //Move character out
                _movementLogic.MovementLogic.StartMovingCharacterOut();
            }
            else {

                string nextSpeech = _yesText[++_currentYesTextIndex];

                //Show next yes speech bubble
                _speechInstance.transform.GetComponentInChildren<Text>().text = ParseAllTags(nextSpeech);
            }

            return true;
        }

        else if (_answeredNo)
        {
            if (_currentNoTextIndex >= _noText.Count - 1)
            {
                KillSpeechAndChoices();
                _movementLogic.MovementLogic.StartMovingCharacterOut();
            }
            else {
                string nextSpeech = _noText[++_currentNoTextIndex];

                //Show next yes speech bubble
                _speechInstance.transform.GetComponentInChildren<Text>().text = ParseAllTags(nextSpeech);
            }

            return true;
        }

        return false;
    }

    private void GameOverLogic()
    {
        _gameover.SetActive(true);
        _gameover.GetComponent<CurtainActivate>().startEndDay();

        GlobalReferencesBehaviour.instance.SceneData.musicController.GetComponent<SimpleMusicController>().fade_out();
        
        KillSpeechAndChoices();

        _disableSpeech = true;
    }

    private bool checkForTags(string newSpeech, string tag)
    {
        if (newSpeech.Contains(tag))
        {
            return true;
        }

        return false;
    }
    
    private void removeTagFromEndOfText(ref string newSpeech, int length)
    {
        string temp = newSpeech;
        string temp2 = temp.Remove(temp.Length - length, length);
        newSpeech = temp2;
    }

    private void removeTagFromBeginningOfText(ref string newSpeech, int length)
    {
        string temp = newSpeech;
        string temp2 = temp.Remove(0, length);
        newSpeech = temp2;
    }

    private void executePassiveOne()
    {
        IChoiceLogic choiceLogic = new PassiveOneChoiceLogic();

        _thisTransform.GetComponent<ExecuteChoicesBehaviour>().ExecuteChoices.ExecuteChoice(choiceLogic);
    }

    private void executePassiveTwo()
    {
        IChoiceLogic choiceLogic = new PassiveTwoChoiceLogic();

        _thisTransform.GetComponent<ExecuteChoicesBehaviour>().ExecuteChoices.ExecuteChoice(choiceLogic);
    }

    public void ExecuteCantAffordSpeech()
    {
        if (_activated)
        {
            if (_dialogScript.cantAffordDialog == null)
            {
                Debug.LogError("dialogScript.cantAffordDialog is null for character -> " + _thisTransform.name);
            }
            else
            {
                _speechInstance.transform.GetComponentInChildren<Text>().text = _dialogScript.cantAffordDialog[0];
            }
            //Play the page turning sound
            _soundScript.fire();
        }
    }

    private void createChoices()
    {
        if (!_choicesOpen)
        {
            _choices.SetActive(true);
            _choicesOpen = true;
        }
    }

    private void KillSpeechAndChoices()
    {
        _choices.SetActive(false);

        RemoveSpeechBubble();

        //Play the page turning sound
        _soundScript.fire();
    }

    private void killSelf()
    {
        _thisTransform.localPosition = new Vector2(9999, 9999);
        _currentTextIndex = 0;
        _controllerLogic.GetComponent<ControllerLogic>().nextCharacter();
    }
}
