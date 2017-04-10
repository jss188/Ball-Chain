using UnityEngine;
using System.Collections;

public class ColorPulse : MonoBehaviour {

	public Color startColor = Color.red;
	public Color endColor = Color.black;
	public float pulseTime = 1;

	private float pulseTimer = 0;
	private Renderer rend;

	public ColorPulse(Color startColor, Color endColor, float pulseTime) {
		this.startColor = startColor;
		this.endColor = endColor;
		this.pulseTime = pulseTime;
	}

	void Awake() {
		rend = GetComponent<Renderer>();
	}

	void Update () {
		pulseTimer = (pulseTimer + Time.deltaTime) % pulseTime;
		rend.material.color = Color.Lerp(startColor, endColor, pulseTimer / pulseTime);
	}
}
