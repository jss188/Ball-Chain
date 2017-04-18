using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

	public PlayerNameTag nameTagPrefab;
	public RectTransform bottomBar;

	public Text matchTimerText;
	public Text top5Text;

	public void CreateNameTagForPlayer(PlayerMain player) {
		PlayerNameTag nameTag = Instantiate(nameTagPrefab, bottomBar);
		nameTag.transform.ResetTransform();
		nameTag.SetupNameTag(player);
	}

	public void UpdateHUDMatchTimer(MultiplayerManagement gameManager) {
		matchTimerText.text = float2Time(gameManager.gameTime);
	}

	public void UpdateHUDTop5Leaderboard(MultiplayerManagement gameManager) {
		top5Text.text = getTop5PlayersText(gameManager);
	}

	public string float2Time(float floatTime) {
		int minutes = (int)floatTime / 60;
		int seconds = (int)floatTime % 60;
		//string milliseconds = (floatTime - (int)floatTime).ToString(F);

		return string.Format("{0}:{1}", minutes.ToString().PadLeft(2,'0'), seconds.ToString().PadLeft(2,'0'));
	}

	public string getTop5PlayersText(MultiplayerManagement gameManager) {
		PlayerScoring[] players = new PlayerScoring[MultiplayerManagement.players.Count];
		int pIndex = 0;
		foreach (PlayerMain player in MultiplayerManagement.players) {
			players[pIndex++] = player.scoring;
		}

		System.Array.Sort(players, (x,y) => y.score - x.score);

		int minPlayerCount = Mathf.Min(MultiplayerManagement.players.Count, 5);
		string leaderBoardText = "High Scores ";
		for (int i = 0; i < minPlayerCount; i++) {
			leaderBoardText += string.Format("<color={0}>Player {1}</color> ({2}) | ", ColorTypeConverter.ToRGBHex(players[i].main.playerColor), players[i].main.playerNumber+1, (players[i].score*10).ToString("F0"));
		}

		return leaderBoardText;
	}

}

public static class Util {
	public static void ResetTransform (this Transform trans) {
		trans.localPosition = Vector3.zero;
		trans.localRotation = Quaternion.identity;
		trans.localScale = Vector3.one;
	}
}

 public static class ColorTypeConverter
 {
     public static string ToRGBHex(Color c)
     {
         return string.Format("#{0:X2}{1:X2}{2:X2}", ToByte(c.r), ToByte(c.g), ToByte(c.b));
     }
 
     private static byte ToByte(float f)
     {
         f = Mathf.Clamp01(f);
         return (byte)(f * 255);
     }
}