using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashTextControllerLogic
{
    public Image UiPanel { get { return _uiPanel; } set { _uiPanel = value; } }
    public Text FlashMoneyText { get { return _flashMoneyText; } set { _flashMoneyText = value; } }
    public Text FlashMilitaryText { get { return _flashMilitaryText; } set { _flashMilitaryText = value; } }
    public Image FlashMoodImage { get { return _flashMoodImage; } set { _flashMoodImage = value; } }
    public Sprite FlashMoodUpSprite { get { return _flashMoodUpSprite; } set { _flashMoodUpSprite = value; } }
    public Sprite FlashMoodSameSprite { get { return _flashMoodSameSprite; } set { _flashMoodSameSprite = value; } }
    public Sprite FlashMoodDownSprite { get { return _flashMoodDownSprite; } set { _flashMoodDownSprite = value; } }
    public FlashTextControllerBehaviour FlashTextControllerBehaviour { get { return _flashTextControllerBehaviour; } set { _flashTextControllerBehaviour = value; } }
    public Color GoodColor { get { return _goodColor; } set { _goodColor = value; } }
    public Color BadColor { get { return _badColor; } set { _badColor = value; } }
    
    private Image _uiPanel;
    private Text _flashMoneyText;
    private Text _flashMilitaryText;
    private Image _flashMoodImage;
    private Sprite _flashMoodUpSprite;
    private Sprite _flashMoodSameSprite;
    private Sprite _flashMoodDownSprite;

    private float _timeBetweenFade;

    private Color _goodColor;
    private Color _badColor;

    private FlashTextControllerBehaviour _flashTextControllerBehaviour;

    public void flash(int moneyAmount, int militaryAmount, int depressionAmount)
    {
        Color moneyCol = Color.black;
        Color militaryCol = Color.black;
        Color depressionCol = Color.black;

        //Money
        if (moneyAmount > 0)
        {
            _flashMoneyText.text = "+" + moneyAmount.ToString();
            moneyCol = _goodColor;
        }
        if (moneyAmount < 0)
        {
            _flashMoneyText.text = moneyAmount.ToString();
            moneyCol = _badColor;
        }
        else if (moneyAmount == 0)
        {
            _flashMoneyText.text = moneyAmount.ToString();
            moneyCol = Color.black;
        }

        if (militaryAmount > 0)
        {
            _flashMilitaryText.text = "+" + militaryAmount.ToString();
            militaryCol = _goodColor;
        }
        if (militaryAmount < 0)
        {
            _flashMilitaryText.text = militaryAmount.ToString();
            militaryCol = _badColor;
        }
        else if (militaryAmount == 0)
        {
            _flashMilitaryText.text = militaryAmount.ToString();
            militaryCol = Color.black;
        }

        if (depressionAmount > 0)
        {
            _flashMoodImage.GetComponent<Image>().sprite = _flashMoodUpSprite;
            depressionCol = _goodColor;
        }
        if (depressionAmount < 0)
        {
            _flashMoodImage.GetComponent<Image>().sprite = _flashMoodDownSprite;
            depressionCol = _badColor;
        }
        else if (depressionAmount == 0)
        {
            _flashMoodImage.GetComponent<Image>().sprite = _flashMoodSameSprite;
            depressionCol = Color.black;
        }

        _flashTextControllerBehaviour.Callback_StartFadingIn(moneyCol, militaryCol, depressionCol);
    }

    public void SetAlphaToZero()
    {
        _uiPanel.color = new Color(_uiPanel.color.r,
                                  _uiPanel.color.g,
                                  _uiPanel.color.b,
                                  0f);

        _flashMoneyText.color = new Color(_flashMoneyText.color.r,
                                         _flashMoneyText.color.g,
                                         _flashMoneyText.color.b,
                                         0f);

        _flashMilitaryText.color = new Color(_flashMilitaryText.color.r,
                                            _flashMilitaryText.color.g,
                                            _flashMilitaryText.color.b,
                                            0f);

        _flashMoodImage.color = new Color(_flashMoodImage.color.r,
                                               _flashMoodImage.color.g,
                                               _flashMoodImage.color.b,
                                               0f);
    }

    public IEnumerator fadeIn(Color monC, Color milC, Color depC)
    {
        float alpha = 0;
        Color moneyCol = new Color(monC.r, monC.g, monC.b, alpha);
        Color militaryCol = new Color(milC.r, milC.g, milC.b, alpha);
        Color depressionCol = new Color(depC.r, depC.g, depC.b, alpha);
        Color uiPanelColor = new Color(depC.r, depC.g, depC.b, alpha);

        _flashMoneyText.color = moneyCol;
        _flashMilitaryText.color = militaryCol;

        while (_flashMoneyText.color.a < 1)
        {
            alpha += .05f;

            moneyCol.a = alpha;
            militaryCol.a = alpha;
            depressionCol.a = alpha;
            _flashMoodImage.color = new Color(alpha, alpha, alpha, alpha);

            if(alpha <= .392)
                _uiPanel.color = new Color(_uiPanel.color.r, _uiPanel.color.g, _uiPanel.color.b, alpha);

            _flashMoneyText.color = moneyCol;
            _flashMilitaryText.color = militaryCol;

            yield return new WaitForSeconds(_timeBetweenFade);
        }

        yield return new WaitForSeconds(3);

        _flashTextControllerBehaviour.Callback_StartFadingOut(moneyCol, militaryCol, depressionCol);
    }

    public IEnumerator fadeOut(Color monC, Color milC, Color depC)
    {
        float alpha = 1;
        Color moneyCol = _flashMoneyText.color = new Color(monC.r, monC.g, monC.b, alpha);
        Color militaryCol = _flashMoneyText.color = new Color(milC.r, milC.g, milC.b, alpha);
        Color depressionCol = _flashMoneyText.color = new Color(depC.r, depC.g, depC.b, alpha);

        while (_flashMoneyText.color.a > 0)
        {
            alpha -= .05f;

            moneyCol.a = alpha;
            militaryCol.a = alpha;
            depressionCol.a = alpha;
            _flashMoodImage.color = new Color(alpha, alpha, alpha, alpha);

            if (alpha <= .392)
                _uiPanel.color = new Color(_uiPanel.color.r, _uiPanel.color.g, _uiPanel.color.b, alpha);

            _flashMoneyText.color = moneyCol;
            _flashMilitaryText.color = militaryCol;

            yield return new WaitForSeconds(_timeBetweenFade);
        }


    }
}
