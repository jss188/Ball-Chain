using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbManager : MonoBehaviour {
	
	public Orb orbPref;
	public static Orb orbPrefab;

	void Awake() {
		orbPrefab = orbPref;
	}

	public static void PlaceOrbAt(Vector3 position, int playerNumber) {
		Vector3 newPos = new Vector3(position.x, Mathf.Max(position.y, 0f), position.z);
		Orb orb = Instantiate(orbPrefab, newPos, Quaternion.identity * Quaternion.AngleAxis(90, Vector3.right)) as Orb;
		orb.playerNumber = playerNumber;
	}
}
