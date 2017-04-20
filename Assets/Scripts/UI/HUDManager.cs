using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

	public PlayerNameTag nameTagPrefab;
	public RectTransform bottomBar;

	public Text matchTimerText;
	public Text top5Text;

	public Text giantText;
	public Color giantTextColor;
	private Coroutine currentGiantTextMessage;

	public PlayerJoinIndicator joinTag;
	public RectTransform joinBar;
	public RectTransform joinBarHolder;

	public Text scoreTable;

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
		int milliseconds = (int)((floatTime % 1f) * 1000f);

		return string.Format("{0}:{1}:{2}", minutes.ToString().PadLeft(2,'0'), seconds.ToString().PadLeft(2,'0'), milliseconds.ToString().PadLeft(3,'0'));

		//int seconds = (int)floatTime; 
		//return string.Format("{0}:{1}", seconds.ToString().PadLeft(2,'0'), milliseconds.ToString().PadLeft(3,'0'));

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

	public void ShowGiantTextMessage(string message, float fadeInTime, float holdTime, float fadeOutTime) {
		if(currentGiantTextMessage != null)
			StopCoroutine(currentGiantTextMessage);
		currentGiantTextMessage = StartCoroutine(GiantTextMessage(fadeInTime, holdTime, fadeOutTime, message));
	}

	IEnumerator GiantTextMessage(float fadeInTime, float holdTime, float fadeOutTime, string message) {
		giantText.gameObject.SetActive(true);
		giantText.text = message;
		giantText.color = Color.clear;

		float timer = 0f;
		//Fade In
		while (timer < fadeInTime) {
			timer += Time.deltaTime;
			giantText.color = Color.Lerp(Color.clear, giantTextColor, timer/fadeInTime);
			yield return new WaitForEndOfFrame();
		}

		//Hold
		giantText.color = giantTextColor;
		yield return new WaitForSeconds(holdTime);

		//Fade Out
		timer = 0;
		while (timer < fadeOutTime) {
			timer += Time.deltaTime;
			giantText.color = Color.Lerp(giantTextColor, Color.clear, timer/fadeOutTime);
			yield return new WaitForEndOfFrame();
		}

		giantText.color = Color.clear;
		giantText.gameObject.SetActive(false);
	}

	public void UpdatePlayerJoinIndicators(MultiplayerManagement gameManager) {
		foreach (Transform child in joinBar)
			Destroy(child.gameObject);

		foreach (JoinPlayerData player in gameManager.joiningPlayers)
			AddPlayerJoinIndicator(player);

	}

	public void AddPlayerJoinIndicator(JoinPlayerData player) {
		PlayerJoinIndicator indic = Instantiate(joinTag, joinBar);
		indic.transform.ResetTransform();
		indic.title.text = "Player " + (player.playerNumber+1);
		indic.outline.color = player.playerColor;
	}

	public void UpdateScoreTable() {
		scoreTable.gameObject.SetActive(true);

		List<PlayerMain> players = MultiplayerManagement.players;
		players.Sort( (p1,p2)=> p2.scoring.score - p1.scoring.score );

		StringBuilder result = new StringBuilder();
		for(int i = 0; i < players.Count; i++) {
			if(i == 0)
				result.AppendLine( string.Format("<color={2}><b>Player {0} - {1} WINNER!</b></color>", players[i].playerNumber+1, players[i].scoring.score*10, ColorTypeConverter.ToRGBHex(players[i].playerColor)) );
			else
				result.AppendLine( string.Format("<color={2}>Player {0} - {1}</color>", players[i].playerNumber+1, players[i].scoring.score*10, ColorTypeConverter.ToRGBHex(players[i].playerColor)) );
		}

		scoreTable.text = result.ToString();
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