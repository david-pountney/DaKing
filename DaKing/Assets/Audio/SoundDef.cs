using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundDef : MonoBehaviour {
	
	public List<AudioClip> sounds;
	
	public List<AudioSource> sources;
	
	public int numSources;
	
	public float minFrequency = 1, MaxFrequency = 7;
	
	public float[] timers;
	
	public float vol = 1f;

	public bool looping = true;
	
	// Use this for initialization
	void Start () {
		
		timers = new float[numSources];
		
		sources = new List<AudioSource>();
		
		for(int i = 0; i < numSources; i++)
		{
            AudioSource theSource = Camera.main.GetComponent<AudioSource>();//transform.gameObject.AddComponent<AudioSource>();
			sources.Add(theSource);
			sources[i].volume = vol;
			timers[i] = Random.Range(minFrequency, MaxFrequency);
			int soundIndex = (int)Mathf.Floor(Random.Range(0f, sounds.Count));
			sources[i].clip = sounds[soundIndex];
			sources[i].pitch = Random.Range(0.8f, 1.2f);
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		if(looping)
		{
			for(int i = 0; i < sources.Count; i++)
			{
				timers[i] -= Time.deltaTime;
				
				if(timers[i] <= 0)
				{
					int soundIndex = (int)Mathf.Floor(Random.Range(0f, sounds.Count));
					sources[i].clip = sounds[soundIndex];
					sources[i].pitch = Random.Range(0.8f, 1.2f);
					sources[i].volume = Random.Range(0.8f*vol, 1f*vol);
					//Debug.Log (sources[i].volume);
					sources[i].PlayOneShot(sources[i].clip);
					timers[i] = Random.Range(minFrequency, MaxFrequency);
				}
			}
		}
	
	}

	public void fire() 
	{
		int i = Random.Range (0, sources.Count);
		int soundIndex = (int)Mathf.Floor(Random.Range(0f, sounds.Count));
		sources[i].clip = sounds[soundIndex];
		sources[i].pitch = Random.Range(0.8f, 1.2f);
		sources[i].volume = Random.Range(0.8f*vol, 1f*vol);
		//Debug.Log (sources[i].volume);
		sources[i].Play();
	}
}
