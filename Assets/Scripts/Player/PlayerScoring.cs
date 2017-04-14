using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoring : MonoBehaviour {

	public int score = 0;
	public int orbsCollected = 0;

	public int AddOrb() {
		orbsCollected++;
		score += orbsCollected;
		return orbsCollected;
	}

	public void DropChain() {
		orbsCollected = 0;
	}

	public float HitMultiplier() {
		return orbsCollected;
	}
}
