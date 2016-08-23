using UnityEngine;
using System.Collections;

public class ExecuteChoices : MonoBehaviour
{
    private PlayerAttributes playerAttributes;
    private MovementForChars movementForChars;

    public SpawnCoins spawnCoins;

    public int yesMoneyOutcome;
    public int noMoneyOutcome;

    public int yesMilitaryOutcome;
    public int noMilitaryOutcome;

    public int noDepressionOutcome;
    public int yesDepressionOutcome;

    public int passiveOneMoneyOutcome;
    public int passiveOneMilitaryOutcome;
    public int passiveOneDepressionOutcome;

    public int passiveTwoMoneyOutcome;
    public int passiveTwoMilitaryOutcome;
    public int passiveTwoDepressionOutcome;

    //Stores whether the user said yes or no to this choice
    public bool outcomeChoice;

    void Start()
    {
        playerAttributes = GameObject.FindGameObjectWithTag("King").GetComponent<PlayerAttributes>();
        movementForChars = GetComponent<MovementForChars>();
    }

    public void executeYesChoice()
    {
        if ((yesMoneyOutcome > 0 || (playerAttributes.money >= Mathf.Abs(yesMoneyOutcome))) &&
            (yesMilitaryOutcome > 0 || (playerAttributes.military >= Mathf.Abs(yesMilitaryOutcome))))
        {
            movementForChars.showYesSpeech();

            // Handle Money
            Debug.Log("yesMoneyOutcome" + yesMoneyOutcome);
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.money, "to", playerAttributes.money + yesMoneyOutcome, "onupdate", "itweenChangeMoney"));


