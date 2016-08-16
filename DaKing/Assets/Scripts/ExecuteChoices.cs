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

            //Give money
            if (yesMoneyOutcome > 0)
                spawnCoins.startDroppingCoins(yesMoneyOutcome);

            //Remove money
            else if (yesMoneyOutcome < 0)
                spawnCoins.startRemovingCoins(yesMoneyOutcome);

            //Give military
            if (yesMilitaryOutcome > 0)
            {
                Debug.Log("yesMilitaryOutcome" + yesMilitaryOutcome);
                iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.military, "to", playerAttributes.military + yesMilitaryOutcome, "onupdate", "itweenChangeMilitary"));
                // StartCoroutine(changeMilitary(yesMilitaryOutcome, 1));
            }

            //Remove military
            else if (yesMilitaryOutcome < 0)
            {
                Debug.Log("yesMilitaryOutcome" + yesMilitaryOutcome);
                iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.military, "to", playerAttributes.military + yesMilitaryOutcome, "onupdate", "itweenChangeMilitary"));
                // StartCoroutine(changeMilitary(yesMilitaryOutcome, -1));
            }


            //Give depression
            if (yesDepressionOutcome > 0)
                StartCoroutine(changeDepression(yesDepressionOutcome, 1));
            //Remove depression
            else if (yesDepressionOutcome < 0)
                StartCoroutine(changeDepression(yesDepressionOutcome, -1));

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
            StartCoroutine(changeDepression(-5, -1));

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

            //Give money
            if (noMoneyOutcome > 0)
                spawnCoins.startDroppingCoins(noMoneyOutcome);
            //Remove money
            else if (noMoneyOutcome < 0)
                spawnCoins.startRemovingCoins(noMoneyOutcome);

            //Give military
            if (noMilitaryOutcome > 0)
            {
                Debug.Log("noMilitaryOutcome" + noMilitaryOutcome);
                iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.military, "to", playerAttributes.military + noMilitaryOutcome, "onupdate", "itweenChangeMilitary"));
                // StartCoroutine(changeMilitary(noMilitaryOutcome, 1));
            }

            //Remove military
            else if (noMilitaryOutcome < 0)
            {
                Debug.Log("noMilitaryOutcome" + noMilitaryOutcome);
                iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.military, "to", playerAttributes.military + noMilitaryOutcome, "onupdate", "itweenChangeMilitary"));
                // StartCoroutine(changeMilitary(noMilitaryOutcome, -1));
            }


            //Give depression
            if (noDepressionOutcome > 0)
                StartCoroutine(changeDepression(noDepressionOutcome, 1));
            //Remove depression
            else if (noDepressionOutcome < 0)
                StartCoroutine(changeDepression(noDepressionOutcome, -1));

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
            StartCoroutine(changeDepression(-5, -1));

            //Display flash text
            playerAttributes.flashTextValues(0, 0, -5);
        }
    }

    public void executePassiveOneChoice()
    {
        //Give money
        if (passiveOneMoneyOutcome > 0)
            spawnCoins.startDroppingCoins(passiveOneMoneyOutcome);
        //Remove money
        else if (passiveOneMoneyOutcome < 0)
        {
            spawnCoins.startRemovingCoins(passiveOneMoneyOutcome);
        }

        //Give military
        if (passiveOneMilitaryOutcome > 0)
        {
            Debug.Log("passiveOneMilitaryOutcome" + passiveOneMilitaryOutcome);
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.military, "to", playerAttributes.military + passiveOneMilitaryOutcome, "onupdate", "itweenChangeMilitary"));
            // StartCoroutine(changeMilitary(passiveOneMilitaryOutcome, 1));
        }

        //Remove military
        else if (passiveOneMilitaryOutcome < 0)
        {
            Debug.Log("passiveOneMilitaryOutcome" + passiveOneMilitaryOutcome);
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.military, "to", playerAttributes.military + passiveOneMilitaryOutcome, "onupdate", "itweenChangeMilitary"));
            // StartCoroutine(changeMilitary(passiveOneMilitaryOutcome, -1));
        }


        //Give depression
        if (passiveOneDepressionOutcome > 0)
            StartCoroutine(changeDepression(passiveOneDepressionOutcome, 1));
        //Remove depression
        else if (passiveOneDepressionOutcome < 0)
            StartCoroutine(changeDepression(passiveOneDepressionOutcome, -1));

        //Display flash text
        playerAttributes.flashTextValues(passiveOneMoneyOutcome, passiveOneMilitaryOutcome, passiveOneDepressionOutcome);

    }

    public void executePassiveTwoChoice()
    {
        //Give money
        if (passiveTwoMoneyOutcome > 0)
            spawnCoins.startDroppingCoins(passiveTwoMoneyOutcome);
        //Remove money
        else if (passiveTwoMoneyOutcome < 0)
        {
            spawnCoins.startRemovingCoins(passiveTwoMoneyOutcome);
        }

        //Give military
        if (passiveTwoMilitaryOutcome > 0)
        {
            Debug.Log("passiveTwoMilitaryOutcome" + passiveTwoMilitaryOutcome);
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.military, "to", playerAttributes.military + passiveTwoMilitaryOutcome, "onupdate", "itweenChangeMilitary"));
            // StartCoroutine(changeMilitary(passiveTwoMilitaryOutcome, 1));
        }

        //Remove military
        else if (passiveTwoMilitaryOutcome < 0)
        {
            Debug.Log("passiveTwoMilitaryOutcome" + passiveTwoMilitaryOutcome);
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.military, "to", playerAttributes.military + passiveTwoMilitaryOutcome, "onupdate", "itweenChangeMilitary"));
            // StartCoroutine(changeMilitary(passiveTwoMilitaryOutcome, -1));
        }


        //Give depression
        if (passiveTwoDepressionOutcome > 0)
            StartCoroutine(changeDepression(passiveTwoDepressionOutcome, 1));
        //Remove depression
        else if (passiveTwoDepressionOutcome < 0)
            StartCoroutine(changeDepression(passiveTwoDepressionOutcome, -1));

        //Display flash text
        playerAttributes.flashTextValues(passiveTwoMoneyOutcome, passiveTwoMilitaryOutcome, passiveTwoDepressionOutcome);
    }

    private void itweenChangeMilitary(int newVal)
    {
        playerAttributes.setMilitary(newVal);
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
