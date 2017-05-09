using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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

    public void StartSpeak()
    {
        _speechInstance.transform.localPosition = new Vector2(_speechBubbleX, _speechBubbleY);

        //Get dialog
        _dialogText = _dialogScript.GetDialog();

        _speechInstance.transform.GetComponentInChildren<Text>().text = _dialogText[_currentTextIndex];

        //Check if we show choices, or execute passive abilities, and then remove the tags from the speech bubble text
        ParseAllTags();

        PlayPageTurnSound();
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

        //Check if we show choices, or execute passive abilities, and then remove the tags from the speech bubble text
        ParseAllTags();

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

        _speechInstance.transform.GetComponentInChildren<Text>().text = _dialogScript.SpeechYes[_currentYesTextIndex];
        
        _answeredYes = true;

        //Check if we show choices, or execute passive abilities, and then remove the tags from the speech bubble text
        ParseAllTags();
        
        PlayPageTurnSound();
    }

    public void ShowNoSpeech()
    {
        //Get yes/no dialog
        _noText = _dialogScript.SpeechNo;

        _speechInstance.transform.GetComponentInChildren<Text>().text = _dialogScript.SpeechNo[_currentNoTextIndex];

        _answeredNo = true;

        //Check if we show choices, or execute passive abilities, and then remove the tags from the speech bubble text
        ParseAllTags();

        //Play the page turning sound
        PlayPageTurnSound();
    }

    private void NormalDialogLogic()
    {
        if (_currentTextIndex >= DialogText.Count - 1)
        {
            killSpeechAndChoices();
            _movementLogic.MovementLogic.StartMovingCharacterOut();

            return;
        }

        _speechInstance.transform.GetComponentInChildren<Text>().text = _dialogText[++_currentTextIndex];
    }

    private void PlayPageTurnSound()
    {
        //Play the page turning sound
        _soundScript.fire();
    }

    private bool CheckIfSpeechEnabled()
    {
        if (_choices.activeSelf && _choicesOpen) return false;

        return true;
    }

    private void ParseAllTags()
    {
        //Check if we show choices now
        if (checkForTags(@"|c"))
        {
            removeTagFromText(2);
            createChoices();
        }

        //Check if we exexcute passives now
        if (checkForTags(@"|p1"))
        {
            removeTagFromText(3);
            executePassiveOne();
        }

        //Check if we exexcute passives now
        if (checkForTags(@"|p2"))
        {
            removeTagFromText(3);
            executePassiveTwo();
        }

        //Check if the player has died
        if (checkForTags(@"|d"))
        {
            removeTagFromText(2);
            _gameIsNowOver = true;
        }

        //Check if we should remove all the players money
        if (checkForTags(@"|rmon"))
        {
            removeTagFromText(5);
            executeRemoveAllMoney();
        }

        //Check if we should remove all the players military
        if (checkForTags(@"|rmil"))
        {
            removeTagFromText(5);
            executeRemoveAllMilitary();
        }

        //Adds to the super soldier count
        if (checkForTags(@"|super"))
        {
            GlobalReferencesBehaviour.instance.SceneData.gameMaster.GameMasterLogic.SuperSoldierCount++;
            removeTagFromText(6);
        }

        //Triggers the check for whether the player has enough super soldiers
        if (checkForTags(@"|superlast"))
        {
            removeTagFromText(10);
            GenerateCharactersByJSONBehaviour gm = GlobalReferencesBehaviour.instance.SceneData.gameMaster;
            _thisTransform.GetComponent<ExecuteChoices>().outcomeChoice = gm.GameMasterLogic.SuperSoldierCount >= gm.GameMasterLogic.SuperSoldierNeeded;
        }
    }

    private bool AnsweredLogic()
    {
        if (_answeredYes)
        {
            //Are we at the end of the yes logic?
            if (_currentYesTextIndex >= _yesText.Count - 1)
            {
                //Get rid of the speech bubble
                killSpeechAndChoices();
                //Move character out
                _movementLogic.MovementLogic.StartMovingCharacterOut();
            }
            else
                //Show next yes speech bubble
                _speechInstance.transform.GetComponentInChildren<Text>().text = _yesText[++_currentYesTextIndex];

            return true;
        }

        else if (_answeredNo)
        {
            if (_currentNoTextIndex >= _noText.Count - 1)
            {
                killSpeechAndChoices();
                _movementLogic.MovementLogic.StartMovingCharacterOut();
            }
            else
                _speechInstance.transform.GetComponentInChildren<Text>().text = _noText[++_currentNoTextIndex];

            return true;
        }

        return false;
    }

    private void GameOverLogic()
    {
        _gameover.SetActive(true);
        _gameover.GetComponent<CurtainActivate>().startEndDay();

        GameObject.Find("MusicController").GetComponent<SimpleMusicController>().fade_out();

        killSpeechAndChoices();

        _disableSpeech = true;
    }

    private bool checkForTags(string tag)
    {
        if (_speechInstance.transform.GetComponentInChildren<Text>().text.Contains(tag))
        {
            return true;
        }
        return false;
    }
    
    private void removeTagFromText(int length)
    {
        //Ch string
        string temp = _speechInstance.transform.GetComponentInChildren<Text>().text;
        string temp2 = temp.Remove(temp.Length - length, length);
        _speechInstance.transform.GetComponentInChildren<Text>().text = temp2;
    }

    private void executePassiveOne()
    {
       // _thisTransform.GetComponent<ExecuteChoices>().ExecutePassiveOneChoice();
    }

    private void executePassiveTwo()
    {
        //_thisTransform.GetComponent<ExecuteChoices>().ExecutePassiveTwoChoice();
    }

    public void executeRemoveAllMoney()
    {
        //_thisTransform.GetComponent<ExecuteChoices>().ExecuteRemoveAll(true, false, false);
    }

    public void executeRemoveAllMilitary()
    {
        //_thisTransform.GetComponent<ExecuteChoices>().ExecuteRemoveAll(false, true, false);
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
    
    public void ParseTags()
    {
        if (checkForTags(@"|d"))
        {
            removeTagFromText(2);
            _gameIsNowOver = true;
        }

        if (checkForTags(@"|rmon"))
        {
            removeTagFromText(5);
            executeRemoveAllMoney();
        }

        if (checkForTags(@"|rmil"))
        {
            removeTagFromText(5);
            executeRemoveAllMilitary();
        }

        if (checkForTags(@"|super"))
        {
            GlobalReferencesBehaviour.instance.SceneData.gameMaster.GameMasterLogic.SuperSoldierCount++;
            removeTagFromText(6);
        }

        if (checkForTags(@"|superlast"))
        {
            removeTagFromText(10);
            GenerateCharactersByJSONBehaviour gm = GlobalReferencesBehaviour.instance.SceneData.gameMaster;
            _thisTransform.GetComponent<ExecuteChoices>().outcomeChoice = gm.GameMasterLogic.SuperSoldierCount >= gm.GameMasterLogic.SuperSoldierNeeded;
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
        _controllerLogic.GetComponent<ControllerLogic>().nextCharacter();
    }
}
