using UnityEngine;
using System.Collections;

public class QuickRotate : MonoBehaviour {

	public Vector3 rotationAngles = Vector3.zero;
	public Space rotationSpace = Space.World;

	void Update () {
		transform.Rotate(rotationAngles * Time.deltaTime, rotationSpace);
	}
}
