using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuButton : MonoBehaviour {
	public Button button;
	public Text text;
	public bool alwaysUpdateTitle = false;
	public OptionsMenuManager.TextUpdate textUpdate;

	void Update() {
		if(alwaysUpdateTitle)
			text.text = textUpdate.Invoke();
	}
}
