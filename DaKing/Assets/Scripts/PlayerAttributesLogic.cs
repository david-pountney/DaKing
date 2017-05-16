using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerAttributesLogic
{
    public int NewDayMoney { get { return _newDayMoney; } set { _newDayMoney = value; } }
    public int Money { get { return _money; } set { _money = value; } }
    public Text MoneyText { get { return _moneyText; } set { _moneyText = value; } }

    public int NewDayMilitary { get { return _newDayMilitary; } set { _newDayMilitary = value; } }
    public int Military { get { return _military; } set { _military = value; } }
    public Text MilitaryText { get { return _militaryText; } set { _militaryText = value; } }

    public int NewDayMood { get { return _newDayMood; } set { _newDayMood = value; } }
    public int Mood { get { return _mood; } set { _mood = value; } }

    public int MaxMood { get { return _maxMood; } set { _maxMood = value; } }

    public FlashTextControllerBehaviour FlashTextController { get { return _flashTextController; } set { _flashTextController = value; } }
    public Animator Animator { get { return _animator; } set { _animator = value; } }
    public bool HasAlreadyChangedMood { get { return _hasAlreadyChangedMood; } set { _hasAlreadyChangedMood = value; } }

    //Money
    private int _newDayMoney;
    private int _money;
    private Text _moneyText;

    //Military
    private int _newDayMilitary;
    private int _military;
    private Text _militaryText;

    //Depression
    private int _newDayMood;
    private int _mood;

    //Whats the highest value for depression?
    private int _maxMood = 100;

    //Flash text contorller
    private FlashTextControllerBehaviour _flashTextController;

    private Animator _animator;

    private bool _hasAlreadyChangedMood = false;
    
    public void SetMoney(int newVal)
    {
        _money = newVal;
        _moneyText.text = _money.ToString();
    }

    public void SetMilitary(int newVal)
    {
        _military = newVal;
        _militaryText.text = _military.ToString();
    }

    public void SetMood(int newVal)
    {
        _mood = newVal;
        GlobalReferencesBehaviour.instance.SceneData.controller.GetComponent<MoodDisplayScript>().handleMood(_mood);
    }

    public void FlashTextValues(int moneyAmount, int militaryAmount, int depressionAmount)
    {
        _flashTextController.FlashTextController.flash(moneyAmount, militaryAmount, depressionAmount);
    }

    public void SetNewDayValues()
    {
        //Make note of starting stats
        _newDayMoney = _money;
        _newDayMilitary = _military;
        _newDayMood = _mood;
    }

    public void SetMoodState(int mood)
    {
        if (_hasAlreadyChangedMood) return;

        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        _animator.SetInteger("mood", mood);
    }

}
