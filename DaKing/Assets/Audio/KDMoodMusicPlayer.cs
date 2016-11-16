using UnityEngine;
using System.Collections;

public class KDMoodMusicPlayer : ParallelMusicPlayer {

	public int currentMoodIndex;

	public int getNewMoodIndex(float mood) {
		float newMood = mood / 100f;
		if (newMood >= 0.75f) {
			return 0;
		} else if (newMood < 0.75f && newMood > 0.5f) {
			return 1;
		} else if (newMood < 0.5f && newMood > 0.25f) {
			return 2;
		} else {
			return -1;
		}
	}

	public void transitionMood(float mood) {
		int newMoodIndex = getNewMoodIndex (mood);
		if (currentMoodIndex > -1) {
			base.fadeOutTrack (currentMoodIndex);
		}
		if (newMoodIndex > -1) {
			base.fadeInTrack (newMoodIndex);
		} else {
			base.fadeOutAll ();
		}
		currentMoodIndex = newMoodIndex;
	}

	public void setMoodIndex(float mood) {
		currentMoodIndex = getNewMoodIndex (mood);
	}

}
