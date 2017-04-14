using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { WaitForStart, Running, GameEnded }
public class MultiplayerManagement : MonoBehaviour {

	public static MultiplayerManagement main;
	public PlayerMain playerPrefab;

	public Transform[] spawnLocations;
	private static int CurrentSpawnLocation = 0;

	public static List<PlayerMain> players = new List<PlayerMain>();
	public int playerCount;

	public GameState gameState = GameState.Running;

	public Color[] playerColors = new Color[]{Color.red, Color.yellow, Color.magenta};

	void Awake() {
		main = this; //Poorly implemented singleton thing, I guess.
	}

	void Start() {
		if (gameState == GameState.Running) {
			CreatePlayer();
			CreatePlayer();
		}
	}

	void Update() {
		playerCount = players.Count;
	}

	public void CreatePlayer() {
		PlayerMain player = Instantiate(playerPrefab, spawnLocations[SelectSpawnPointIndex()].position, Quaternion.identity);
		player.playerNumber = players.Count;
		player.playerColor = playerColors[ Random.Range(0, playerColors.Length) ]; //Random.ColorHSV(0,1);
		players.Add(player);
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
