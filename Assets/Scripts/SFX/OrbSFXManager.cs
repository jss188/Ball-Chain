using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSFXManager : MonoBehaviour {

	private static OrbSFXManager main;

	public AudioSource orbPickUpSound;
	public AudioClip pickUpOrbSound, dropOrbSound;

	void Awake() {
		main = this;
	}

	public static void PlayOrbPickUpSound() {
		main.orbPickUpSound.clip = main.pickUpOrbSound;
		main.PlayOrbSoundInstance();
	}

	public static void PlayOrbDropSound() {
		main.orbPickUpSound.clip = main.dropOrbSound;
		main.PlayOrbSoundInstance();
	}

	public void PlayOrbSoundInstance() {
		orbPickUpSound.Stop();
		orbPickUpSound.Play();
	}

}
