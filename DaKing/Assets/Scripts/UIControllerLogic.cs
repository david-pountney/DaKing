using UnityEngine;
using System.Collections;

public class UIControllerLogic {

    public GameObject LoadingUI { get { return _loadingUI; } set { _loadingUI = value; } }
    public GameObject MainMenuCanvas { get { return _mainMenuCanvas; } set { _mainMenuCanvas = value; } }

    private GameObject _loadingUI;
    private GameObject _mainMenuCanvas;

    public void DisableLoadingScreen()
    {
        _loadingUI.SetActive(false);
    }

    public void EnableMainMenuCanvas()
    {
        _mainMenuCanvas.SetActive(true);
    }

}
