using UnityEngine;
using System.Collections;
using System;

public class UIControllerBehaviour : MonoBehaviour {

    private UIControllerLogic _uiControllerLogic;

    void Awake()
    {
        _uiControllerLogic = new UIControllerLogic();


    }

    void Start()
    {
        Setup();
    }

    private void Setup()
    {
        _uiControllerLogic.LoadingUI = GlobalReferencesBehaviour.instance.SceneData.loadingui;
        _uiControllerLogic.MainMenuCanvas = GlobalReferencesBehaviour.instance.SceneData.menuui;
        _uiControllerLogic.GameUI = GlobalReferencesBehaviour.instance.SceneData.gameui;

        _uiControllerLogic.FadeOutGameUI();
    }

    public void EventCallback_FinishedLoading()
    {
        _uiControllerLogic.DisableLoadingScreen();
        _uiControllerLogic.EnableMainMenuCanvas();
    }


}
