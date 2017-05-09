using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NextDayBehaviour : MonoBehaviour {

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
    private ControllerBehaviour _controllerBehaviour;

    // Use this for initialization
    void Start()
    {
        _playerAttributes = GlobalReferencesBehaviour.instance.SceneData.playerAttributes;
        _controllerBehaviour = GlobalReferencesBehaviour.instance.SceneData.controller.GetComponent<ControllerBehaviour>();
    }
}
