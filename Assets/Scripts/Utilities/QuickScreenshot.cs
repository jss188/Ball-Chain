using UnityEngine;
using System.Collections;

public class QuickScreenshot : MonoBehaviour {
	
	private static int screenshotNumber = 0;
	public KeyCode screenKey = KeyCode.ScrollLock;
	public int supersize = 2;
	public static int counter = 0;
	void Update () {
		if(Input.GetKeyDown(screenKey)){
			Application.CaptureScreenshot("Capture " + (counter++) + ".png", supersize);
		}
	}

}
