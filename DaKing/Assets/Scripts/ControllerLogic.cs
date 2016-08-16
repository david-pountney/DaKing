using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ControllerLogic : MonoBehaviour {

    public List<GameObject> _listOfCharacters;

    //The index of the current character in the list of characters
    private int currentCharIndex;

    //The current character object in the list of characters
    public MovementForChars currentChar;

    public GameObject musicController;

    //Current day number
    private int dayNumber = 0;

    //This is the character we are bringing on the screen
    private MovementForChars characterChild;

    // Use this for initialization
    void Start () {

        musicController.GetComponent<SimpleMusicController>().fade_in();

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
            if (character.tag == "NextDay")
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
                    }
                    if (moveScript)
                        moveScript.Activated = true;

                    characterChild.GetComponent<MovementForChars>().setup();
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
