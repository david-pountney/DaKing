using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ChoicesScript : MonoBehaviour {

    public void yesButtonClicked()
    {
        MovementForChars character = GameObject.FindGameObjectWithTag("Controller").GetComponent<ControllerLogic>().CurrentChar; 
        
        if(character)
            character.GetComponent<ExecuteChoices>().executeYesChoice();
        
        gameObject.SetActive(false);

    }

    public void noButtonClicked()
    {
        MovementForChars character = GameObject.FindGameObjectWithTag("Controller").GetComponent<ControllerLogic>().CurrentChar;

        if (character)
            character.GetComponent<ExecuteChoices>().executeNoChoice();
        
        gameObject.SetActive(false);
    }
}
