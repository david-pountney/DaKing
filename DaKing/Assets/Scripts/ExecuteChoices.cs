using UnityEngine;
using System.Collections;

public class ExecuteChoices
{
    public Transform ThisTransform { get { return _thisTransform; } set { _thisTransform = value; } }

    public PlayerAttributes PlayerAttributes { get { return _playerAttributes; } set { _playerAttributes = value; } }
    public MovementBehaviour MovementForChars { get { return _movementForChars; } set { _movementForChars = value; } }
    public SpeechBehaviour SpeechBehaviour { get { return _speechBehaviour; } set { _speechBehaviour = value; } }
    public SpawnCoins SpawnCoins { get { return _spawnCoins; } set { _spawnCoins = value; } }

    public int YesMoneyOutcome { get { return _yesMoneyOutcome; } set { _yesMoneyOutcome = value; } }
    public int NoMoneyOutcome { get { return _noMoneyOutcome; } set { _noMoneyOutcome = value; } }
    
    public int YesMilitaryOutcome { get { return _yesMilitaryOutcome; } set { _yesMilitaryOutcome = value; } }
    public int NoMilitaryOutcome { get { return _noMoneyOutcome; } set { _noMoneyOutcome = value; } }

    public int YesMoodOutcome { get { return _yesMoodOutcome; } set { _yesMoodOutcome = value; } }
    public int NoMoodOutcome { get { return _noMoodOutcome; } set { _noMoodOutcome = value; } }

    public int PassiveOneMoneyOutcome { get { return _passiveOneMoneyOutcome; } set { _passiveOneMoneyOutcome = value; } }
    public int PassiveOneMilitaryOutcome { get { return _passiveOneMilitaryOutcome; } set { _passiveOneMilitaryOutcome = value; } }
    public int PassiveOneMoodOutcome { get { return _passiveOneMoodOutcome; } set { _passiveOneMoodOutcome = value; } }

    public int PassiveTwoMoneyOutcome { get { return _passiveTwoMoneyOutcome; } set { _passiveTwoMoneyOutcome = value; } }
    public int PassiveTwoMilitaryOutcome { get { return _passiveTwoMilitaryOutcome; } set { _passiveTwoMilitaryOutcome = value; } }
    public int PassiveTwoMoodOutcome { get { return _passiveTwoMoodOutcome; } set { _passiveTwoMoodOutcome = value; } }

    public int MoodLoss{ get { return _moodLoss; } set { _moodLoss = value; } }

    private Transform _thisTransform;

    private PlayerAttributes _playerAttributes;
    private MovementBehaviour _movementForChars;
    private SpeechBehaviour _speechBehaviour;

    private SpawnCoins _spawnCoins;

    [SerializeField]
    private int _yesMoneyOutcome;
    [SerializeField]
    private int _noMoneyOutcome;

    [SerializeField]
    private int _yesMilitaryOutcome;
    [SerializeField]
    private int _noMilitaryOutcome;

    [SerializeField]
    private int _yesMoodOutcome;
    [SerializeField]
    private int _noMoodOutcome;

    [SerializeField]
    private int _passiveOneMoneyOutcome;
    [SerializeField]
    private int _passiveOneMilitaryOutcome;
    [SerializeField]
    private int _passiveOneMoodOutcome;

    [SerializeField]
    private int _passiveTwoMoneyOutcome;
    [SerializeField]
    private int _passiveTwoMilitaryOutcome;
    [SerializeField]
    private int _passiveTwoMoodOutcome;

    //Stores whether the user said yes or no to this choice
    public bool outcomeChoice;

    //How much mood does the player lose if he doesn't have enough resources to make a choice?
    private int _moodLoss = 5;

