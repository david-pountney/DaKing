using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOver : AnimatedMenu {

    public Text gameOverText;
    public int waitBeforeFadingOut;
    public int waitBeforeExitingGame;

    /// <summary>
    /// Entry point to kick start the script
    /// </summary>
    public override void startFadingIn()
    {
        Color newRed = new Color(204.0f / 255.0f, 45.0f / 255.0f, 45.0f / 255.0f, 0);

        StartCoroutine(fadeIn(newRed, gameOverText));
    }

    private IEnumerator fadeIn(Color theCol, Component theCom)
    {
        float alpha = 0;

        Image img = theCom as Image;
        Text txt = theCom as Text;

        while (alpha < 1)
        {
            alpha += .05f;

            theCol.a = alpha;

            if (img) img.color = theCol;
            else if (txt) txt.color = theCol;

            yield return new WaitForSeconds(.02f);
        }

        yield return new WaitForSeconds(waitBeforeFadingOut);

        StartCoroutine(fadeOut(theCol, theCom));
    }

    private IEnumerator fadeOut(Color theCol, Component theCom)
    {
        float alpha = 1;

        Image img = theCom as Image;
        Text txt = theCom as Text;

        while (alpha > 0)
        {
            alpha -= .05f;

            theCol.a = alpha;

            if (img) img.color = theCol;
            else if (txt) txt.color = theCol;

            yield return new WaitForSeconds(.02f);

        }

        yield return new WaitForSeconds(4);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
