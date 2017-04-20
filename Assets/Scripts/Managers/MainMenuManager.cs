using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

	void Awake() {
		VirtualControlManager.SetAllDefaultPlayerControls();
	}

	public void ExitGame() {
		LoadingFade.LoadScreen(2f, delegate { Application.Quit(); });
	}

	public void GoToLevel(string levelName) {
		LoadingFade.LoadScreen(1f, delegate { SceneManager.LoadScene(levelName); });
	}

}
