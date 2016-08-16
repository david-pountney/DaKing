using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OneOptionDialog : DeterminDialog
{
    public override List<string> GetDialog()
    {
        return dialogOption1;
    }
}
