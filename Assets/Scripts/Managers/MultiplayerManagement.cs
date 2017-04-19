using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { WaitForStart, Running, GameEnded }
public class MultiplayerManagement : MonoBehaviour {

	public static MultiplayerManagement main;
	public PlayerMain playerPrefab;

	public Transform[] spawnLocations;
	private static int CurrentSpawnLocation = 0;

	public static List<PlayerMain> players = new List<PlayerMain>();
	public int playerCount;

	public GameState gameState = GameState.WaitForStart;

	public Color[] playerColors = new Color[]{Color.red, Color.yellow, Color.magenta};

	private HUDManager hud;

	public int testInitPlayerCount = 8;

	public float gameTime = 100f;

	private MusicSelector musicPlayer;

	public int StartMatchCountDownSeconds = 5;
	private bool endingMatch = false;

	void Awake() {
		main = this; //Poorly implemented singleton thing, I guess.
		hud = FindObjectOfType<HUDManager>();
		musicPlayer = GetComponentInChildren<MusicSelector>();
	}

	void Start() {
		//StartCoroutine(StartMatchCountDown(StartMatchCountDownSeconds));
		hud.ShowGiantTextMessage("Press ~ to start match!", 1f, 10f, 1f);
	}

	void Update() {
		playerCount = players.Count;

		switch(gameState) {

			case GameState.WaitForStart:
				if(Input.GetKeyDown(KeyCode.BackQuote))
					StartCoroutine(StartMatchCountDown(StartMatchCountDownSeconds));
				break;

			case GameState.Running:
				MatchUpdate();
				break;

			case GameState.GameEnded:
				if(!endingMatch) {
					EndMatch();
				}
				break;
		}

	}

	IEnumerator StartMatchCountDown(int matchCountDownSeconds) {
		int countDown = matchCountDownSeconds;
		while (countDown > 0) {
			hud.ShowGiantTextMessage(countDown.ToString(), 0, 0.7f, 0.2f);
			countDown--;
			yield return new WaitForSeconds(1);
		}

		hud.ShowGiantTextMessage("GO!", 0, 0.7f, 0.2f);

		StartMatch();
	}

	void StartMatch() {
		musicPlayer.SelectTrackAndPlay();
		gameState = GameState.Running;

		ResetManager();

		for (int i = 0; i < testInitPlayerCount; i++) {
			CreatePlayer();
		}
	}

	void ResetManager() {
		players = new List<PlayerMain>();
		CurrentSpawnLocation = 0;
	}

	void EndMatch() {
		endingMatch = true;
		LoadingFade.LoadScreen(1f, delegate { SceneManager.LoadScene("MainMenu"); });
	}

	public void MatchUpdate() {
		gameTime -= Time.deltaTime;

		hud.UpdateHUDMatchTimer(this);
		hud.UpdateHUDTop5Leaderboard(this);

		if(gameTime <= 0f)
			gameState = GameState.GameEnded;
	}

	public void CreatePlayer() {
		PlayerMain player = Instantiate(playerPrefab, spawnLocations[SelectSpawnPointIndex()].position, Quaternion.identity);
		player.playerNumber = players.Count;
		player.playerColor = playerColors[ Random.Range(0, playerColors.Length) ];
		players.Add(player);

		hud.CreateNameTagForPlayer(player);
	}

	public static Vector3 GetRespawnPosition() {
		return main.spawnLocations[main.SelectSpawnPointIndex()].position;
	}

	public int SelectSpawnPointIndex() {
		CurrentSpawnLocation = (CurrentSpawnLocation + 1) % spawnLocations.Length;
		return CurrentSpawnLocation;
	}

	public static PlayerMain GetPlayer(int playerName) {
		return players[playerName];
	}
}
