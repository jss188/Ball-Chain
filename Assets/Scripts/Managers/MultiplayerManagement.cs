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
	private float initGameTime;

	private MusicSelector musicPlayer;

	public int StartMatchCountDownSeconds = 5;

	private bool finishedWaitingForGameStart = false;
	private bool endingMatch = false;

	public float waitToLockInJoiningPlayers = 15f;
	public float resetWaitTimeAmount = 10;
	public List<JoinPlayerData> joiningPlayers = new List<JoinPlayerData>(); 
	private bool lockInJoiningPlayers = false;

	private EnemyPlacer enemyPlacer;

	void Awake() {
		main = this; //Poorly implemented singleton thing, I guess.
		hud = FindObjectOfType<HUDManager>();
		musicPlayer = GetComponentInChildren<MusicSelector>();
		VirtualControlManager.SetAllDefaultPlayerControls();
		enemyPlacer = GetComponent<EnemyPlacer>();
	}

	void Start() {
		//StartCoroutine(StartMatchCountDown(StartMatchCountDownSeconds));
		//hud.ShowGiantTextMessage("Press ~ to start match!", 1f, 10f, 1f);
		musicPlayer.musicPlayer.Play();
	}

	void Update() {
		playerCount = players.Count;

		switch(gameState) {
			case GameState.WaitForStart:
				if(!finishedWaitingForGameStart)
					WaitForMatchStartUpdate();
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

	void WaitForMatchStartUpdate() {

		bool updateIndicators = false;
		if(!lockInJoiningPlayers)
			GatherJoiningPlayerInput(out updateIndicators);

		//Player Triggered Update
		if (updateIndicators) {
			hud.UpdatePlayerJoinIndicators(this);

			//Set Timer on Player Update
			if(joiningPlayers.Count > 0)
				waitToLockInJoiningPlayers = resetWaitTimeAmount;
			else
				waitToLockInJoiningPlayers = Mathf.Infinity;

		}

		//Drive the lobby countdown and start match
		if(joiningPlayers.Count > 1) {
			if (waitToLockInJoiningPlayers > 0) {
				hud.ShowGiantTextMessage("Match starts in\n" + waitToLockInJoiningPlayers.ToString("F0") + " seconds", 0, 1f, 0);
				waitToLockInJoiningPlayers -= Time.deltaTime;
			}
			else {
				lockInJoiningPlayers = true;
				hud.joinBarHolder.gameObject.SetActive(false);
				StartCoroutine(StartMatchCountDown(StartMatchCountDownSeconds));
				finishedWaitingForGameStart = true;
			}
		}

	}

	void GatherJoiningPlayerInput(out bool updateIndicators) {
		updateIndicators = false;

		for (int i = 0; i < 4; i++) {
			//Join or Leave
			if (cInput.GetKeyDown("Menu"+i)) {
				int playerIndex = joiningPlayers.FindIndex(givenPlayer => givenPlayer.playerNumber == i);
				if (playerIndex < 0) {
					joiningPlayers.Add(new JoinPlayerData(i, playerColors[i], i));
					joiningPlayers.Sort( (p1, p2) => p1.playerNumber - p2.playerNumber );
				}
				else {
					joiningPlayers.RemoveAt( joiningPlayers.FindIndex(player => player.playerNumber == i) );
				}
				updateIndicators = true;
			}

			//Select Color
			float horizontal = 0;
			if(cInput.GetKeyDown("Left"+i)) horizontal = -1;
			if (cInput.GetKeyDown("Right"+i)) horizontal = 1;

			if(horizontal != 0) {
				int index = joiningPlayers.FindIndex(givenPlayer => givenPlayer.playerNumber == i);
				if(index >= 0) {
					JoinPlayerData player = joiningPlayers[ index ];
					int cIndex = player.playerColorIndex;
					cIndex = horizontal < 0 ? (cIndex + playerColors.Length - 1) % playerColors.Length : (cIndex + 1) % playerColors.Length;
					player.playerColor = playerColors[cIndex];
					player.playerColorIndex = cIndex;
					updateIndicators = true;
					//Debug.Log("Update " + i);
				}
			}
		}
	}

	IEnumerator StartMatchCountDown(int matchCountDownSeconds) {
		int countDown = matchCountDownSeconds;

		float initVolume = musicPlayer.musicPlayer.volume;
		float timer = 0;

		//while (countDown > 0) {
		//	hud.ShowGiantTextMessage(countDown.ToString(), 0, 0.7f, 0.2f);
		//	countDown--;
		//	yield return new WaitForSeconds(1);
		//}

		while(timer < matchCountDownSeconds) {
			timer += Time.deltaTime;
			musicPlayer.musicPlayer.volume = Mathf.Lerp(initVolume, 0, timer/matchCountDownSeconds);

			if(timer > matchCountDownSeconds - countDown) {
				hud.ShowGiantTextMessage(countDown.ToString(), 0, 0.7f, 0.2f);
				countDown--;
			}

			yield return new WaitForEndOfFrame();
		}

		musicPlayer.musicPlayer.Stop();
		musicPlayer.musicPlayer.volume = initVolume;

		hud.ShowGiantTextMessage("GO!", 0, 0.7f, 0.2f);

		StartMatch();
	}

	void StartMatch() {
		musicPlayer.SelectTrackAndPlay();
		gameState = GameState.Running;

		ResetManager();

		hud.joinBarHolder.gameObject.SetActive(false);

		initGameTime = gameTime;

		for (int i = 0; i < joiningPlayers.Count; i++) {
			CreatePlayer(joiningPlayers[i].playerNumber, joiningPlayers[i].playerColor);
		}

		StartCoroutine(PlaceEnemiesDuringGameLoop());
	}

	void ResetManager() {
		players = new List<PlayerMain>();
		CurrentSpawnLocation = 0;
	}

	void EndMatch() {
		endingMatch = true;
		hud.ShowGiantTextMessage("Time Up!", 0.15f, 1f, 0.5f);
		StartCoroutine(showScoresAndEndMatch());
	}

	public void MatchUpdate() {
		gameTime -= Time.deltaTime;
		gameTime = Mathf.Max(gameTime, 0);

		hud.UpdateHUDMatchTimer(this);
		hud.UpdateHUDTop5Leaderboard(this);

		if(gameTime <= 0f)
			gameState = GameState.GameEnded;
	}

	IEnumerator PlaceEnemiesDuringGameLoop() {
		while(gameState == GameState.Running) {
			enemyPlacer.PlaceEnemies( (int)Mathf.Lerp(10f, 50f, 1 - Mathf.Clamp01(gameTime/initGameTime) ) );
			yield return new WaitForSeconds(5);
		}
	}

	public void CreatePlayer() {
		CreatePlayer(players.Count, playerColors[ Random.Range(0, playerColors.Length) ]);
	}

	public void CreatePlayer(int playerNumber, Color playerColor) {
		PlayerMain player = Instantiate(playerPrefab, spawnLocations[SelectSpawnPointIndex()].position, Quaternion.identity);
		player.playerNumber = playerNumber;
		player.playerColor = playerColor;
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

	IEnumerator showScoresAndEndMatch () {
		yield return new WaitForSeconds(2f);
		hud.UpdateScoreTable();
		yield return new WaitForSeconds(10f);
		ReturnToMainMenu();
	}

	public void ReturnToMainMenu() {
		LoadingFade.LoadScreen(2f, delegate { SceneManager.LoadScene("MainMenu"); });
	}
}

[System.Serializable]
public class JoinPlayerData {
	public int playerNumber;
	public int playerColorIndex;
	public Color playerColor;

	public JoinPlayerData(int playerNumber, Color playerColor, int playerColorIndex) {
		this.playerColor = playerColor;
		this.playerNumber = playerNumber;
		this.playerColorIndex = playerColorIndex;
	}
}