using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicSelector : MonoBehaviour {

	private AudioSource musicPlayer;

	public AudioClip[] musicTracks;

	void Awake() {
		musicPlayer = GetComponent<AudioSource>();
		if(musicTracks.Length > 0) {
			musicPlayer.clip = musicTracks[ Random.Range(0, musicTracks.Length) ];
			musicPlayer.Play();
		}
	}


}
