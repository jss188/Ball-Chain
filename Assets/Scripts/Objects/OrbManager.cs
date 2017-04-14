using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbManager : MonoBehaviour {
	
	public static OrbManager orbManager;
	public Orb orbPref;
	public static Orb orbPrefab;

	public TextMesh floatingScoreText;

	void Awake() {
		orbPrefab = orbPref;
		orbManager = this;
	}

	public static void PlaceOrbAt(Vector3 position, int playerNumber) {
		Vector3 newPos = new Vector3(position.x, Mathf.Max(position.y, 0f), position.z);
		Orb orb = Instantiate(orbPrefab, newPos, Quaternion.identity * Quaternion.AngleAxis(90, Vector3.right)) as Orb;
		orb.playerNumber = playerNumber;
		MeshRenderer orbRend =  orb.GetComponent<MeshRenderer>();
		orbRend.material.SetColor("_TintColor", MultiplayerManagement.GetPlayer(playerNumber).playerColor);
	}

	public static void Place3DText(Vector3 position, int number) {
		orbManager.Place3DTextInstance(position, number);
	}

	public void Place3DTextInstance(Vector3 position, int number) {
		TextMesh textMesh = Instantiate(floatingScoreText, position, Quaternion.Euler(90,0,0));
		textMesh.text = "+" + number*10;
		textMesh.transform.localScale = Vector3.one * Mathf.Lerp(0.5f, 1.25f, Mathf.InverseLerp(0, 25, number));
		textMesh.fontStyle = number >= 15 ? FontStyle.Bold : FontStyle.Normal;
	}
}
