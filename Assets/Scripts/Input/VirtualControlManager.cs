using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VirtualControlManager {

	private static List<int> SetupPlayers = new List<int>();

	public static float defaultAxisSensitivity = 1000f;
	public static float defaultAxisGravity = 1000f;
	public static float defaultAxisDeadzone = 0.15f;

	public static void SetupDefaultControls(int playerNumber) {
		//Don't setup controls if already done.
		if (SetupPlayers.Contains(playerNumber))
			return;

		//Movement
		cInput.SetKey("Up" + playerNumber, Keys.W);
		cInput.SetKey("Down" + playerNumber, Keys.S);
		cInput.SetKey("Left" + playerNumber, Keys.A);
		cInput.SetKey("Right" + playerNumber, Keys.D);

		cInput.SetAxis("Horizontal" + playerNumber, "Left"+playerNumber, "Right"+playerNumber, defaultAxisSensitivity, defaultAxisGravity, defaultAxisDeadzone);
		cInput.SetAxis("Vertical" + playerNumber, "Down"+playerNumber, "Up"+playerNumber, defaultAxisSensitivity, defaultAxisGravity, defaultAxisDeadzone);

		//Aiming Input
		cInput.SetKey("AimUp" + playerNumber, Keys.UpArrow);
		cInput.SetKey("AimDown" + playerNumber, Keys.DownArrow);
		cInput.SetKey("AimLeft" + playerNumber, Keys.LeftArrow);
		cInput.SetKey("AimRight" + playerNumber, Keys.RightArrow);

		cInput.SetAxis("AimHorizontal" + playerNumber, "AimLeft"+playerNumber, "AimRight"+playerNumber, defaultAxisSensitivity, defaultAxisGravity, defaultAxisDeadzone);
		cInput.SetAxis("AimVertical" + playerNumber, "AimDown"+playerNumber, "AimUp"+playerNumber, defaultAxisSensitivity, defaultAxisGravity, defaultAxisDeadzone);

		//Firing Input
		cInput.SetKey("Fire1" + playerNumber, Keys.Mouse0);
		cInput.SetKey("Fire2" + playerNumber, Keys.Mouse1);

		cInput.SetKey("Menu" + playerNumber, Keys.Escape);

		//Mark player as setup
		SetupPlayers.Add(playerNumber);
	}

	public static InputData SampleInput(int playerNumber) {
		InputData data = new InputData();

		data.Horizontal = cInput.GetAxis("Horizontal"+playerNumber);
		data.Vertical = cInput.GetAxis("Vertical"+playerNumber);
		data.AimHorizontal = cInput.GetAxis("AimHorizontal"+playerNumber);
		data.AimVertical = cInput.GetAxis("AimVertical"+playerNumber);

		data.Fire1 = cInput.GetKey("Fire1"+playerNumber);
		data.Fire2 = cInput.GetKey("Fire2"+playerNumber);

		data.Menu = cInput.GetKey("Menu"+playerNumber);

		return data;
	}

}

public class InputData {
	public float Horizontal, Vertical, AimHorizontal, AimVertical;
	public bool Fire1, Fire2, Menu;
}