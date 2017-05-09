using UnityEngine;
using System.Collections;
using System;

public class YesChoiceLogic : IChoiceLogic
{
    public Transform ThisTransform { get { return _thisTransform; } set { _thisTransform = value; } }
    public SpeechBehaviour SpeechBehaviour { get { return _speechBehaviour; } set { _speechBehaviour = value; } }
    public PlayerAttributes PlayerAttributes { get { return _playerAttributes; } set { _playerAttributes = value; } }
    public SpawnCoins SpawnCoins { get { return _spawnCoins; } set { _spawnCoins = value; } }

    public bool OutcomeChoice { get { return _outcomeChoice; } set { _outcomeChoice = value; } }

    private Transform _thisTransform;
    private SpeechBehaviour _speechBehaviour;
    private PlayerAttributes _playerAttributes;
    private SpawnCoins _spawnCoins;

    //Stores whether the user said yes or no to this choice
    private bool _outcomeChoice;

    public void ExecuteChoice(ExecuteChoices executeChoices)
    {
        _playerAttributes = executeChoices.PlayerAttributes;
        _speechBehaviour = executeChoices.SpeechBehaviour;
        _spawnCoins = executeChoices.SpawnCoins;
        _thisTransform = executeChoices.ThisTransform;

        if (CheckIfYouHaveEnoughResources(executeChoices))
        {
            _speechBehaviour.SpeechLogic.ShowYesSpeech();

            // Handle Money
            iTween.ValueTo(_thisTransform.gameObject, iTween.Hash("from", _playerAttributes.money, "to", _playerAttributes.money + executeChoices.YesMoneyOutcome, "onupdate", "itweenChangeMoney"));
            _spawnCoins.updateCoins(executeChoices.YesMoneyOutcome);

            // Handle military
            iTween.ValueTo(_thisTransform.gameObject, iTween.Hash("from", _playerAttributes.military, "to", _playerAttributes.military + executeChoices.YesMilitaryOutcome, "onupdate", "itweenChangeMilitary"));

            // // Handle mood
            int moodOutcome = _playerAttributes.depression + executeChoices.YesMoneyOutcome;
            if (moodOutcome > _playerAttributes.maxDepression) moodOutcome = _playerAttributes.maxDepression;
            if (moodOutcome <= 0) _speechBehaviour.SpeechLogic.GameIsNowOver = true;

            if (moodOutcome != _playerAttributes.depression)
                AnimateMoodValueChanging(executeChoices, moodOutcome);

            //Display flash text
            _playerAttributes.flashTextValues(executeChoices.YesMoneyOutcome, executeChoices.YesMilitaryOutcome, executeChoices.YesMoodOutcome);

            //Save the decision the player made
            _outcomeChoice = true;
        }
        //Not enough resource
        else
        {
            _speechBehaviour.SpeechLogic.ExecuteCantAffordSpeech();

            //Remove depression
            AnimateMoodValueChanging(executeChoices, _playerAttributes.depression - executeChoices.MoodLoss);

            //Display flash text
            _playerAttributes.flashTextValues(0, 0, -executeChoices.MoodLoss);
        }
    }

    private void AnimateMoodValueChanging(ExecuteChoices executeChoices, int moodOutcome)
    {
        _playerAttributes.setMoodState(moodOutcome);

        //Change mood value shown on screen
        iTween.ValueTo(_thisTransform.gameObject, iTween.Hash("from", _playerAttributes.depression, "to", moodOutcome, "onupdate", "itweenChangeMood"
            , "oncomplete", "itweenCompleteMood"));
    }

    private bool CheckIfYouHaveEnoughResources(ExecuteChoices executeChoices)
    {
        if ((executeChoices.YesMoneyOutcome > 0 || (_playerAttributes.money >= Mathf.Abs(executeChoices.YesMoneyOutcome))) &&
            (executeChoices.YesMilitaryOutcome > 0 || (_playerAttributes.military >= Mathf.Abs(executeChoices.YesMilitaryOutcome))))

            return true;

        return false;
    }
}
