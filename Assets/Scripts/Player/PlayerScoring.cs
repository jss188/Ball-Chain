using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoring : MonoBehaviour {

	public PlayerMain main;

	public int score = 0;
	public int orbsCollected = 0;

	void Awake() {
		main = GetComponent<PlayerMain>();
	}

	public int AddOrb() {
		if(MultiplayerManagement.main.gameState == GameState.Running) {
			orbsCollected++;
			score += orbsCollected;
		}
		return orbsCollected;
	}

	public void DropChain() {
		if(MultiplayerManagement.main.gameState == GameState.Running)
			orbsCollected = 0;
	}

	public float HitMultiplier() {
		return orbsCollected;
	}
}
