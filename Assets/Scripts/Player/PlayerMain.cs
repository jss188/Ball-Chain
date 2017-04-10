using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour {

	public int playerNumber = 0;

	void Start() {
		VirtualControlManager.SetupDefaultControls(playerNumber);
	}
}
