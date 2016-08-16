using UnityEngine;
using System.Collections;

public class SimpleMusicController : MonoBehaviour {

	public AudioSource source;

	public int fade_dir = 0;

	public float target_vol;

	public float fade_percent = 1;

	// Use this for initialization
	void Start () {
	
		target_vol = source.volume;
		source.volume = 0;

		//fade_in ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if(fade_dir != 0) {
			float fade_amount = target_vol / 100 * fade_percent * fade_dir;
			float current_vol = source.volume;
			current_vol += fade_amount;
			if(current_vol > target_vol) {
				current_vol = target_vol;	
			} else if(current_vol < 0) {
				current_vol = 0;
			}
			if (current_vol == target_vol || current_vol == 0) {
				fade_dir = 0;
			}
			source.volume = current_vol;
		}

	}

	public void fade_in() {
		fade_dir = 1;
	}

	public void fade_out() {
		fade_dir = -1;
	}
}
