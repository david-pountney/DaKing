using UnityEngine;
using System.Collections;
using System;

public class NoChoiceLogic : IChoiceLogic {

    public Transform ThisTransform { get { return _thisTransform; } set { _thisTransform = value; } }
    public SpeechBehaviour SpeechBehaviour { get { return _speechBehaviour; } set { _speechBehaviour = value; } }
    public PlayerAttributesLogic PlayerAttributes { get { return _playerAttributes; } set { _playerAttributes = value; } }
    public SpawnCoins SpawnCoins { get { return _spawnCoins; } set { _spawnCoins = value; } }

    public bool OutcomeChoice { get { return _outcomeChoice; } set { _outcomeChoice = value; } }

    private Transform _thisTransform;
    private SpeechBehaviour _speechBehaviour;
    private PlayerAttributesLogic _playerAttributes;
    private SpawnCoins _spawnCoins;

    //Stores whether the user said yes or no to this choice
    private bool _outcomeChoice;

    public void ExecuteChoice(ExecuteChoices executeChoices)
    {
        _playerAttributes = executeChoices.PlayerAttributes.PlayerAttributesLogic;
        _speechBehaviour = executeChoices.SpeechBehaviour;
        _spawnCoins = executeChoices.SpawnCoins;
        _thisTransform = executeChoices.ThisTransform;

        int moneyAmount = executeChoices.NoMoneyOutcome;
        int militaryAmount = executeChoices.NoMilitaryOutcome;
        int moodAmount = executeChoices.NoMoodOutcome;

        //If they are any special values to represent the player loosing all of their money e.g. we check here
        executeChoices.CheckForSpecialValues(ref moneyAmount, ref militaryAmount, ref moodAmount);

        if (executeChoices.CheckIfYouHaveEnoughResources(moneyAmount, militaryAmount))
        {
            _speechBehaviour.SpeechLogic.ShowNoSpeech();

            // Handle Money
            iTween.ValueTo(_thisTransform.gameObject, iTween.Hash("from", _playerAttributes.Money, "to", _playerAttributes.Money + moneyAmount, "onupdate", "itweenCallback_ChangeMoney"));
            _spawnCoins.updateCoins(moneyAmount);
            
            // Handle Military
            iTween.ValueTo(_thisTransform.gameObject, iTween.Hash("from", _playerAttributes.Military, "to", _playerAttributes.Military + militaryAmount, "onupdate", "itweenCallback_ChangeMilitary"));

            // Handle Mood
            int moodOutcome = _playerAttributes.Mood + moodAmount;
            if (moodOutcome > _playerAttributes.MaxMood) moodOutcome = _playerAttributes.MaxMood;
            if (moodOutcome <= 0) _speechBehaviour.SpeechLogic.GameIsNowOver = true;

            if (moodOutcome != _playerAttributes.Mood)
                executeChoices.AnimateMoodValueChanging(moodOutcome);

            //Display flash text
            _playerAttributes.FlashTextValues(moneyAmount, militaryAmount, moodAmount);

            //Save the decision the player made
            _outcomeChoice = false;
        }
        //Not enough resource
        else
        {
            _speechBehaviour.SpeechLogic.ExecuteCantAffordSpeech();

            //Remove depression
            executeChoices.AnimateMoodValueChanging(_playerAttributes.Mood - executeChoices.MoodLoss);

            //Display flash text
            _playerAttributes.FlashTextValues(0, 0, -executeChoices.MoodLoss);
        }
    }



}
