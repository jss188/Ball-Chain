﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float lifeTime = 0.5f;
	public float speed = 100f;
	public int playerNumber = -1;

	public float playerPushAmount = 1f;
	public float stunAmount = 1f;

	void Awake() {
		Destroy(gameObject, lifeTime);
	}

	void Update() {
		transform.position += transform.up * speed * Time.deltaTime;
	}

	void OnCollisionEnter(Collision col) {
		EnemyHealth enemy;
		if (col.gameObject.CompareTag("Enemy") && (enemy = col.gameObject.GetComponent<EnemyHealth>()) != null) {
			enemy.health--;
			enemy.lastHarmed = playerNumber;
		}
		if(col.gameObject.layer == LayerMask.NameToLayer("Player")) {
			PlayerMain hitPlayer = col.gameObject.GetComponent<PlayerMain>();
			Vector3 pushBackForce = transform.up * playerPushAmount * hitPlayer.scoring.HitMultiplier() * 0.1f;
			hitPlayer.movement.PushBackPlayer(pushBackForce, stunAmount);
		}

		Destroy(gameObject);
	}
	
}
