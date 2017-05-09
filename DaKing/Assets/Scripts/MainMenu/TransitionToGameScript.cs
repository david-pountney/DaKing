using UnityEngine;
using System.Collections;

public class TransitionToGameScript : MonoBehaviour
{

    [Tooltip("The script responcible for starting the game")]
    private ControllerBehaviour controllerBehaviour;

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

    [Header("Audio Options")]

    [Tooltip("Sound clip that plays when the game starts")]
    public AudioClip audioClip;

    [Tooltip("The object which we reference to know where to animate the main camera to when the game is started")]
    private Transform gameCameraPosition;

    [Tooltip("The main menu canvas")]
    private CanvasGroup mainMenuCanvasGroup;

    private AudioSource audioSource;

    //Is the main menu enabled? 
    private bool mainMenuEnabled =  true;

    void Awake()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        controllerBehaviour = GlobalReferencesBehaviour.instance.SceneData.controller.GetComponent<ControllerBehaviour>();
        gameCameraPosition = GlobalReferencesBehaviour.instance.SceneData.gameCameraPosition.transform;
        mainMenuCanvasGroup = transform.parent.GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// When we start the game by pressing the start button
    /// </summary>
    public void OnPressed()
    {
        if (mainMenuEnabled)
        {
            //if( !ResourceManager.instance.hasJsonLoaded() ) return;
            //Turn off main menu
            mainMenuEnabled = false;

            //Play starting game sound
            audioSource.PlayOneShot(audioClip);

            //Fade main menu out
            StartCoroutine(FadeOut());

            //Start reading in characters json files
            controllerBehaviour.GetComponent<GenerateCharactersByJSONBehaviour>().GameMasterLogic.Init();
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
        controllerBehaviour.ControllerLogic.Init();

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
