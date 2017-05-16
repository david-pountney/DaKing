using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class FlashTextControllerBehaviour : MonoBehaviour {

    public FlashTextControllerLogic FlashTextController { get { return _flashTextController; } set { _flashTextController = value; } }

    public Image uiPanel;
    public Text flashMoneyText;
    public Text flashMilitaryText;
    public Image flashDepressionImage;

    [Header("Mood Sprite Options")]

    public Sprite flashMoodUpSprite;
    public Sprite flashMoodSameSprite;
    public Sprite flashMoodDownSprite;

    [Header("Color Options")]

    public Color goodColor;
    public Color badColor;

    public float timeBetweenFade;

    private FlashTextControllerLogic _flashTextController;

    void Awake()
    {
        _flashTextController = new FlashTextControllerLogic();
    }

	// Use this for initialization
	void Start () {
        SetUp();
	}

    private void SetUp()
    {
        _flashTextController.FlashTextControllerBehaviour = this;
        _flashTextController.UiPanel = uiPanel;
        _flashTextController.FlashMoneyText = flashMoneyText;
        _flashTextController.FlashMilitaryText = flashMilitaryText;
        _flashTextController.FlashMoodImage = flashDepressionImage;
        _flashTextController.FlashMoodSameSprite = flashMoodSameSprite;
        _flashTextController.FlashMoodUpSprite = flashMoodUpSprite;
        _flashTextController.FlashMoodDownSprite = flashMoodDownSprite;
        _flashTextController.GoodColor = goodColor;
        _flashTextController.BadColor = badColor;

        _flashTextController.SetAlphaToZero();
    }

    public void Callback_StartFadingIn(Color monC, Color milC, Color depC)
    {
        StartCoroutine(_flashTextController.fadeIn(monC, milC, depC));
    }

    public void Callback_StartFadingOut(Color monC, Color milC, Color depC)
    {
        StartCoroutine(_flashTextController.fadeOut(monC, milC, depC));
    }
}
