using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeterminDialog : MonoBehaviour {

    public ExecuteChoicesBehaviour theChoice;
    public List<string> dialogOption1;
    public List<string> speechYes;
    public List<string> speechNo;
    public List<string> cantAffordDialog;

    void Awake()
    {
        theChoice = GetComponent<ExecuteChoicesBehaviour>();
    }

    public virtual List<string> GetDialog()
    {
        return null;
    }

    public List<string> SpeechYes
    {
        get
        {
            return speechYes;
        }

        set
        {
            speechYes = value;
        }
    }

    public List<string> SpeechNo
    {
        get
        {
            return speechNo;
        }

        set
        {
            speechNo = value;
        }
    }

    public List<string> DialogOption1
    {
        get
        {
            return dialogOption1;
        }

        set
        {
            dialogOption1 = value;
        }
    }
}
