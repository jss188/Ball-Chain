using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlacer : MonoBehaviour {

	public GameObject enemy;
	public Vector2 spawnRange = new Vector2(16f, 9f);

	void Update() {
		if(Input.GetKeyDown(KeyCode.Alpha2))
			PlaceEnemies(10);
	}

	public void PlaceEnemies(int enemyCount) {
		for(int i = 0; i < enemyCount; i++) {
			Vector3 randomPoint = new Vector3(
				Random.Range(-spawnRange.x, spawnRange.x),
				0f,
				Random.Range(-spawnRange.y, spawnRange.y));

			Instantiate(enemy, randomPoint, Quaternion.identity);
		}
	}

}
