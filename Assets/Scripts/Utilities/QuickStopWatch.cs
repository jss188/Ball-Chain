using UnityEngine;
using System.Collections;

public class QuickStopWatch : MonoBehaviour {

	public Color textColor;
	public float vOffset = 0;
	public float time = 0;
	public KeyCode recordingKey = KeyCode.T;

	private float startingTime = 0;
	private float finalTime = 0;
	private bool recording = false;

	void Update(){
		if(Input.GetKeyDown(recordingKey)){
			recording = !recording;
			if(recording){
				startingTime = Time.realtimeSinceStartup;
				finalTime = 0;
			}
			else{
				finalTime = Time.realtimeSinceStartup - startingTime;
			}
		}
	}

	void OnGUI()
	{
		int w = Screen.width, h = Screen.height;
		
		GUIStyle style = new GUIStyle();
		
		Rect rect = new Rect(0, vOffset * h, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = textColor; //new Color (0.0f, 0.0f, 0.5f, 1.0f);

		string text;
		if (recording){
			float measuredTime = Time.realtimeSinceStartup - startingTime;
			text = string.Format("{0:0.0} sec", measuredTime);
		}
		else
			text = string.Format("{0:0.0} sec", finalTime);

		GUI.Label(rect, text, style);
	}
}
