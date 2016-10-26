using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class MovementForChars : MonoBehaviour {

    public GameObject speech;
    public GameObject choices;
    private GameObject controllerLogic;
    public GameObject gameover;

    //Might wanna make this a list later on, deping if we want multiple previous decisions effecting dialog
    private DeterminDialog dialogScript;

    //Possible dialogs depending on previous decisions
    private List<string> _dialogText;

    //Yes dialogs
    private List<string> _yesText;

    //No dialogs
    private List<string> _noText;

    public float enterSpeed;
    public float stoppingPointX = 400f;
    public float endingPointX = 1200f;

    public float startingPointX = 1200f;
    public float startingPointY = 1200f;

    public float speechBubbleX;
    public float speechBubbleY;

    public float exitSpeed;
    
    public float amplitudeY;
    public float omegaY;
    
    public bool enter;
    public bool exit;

    private float index;
    private int currentTextIndex = 0;

    private GameObject speechInstance;

    private bool choicesOpen = false;
    private bool gameIsNowOver = false;

    private bool activated = false;

    private bool answeredYes = false;
    private bool answeredNo = false;

    //Which yes dialog are we currently on?
    private int currentYesTextIndex = 0;
    //Which no dialog are we currently on?
    private int currentNoTextIndex = 0;

    //Sound script
    private SoundDef soundScript;

    void Start()
    {
        dialogScript = GetComponent<DeterminDialog>();

        //Set up sound player
        soundScript = GameObject.FindGameObjectWithTag("SoundPlayer").GetComponent<SoundDef>();

        //Set up controller logic script
        controllerLogic = GameObject.Find("Controller");
    }

    public void Update()
    {
        if (Activated)
        {
            //Sine wave
            index += Time.deltaTime;
            float y = amplitudeY * Mathf.Sin(omegaY * index);
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + y);

            //Enter/exit
            if (enter && transform.localPosition.x > stoppingPointX)
            {
                moveLeft();
            }
            else if (enter)
            {
                startSpeak();
                enter = false;
            }
            else if (exit)
            {
                DestroyObject(speechInstance);
                moveRight();
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (choices && !choices.activeSelf && !enter && !exit && speechInstance.activeSelf)
                    changeSpeak();
                else if (!choices.activeSelf && !enter && !exit && speechInstance.activeSelf)
                    changeSpeak();
            }
        }
    }

    public void setup()
    {
        transform.localPosition = new Vector2(startingPointX, startingPointY);
        choicesOpen = false;
        enter = true;
    }

    private void startSpeak()
    {
        speechInstance = Instantiate(speech, new Vector2(0, 0), Quaternion.identity) as GameObject;
        speechInstance.transform.SetParent(GameObject.Find("GameCanvas").transform);
        speechInstance.transform.localPosition = new Vector2(speechBubbleX, speechBubbleY);

        //Get dialog
        _dialogText = dialogScript.GetDialog();

        speechInstance.transform.GetComponentInChildren<Text>().text = _dialogText[currentTextIndex];

        //Check if we show choices now
        if (speechInstance.transform.GetComponentInChildren<Text>().text.Contains(@"|c"))
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
    }

    public void changeSpeak()
    {
        //If we are displaying the chocies to the player, dont allow them to skip dialog
        if (choices.activeSelf) return;

        //Play the page turning sound
        soundScript.fire();

        //Check if the king just died
        if (gameIsNowOver)
        {
            gameover.SetActive(true);
            gameover.GetComponent<CurtainActivate>().startEndDay();

			GameObject.Find("MusicController").GetComponent<KDMoodMusicPlayer>().fadeOutAll();

            killSpeechAndChoices();
            
            return;
        }

        if(answeredYes)
        {
            if (currentYesTextIndex >= _yesText.Count - 1)
            {
                killSpeechAndChoices();
                exit = true;
                 
                //FlipCharacter();

                return;
            }
            else
                speechInstance.transform.GetComponentInChildren<Text>().text = _yesText[++currentYesTextIndex];
            return;
        }

        if (answeredNo)
        {
            if (currentNoTextIndex >= _noText.Count - 1)
            {
                killSpeechAndChoices();
                exit = true;

                //FlipCharacter();

                return;
            }
            else
                speechInstance.transform.GetComponentInChildren<Text>().text = _noText[++currentNoTextIndex];
            return;
        }

        if (currentTextIndex >= _dialogText.Count - 1)
        {
            killSpeechAndChoices();
            exit = true;

            //FlipCharacter();
            
            return;
        }
        
        speechInstance.transform.GetComponentInChildren<Text>().text = _dialogText[++currentTextIndex];

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

    }

    /// <summary>
    /// Flips character around when leaving the room
    /// </summary>
    private void FlipCharacter()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);

    }

    private bool checkForTags(string tag)
    {
        if (speechInstance.transform.GetComponentInChildren<Text>().text.Contains(tag))
        {
            return true;
        }
        return false;
    }

    private void removeTagFromText(int length)
    {
        //Ch string
        string temp = speechInstance.transform.GetComponentInChildren<Text>().text;
        string temp2 = temp.Remove(temp.Length - length, length);
        speechInstance.transform.GetComponentInChildren<Text>().text = temp2;
    }

    private void executePassiveOne()
    {
        GetComponent<ExecuteChoices>().executePassiveOneChoice();
    }

    private void executePassiveTwo()
    {
        GetComponent<ExecuteChoices>().executePassiveTwoChoice();
    }

    public void executeCantAffordSpeech()
    {
        if (activated)
        {
            if (dialogScript.cantAffordDialog == null)
            {
                Debug.LogError("dialogScript.cantAffordDialog is null for character -> " + transform.name);
            }
            else
            {
                speechInstance.transform.GetComponentInChildren<Text>().text = dialogScript.cantAffordDialog[0];
            }
            //Play the page turning sound
            soundScript.fire();
        }
    }

    public void showYesSpeech()
    {
        if (activated)
        {
            //Get yes/no dialog
            _yesText = dialogScript.SpeechYes;

            speechInstance.transform.GetComponentInChildren<Text>().text = dialogScript.SpeechYes[currentYesTextIndex];

            //Play the page turning sound
            soundScript.fire();

            answeredYes = true;

            if (checkForTags(@"|d"))
            {
                removeTagFromText(2);
                gameIsNowOver = true;
            }
        }
    }

    public void showNoSpeech()
    {
        if (activated)
        {
            //Get yes/no dialog
            _noText = dialogScript.SpeechNo;

            speechInstance.transform.GetComponentInChildren<Text>().text = dialogScript.SpeechNo[currentNoTextIndex];

            //Play the page turning sound
            soundScript.fire();

            answeredNo = true;

            if (checkForTags(@"|d"))
            {
                removeTagFromText(2);
                gameIsNowOver = true;
            }


        }
    }

    private void createChoices()
    {
        if (!choicesOpen)
        {
            choices.SetActive(true);
            choicesOpen = true;
        }
    }


    private void moveLeft()
    {
        transform.position = new Vector2(transform.position.x - (enterSpeed * Time.deltaTime), transform.position.y);

    }

    private void moveRight()
    {
        transform.position = new Vector2(transform.position.x + (exitSpeed * Time.deltaTime), transform.position.y);
        if (transform.position.x > endingPointX) {
            exit = false;
            killSelf();
        }
    }

    private void killSpeechAndChoices()
    {
        //if(speechInstance && speechInstance.active)  DestroyObject(speechInstance);
        //if(choicesInstance && choices.activeSelf) DestroyObject(choicesInstance);

        choices.SetActive(false);
        speechInstance.SetActive(false);

        //Play the page turning sound
        soundScript.fire();
    }

    public void killChoices()
    {
        //if (choicesInstance && choicesInstance.active) DestroyObject(choicesInstance);
        //choices.SetActive(false);
    }

    private void killSelf()
    {
        transform.localPosition = new Vector2(9999, 9999);
        currentTextIndex = 0;
        controllerLogic.GetComponent<ControllerLogic>().nextCharacter();

    }

    public bool Activated
    {
        get
        {
            return activated;
        }

        set
        {
            activated = value;
        }
    }

    public bool GameIsNowOver
    {
        get
        {
            return gameIsNowOver;
        }

        set
        {
            gameIsNowOver = value;
        }
    }
}
