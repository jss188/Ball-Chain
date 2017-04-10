using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	public int health = 1;
	public int lastHarmed = -1;

	void Update() {
		if(health <= 0)
			DestroyEnemy();
	}

	void DestroyEnemy(int playerNumber) {
		OrbManager.PlaceOrbAt(transform.position, playerNumber);
		Destroy(gameObject);
	}

	void DestroyEnemy() {
		DestroyEnemy(lastHarmed);
	}
	
}
