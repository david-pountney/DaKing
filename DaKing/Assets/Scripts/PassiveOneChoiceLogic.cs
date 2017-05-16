using UnityEngine;
using System.Collections;
using System;

public class PassiveOneChoiceLogic : IChoiceLogic {

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

        int moneyAmount = executeChoices.PassiveOneMoneyOutcome;
        int militaryAmount = executeChoices.PassiveOneMilitaryOutcome;
        int moodAmount = executeChoices.PassiveOneMoodOutcome;

        //If they are any special values to represent the player loosing all of their money e.g. we check here
        executeChoices.CheckForSpecialValues(ref moneyAmount, ref militaryAmount, ref moodAmount);

        // Handle Money
        if (executeChoices.CheckIfYouHaveEnoughResources(executeChoices.PassiveOneMoneyOutcome, executeChoices.PassiveOneMilitaryOutcome))
        {
            iTween.ValueTo(_thisTransform.gameObject, iTween.Hash("from", _playerAttributes.Money, "to", _playerAttributes.Money + moneyAmount, "onupdate", "itweenCallback_ChangeMoney"));
            _spawnCoins.updateCoins(moneyAmount);

            // Handle Militry
            iTween.ValueTo(_thisTransform.gameObject, iTween.Hash("from", _playerAttributes.Military, "to", _playerAttributes.Military + militaryAmount, "onupdate", "itweenCallback_ChangeMilitary"));

            // // Handle depression
            int moodOutcome = _playerAttributes.Mood + moodAmount;
            if (moodOutcome > _playerAttributes.MaxMood) moodOutcome = _playerAttributes.MaxMood;
            if (moodOutcome <= 0) _speechBehaviour.SpeechLogic.GameIsNowOver = true;

            if (moodOutcome != _playerAttributes.Mood)
                executeChoices.AnimateMoodValueChanging(moodOutcome);

            //Display flash text
            _playerAttributes.FlashTextValues(moneyAmount, militaryAmount, moodAmount);
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
