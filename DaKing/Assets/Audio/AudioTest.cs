using UnityEngine;
using System.Collections;

public class AudioTest : MonoBehaviour {

	//public SoundDef def;

	public GameObject pageDef;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (pageDef != null && pageDef.GetComponent<SoundDef> () != null) {
				pageDef.GetComponent<SoundDef> ().fire ();
			}
		}

	}
}
