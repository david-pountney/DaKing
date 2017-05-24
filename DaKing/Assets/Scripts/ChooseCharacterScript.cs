using UnityEngine;
using System.Collections;

/// <summary>
/// This script is just to determin which character out of the objects children should be chosen
/// for this 'day' 
/// Decision is bassed off ExecuteChoices.outcomeChoice yes or no boolean
/// </summary>
public class ChooseCharacterScript : MonoBehaviour {

    public ExecuteChoicesBehaviour PreviouscharacterChoice { get { return _previousCharacterChoice; } set { _previousCharacterChoice = value; } }

    //Which character in another Character objects children are we basing this decision on?
    public GameObject theCharacter;

    //Get the script that contains the 'yes'/'no' decision a character made
    [SerializeField]
    private ExecuteChoicesBehaviour _previousCharacterChoice;

    /// <summary>
    /// Call this method to decide which character is chosen based on a previous choice the player has made
    /// </summary>
    public MovementBehaviour ChooseCharacter()
    {
        Transform child1 = null;
        Transform child2 = null;

        if(transform.childCount > 0)
            child1 = transform.GetChild(0);

        if (transform.childCount > 1)
            child2 = transform.GetChild(1);

        if (!child1)
        {
            Debug.Log("You forgot to set one of the character objects ( " + gameObject.name + " ) children! ( child at index 0 )");
            return null;
        }
        //There is no previous choice effecting which character we pick, so just pick the first child
        if (!_previousCharacterChoice)
            return child1.GetComponent<MovementBehaviour>();

        //We always assume a previous 'yes' choice means the first child, otherwise the second child
        //If 'yes'...
        if (_previousCharacterChoice.ExecuteChoices.outcomeChoice)
        {
            if (child1)
                return child1.GetComponent<MovementBehaviour>();
        }
        else
        //if answer was no
        {
            if (child2)
                return child2.GetComponent<MovementBehaviour>();
            else
                return child1.GetComponent<MovementBehaviour>();
        }
        
        return null;
        
    }
}
