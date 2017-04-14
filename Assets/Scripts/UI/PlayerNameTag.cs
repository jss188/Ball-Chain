using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameTag : MonoBehaviour {

	public Image BoxBG;
	public Text PlayerName, PlayerScore, PlayerMultiplier;
	public Outline BoxOutline, MultiplierOutline;

	private PlayerMain player;

	public void SetupNameTag(PlayerMain player) {
		this.player = player;
		PlayerName.text = "Player " + (player.playerNumber + 1);
		BoxOutline.effectColor = MultiplierOutline.effectColor = player.playerColor;
	}

	void Update() {
		if(player != null) {
			PlayerScore.text = (player.scoring.score * 10).ToString();
			PlayerMultiplier.text = string.Format("x{0}", player.scoring.orbsCollected);
		}
	}

}
