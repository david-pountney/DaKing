using UnityEngine;
using System.Collections;

public class PlayAnimationScript : MonoBehaviour {

    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        animator.Play("Entry");
    }
}
