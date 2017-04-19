using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoadingFade : MonoBehaviour {

	public Material fader;
	
	public void FadeAction(float fadeTime, UnityAction action) {
		StartCoroutine(fadeThenAction(fadeTime, action));
	}

	IEnumerator fadeThenAction(float fadeTime, UnityAction action) {
		float timer = 0;

		while(timer < fadeTime) {
			timer += Time.deltaTime;
			fader.SetFloat("_FadeAmount", timer/fadeTime);
			yield return new WaitForEndOfFrame();
		}

		fader.SetFloat("_FadeAmount", 0f);
		if(action != null)
			action();

	}

	public static void LoadScreen(float fadeTime, UnityAction action) {
		LoadingFade faderPrefab = Resources.Load<LoadingFade>("LoadingCamera");
		LoadingFade fader = Instantiate(faderPrefab);
		fader.FadeAction(fadeTime, action);
	}

}
