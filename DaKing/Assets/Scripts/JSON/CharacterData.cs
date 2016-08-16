using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class CharacterData 
{
    public string charName;
    public string charType;
    public string dependentCharName;

    public List<string> lstDialogOne;
    public List<string> lstDialogTwo;

    public List<string> lstOutcomeYesText;
    public List<string> lstOutcomeNoText;

    public List<string> lstCantAffordText;

    public List<int> lstOutcomeYesResult;
    public List<int> lstOutcomeNoResult;
    public List<int> lstOutcomePassiveResultOne;
    public List<int> lstOutcomePassiveResultTwo;
}