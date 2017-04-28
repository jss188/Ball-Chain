using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public Arena arenaPrefab;
    private Arena arenaInstance;
    public int arenaWidth, arenaHeight, wallsPerQuadrant;


    // Use this for initialization
    void Start () {
        BeginGame();
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) && MultiplayerManagement.main.gameState == GameState.WaitForStart)
        {
            RestartGame();
        }
    }
    private void BeginGame()
    {
        arenaInstance = Instantiate(arenaPrefab, Vector3.zero, Quaternion.identity) as Arena;
        //checkArenaDimensions();
        //arenaWidth = 40;
        //arenaHeight = 40;
        //wallsPerQuadrant = 5;
        arenaInstance.init(arenaWidth, arenaHeight, wallsPerQuadrant);


    }

    private void checkArenaDimensions()
    {
        if (arenaHeight < 20)
        {
            arenaHeight = 20;
        }
        if (arenaWidth < 20)
        {
            arenaWidth = 20;
        }
    }

    private void RestartGame()
    {
        Destroy(arenaInstance.gameObject);
        BeginGame();
    }


}
