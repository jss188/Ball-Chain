using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour {

	public int playerNumber = 0;

	public PlayerScoring scoring;
	public PlayerShooting shooting;
	public PlayerMovement movement;
	public PlayerVFX vfx;

	public Color playerColor = new Color( 144, 242, 222, 255 );

	void Awake() {
		scoring = GetComponent<PlayerScoring>();
		shooting = GetComponent<PlayerShooting>();
		movement = GetComponent<PlayerMovement>();
		vfx = GetComponent<PlayerVFX>();
	}

	void Start() {
		VirtualControlManager.SetupDefaultControls(playerNumber);
		vfx.SetVFXColors(playerColor);
	}

	void OnCollisionEnter(Collision col) {
		if(col.collider.CompareTag("Orb Collector")) {
			Kill();
			scoring.DropChain();
		}
	}

	void Kill() {
		gameObject.SetActive(false);
		Invoke("Respawn", 3);
	}

	void Respawn() {
		transform.position = MultiplayerManagement.GetRespawnPosition();
		movement.rigid.velocity = Vector3.zero;
		gameObject.SetActive(true);
	}
}
