﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoubleOptionDialog : DeterminDialog {

    public List<string> dialogOption2;
    public ExecuteChoicesBehaviour previousCharactersDecision;

    public override List<string> GetDialog()
    {
        if (previousCharactersDecision != null)
            Debug.Log("ERROR: No previous character reference on " + this.gameObject.name + " on Double Option script");

        //if yes
        if (previousCharactersDecision.ExecuteChoices.outcomeChoice)
        {
            return dialogOption1;
        }
        //if no
        if (!previousCharactersDecision.ExecuteChoices.outcomeChoice)
        {
            return dialogOption2;
        }

        return null;
    }
}