    public void ExecuteChoice(IChoiceLogic theChoiceType)
    {
        theChoiceType.ExecuteChoice(this);
    }
    /*
    public void ExecuteYesChoice()
    {
        if (CheckIfYouHaveEnoughResources())
        {
            _speechBehaviour.SpeechLogic.ShowYesSpeech();

            // Handle Money
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.money, "to", playerAttributes.money + _yesMoneyOutcome, "onupdate", "itweenChangeMoney"));
            spawnCoins.updateCoins(_yesMoneyOutcome);

            // Handle military
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.military, "to", playerAttributes.military + _yesMilitaryOutcome, "onupdate", "itweenChangeMilitary"));

            // // Handle mood
            int moodOutcome = playerAttributes.depression + _yesMoodOutcome;
            if (moodOutcome > playerAttributes.maxDepression) moodOutcome = playerAttributes.maxDepression;
            if (moodOutcome <= 0) _speechBehaviour.SpeechLogic.GameIsNowOver = true;

            if (moodOutcome != playerAttributes.depression)
                AnimateMoodValueChanging(moodOutcome);

            //Display flash text
            playerAttributes.flashTextValues(_yesMoneyOutcome, _yesMilitaryOutcome, _yesMoodOutcome);

            //Save the decision the player made
            outcomeChoice = true;

            endChoiceLogic();
        }
        //Not enough resource
        else
        {
            _speechBehaviour.SpeechLogic.ExecuteCantAffordSpeech();

            //Remove depression
            AnimateMoodValueChanging(playerAttributes.depression - 5);


            //Display flash text
            playerAttributes.flashTextValues(0, 0, -5);
        }

    }
    
    public void ExecuteNoChoice()
    {
        if ((_noMoneyOutcome > 0 || (playerAttributes.money >= Mathf.Abs(_noMoneyOutcome))) &&
            (_noMilitaryOutcome > 0 || (playerAttributes.military >= Mathf.Abs(_noMilitaryOutcome))))
        {
            _speechBehaviour.SpeechLogic.ShowNoSpeech();

            // Handle Money
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.money, "to", playerAttributes.money + _noMoneyOutcome, "onupdate", "itweenChangeMoney"));
            spawnCoins.updateCoins(_noMoneyOutcome);

            // Handle Military
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.military, "to", playerAttributes.military + _noMilitaryOutcome, "onupdate", "itweenChangeMilitary"));

            // Handle Mood
            int moodOutcome = playerAttributes.depression + _noMoodOutcome;
            if (moodOutcome > playerAttributes.maxDepression) moodOutcome = playerAttributes.maxDepression;
            if (moodOutcome <= 0) _speechBehaviour.SpeechLogic.GameIsNowOver = true;

            if (moodOutcome != playerAttributes.depression)
                AnimateMoodValueChanging(moodOutcome);

            //Display flash text
            playerAttributes.flashTextValues(_noMoneyOutcome, _noMilitaryOutcome, _noMoodOutcome);

            //Save the decision the player made
            outcomeChoice = false;

            endChoiceLogic();
        }
        //Not enough resource
        else
        {
            _speechBehaviour.SpeechLogic.ExecuteCantAffordSpeech();

            //Remove depression
            AnimateMoodValueChanging(playerAttributes.depression - 5);

            // StartCoroutine(changeDepression(-5, -1));

            //Display flash text
            playerAttributes.flashTextValues(0, 0, -5);
        }
    }

    public void ExecutePassiveOneChoice()
    {
        // Handle Money
        // Debug.Log("passiveOneMoneyOutcome" + passiveOneMoneyOutcome);
        if ((_passiveOneMoneyOutcome > 0 || (playerAttributes.money >= Mathf.Abs(_passiveOneMoneyOutcome))) &&
            (_passiveOneMilitaryOutcome > 0 || (playerAttributes.military >= Mathf.Abs(_passiveOneMilitaryOutcome))))
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.money, "to", playerAttributes.money + _passiveOneMoneyOutcome, "onupdate", "itweenChangeMoney"));
            spawnCoins.updateCoins(_passiveOneMoneyOutcome);

            // Handle Militry
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.military, "to", playerAttributes.military + _passiveOneMilitaryOutcome, "onupdate", "itweenChangeMilitary"));

            // // Handle depression
            int moodOutcome = playerAttributes.depression + _passiveOneMoodOutcome;
            if (moodOutcome > playerAttributes.maxDepression) moodOutcome = playerAttributes.maxDepression;
            if (moodOutcome <= 0) _speechBehaviour.SpeechLogic.GameIsNowOver = true;

            if (moodOutcome != playerAttributes.depression)
                AnimateMoodValueChanging(moodOutcome);

            // if (passiveOneDepressionOutcome > 0) StartCoroutine(changeDepression(passiveOneDepressionOutcome, 1));
            // else if (passiveOneDepressionOutcome < 0) StartCoroutine(changeDepression(passiveOneDepressionOutcome, -1));

            //Display flash text
            playerAttributes.flashTextValues(_passiveOneMoneyOutcome, _passiveOneMilitaryOutcome, _passiveOneMoodOutcome);
        }
        //Not enough resource
        else
        {
            AnimateMoodValueChanging(playerAttributes.depression - 5);

            //Display flash text
            playerAttributes.flashTextValues(0, 0, -5);
        }
    }

    public void ExecuteRemoveAll(bool loseMoney, bool loseMilitary, bool loseDepression)
    {
        // Handle Money
        if (loseMoney)
        {
            _yesMoneyOutcome = -playerAttributes.money;
            _noMoneyOutcome = -playerAttributes.money;
        }

        // Handle Militry
        if (loseMilitary)
        {
            _yesMilitaryOutcome = -playerAttributes.military;
            _noMilitaryOutcome = -playerAttributes.military;
        }

        if (loseDepression)
            Debug.Log("Not implemented code");
    }

    public void ExecutePassiveTwoChoice()
    {
        if ((_passiveTwoMoneyOutcome > 0 || (playerAttributes.money >= Mathf.Abs(_passiveTwoMoneyOutcome))) &&
        (_passiveTwoMilitaryOutcome > 0 || (playerAttributes.military >= Mathf.Abs(_passiveTwoMilitaryOutcome))))
        {
            // Handle Money
            // Debug.Log("passiveTwoMoneyOutcome" + passiveTwoMoneyOutcome);
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.money, "to", playerAttributes.money + _passiveTwoMoneyOutcome, "onupdate", "itweenChangeMoney"));
            spawnCoins.updateCoins(_passiveTwoMoneyOutcome);

            // Debug.Log("passiveTwoMilitaryOutcome" + passiveTwoMilitaryOutcome);
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.military, "to", playerAttributes.military + _passiveTwoMilitaryOutcome, "onupdate", "itweenChangeMilitary"));

            // // Handle depression
            int moodOutcome = playerAttributes.depression + _passiveTwoMoodOutcome;
            if (moodOutcome > playerAttributes.maxDepression) moodOutcome = playerAttributes.maxDepression;
            if (moodOutcome <= 0) _speechBehaviour.SpeechLogic.GameIsNowOver = true;

            AnimateMoodValueChanging(moodOutcome);
            
            playerAttributes.setMoodState(moodOutcome);

            //Display flash text
            playerAttributes.flashTextValues(_passiveTwoMoneyOutcome, _passiveTwoMilitaryOutcome, _passiveTwoMoodOutcome);
        }
        //Not enough resource
        else
        {
            AnimateMoodValueChanging(playerAttributes.depression - 5);

            //Display flash text
            playerAttributes.flashTextValues(0, 0, -5);
        }
    }
    */

    private void itweenChangeMilitary(int newVal)
    {
        _playerAttributes.setMilitary(newVal);
    }

    private void itweenChangeMoney(int newVal)
    {
        // Debug.Log("set Monnie: "+newVal);
        _playerAttributes.setMoney(newVal);
    }

    private void itweenChangeMood(int newVal)
    {
        _playerAttributes.setMood(newVal);
    }

    private void itweenCompleteMood()
    {
        Debug.Log("got here");
        _playerAttributes.HasAlreadyChangedMood = false;
    }

    private void endChoiceLogic()
    {
        
    }


}
