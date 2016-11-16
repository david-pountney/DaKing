using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class NextDayOutcome : AnimatedMenu {

    public Text dayNumberText;

    public Text moneyOutcomeText;
    public Text militaryOutcomeText;
    public Text depressionOutcomeText;

    //Fades in and out the music
    public GameObject musicController;

    //Sound effect (Boom)
    public AudioClip boomSFX;

    //The outcome in difference from the last day
    private int moneyDif; 
    private int militaryDif; 
    private int depressionDif;

    //We use this for player stats
    private PlayerAttributes _playerAttributes;
    private ControllerLogic _controllerLogic;

    // Use this for initialization
    void Start () {
        _playerAttributes = GameObject.FindGameObjectWithTag("King").GetComponent<PlayerAttributes>();
        _controllerLogic = GameObject.FindGameObjectWithTag("Controller").GetComponent<ControllerLogic>();
    }

    /// <summary>
    /// Entry point to kick start the script
    /// </summary>
    public override void startFadingIn()
    {
        setupDayText();
        calculateOutcomeValues();
        setupStatsText();
        StartCoroutine(startFadingSequence());
    }

    private void setupDayText()
    {
        dayNumberText.text = "Day " + _controllerLogic.DayNumber;
    }

    private void finalFade()
    {
        Color newGreen = new Color(54.0f/255.0f, 158.0f/255.0f, 90.0f/255.0f, 0);
        Color newRed = new Color(204.0f/255.0f, 45.0f/255.0f, 45.0f/255.0f, 0);
        Color newWhite = new Color(230.0f / 255.0f, 245.0f/255.0f, 250.0f/255.0f, 0);
        
        //Money color
        if (moneyDif > 0)
            StartCoroutine(fadeIn(newGreen, moneyOutcomeText));
        else if (moneyDif < 0)
            StartCoroutine(fadeIn(newRed, moneyOutcomeText));
        else StartCoroutine(fadeIn(newWhite, moneyOutcomeText));

        //Military color
        if (militaryDif > 0)
            StartCoroutine(fadeIn(newGreen, militaryOutcomeText));
        else if (militaryDif < 0)
            StartCoroutine(fadeIn(newRed, militaryOutcomeText));
        else StartCoroutine(fadeIn(newWhite, militaryOutcomeText));
        
        //Depression color
        if (depressionDif > 0)
            StartCoroutine(fadeIn(newGreen, depressionOutcomeText));
        else if (depressionDif < 0)
            StartCoroutine(fadeIn(newRed, depressionOutcomeText));
        else StartCoroutine(fadeIn(newWhite, depressionOutcomeText));

    }

    private void fadeDayNumber()
    {
        //Fade day
        StartCoroutine(fadeIn(Color.white, dayNumberText));

        //Play boom sound
        PlayBoomSound();
    }

    private void PlayBoomSound()
    {
        //Play boom sound
        Camera.main.GetComponent<AudioSource>().clip = boomSFX;
        Camera.main.GetComponent<AudioSource>().Play();
    }

    private void setupStatsText()
    {
        Color moneyCol = Color.white;
        Color militaryCol = Color.white;
        Color depressionCol = Color.white;

        moneyOutcomeText.text = "Gold ";
        militaryOutcomeText.text = "Military ";
        depressionOutcomeText.text = "Depression ";

        //Add sign to number
        if (moneyDif > 0) moneyOutcomeText.text = moneyOutcomeText.text + "+";
        else if (moneyDif < 0) moneyOutcomeText.text = moneyOutcomeText.text;
        else moneyOutcomeText.text = moneyOutcomeText.text + "+";

        if (militaryDif > 0) militaryOutcomeText.text = militaryOutcomeText.text + "+";
        else if (militaryDif < 0) militaryOutcomeText.text = militaryOutcomeText.text;
        else militaryOutcomeText.text = militaryOutcomeText.text + "+";

        if (depressionDif > 0) depressionOutcomeText.text = depressionOutcomeText.text + "+";
        else if (depressionDif < 0) depressionOutcomeText.text = depressionOutcomeText.text;
        else depressionOutcomeText.text = depressionOutcomeText.text + "+";
        
        //Change text
        moneyOutcomeText.text += moneyDif;
        militaryOutcomeText.text += militaryDif;
        depressionOutcomeText.text += depressionDif;
    }

    /// <summary>
    /// Calculate outcome for this day
    /// </summary>
    private void calculateOutcomeValues()
    {
        moneyDif = (_playerAttributes.money - PlayerAttributes.newDayMoney);
        militaryDif = (_playerAttributes.military - PlayerAttributes.newDayMilitary);
        depressionDif = (_playerAttributes.depression - PlayerAttributes.newDayDepression);

        //Set new day money to current day's money
        PlayerAttributes.newDayMoney = _playerAttributes.money;
        PlayerAttributes.newDayMilitary = _playerAttributes.military;
        PlayerAttributes.newDayDepression = _playerAttributes.depression;
    }

    private IEnumerator startFadingSequence()
    {
        fadeDayNumber();
        yield return new WaitForSeconds(3f);
        finalFade();
        yield return new WaitForSeconds(3f);
        fadeAllComponents();
        yield return new WaitForSeconds(3f);

        //Finishing code
        _controllerLogic.nextCharacter();
    }

    private void fadeAllComponents()
    {
		KDMoodMusicPlayer musicPlayer = musicController.GetComponent<KDMoodMusicPlayer> ();
		musicPlayer.stopAll();
		musicPlayer.startAll ();
		//musicPlayer.fadeInTrack(0);
		musicPlayer.transitionMood(ResourceManager.instance.getPlayerAttributes().depression);

        Image curtain = GetComponent<CurtainActivate>().curtains;

        //Fade out text
        StartCoroutine(fadeOut(new Color(dayNumberText.color.r, dayNumberText.color.g, dayNumberText.color.b, 1), dayNumberText));
        StartCoroutine(fadeOut(new Color(moneyOutcomeText.color.r, moneyOutcomeText.color.g, moneyOutcomeText.color.b, 1), moneyOutcomeText));
        StartCoroutine(fadeOut(new Color(militaryOutcomeText.color.r, militaryOutcomeText.color.g, militaryOutcomeText.color.b, 1), militaryOutcomeText));
        StartCoroutine(fadeOut(new Color(depressionOutcomeText.color.r, depressionOutcomeText.color.g, depressionOutcomeText.color.b, 1), depressionOutcomeText));
        //Fade out curtains
        StartCoroutine(fadeOut(new Color(curtain.color.r, curtain.color.g, curtain.color.b, 1), curtain));
    }

    private IEnumerator fadeIn(Color theCol, Component theCom)
    {
        float alpha = 0;

        Image img = theCom as Image;
        Text txt = theCom as Text;

        while (alpha < 1)
        {
            alpha += .05f;

            theCol.a = alpha;



            if (img) img.color = theCol;
            else if (txt) txt.color = theCol;

            yield return new WaitForSeconds(.02f);
        }
        
    }

    private IEnumerator fadeOut(Color theCol, Component theCom)
    {
        float alpha = 1;

        Image img = theCom as Image;
        Text txt = theCom as Text;
        
        while (alpha > 0)
        {
            alpha -= .05f;

            theCol.a = alpha;

            if (img) img.color = theCol;
            else if (txt) txt.color = theCol;

            yield return new WaitForSeconds(.02f);

        }
    }

}
