using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class PlayerAttributesBehaviour : MonoBehaviour {

    public PlayerAttributesLogic PlayerAttributesLogic { get { return _playerAttributes; } set { _playerAttributes = value; } }

    //Money
    public static int newDayMoney;
    public int money;
    public Text moneyText;

    //Military
    public static int newDayMilitary;
    public int military;
    public Text militaryText;

    //Depression
    public static int newDayMood;
    public int mood;

    //Whats the highest value for depression?
    public int maxMood = 100;

    //Flash text contorller
    public FlashTextControllerBehaviour flashTextController;

    private PlayerAttributesLogic _playerAttributes;

    void Awake()
    {
        _playerAttributes = new PlayerAttributesLogic();
    }

    void Start()
    {
        SetUp();
    }

    private void SetUp()
    {
        //Make note of starting stats
        _playerAttributes.NewDayMoney = money;
        _playerAttributes.NewDayMilitary = military;
        _playerAttributes.NewDayMood = mood;

        _playerAttributes.Money = money;
        _playerAttributes.Military = military;
        _playerAttributes.Mood = mood;

        _playerAttributes.MoneyText = moneyText;
        _playerAttributes.MilitaryText = militaryText;
        _playerAttributes.FlashTextController = flashTextController;

        _playerAttributes.MoneyText.text = money.ToString();
        _playerAttributes.MilitaryText.text = military.ToString();

        _playerAttributes.Animator = GetComponent<Animator>();
    }

}
