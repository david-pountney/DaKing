using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ControllerLogic : MonoBehaviour {

    public List<GameObject> _listOfCharacters;

    //The index of the current character in the list of characters
    private int currentCharIndex;

    //The current character object in the list of characters
    public MovementForChars currentChar;

    private GameObject musicController;

    //Current day number
    private int dayNumber = 0;

    //This is the character we are bringing on the screen
    private MovementForChars characterChild;

    // Use this for initialization
    void Awake () {
        musicController = GameObject.Find("MusicControllerMood");
        currentCharIndex = 0;
        GetAllCharacters();
    }
    
    void OnLevelWasLoaded()
    {
        musicController = GameObject.Find("MusicControllerMood");
        currentCharIndex = 0;
        GetAllCharacters();
    }

    private void GetAllCharacters()
    {
        GameObject characterContainer = GameObject.Find("Characters");
        GameObject nextDay = GameObject.Find("NextDayMarker");

        _listOfCharacters = new List<GameObject>();

        int i = 0;
        foreach (Transform child in characterContainer.transform)
        {
            if(i % 5 == 0 && i != 0) _listOfCharacters.Add(nextDay);
            _listOfCharacters.Add(child.gameObject);

            i++;
        }
    }



    public void Init()
    {
		// ensure music is reset
		//musicController.GetComponent<KDMoodMusicPlayer>().stopAll ();
		musicController.GetComponent<KDMoodMusicPlayer>().startAll ();
		musicController.GetComponent<KDMoodMusicPlayer>().fadeInTrack(0);

        Debug.Log(ResourceManager.instance.getPlayerAttributes().depression);

        PlayerAttributes playerAttributes = GameObject.FindGameObjectWithTag("King").GetComponent<PlayerAttributes>();

        //Init default values of mood
        MoodDisplayScript.instance.handleMood(playerAttributes.depression);

        nextCharacter();
    }

    public void nextCharacter()
    {
        GameObject character = null;
        MovementForChars moveScript = null;

        //Set character we have just finished with to be inactive
        if (CurrentChar && CurrentChar.GetComponent<MovementForChars>())
            CurrentChar.GetComponent<MovementForChars>().Activated = false;

        if (_listOfCharacters.Count > currentCharIndex)
        {
            character = _listOfCharacters[currentCharIndex++];

            //Check if end of characters for day
            if (character.name == "NextDayMarker")
            {
                //We are now onto the next day
                DayNumber++;
                //Start fading next day
                GameObject.FindGameObjectWithTag("NextDay").GetComponent<CurtainActivate>().startEndDay();
            }
            //Else just set up the next character to walk in
            else
            {
                //Null check
                if (character.GetComponent<ChooseCharacterScript>())
                {
                    //Here we get the actual character who will enter the room from the list of children the 'character' object has
                    characterChild = character.GetComponent<ChooseCharacterScript>().ChooseCharacter();

                    if (characterChild)
                    {
                        CurrentChar = characterChild.GetComponent<MovementForChars>();
                        moveScript = characterChild.GetComponent<MovementForChars>();
                        characterChild.GetComponent<MovementForChars>().setup();
                    }
                    if (moveScript)
                        moveScript.Activated = true;

                }
            }
        }
    }

    public int DayNumber
    {
        get
        {
            return dayNumber;
        }

        set
        {
            dayNumber = value;
        }
    }

    public MovementForChars CurrentChar
    {
        get
        {
            return currentChar;
        }

        set
        {
            currentChar = value;
        }
    }
}
