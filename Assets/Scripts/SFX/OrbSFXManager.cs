using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSFXManager : MonoBehaviour {

	public AudioSource orbPickUpSound;
	private static OrbSFXManager main;

	void Awake() {
		main = this;
	}

	public static void PlayOrbPickUpSound() {
		main.PlayOrbPickUpSoundInstance();
	}

	public void PlayOrbPickUpSoundInstance() {
		orbPickUpSound.Stop();
		orbPickUpSound.Play();
	}

}
