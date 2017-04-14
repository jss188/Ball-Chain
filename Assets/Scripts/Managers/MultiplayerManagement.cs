using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { WaitForStart, Running, GameEnded }
public class MultiplayerManagement : MonoBehaviour {

	public PlayerMain playerPrefab;

	public Transform[] spawnLocations;
	private static int CurrentSpawnLocation = 0;

	public static List<PlayerMain> players = new List<PlayerMain>();
	public int playerCount;

	public GameState gameState = GameState.Running;

	public Color[] playerColors = new Color[]{Color.red, Color.yellow, Color.magenta};

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
		PlayerMain player = Instantiate(playerPrefab, spawnLocations[SelectSpawnPoint()].position, Quaternion.identity);
		player.playerNumber = players.Count;
		player.playerColor = playerColors[ Random.Range(0, playerColors.Length) ]; //Random.ColorHSV(0,1);
		players.Add(player);
	}

	public int SelectSpawnPoint() {
		CurrentSpawnLocation = (CurrentSpawnLocation + 1) % spawnLocations.Length;
		return CurrentSpawnLocation;
	}

	public static PlayerMain GetPlayer(int playerName) {
		return players[playerName];
	}
}
