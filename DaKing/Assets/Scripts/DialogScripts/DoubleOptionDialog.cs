using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoubleOptionDialog : DeterminDialog {

    public List<string> dialogOption2;
    private ExecuteChoices previousCharactersDecision;

    public override List<string> GetDialog()
    {
        if(transform.parent.GetComponent<ChooseCharacterScript>().theCharacter)
            previousCharactersDecision = transform.parent.GetComponent<ChooseCharacterScript>().theCharacter.GetComponent<ExecuteChoices>();

        //if yes
        if (previousCharactersDecision.outcomeChoice)
        {
            return dialogOption1;
        }
        //if no
        if (!previousCharactersDecision.outcomeChoice)
        {
            return dialogOption2;
        }

        return null;
    }
}
