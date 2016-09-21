using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashTextController : MonoBehaviour
{

    public Text flashMoneyText;
    public Text flashMilitaryText;
    public Image flashDepressionImage;
    public Sprite flashMoodUpSprite;
    public Sprite flashMoodSameSprite;
    public Sprite flashMoodDownSprite;

    public float timeBetweenFade;

    public void flash(int moneyAmount, int militaryAmount, int depressionAmount)
    {
        Color moneyCol = Color.black;
        Color militaryCol = Color.black;
        Color depressionCol = Color.black;

        //Money
        if (moneyAmount > 0)
        {
            flashMoneyText.text = "+" + moneyAmount.ToString();
            moneyCol = Color.green;
        }
        if (moneyAmount < 0)
        {
            flashMoneyText.text = moneyAmount.ToString();
            moneyCol = Color.red;
        }
        else if (moneyAmount == 0)
        {
            flashMoneyText.text = moneyAmount.ToString();
            moneyCol = Color.black;
        }

        if (militaryAmount > 0)
        {
            flashMilitaryText.text = "+" + militaryAmount.ToString();
            militaryCol = Color.green;
        }
        if (militaryAmount < 0)
        {
            flashMilitaryText.text = militaryAmount.ToString();
            militaryCol = Color.red;
        }
        else if (militaryAmount == 0)
        {
            flashMilitaryText.text = militaryAmount.ToString();
            militaryCol = Color.black;
        }

        if (depressionAmount > 0)
        {
            flashDepressionImage.GetComponent<Image>().sprite = flashMoodUpSprite;
            depressionCol = Color.green;
        }
        if (depressionAmount < 0)
        {
            flashDepressionImage.GetComponent<Image>().sprite = flashMoodDownSprite;
            depressionCol = Color.red;
        }
        else if (depressionAmount == 0)
        {
            flashDepressionImage.GetComponent<Image>().sprite = flashMoodSameSprite;
            depressionCol = Color.black;
        }

        StartCoroutine(fadeIn(moneyCol, militaryCol, depressionCol));
    }

    private IEnumerator fadeIn(Color monC, Color milC, Color depC)
    {
        float alpha = 0;
        Color moneyCol = new Color(monC.r, monC.g, monC.b, alpha);
        Color militaryCol = new Color(milC.r, milC.g, milC.b, alpha);
        Color depressionCol = new Color(depC.r, depC.g, depC.b, alpha);

        flashMoneyText.color = moneyCol;
        flashMilitaryText.color = militaryCol;

        while (flashMoneyText.color.a < 1)
        {
            alpha += .05f;

            moneyCol.a = alpha;
            militaryCol.a = alpha;
            depressionCol.a = alpha;
            flashDepressionImage.GetComponent<Image>().color = new Color(alpha, alpha, alpha, alpha);

            flashMoneyText.color = moneyCol;
            flashMilitaryText.color = militaryCol;

            yield return new WaitForSeconds(timeBetweenFade);
        }

        yield return new WaitForSeconds(3);

        StartCoroutine(fadeOut(moneyCol, militaryCol, depressionCol));
    }

    private IEnumerator fadeOut(Color monC, Color milC, Color depC)
    {
        float alpha = 1;
        Color moneyCol = flashMoneyText.color = new Color(monC.r, monC.g, monC.b, alpha);
        Color militaryCol = flashMoneyText.color = new Color(milC.r, milC.g, milC.b, alpha);
        Color depressionCol = flashMoneyText.color = new Color(depC.r, depC.g, depC.b, alpha);

        while (flashMoneyText.color.a > 0)
        {
            alpha -= .05f;

            moneyCol.a = alpha;
            militaryCol.a = alpha;
            depressionCol.a = alpha;
            flashDepressionImage.GetComponent<Image>().color = new Color(alpha, alpha, alpha, alpha);

            flashMoneyText.color = moneyCol;
            flashMilitaryText.color = militaryCol;

            yield return new WaitForSeconds(timeBetweenFade);
        }


    }
}
