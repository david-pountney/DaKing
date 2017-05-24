using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GenerateCharactersByJSONLogic {

    public List<CharacterData> LstCharData { get { return _lstCharData; } set { _lstCharData = value; } }
    public CharacterData CurrentChar { get { return _currentChar; } set { _currentChar = value; } }
    public JSONManagerBehaviour ResourceManager { get { return _jsonManagerBehaviour; } set { _jsonManagerBehaviour = value; } }
    public int SuperSoldierNeeded { get { return _superSoldiersNeeded; } set { _superSoldiersNeeded = value; } }
    public int SuperSoldierCount { get { return _superSoldiersCount; } set { _superSoldiersCount = value; } }
    public bool DevMode { get { return _devMode; } set { _devMode = value; } }
    
    private List<CharacterData> _lstCharData;
    private CharacterData _currentChar;
    private JSONManagerBehaviour _jsonManagerBehaviour; 

    private int _superSoldiersCount;
    private int _superSoldiersNeeded;

    private bool _devMode = false;

    public void Init()
    {
        PrintToConsole("GameMaster.Start()");

        PrintToConsole("Game master init");

        _lstCharData = new List<CharacterData>();
        int i = 0;
        _superSoldiersCount = 0;
        _superSoldiersNeeded = 4;
        
        PrintToConsole("lstJsonData Count: " + _jsonManagerBehaviour._jsonManagerLogic.LstJsonCharacterData.Count);
        foreach (string jsonFile in _jsonManagerBehaviour._jsonManagerLogic.LstJsonCharacterData)
        {
            PrintToConsole("jsonFile:"+jsonFile);
            _lstCharData.Add(JsonUtility.FromJson<CharacterData>(jsonFile));

            CurrentChar = _lstCharData[i++];

            //Check if character is in the scene
            PrintToConsole("loading character with name: '"+CurrentChar.charName+'"');

            MovementBehaviour currentCharacter = (from go in GlobalReferencesBehaviour.instance.SceneData.Characters
                              where go.gameObject.name == CurrentChar.charName
                              select go).FirstOrDefault();
                              
            if (!currentCharacter) {
                PrintToConsole("Skiping character as it is not in the scene");
                continue;
            }

            //Get the character in the scene via the name
            DeterminDialog charInstance = currentCharacter.GetComponent<DeterminDialog>();

            //Error check
            if (!charInstance) Debug.LogError("Character name not found in scene, please check the name in the JSON file and make sure said character is in the scene. Also check that there is a determinDialog component attached to said character.");

            //If there is a dependent char
            if (CurrentChar.dependentCharName != null)
            {
                PrintToConsole("Character has dependent character, assigning theChoice");
                charInstance.theChoice = GameObject.Find(CurrentChar.dependentCharName).GetComponent<ExecuteChoicesBehaviour>();
            }

            if (CurrentChar.lstDialogOne.Count > 0)
            {
                PrintToConsole("Adding dialog one");
                charInstance.DialogOption1 = CurrentChar.lstDialogOne;
            }

            if (CurrentChar.lstDialogTwo.Count > 0)
            {
                PrintToConsole("Adding dialog two");
                //Get the chars double dialog component
                DoubleOptionDialog doubleCharInstance = currentCharacter.GetComponent<DoubleOptionDialog>();
                if (!doubleCharInstance) Debug.LogError("DoubleOptionDialog component not found on character.");
                doubleCharInstance.dialogOption2 = CurrentChar.lstDialogTwo;
            }

            //Yes speech
            if (CurrentChar.lstOutcomeYesText.Count > 0)
            {
                PrintToConsole("Adding yes outcome speech");
                charInstance.speechYes = CurrentChar.lstOutcomeYesText;
            }
            //No speech
            if (CurrentChar.lstOutcomeNoText.Count > 0)
            {
                PrintToConsole("Adding no outcome speech");
                charInstance.speechNo = CurrentChar.lstOutcomeNoText;
            }
            //Cant afford speech
            if (CurrentChar.lstCantAffordText.Count > 0)
            {
                PrintToConsole("Adding cant afford speech");
                charInstance.cantAffordDialog = CurrentChar.lstCantAffordText;
            }

            //Yes outcome result
            if (CurrentChar.lstOutcomeYesResult.Count > 0)
            {
                PrintToConsole("Adding yes result text");
                charInstance.theChoice.ExecuteChoices.YesMoneyOutcome = CurrentChar.lstOutcomeYesResult[0];
                charInstance.theChoice.ExecuteChoices.YesMilitaryOutcome = CurrentChar.lstOutcomeYesResult[1];
                charInstance.theChoice.ExecuteChoices.YesMoodOutcome = CurrentChar.lstOutcomeYesResult[2];
            }

            //No outcome result
            if (CurrentChar.lstOutcomeNoResult.Count > 0)
            {
                PrintToConsole("Adding no result text");
                charInstance.theChoice.ExecuteChoices.NoMoneyOutcome = CurrentChar.lstOutcomeNoResult[0];
                charInstance.theChoice.ExecuteChoices.NoMilitaryOutcome = CurrentChar.lstOutcomeNoResult[1];
                charInstance.theChoice.ExecuteChoices.NoMoodOutcome = CurrentChar.lstOutcomeNoResult[2];
            }

            //Passive result one
            if (CurrentChar.lstOutcomePassiveResultOne.Count > 0)
            {
                PrintToConsole("Adding passive result 1 text");
                charInstance.theChoice.ExecuteChoices.PassiveOneMoneyOutcome = CurrentChar.lstOutcomePassiveResultOne[0];
                charInstance.theChoice.ExecuteChoices.PassiveOneMilitaryOutcome = CurrentChar.lstOutcomePassiveResultOne[1];
                charInstance.theChoice.ExecuteChoices.PassiveOneMoodOutcome = CurrentChar.lstOutcomePassiveResultOne[2];
            }

            //Passive result two
            if (CurrentChar.lstOutcomePassiveResultTwo.Count > 0)
            {
                PrintToConsole("Adding passive result 2 text");
                charInstance.theChoice.ExecuteChoices.PassiveTwoMoneyOutcome = CurrentChar.lstOutcomePassiveResultTwo[0];
                charInstance.theChoice.ExecuteChoices.PassiveTwoMilitaryOutcome = CurrentChar.lstOutcomePassiveResultTwo[1];
                charInstance.theChoice.ExecuteChoices.PassiveTwoMoodOutcome = CurrentChar.lstOutcomePassiveResultTwo[2];
            }

            if(CurrentChar.previousCharacterDecisionName != null)
            {
                DoubleOptionDialog dod = charInstance as DoubleOptionDialog;
                if (dod)
                    dod.previousCharactersDecision = GetCharacterChoice();
            }

            PrintToConsole("Character loaded.");
            
            currentCharacter.gameObject.SetActive(false);


        }
    }

    private ExecuteChoicesBehaviour GetCharacterChoice()
    {
        MovementBehaviour currentCharacter = (from go in GlobalReferencesBehaviour.instance.SceneData.Characters
                                              where go.gameObject.name == CurrentChar.previousCharacterDecisionName
                                              select go).FirstOrDefault();

        return currentCharacter.GetComponent<ExecuteChoicesBehaviour>();

    }

    private void PrintToConsole(string text)
    {
        if (!_devMode) return;

        Debug.Log(text);
    }
}