using UnityEngine;
using System.Collections;

public class KDMoodMusicPlayer : ParallelMusicPlayer {

	public int currentMoodIndex;

	public int getNewMoodIndex(float mood) {
		float newMood = mood / 100f;
		if (newMood >= 0.5f) {
			return 0;
		} else if (newMood < 0.5f && newMood > 0.25f) {
			return 1;
		} else {
			return 2;
		}
	}

	public void transitionMood(float mood) {
		int newMoodIndex = getNewMoodIndex (mood);
		base.fadeOutTrack (currentMoodIndex);
		base.fadeInTrack (newMoodIndex);
		currentMoodIndex = newMoodIndex;
	}

	public void setMoodIndex(float mood) {
		currentMoodIndex = getNewMoodIndex (mood);
	}

}