            // Handle military
            Debug.Log("yesMilitaryOutcome" + yesMilitaryOutcome);
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.military, "to", playerAttributes.military + yesMilitaryOutcome, "onupdate", "itweenChangeMilitary"));

            // // Handle mood
            int moodOutcome = playerAttributes.depression + yesDepressionOutcome;
            if (moodOutcome > playerAttributes.maxDepression) moodOutcome = playerAttributes.maxDepression;
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.depression, "to", moodOutcome, "onupdate", "itweenChangeMood"));

            // if (yesDepressionOutcome > 0) StartCoroutine(changeDepression(yesDepressionOutcome, 1));
            // else if (yesDepressionOutcome < 0) StartCoroutine(changeDepression(yesDepressionOutcome, -1));

            //Display flash text
            playerAttributes.flashTextValues(yesMoneyOutcome, yesMilitaryOutcome, yesDepressionOutcome);

            //Save the decision the player made
            outcomeChoice = true;

            endChoiceLogic();
        }
        //Not enough money
        else
        {
            movementForChars.executeCantAffordSpeech();

            //Remove depression
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.depression, "to", playerAttributes.depression - 5, "onupdate", "itweenChangeMood"));
            // StartCoroutine(changeDepression(-5, -1));

            //Display flash text
            playerAttributes.flashTextValues(0, 0, -5);
        }

    }

    public void executeNoChoice()
    {
        if ((noMoneyOutcome > 0 || (playerAttributes.money >= Mathf.Abs(noMoneyOutcome))) &&
            (noMilitaryOutcome > 0 || (playerAttributes.military >= Mathf.Abs(noMilitaryOutcome))))
        {
            movementForChars.showNoSpeech();

            // Handle Money
            Debug.Log("noMilitaryOutcome" + noMilitaryOutcome);
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.money, "to", playerAttributes.money + noMilitaryOutcome, "onupdate", "itweenChangeMoney"));

            // Handle Military
            Debug.Log("noMilitaryOutcome" + noMilitaryOutcome);
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.military, "to", playerAttributes.military + noMilitaryOutcome, "onupdate", "itweenChangeMilitary"));

            // Handle Mood
            int moodOutcome = playerAttributes.depression + noDepressionOutcome;
            if (moodOutcome > playerAttributes.maxDepression) moodOutcome = playerAttributes.maxDepression;
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.depression, "to", moodOutcome, "onupdate", "itweenChangeMood"));
            // if (noDepressionOutcome > 0) StartCoroutine(changeDepression(noDepressionOutcome, 1));
            // else if (noDepressionOutcome < 0) StartCoroutine(changeDepression(noDepressionOutcome, -1));

            //Display flash text
            playerAttributes.flashTextValues(noMoneyOutcome, noMilitaryOutcome, noDepressionOutcome);

            //Save the decision the player made
            outcomeChoice = false;

            endChoiceLogic();
        }
        //Not enough money
        else
        {
            movementForChars.executeCantAffordSpeech();

            //Remove depression
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.depression, "to", playerAttributes.depression - 5, "onupdate", "itweenChangeMood"));
            // StartCoroutine(changeDepression(-5, -1));

            //Display flash text
            playerAttributes.flashTextValues(0, 0, -5);
        }
    }

    public void executePassiveOneChoice()
    {

        // Handle Money
        Debug.Log("passiveOneMoneyOutcome" + passiveOneMoneyOutcome);
        iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.money, "to", playerAttributes.money + passiveOneMoneyOutcome, "onupdate", "itweenChangeMoney"));

        // Handle Militry
        iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.military, "to", playerAttributes.military + passiveOneMilitaryOutcome, "onupdate", "itweenChangeMilitary"));

        // // Handle depression
        int moodOutcome = playerAttributes.depression + passiveOneDepressionOutcome;
        if (moodOutcome > playerAttributes.maxDepression) moodOutcome = playerAttributes.maxDepression;
        iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.depression, "to", moodOutcome, "onupdate", "itweenChangeMood"));
        // if (passiveOneDepressionOutcome > 0) StartCoroutine(changeDepression(passiveOneDepressionOutcome, 1));
        // else if (passiveOneDepressionOutcome < 0) StartCoroutine(changeDepression(passiveOneDepressionOutcome, -1));

        //Display flash text
        playerAttributes.flashTextValues(passiveOneMoneyOutcome, passiveOneMilitaryOutcome, passiveOneDepressionOutcome);

    }

    public void executePassiveTwoChoice()
    {

        // Handle Money
        Debug.Log("passiveTwoMoneyOutcome" + passiveTwoMoneyOutcome);
        iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.money, "to", playerAttributes.money + passiveTwoMoneyOutcome, "onupdate", "itweenChangeMoney"));

        Debug.Log("passiveTwoMilitaryOutcome" + passiveTwoMilitaryOutcome);
        iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.military, "to", playerAttributes.military + passiveTwoMilitaryOutcome, "onupdate", "itweenChangeMilitary"));

        // // Handle depression
        int moodOutcome = playerAttributes.depression + passiveTwoDepressionOutcome;
        if (moodOutcome > playerAttributes.maxDepression) moodOutcome = playerAttributes.maxDepression;
        iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.depression, "to", moodOutcome, "onupdate", "itweenChangeMood"));
        // if (passiveTwoDepressionOutcome > 0) StartCoroutine(changeDepression(passiveTwoDepressionOutcome, 1));
        // else if (passiveTwoDepressionOutcome < 0) StartCoroutine(changeDepression(passiveTwoDepressionOutcome, -1));

        //Display flash text
        playerAttributes.flashTextValues(passiveTwoMoneyOutcome, passiveTwoMilitaryOutcome, passiveTwoDepressionOutcome);
    }

    private void itweenChangeMilitary(int newVal)
    {
        playerAttributes.setMilitary(newVal);
    }

    private void itweenChangeMoney(int newVal)
    {
        playerAttributes.setMoney(newVal);
    }

    private void itweenChangeMood(int newVal)
    {
        playerAttributes.setMood(newVal);
    }

    private IEnumerator changeMilitary(int amount, int step)
    {
        int flip = System.Math.Abs(amount);

        for (int i = 0; i < flip; ++i)
        {
            //Update UI
            playerAttributes.militaryChanged(step);

            yield return new WaitForSeconds(spawnCoins.timeBetweenCoinAdding);
        }
    }

    private IEnumerator changeDepression(int amount, int step)
    {
        MoodDisplayScript.getInstance().handleMood(amount);

        int flip = System.Math.Abs(amount);

        for (int i = 0; i < flip; ++i)
        {
            //Update UI
            playerAttributes.depressionChanged(step);

            yield return new WaitForSeconds(spawnCoins.timeBetweenCoinAdding);
        }
    }

    private void endChoiceLogic()
    {
        movementForChars.killChoices();
    }


}
