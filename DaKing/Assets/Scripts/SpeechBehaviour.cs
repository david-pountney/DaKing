using UnityEngine;
using System.Collections;
using System;

public class SpeechBehaviour : MonoBehaviour {

    public SpeechLogic SpeechLogic { get { return _speechLogic; } set { _speechLogic = value; } }

    public float speechBubbleX;
    public float speechBubbleY;
    
    private SpeechLogic _speechLogic;
    private MovementBehaviour _movementLogic;

	// Use this for initialization
	void Awake () {
        _speechLogic = new SpeechLogic();
        _movementLogic = this.GetComponent<MovementBehaviour>();
        
	}

    private void SetUpLogic()
    {
        _speechLogic.ThisTransform = this.transform;
        _speechLogic.MovementLogic = _movementLogic;
        _speechLogic.SpeechInstance = GlobalReferencesBehaviour.instance.SceneData.speechBubble;
        _speechLogic.DialogScript = GetComponent<DeterminDialog>();
        _speechLogic.Choices = GlobalReferencesBehaviour.instance.sceneData.choices.gameObject;
        _speechLogic.GameOver = GlobalReferencesBehaviour.instance.sceneData.gameOver;
        _speechLogic.SpeechBubbleX = speechBubbleX;
        _speechLogic.SpeechBubbleY = speechBubbleY;
        _speechLogic.SoundScript = GlobalReferencesBehaviour.instance.sceneData.soundScript;
    }

    void Start()
    {
        SetUpLogic();

    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            _speechLogic.HandleInput();
    }
	

}
