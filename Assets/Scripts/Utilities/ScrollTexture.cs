using UnityEngine;
using System.Collections;

public class ScrollTexture : MonoBehaviour {
	private Renderer rend;
	public Vector2 scrollSpeed = Vector2.one;
	public Vector2 initOffset = Vector2.zero;
	public bool setInitOffsetAtStart = true;
	private float offsetX = 0;
	private float offsetY = 0;

	void Start() {
		rend = GetComponent<Renderer>();
		if(setInitOffsetAtStart) {
			offsetX = initOffset.x;
			offsetY = initOffset.y;
		}
		else {
			offsetX = rend.material.mainTextureOffset.x;
			offsetY = rend.material.mainTextureOffset.y;
		}
	}

	void Update() {
		offsetX = (offsetX + scrollSpeed.x * Time.deltaTime) % 1;
		offsetY = (offsetY + scrollSpeed.y * Time.deltaTime) % 1;
		rend.material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
	}
}
