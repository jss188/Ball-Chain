using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utility : MonoBehaviour {

	public static bool clickOnScreenWorld(Camera cam, Vector2 mousePosition, float distance, out RaycastHit hit, LayerMask mask){
		Ray ray = cam.ScreenPointToRay(mousePosition);
		return Physics.Raycast(ray, out hit, distance, mask);
	}

	public static int roundFloatToMultiple(float value, float divisions){
		return Mathf.RoundToInt( (float)Mathf.Abs(value) / Mathf.Abs(divisions) );
	}

	public static Vector3 editorGetCameraForward(Camera cameraView){
		float cameraDot = Vector3.Dot(Vector3.up, cameraView.transform.forward);
		if(Mathf.Abs(cameraDot) <= 0.9999f){
			//Z Part
			Vector3 zDirection = cameraView.transform.forward - Vector3.up * Vector3.Dot(cameraView.transform.forward, Vector3.up);
			zDirection *= (cameraDot < 0) ? 1 : -1;
			return zDirection.normalized;
		}
		else{
			return cameraView.transform.up;
		}
	}

	public static Vector3 editorGetCameraRight(Camera cameraView){
		float cameraDot = Vector3.Dot(Vector3.up, cameraView.transform.forward);
		if(Mathf.Abs(cameraDot) <= 0.9999f){
			return Vector3.Cross(Vector3.up, cameraView.transform.forward).normalized;
		}
		else{
			return cameraView.transform.right;
		}
	}

	public static string formatSecondsToTime(float totalTime) {
		int minutes = (int)totalTime / 60;
		int seconds = (int)totalTime % 60;
		int milliseconds = (int)((totalTime % 1f) * 1000f);

		return string.Format("{0:D2}\'{1:D2}\"{2:D3}", minutes, seconds, milliseconds);
	}
}

//Stolen from: http://stackoverflow.com/questions/24855908/how-to-dequeue-element-from-a-list
static class ListExtension{
    public static T PopAt<T>(this List<T> list, int index){
        T r = list[index];
        list.RemoveAt(index);
        return r;
    }
}