using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct PlayerAudioData {
	public int fadeDir;
	public float transitionDuration;
	public float fadeTimer;
}

public class ParallelMusicPlayer : MonoBehaviour {

	public List<AudioClip> sounds;
	public List<AudioSource> sources;

	public PlayerAudioData[] additionalAudioData;

	public bool playAllOnAwake = false;

	public float target_vol;

	public float fade_percent = 1;

	// Use this for initialization
	void Start () {
	
		sources = new List<AudioSource> ();

		additionalAudioData = new PlayerAudioData[sounds.Count];

		for (int i = 0; i < sounds.Count; i++) {
			AudioSource theSource = transform.gameObject.AddComponent<AudioSource>();
			sources.Add (theSource);
			sources[i].clip = sounds[i];
			sources[i].volume = 0;
			additionalAudioData [i] = new PlayerAudioData ();
			if (playAllOnAwake) {
				sources [i].Play ();
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < sources.Count; i++) {
			if(additionalAudioData[i].fadeDir != 0) {
				float fade_amount = target_vol / 100 * fade_percent * additionalAudioData[i].fadeDir;
				float current_vol = sources[i].volume;
				current_vol += fade_amount;
				if(current_vol > target_vol) {
					current_vol = target_vol;	
				} else if(current_vol < 0) {
					current_vol = 0;
				}
				if (current_vol == target_vol || current_vol == 0) {
					additionalAudioData[i].fadeDir = 0;
				}
				sources[i].volume = current_vol;
			}
		}
	}

	public void startAll() {
		for (int i = 0; i < sounds.Count; i++) {
			sources [i].Play ();
		}
	}

	public void stopAll() {
		for (int i = 0; i < sounds.Count; i++) {
			sources [i].Stop ();
		}
	}

	public void fadeOutTrack(int index) {
		additionalAudioData[index].fadeDir = -1;
	}

	//public override void fadeOutTrack(int index, float duration) {
	//	additionalAudioData[index].fadeDir = -1;
	//}

	public void fadeInTrack(int index) {
		additionalAudioData[index].fadeDir = 1;
	}

	//public override void fadeInTrack(int index, float duration) {
	//	additionalAudioData[index].fadeDir = 1;
	//}
}
