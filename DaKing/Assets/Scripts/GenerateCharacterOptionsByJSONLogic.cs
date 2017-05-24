using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class GenerateCharacterOptionsByJSONLogic {

    public List<CharacterOptionData> LstCharacterOptionData { get { return _lstCharacterOptionData; } set { _lstCharacterOptionData = value; } }
    public CharacterOptionData CurrentCharacterOption { get { return _currentCharacterOption; } set { _currentCharacterOption = value; } }
    public JSONManagerBehaviour ResourceManager { get { return _jsonManagerBehaviour; } set { _jsonManagerBehaviour = value; } }
    public bool DevMode { get { return _devMode; } set { _devMode = value; } }

    private List<CharacterOptionData> _lstCharacterOptionData;
    private CharacterOptionData _currentCharacterOption;
    private JSONManagerBehaviour _jsonManagerBehaviour;

    private IEnumerable<ChooseCharacterScript> chooseCharacterScripts;

    private bool _devMode = false;

    public void Init()
    {
        PrintToConsole("GameMaster.Start()");

        PrintToConsole("Game master init");

        _lstCharacterOptionData = new List<CharacterOptionData>();
        int i = 0;

        PrintToConsole("lstCharacterOptionJsonData Count: " + _jsonManagerBehaviour._jsonManagerLogic.LstJsonCharacterData.Count);
        foreach (string jsonFile in _jsonManagerBehaviour._jsonManagerLogic.LstJsonCharacterOptionsData)
        {
            PrintToConsole("jsonFile:" + jsonFile);
            _lstCharacterOptionData.Add(JsonUtility.FromJson<CharacterOptionData>(jsonFile));

            CurrentCharacterOption = _lstCharacterOptionData[i++];

            //Check if character is in the scene
            PrintToConsole("loading character option with name: '" + CurrentCharacterOption.charName + '"');

            List<ChooseCharacterScript> chooseCharacterScripts = new List<ChooseCharacterScript>();
            GlobalReferencesBehaviour.instance.SceneData.characterContainer.GetComponentsInChildren<ChooseCharacterScript>(false, chooseCharacterScripts);

            ChooseCharacterScript currentCharacter = (from go in chooseCharacterScripts
                                                      where go.gameObject.name.Equals(CurrentCharacterOption.charName, StringComparison.CurrentCultureIgnoreCase)
                                                      select go).FirstOrDefault();

            if (!currentCharacter)
            {
                PrintToConsole("Skiping character as it is not in the scene");
                continue;
            }

            if(CurrentCharacterOption.previousCharacterDecisionName != null)
                currentCharacter.PreviouscharacterChoice = GetChooseCharacterScript();

            PrintToConsole("Character loaded.");
        }
    }

    private ExecuteChoicesBehaviour GetChooseCharacterScript()
    {
        MovementBehaviour currentCharacter = (from go in GlobalReferencesBehaviour.instance.SceneData.Characters
                                              where go.gameObject.name == CurrentCharacterOption.previousCharacterDecisionName
                                              select go).FirstOrDefault();

        return currentCharacter.GetComponent<ExecuteChoicesBehaviour>();

    }

    private void PrintToConsole(string text)
    {
        if (!_devMode) return;

        Debug.Log(text);
    }
}
