using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoring : MonoBehaviour {

	public int orbsCollected = 0;

	public void AddOrb() {
		orbsCollected++;
	}

	public void DropChain() {
		orbsCollected = 0;
	}

	public float HitMultiplier() {
		return orbsCollected;
	}
}
