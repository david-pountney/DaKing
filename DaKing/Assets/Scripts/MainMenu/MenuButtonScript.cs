using UnityEngine;
using System.Collections;

public class MenuButtonScript : MonoBehaviour {

    public AudioClip audioClip;

    private AudioSource audioSource;

    // Use this for initialization
    void Awake () {
        audioSource = Camera.main.GetComponent<AudioSource>();    
	}

    public void OnSelect()
    {
        audioSource.PlayOneShot(audioClip);
    }
}
