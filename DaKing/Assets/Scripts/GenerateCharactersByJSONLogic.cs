using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GenerateCharactersByJSONLogic {

    public List<CharacterData> LstCharData { get { return _lstCharData; } set { _lstCharData = value; } }
    public CharacterData CurrentChar { get { return _currentChar; } set { _currentChar = value; } }
    public JSONManagerBehaviour ResourceManager { get { return _resourceManager; } set { _resourceManager = value; } }
    public int SuperSoldierNeeded { get { return _superSoldiersNeeded; } set { _superSoldiersNeeded = value; } }
    public int SuperSoldierCount { get { return _superSoldiersCount; } set { _superSoldiersCount = value; } }
    public bool DevMode { get { return _devMode; } set { _devMode = value; } }
    
    private List<CharacterData> _lstCharData;
    private CharacterData _currentChar;
    private JSONManagerBehaviour _resourceManager; 

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
        
        PrintToConsole("lstJsonData Count: " + _resourceManager._resourceManagerLogic.LstJsonData.Count);
        foreach (string jsonFile in _resourceManager._resourceManagerLogic.LstJsonData)
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
                charInstance.theChoice = GameObject.Find(CurrentChar.dependentCharName).GetComponent<ExecuteChoices>();
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
                charInstance.theChoice.yesMoneyOutcome = CurrentChar.lstOutcomeYesResult[0];
                charInstance.theChoice.yesMilitaryOutcome = CurrentChar.lstOutcomeYesResult[1];
                charInstance.theChoice.yesDepressionOutcome = CurrentChar.lstOutcomeYesResult[2];
            }

            //No outcome result
            if (CurrentChar.lstOutcomeNoResult.Count > 0)
            {
                PrintToConsole("Adding no result text");
                charInstance.theChoice.noMoneyOutcome = CurrentChar.lstOutcomeNoResult[0];
                charInstance.theChoice.noMilitaryOutcome = CurrentChar.lstOutcomeNoResult[1];
                charInstance.theChoice.noDepressionOutcome = CurrentChar.lstOutcomeNoResult[2];
            }

            //Passive result one
            if (CurrentChar.lstOutcomePassiveResultOne.Count > 0)
            {
                PrintToConsole("Adding passive result 1 text");
                charInstance.theChoice.passiveOneMoneyOutcome = CurrentChar.lstOutcomePassiveResultOne[0];
                charInstance.theChoice.passiveOneMilitaryOutcome = CurrentChar.lstOutcomePassiveResultOne[1];
                charInstance.theChoice.passiveOneDepressionOutcome = CurrentChar.lstOutcomePassiveResultOne[2];
            }

            //Passive result two
            if (CurrentChar.lstOutcomePassiveResultTwo.Count > 0)
            {
                PrintToConsole("Adding passive result 2 text");
                charInstance.theChoice.passiveTwoMoneyOutcome = CurrentChar.lstOutcomePassiveResultTwo[0];
                charInstance.theChoice.passiveTwoMilitaryOutcome = CurrentChar.lstOutcomePassiveResultTwo[1];
                charInstance.theChoice.passiveTwoDepressionOutcome = CurrentChar.lstOutcomePassiveResultTwo[2];
            }
            PrintToConsole("Character loaded.");

            currentCharacter.gameObject.SetActive(false);
        }
    }

    private void PrintToConsole(string text)
    {
        if (!_devMode) return;

        Debug.Log(text);
    }
}