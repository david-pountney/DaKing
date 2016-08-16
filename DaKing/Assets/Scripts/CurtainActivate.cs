using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CurtainActivate : MonoBehaviour {

    public Image curtains;
    public AnimatedMenu menu;

    public GameObject musicController;

    public int waitBeforeFade;

    public void startEndDay()
    {
        if(musicController)
            musicController.GetComponent<SimpleMusicController>().fade_out();

        StartCoroutine(fadeIn(new Color(0,0,0), curtains));
    }

    private IEnumerator fadeIn(Color theCol, Component theCom  )
    {
        yield return new WaitForSeconds(waitBeforeFade);
        
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
        
        yield return new WaitForSeconds(2);

        menu.startFadingIn();

        //StartCoroutine(fadeOut(theCol, theCom));
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


    }
}
