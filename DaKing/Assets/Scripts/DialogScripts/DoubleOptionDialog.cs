using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoubleOptionDialog : DeterminDialog {

    public List<string> dialogOption2;

    public override List<string> GetDialog()
    {
        //if yes
        if(theChoice.outcomeChoice)
        {
            return dialogOption1;
        }
        //if no
        if (!theChoice.outcomeChoice)
        {
            return dialogOption2;
        }

        return null;
    }
}
