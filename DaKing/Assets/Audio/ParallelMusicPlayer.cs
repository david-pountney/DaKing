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

	public float defaultTransitionTime = 1f;
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
			sources [i].Play ();
		}

	}

}
