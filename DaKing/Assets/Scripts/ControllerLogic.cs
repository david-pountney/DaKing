using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ControllerLogic {

    public GameObject MusicController { get { return _musicController; } set { _musicController = value; } }
    public MoodDisplayScript MoodDisplay { get { return _moodDisplay; } set { _moodDisplay = value; } }

    public List<GameObject> _listOfCharacters;

    //The current character object in the list of characters
    public MovementBehaviour currentChar;

    //The index of the current character in the list of characters
    private int currentCharIndex = 0;
    
    private GameObject _musicController;

    //Current day number
    private int dayNumber = 0;

    //This is the character we are bringing on the screen
    private MovementBehaviour characterChild;

    private MoodDisplayScript _moodDisplay;

    public void GetAllCharacters()
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
        _musicController.GetComponent<SimpleMusicController>().fade_in();

        nextCharacter();
    }

    public void nextCharacter()
    {
        GameObject character = null;
        MovementBehaviour moveScript = null;

        //Set character we have just finished with to be inactive
        if (CurrentChar && CurrentChar.GetComponent<MovementBehaviour>())
            CurrentChar.gameObject.SetActive(false);

        if (_listOfCharacters.Count > currentCharIndex)
        {
            character = _listOfCharacters[currentCharIndex++];

            //Check if end of characters for day
            if (character.name == "NextDayMarker")
            {
                //We are now onto the next day
                DayNumber++;
                //Start fading next day
                GlobalReferencesBehaviour.instance.SceneData.nextDay.GetComponent<CurtainActivate>().startEndDay();
            }
            //Else just set up the next character to walk in
            else
            {
                //Null check
                if (character.GetComponent<ChooseCharacterScript>())
                {
                    //Here we get the actual character who will enter the room from the list of children the 'character' object has
                    characterChild = character.GetComponent<ChooseCharacterScript>().ChooseCharacter();
                    characterChild.gameObject.SetActive(true);

                    if (characterChild)
                    {
                        CurrentChar = characterChild.GetComponent<MovementBehaviour>();
                        moveScript = characterChild.GetComponent<MovementBehaviour>();
                    }
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

    public MovementBehaviour CurrentChar
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
