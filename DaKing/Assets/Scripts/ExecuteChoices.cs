using UnityEngine;
using System.Collections;

public class ExecuteChoices
{
    public Transform ThisTransform { get { return _thisTransform; } set { _thisTransform = value; } }

    public PlayerAttributesBehaviour PlayerAttributes { get { return _playerAttributes; } set { _playerAttributes = value; } }
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

    private PlayerAttributesBehaviour _playerAttributes;
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

    //Values for special tags to remove all of something
    private const int _loseAllMoney = -99999;
    private const int _loseAllMilitary = -99999;
    private const int _loseAllMood = -99999;

    public void ExecuteChoice(IChoiceLogic theChoiceType)
    {
        theChoiceType.ExecuteChoice(this);
    }
    
    public bool CheckIfYouHaveEnoughResources(int moneyAmount, int militaryAmount)
    {
        if ((moneyAmount > 0 || (_playerAttributes.PlayerAttributesLogic.Money >= Mathf.Abs(moneyAmount))) &&
            (militaryAmount > 0 || (_playerAttributes.PlayerAttributesLogic.Military >= Mathf.Abs(militaryAmount))))

            return true;

        return false;
    }

    public void AnimateMoodValueChanging(int moodOutcome)
    {
        _playerAttributes.PlayerAttributesLogic.SetMoodState(moodOutcome);

        //Change mood value shown on screen
        iTween.ValueTo(_thisTransform.gameObject, iTween.Hash("from", _playerAttributes.PlayerAttributesLogic.Mood, "to", moodOutcome, "onupdate", "itweenCallback_ChangeMood"
            ));
    }
    
    //Handles 'remove all' special codes
    public void CheckForSpecialValues(ref int moneyAmount, ref int militaryAmount, ref int moodAmount)
    {
        if (moneyAmount == _loseAllMoney)
            moneyAmount = -_playerAttributes.PlayerAttributesLogic.Money;

        if (militaryAmount == _loseAllMilitary)
            militaryAmount = -_playerAttributes.PlayerAttributesLogic.Military;

        if (moodAmount == _loseAllMood)
            moodAmount = -_playerAttributes.PlayerAttributesLogic.Mood;
    }

}
