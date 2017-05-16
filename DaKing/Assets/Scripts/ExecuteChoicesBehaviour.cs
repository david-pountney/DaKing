using UnityEngine;
using System.Collections;
using System;

public class ExecuteChoicesBehaviour : MonoBehaviour {

    public ExecuteChoices ExecuteChoices { get { return _executeChoices; } set { _executeChoices = value; } }

    private ExecuteChoices _executeChoices;

    void Awake()
    {
        _executeChoices = new ExecuteChoices();
    }

    void Start()
    {
        SetUp();
    }

    private void SetUp()
    {
        _executeChoices.ThisTransform = this.transform;
        _executeChoices.PlayerAttributes = GlobalReferencesBehaviour.instance.SceneData.playerAttributes;
        _executeChoices.MovementForChars = GetComponent<MovementBehaviour>();
        _executeChoices.SpeechBehaviour = GetComponent<SpeechBehaviour>();
        _executeChoices.SpawnCoins = GlobalReferencesBehaviour.instance.SceneData.controller.GetComponent<SpawnCoins>();
    }

    private void itweenCallback_ChangeMilitary(int newVal)
    {
        _executeChoices.PlayerAttributes.PlayerAttributesLogic.SetMilitary(newVal);
    }

    private void itweenCallback_ChangeMoney(int newVal)
    {
        _executeChoices.PlayerAttributes.PlayerAttributesLogic.SetMoney(newVal);
    }

    private void itweenCallback_ChangeMood(int newVal)
    {
        _executeChoices.PlayerAttributes.PlayerAttributesLogic.SetMood(newVal);
    }

    private void itweenCallback_CompleteMood()
    {
        
    }
}
