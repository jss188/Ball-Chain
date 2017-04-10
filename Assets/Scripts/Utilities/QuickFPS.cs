using UnityEngine;
using System.Collections;

public class QuickFPS : MonoBehaviour {

	public Color textColor;
	public float vOffset = 0;
	float deltaTime = 0.0f;

	private GUIStyle style;
	private Rect rect;
	private int w, h;

	public static bool FPSEnabled = false;

	void Update() {
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

		if(Screen.width != w || Screen.height != h)
			makeNewStyle();

	}
	
	void Awake() {
		makeNewStyle();
	}

	void OnGUI() {
		if(FPSEnabled) {
			float msec = deltaTime * 1000.0f;
			float fps = 1.0f / deltaTime;
			string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
			GUI.Label(rect, text, style);
		}
	}

	/*
	void OnGUI()
	{
		//if(!DevFeatures.specialCommandsEnabled)
		//	return;

		if(Screen.width != w || Screen.height != h)
			makeNewStyle();

		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		GUI.Label(rect, text, style);
	}
	*/

	void makeNewStyle() {
		w = Screen.width;
		h = Screen.height;
		
		style = new GUIStyle();
		
		rect = new Rect(0, vOffset * h, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = textColor; //new Color (0.0f, 0.0f, 0.5f, 1.0f);
	}
}