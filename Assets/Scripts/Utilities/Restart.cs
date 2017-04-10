using UnityEngine;
using System.Collections;

public class Restart : MonoBehaviour {

	void Update () {
		if(Input.GetKeyDown(KeyCode.R))
			RestartLevel();
	}

	public static void RestartLevel() {
		Application.LoadLevel(Application.loadedLevel);
	}
}
