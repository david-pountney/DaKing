using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

    public GameObject loadingUI;
    public GameObject mainMenuCanvas;
	
    public void FinishedLoading()
    {
        DisableLoadingScreen();
        EnableMainMenuCanvas();

        Debug.Log("shutting off loading ui");
    }

    private void DisableLoadingScreen()
    {
        loadingUI.SetActive(false);
    }

    private void EnableMainMenuCanvas()
    {
        mainMenuCanvas.SetActive(true);
    }
}
