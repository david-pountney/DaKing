using UnityEngine;
using System.Collections;

public class TransitionToGameScript : MonoBehaviour
{

    [Tooltip("The script responcible for starting the game")]
    private ControllerLogic controllerLogic;

    [Tooltip("This is the game canvas that draws the in game UI")]
    public CanvasGroup gameCanvas;

    [Header("Timer Options")]

    [Tooltip("Time to animate into the game after the start button is selected")]
    public float timeToAnimateIntoGameScreen = 3f;

    [Tooltip("Time to animate into the game after the start button is selected")]
    public float timeToAnimateFadeOutMainMenu = .75f;

    [Tooltip("Time to wait before animating the camera to the game screen")]
    public float timeToDelayAnimatingOntoGameScreen = 1f;

    [Tooltip("Time to animate the game menu fading in")]
    public float timeToAnimateGameScreenFadingIn = 1f;

    [Tooltip("The object which we reference to know where to animate the main camera to when the game is started")]
    private Transform gameCameraPosition;

    [Tooltip("The main menu canvas")]
    private CanvasGroup mainMenuCanvasGroup;

    //Is the main menu enabled? 
    private bool mainMenuEnabled =  true;

    // Use this for initialization
    void Start()
    {
        controllerLogic = GameObject.Find("Controller").GetComponent<ControllerLogic>();
        gameCameraPosition = GameObject.Find("GameCameraPosition").transform;
        mainMenuCanvasGroup = transform.parent.GetComponent<CanvasGroup>();
    }

    void OnLevelWasLoaded()
    {
        controllerLogic = GameObject.Find("Controller").GetComponent<ControllerLogic>();
        gameCameraPosition = GameObject.Find("GameCameraPosition").transform;
        mainMenuCanvasGroup = transform.parent.GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// When we start the game by pressing the start button
    /// </summary>
    public void OnPressed()
    {
        if (mainMenuEnabled)
        {
            //Turn off main menu
            mainMenuEnabled = false;

            //Fade main menu out
            StartCoroutine(FadeOut());

            //Start reading in characters json files
            controllerLogic.GetComponent<GameMaster>().Init();
        }
    }

    /// <summary>
    /// Fade out the main menu canvas
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeOut()
    {
        float time = timeToAnimateFadeOutMainMenu;
        while (mainMenuCanvasGroup.alpha > 0)
        {
            mainMenuCanvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }

        iTween.MoveTo(Camera.main.gameObject, iTween.Hash("delay", timeToDelayAnimatingOntoGameScreen,
            "position", new Vector3(gameCameraPosition.position.x, gameCameraPosition.position.y, gameCameraPosition.position.z),
            "time", timeToAnimateIntoGameScreen));

        StartCoroutine(WaitBeforeStartingGame());
    }

    /// <summary>
    /// Called when we want the game to start
    /// </summary>
    public void StartGame()
    {
        //controllerLogic.gameObject.SetActive(true);

        controllerLogic.Init();

        gameCanvas.gameObject.SetActive(true);

        StartCoroutine(AnimateGameMenuFadingIn());
    }

    /// <summary>
    /// HACK: because Itween only calls oncomplete on the object your animating, I manually set another coroutine just to wait the 
    /// same length of time of the animation and then start the controller object
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitBeforeStartingGame()
    {
        yield return new WaitForSeconds(timeToDelayAnimatingOntoGameScreen + timeToAnimateIntoGameScreen);

        StartGame();
    }

    /// <summary>
    /// Animate the game UI menu fading in
    /// </summary>
    /// <returns></returns>
    private IEnumerator AnimateGameMenuFadingIn()
    {
        float time = timeToAnimateGameScreenFadingIn;
        while (gameCanvas.alpha < 1)
        {
            gameCanvas.alpha += Time.deltaTime / time;
            yield return null;
        }
    }
}
