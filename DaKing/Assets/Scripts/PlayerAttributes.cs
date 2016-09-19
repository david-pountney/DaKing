using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerAttributes : MonoBehaviour
{
    //Money
    public static int newDayMoney;
    public int money;
    public Text moneyText;

    //Military
    public static int newDayMilitary;
    public int military;
    public Text militaryText;

    //Depression
    public static int newDayDepression;
    public int depression;
    public Text depressionText;

    //Depression sprites
    public Sprite noDepressionKing;
    public Sprite quarterPercentDepressionKing;
    public Sprite halfPercentDepressionKing;
    public Sprite threeQuarterPercentDepressionKing;

    //Whats the highest value for depression?
    public int maxDepression = 100;

    public int noDepressionValue;
    public int threeQuarterPercentDepressionValue;
    public int halfPercentDepressionValue;
    public int quarterPercentDepressionValue;

    //Flash text contorller
    public FlashTextController flashTextController;

    void Start()
    {
        //Make note of starting stats
        newDayMoney = money;
        newDayMilitary = military;
        newDayDepression = depression;

        moneyText.text = money.ToString();
        militaryText.text = military.ToString();
        depressionText.text = depression.ToString();
    }

    public void setMoney(int newVal)
    {
        money = newVal;
        moneyText.text = money.ToString();
        // Debug.Log("PlayerAttribute.money: " + money);
    }

    public void setMilitary(int newVal)
    {
        military = newVal;
        militaryText.text = military.ToString();
    }

    public void setMood(int newVal)
    {
        depression = newVal;
        depressionText.text = depression.ToString();
        MoodDisplayScript.instance.handleMood(depression);
        checkForNewStageOfDepression();
    }

    public void flashTextValues(int moneyAmount, int militaryAmount, int depressionAmount)
    {
        flashTextController.flash(moneyAmount, militaryAmount, depressionAmount);
    }

    public void setNewDayValues()
    {
        //Make note of starting stats
        newDayMoney = money;
        newDayMilitary = military;
        newDayDepression = depression;
    }

    private void checkForNewStageOfDepression()
    {
        //Up to Full depression health
        if (depression > quarterPercentDepressionValue)
        {
            GetComponent<SpriteRenderer>().sprite = noDepressionKing;
            //GetComponent<Animator>().runtimeAnimatorController = Resources.Load("animations/KingOnThrone001_0") as RuntimeAnimatorController;
        }
        //Up to Three quater depression health
        else if (depression <= quarterPercentDepressionValue && depression > halfPercentDepressionValue)
        {
            GetComponent<SpriteRenderer>().sprite = threeQuarterPercentDepressionKing;
            //GetComponent<Animator>().runtimeAnimatorController = Resources.Load("animations/KingOnThrone002_0") as RuntimeAnimatorController;
        }
        //Up to Half depression health
        else if (depression <= halfPercentDepressionValue && depression > threeQuarterPercentDepressionValue)
        {
            GetComponent<SpriteRenderer>().sprite = halfPercentDepressionKing;
            //GetComponent<Animator>().runtimeAnimatorController = Resources.Load("animations/KingOnThrone003_0") as RuntimeAnimatorController;
        }
        //Up to a quater depression health
        else if (depression <= threeQuarterPercentDepressionValue)
        {
            GetComponent<SpriteRenderer>().sprite = quarterPercentDepressionKing;
            //GetComponent<Animator>().runtimeAnimatorController = Resources.Load("animations/KingOnThrone004_0") as RuntimeAnimatorController;
        }
    }
}
