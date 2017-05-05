using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameMaster : MonoBehaviour {

    public List<CharacterData> lstCharData;

    private CharacterData currentChar;

    [SerializeField]
    private int superSoldiersCount;
    public int superSoldiersNeeded;

	// Use this for initialization
	void Start () {

	}

    public void Init()
    {
        //Debug.Log("GameMaster.Start()");
        Debug.Log("Game master init");

        lstCharData = new List<CharacterData>();
        int i = 0;
        superSoldiersCount = 0;
        superSoldiersNeeded = 4;

        Debug.Log("lstJsonData Count: " + ResourceManager.instance.lstJsonData.Count);
        foreach (string jsonFile in ResourceManager.instance.lstJsonData)
        {
            Debug.Log("jsonFile:"+jsonFile);
            lstCharData.Add(JsonUtility.FromJson<CharacterData>(jsonFile));

            currentChar = lstCharData[i++];

            //Check if character is in the scene
            Debug.Log("loading character with name: '"+currentChar.charName+'"');

            MovementBehaviour currentCharacter = (from go in GlobalReferencesBehaviour.instance.SceneData.Characters
                              where go.gameObject.name == currentChar.charName
                              select go).FirstOrDefault();
                              
            if (!currentCharacter) {
                Debug.Log("Skiping character as it is not in the scene");
                continue;
            }

            //Get the character in the scene via the name
            DeterminDialog charInstance = currentCharacter.GetComponent<DeterminDialog>();

            //Error check
            if (!charInstance) Debug.LogError("Character name not found in scene, please check the name in the JSON file and make sure said character is in the scene. Also check that there is a determinDialog component attached to said character.");

            //If there is a dependent char
            if (currentChar.dependentCharName != null)
            {
                Debug.Log("Character has dependent character, assigning theChoice");
                charInstance.theChoice = GameObject.Find(currentChar.dependentCharName).GetComponent<ExecuteChoices>();
            }

            if (currentChar.lstDialogOne.Count > 0)
            {
                Debug.Log("Adding dialog one");
                charInstance.DialogOption1 = currentChar.lstDialogOne;
            }

            if (currentChar.lstDialogTwo.Count > 0)
            {
                Debug.Log("Adding dialog two");
                //Get the chars double dialog component
                DoubleOptionDialog doubleCharInstance = currentCharacter.GetComponent<DoubleOptionDialog>();
                if (!doubleCharInstance) Debug.LogError("DoubleOptionDialog component not found on character.");
                doubleCharInstance.dialogOption2 = currentChar.lstDialogTwo;
            }

            //Yes speech
            if (currentChar.lstOutcomeYesText.Count > 0)
            {
                Debug.Log("Adding yes outcome speech");
                charInstance.speechYes = currentChar.lstOutcomeYesText;
            }
            //No speech
            if (currentChar.lstOutcomeNoText.Count > 0)
            {
                Debug.Log("Adding no outcome speech");
                charInstance.speechNo = currentChar.lstOutcomeNoText;
            }
            //Cant afford speech
            if (currentChar.lstCantAffordText.Count > 0)
            {
                Debug.Log("Adding cant afford speech");
                charInstance.cantAffordDialog = currentChar.lstCantAffordText;
            }

            //Yes outcome result
            if (currentChar.lstOutcomeYesResult.Count > 0)
            {
                Debug.Log("Adding yes result text");
                charInstance.theChoice.yesMoneyOutcome = currentChar.lstOutcomeYesResult[0];
                charInstance.theChoice.yesMilitaryOutcome = currentChar.lstOutcomeYesResult[1];
                charInstance.theChoice.yesDepressionOutcome = currentChar.lstOutcomeYesResult[2];
            }

            //No outcome result
            if (currentChar.lstOutcomeNoResult.Count > 0)
            {
                Debug.Log("Adding no result text");
                charInstance.theChoice.noMoneyOutcome = currentChar.lstOutcomeNoResult[0];
                charInstance.theChoice.noMilitaryOutcome = currentChar.lstOutcomeNoResult[1];
                charInstance.theChoice.noDepressionOutcome = currentChar.lstOutcomeNoResult[2];
            }

            //Passive result one
            if (currentChar.lstOutcomePassiveResultOne.Count > 0)
            {
                Debug.Log("Adding passive result 1 text");
                charInstance.theChoice.passiveOneMoneyOutcome = currentChar.lstOutcomePassiveResultOne[0];
                charInstance.theChoice.passiveOneMilitaryOutcome = currentChar.lstOutcomePassiveResultOne[1];
                charInstance.theChoice.passiveOneDepressionOutcome = currentChar.lstOutcomePassiveResultOne[2];
            }

            //Passive result two
            if (currentChar.lstOutcomePassiveResultTwo.Count > 0)
            {
                Debug.Log("Adding passive result 2 text");
                charInstance.theChoice.passiveTwoMoneyOutcome = currentChar.lstOutcomePassiveResultTwo[0];
                charInstance.theChoice.passiveTwoMilitaryOutcome = currentChar.lstOutcomePassiveResultTwo[1];
                charInstance.theChoice.passiveTwoDepressionOutcome = currentChar.lstOutcomePassiveResultTwo[2];
            }
            Debug.Log("Character loaded.");

            currentCharacter.gameObject.SetActive(false);
        }
    }

    public void addSoldierCount()
    {
        superSoldiersCount += 1;
    }
    public int getSoldireCount()
    {
        return superSoldiersCount;
    }
}