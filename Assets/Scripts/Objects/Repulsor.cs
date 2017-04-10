using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Repulsor : MonoBehaviour {

	public static List<Repulsor> repulsors = new List<Repulsor>();
	public float pushForceScale = 1f;

	void Start() {
		repulsors.Add(this);

		int i = 0;
		while (i < repulsors.Count) {
			if (repulsors[i] == null)
				repulsors.RemoveAt(i);
			else
				i++;
		}
	}

}
