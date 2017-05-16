using UnityEngine;
using System.Collections;

public class UIControllerLogic {

    public GameObject LoadingUI { get { return _loadingUI; } set { _loadingUI = value; } }
    public GameObject MainMenuCanvas { get { return _mainMenuCanvas; } set { _mainMenuCanvas = value; } }
    public GameObject GameUI { get { return _gameUI; } set { _gameUI = value; } }

    private GameObject _loadingUI;
    private GameObject _mainMenuCanvas;
    private GameObject _gameUI;

    public void FadeOutGameUI()
    {
        _gameUI.GetComponent<CanvasGroup>().alpha = 0f;
    }

    public void StartGame()
    {
        //Start reading in characters json files
        GlobalReferencesBehaviour.instance.SceneData.menuController.GetComponent<TransitionToGameScript>().BeginTransitionToGame();
    }
}
