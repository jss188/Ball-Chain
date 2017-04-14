using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour {

	public PlayerNameTag nameTagPrefab;

	public RectTransform bottomBar;

	public void CreateNameTagForPlayer(PlayerMain player) {
		PlayerNameTag nameTag = Instantiate(nameTagPrefab, bottomBar);
		nameTag.transform.ResetTransform();
		nameTag.SetupNameTag(player);
	}

}

public static class Util {
	public static void ResetTransform (this Transform trans) {
		trans.localPosition = Vector3.zero;
		trans.localRotation = Quaternion.identity;
		trans.localScale = Vector3.one;
	}
}