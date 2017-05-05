using UnityEngine;
using System.Collections;

public class ExecuteChoices : MonoBehaviour
{
    private PlayerAttributes playerAttributes;
    private MovementBehaviour movementForChars;
    private SpeechBehaviour _speechBehaviour;

    private SpawnCoins spawnCoins;

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
        playerAttributes = GameObject.Find("king").GetComponent<PlayerAttributes>();
        movementForChars = GetComponent<MovementBehaviour>();
        _speechBehaviour = GetComponent<SpeechBehaviour>();
        spawnCoins = GameObject.Find("Controller").GetComponent<SpawnCoins>();
    }

    public void ExecuteYesChoice()
    {
        if ((yesMoneyOutcome > 0 || (playerAttributes.money >= Mathf.Abs(yesMoneyOutcome))) &&
            (yesMilitaryOutcome > 0 || (playerAttributes.military >= Mathf.Abs(yesMilitaryOutcome))))
        {
            _speechBehaviour.SpeechLogic.ShowYesSpeech();

            // Handle Money
            // Debug.Log("yesMoneyOutcome" + yesMoneyOutcome);
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.money, "to", playerAttributes.money + yesMoneyOutcome, "onupdate", "itweenChangeMoney"));
            spawnCoins.updateCoins(yesMoneyOutcome);

            // Handle military
            // Debug.Log("yesMilitaryOutcome" + yesMilitaryOutcome);
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.military, "to", playerAttributes.military + yesMilitaryOutcome, "onupdate", "itweenChangeMilitary"));

            // // Handle mood
            int moodOutcome = playerAttributes.depression + yesDepressionOutcome;
            if (moodOutcome > playerAttributes.maxDepression) moodOutcome = playerAttributes.maxDepression;
            if (moodOutcome <= 0) _speechBehaviour.SpeechLogic.GameIsNowOver = true;
            if (moodOutcome != playerAttributes.depression)
                AnimateMoodValueChanging(moodOutcome);


            // if (yesDepressionOutcome > 0) StartCoroutine(changeDepression(yesDepressionOutcome, 1));
            // else if (yesDepressionOutcome < 0) StartCoroutine(changeDepression(yesDepressionOutcome, -1));

            //Display flash text
            playerAttributes.flashTextValues(yesMoneyOutcome, yesMilitaryOutcome, yesDepressionOutcome);

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
        if ((noMoneyOutcome > 0 || (playerAttributes.money >= Mathf.Abs(noMoneyOutcome))) &&
            (noMilitaryOutcome > 0 || (playerAttributes.military >= Mathf.Abs(noMilitaryOutcome))))
        {
            _speechBehaviour.SpeechLogic.ShowNoSpeech();

            // Handle Money
            // Debug.Log("noMilitaryOutcome" + noMoneyOutcome);
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.money, "to", playerAttributes.money + noMoneyOutcome, "onupdate", "itweenChangeMoney"));
            spawnCoins.updateCoins(noMoneyOutcome);

            // Handle Military
            // Debug.Log("noMilitaryOutcome" + noMilitaryOutcome);
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.military, "to", playerAttributes.military + noMilitaryOutcome, "onupdate", "itweenChangeMilitary"));

            // Handle Mood
            int moodOutcome = playerAttributes.depression + noDepressionOutcome;
            if (moodOutcome > playerAttributes.maxDepression) moodOutcome = playerAttributes.maxDepression;
            if (moodOutcome <= 0) _speechBehaviour.SpeechLogic.GameIsNowOver = true;

            if (moodOutcome != playerAttributes.depression)
                AnimateMoodValueChanging(moodOutcome);

            // if (noDepressionOutcome > 0) StartCoroutine(changeDepression(noDepressionOutcome, 1));
            // else if (noDepressionOutcome < 0) StartCoroutine(changeDepression(noDepressionOutcome, -1));

            //Display flash text
            playerAttributes.flashTextValues(noMoneyOutcome, noMilitaryOutcome, noDepressionOutcome);

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
        if ((passiveOneMoneyOutcome > 0 || (playerAttributes.money >= Mathf.Abs(passiveOneMoneyOutcome))) &&
            (passiveOneMilitaryOutcome > 0 || (playerAttributes.military >= Mathf.Abs(passiveOneMilitaryOutcome))))
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.money, "to", playerAttributes.money + passiveOneMoneyOutcome, "onupdate", "itweenChangeMoney"));
            spawnCoins.updateCoins(passiveOneMoneyOutcome);

            // Handle Militry
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.military, "to", playerAttributes.military + passiveOneMilitaryOutcome, "onupdate", "itweenChangeMilitary"));

            // // Handle depression
            int moodOutcome = playerAttributes.depression + passiveOneDepressionOutcome;
            if (moodOutcome > playerAttributes.maxDepression) moodOutcome = playerAttributes.maxDepression;
            if (moodOutcome <= 0) _speechBehaviour.SpeechLogic.GameIsNowOver = true;

            if (moodOutcome != playerAttributes.depression)
                AnimateMoodValueChanging(moodOutcome);

            // if (passiveOneDepressionOutcome > 0) StartCoroutine(changeDepression(passiveOneDepressionOutcome, 1));
            // else if (passiveOneDepressionOutcome < 0) StartCoroutine(changeDepression(passiveOneDepressionOutcome, -1));

            //Display flash text
            playerAttributes.flashTextValues(passiveOneMoneyOutcome, passiveOneMilitaryOutcome, passiveOneDepressionOutcome);
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
            yesMoneyOutcome = -playerAttributes.money;
            noMoneyOutcome = -playerAttributes.money;
        }

        // Handle Militry
        if (loseMilitary)
        {
            yesMilitaryOutcome = -playerAttributes.military;
            noMilitaryOutcome = -playerAttributes.military;
        }

        if (loseDepression)
            Debug.Log("Not implemented code");
    }

    public void ExecutePassiveTwoChoice()
    {
        if ((passiveTwoMoneyOutcome > 0 || (playerAttributes.money >= Mathf.Abs(passiveTwoMoneyOutcome))) &&
        (passiveTwoMilitaryOutcome > 0 || (playerAttributes.military >= Mathf.Abs(passiveTwoMilitaryOutcome))))
        {
            // Handle Money
            // Debug.Log("passiveTwoMoneyOutcome" + passiveTwoMoneyOutcome);
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.money, "to", playerAttributes.money + passiveTwoMoneyOutcome, "onupdate", "itweenChangeMoney"));
            spawnCoins.updateCoins(passiveTwoMoneyOutcome);

            // Debug.Log("passiveTwoMilitaryOutcome" + passiveTwoMilitaryOutcome);
            iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.military, "to", playerAttributes.military + passiveTwoMilitaryOutcome, "onupdate", "itweenChangeMilitary"));

            // // Handle depression
            int moodOutcome = playerAttributes.depression + passiveTwoDepressionOutcome;
            if (moodOutcome > playerAttributes.maxDepression) moodOutcome = playerAttributes.maxDepression;
            if (moodOutcome <= 0) _speechBehaviour.SpeechLogic.GameIsNowOver = true;

            AnimateMoodValueChanging(moodOutcome);
            
            playerAttributes.setMoodState(moodOutcome);

            //Display flash text
            playerAttributes.flashTextValues(passiveTwoMoneyOutcome, passiveTwoMilitaryOutcome, passiveTwoDepressionOutcome);
        }
        //Not enough resource
        else
        {
            AnimateMoodValueChanging(playerAttributes.depression - 5);

            //Display flash text
            playerAttributes.flashTextValues(0, 0, -5);
        }
    }

    private void AnimateMoodValueChanging(int moodOutcome)
    {
        playerAttributes.setMoodState(moodOutcome);

        //Change mood value shown on screen
        iTween.ValueTo(gameObject, iTween.Hash("from", playerAttributes.depression, "to", moodOutcome, "onupdate", "itweenChangeMood"
            , "oncomplete", "itweenCompleteMood"));
    }

    private void itweenChangeMilitary(int newVal)
    {
        playerAttributes.setMilitary(newVal);
    }

    private void itweenChangeMoney(int newVal)
    {
        // Debug.Log("set Monnie: "+newVal);
        playerAttributes.setMoney(newVal);
    }

    private void itweenChangeMood(int newVal)
    {
        playerAttributes.setMood(newVal);
    }

    private void itweenCompleteMood()
    {
        Debug.Log("got here");
        playerAttributes.HasAlreadyChangedMood = false;
    }

    private void endChoiceLogic()
    {
        
    }


}
