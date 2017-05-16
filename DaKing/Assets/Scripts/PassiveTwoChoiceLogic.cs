using UnityEngine;
using System.Collections;
using System;

public class PassiveTwoChoiceLogic : IChoiceLogic
{

    public Transform ThisTransform { get { return _thisTransform; } set { _thisTransform = value; } }
    public SpeechBehaviour SpeechBehaviour { get { return _speechBehaviour; } set { _speechBehaviour = value; } }
    public PlayerAttributesLogic PlayerAttributes { get { return _playerAttributes; } set { _playerAttributes = value; } }
    public SpawnCoins SpawnCoins { get { return _spawnCoins; } set { _spawnCoins = value; } }

    private Transform _thisTransform;
    private SpeechBehaviour _speechBehaviour;
    private PlayerAttributesLogic _playerAttributes;
    private SpawnCoins _spawnCoins;

    public void ExecuteChoice(ExecuteChoices executeChoices)
    {
        _playerAttributes = executeChoices.PlayerAttributes.PlayerAttributesLogic;
        _speechBehaviour = executeChoices.SpeechBehaviour;
        _spawnCoins = executeChoices.SpawnCoins;
        _thisTransform = executeChoices.ThisTransform;

        // Handle Money
        if (executeChoices.CheckIfYouHaveEnoughResources(executeChoices.PassiveTwoMoneyOutcome, executeChoices.PassiveTwoMilitaryOutcome))
        {
            iTween.ValueTo(_thisTransform.gameObject, iTween.Hash("from", _playerAttributes.Money, "to", _playerAttributes.Money + executeChoices.PassiveTwoMoneyOutcome, "onupdate", "itweenCallback_ChangeMoney"));
            _spawnCoins.updateCoins(executeChoices.PassiveOneMoneyOutcome);

            // Handle Militry
            iTween.ValueTo(_thisTransform.gameObject, iTween.Hash("from", _playerAttributes.Military, "to", _playerAttributes.Military + executeChoices.PassiveTwoMilitaryOutcome, "onupdate", "itweenCallback_ChangeMilitary"));

            // // Handle depression
            int moodOutcome = _playerAttributes.Mood + executeChoices.PassiveTwoMoodOutcome;
            if (moodOutcome > _playerAttributes.MaxMood) moodOutcome = _playerAttributes.MaxMood;
            if (moodOutcome <= 0) _speechBehaviour.SpeechLogic.GameIsNowOver = true;

            if (moodOutcome != _playerAttributes.Mood)
                executeChoices.AnimateMoodValueChanging(moodOutcome);

            //Display flash text
            _playerAttributes.FlashTextValues(executeChoices.PassiveTwoMoneyOutcome, executeChoices.PassiveTwoMilitaryOutcome, executeChoices.PassiveTwoMoodOutcome);
        }
        //Not enough resource
        else
        {
            executeChoices.AnimateMoodValueChanging(_playerAttributes.Mood - 5);

            //Display flash text
            _playerAttributes.FlashTextValues(0, 0, -5);
        }
    }
}
