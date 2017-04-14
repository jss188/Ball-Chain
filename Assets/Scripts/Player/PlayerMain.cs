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
}
