﻿using UnityEngine;
using System.Collections;

/// <summary>
/// This script is just to determin which character out of the objects children should be chosen
/// for this 'day' 
/// Decision is bassed off ExecuteChoices.outcomeChoice yes or no boolean
/// </summary>
public class ChooseCharacterScript : MonoBehaviour {

    //The previous choice that determins which character is shown
    public ExecuteChoices theChoice;

	// Use this for initialization
	void Start () {
	
	}
	
    /// <summary>
    /// Call this method to decide which character is chosen based on a previous choice the player has made
    /// </summary>
    public MovementForChars ChooseCharacter()
    {
        
        Transform child1 = null;
        Transform child2 = null;

        if(transform.childCount > 0)
            child1 = transform.GetChild(0);

        if (transform.childCount > 1)
            child2 = transform.GetChild(1);

        if (!child1)
        {
            Debug.LogError("You forgot to set one of the character objects ( " + gameObject.name + " ) children! ( child at index 0 )");
            return null;
        }
        //There is no previous choice effecting which character we pick, so just pick the first child
        if (!theChoice)
            return child1.GetComponent<MovementForChars>();

        //We always assume a previous 'yes' choice means the first child, otherwise the second child
        //If 'yes'...
        if (theChoice.outcomeChoice)
        {
            if (child1)
                return child1.GetComponent<MovementForChars>();
        }
        else
        {
            child2 = transform.GetChild(1);

            if (child2)
                return child2.GetComponent<MovementForChars>();
        }
        
        return null;
        
    }
}
