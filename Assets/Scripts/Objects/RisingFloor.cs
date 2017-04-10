using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingFloor : MonoBehaviour {

	private bool raised = false;
	public float repeatTime = 5f;
	public float toggleChance = 0.75f;

	void Start() {
		InvokeRepeating("Rise", 0f, repeatTime);
	}

	public void Rise() {
		if(Random.value > toggleChance)
			raised = !raised;
	}

	void Update() {
		Vector3 pos = transform.position;
		Vector3 newPos = new Vector3(pos.x, raised ? -5 : 3, pos.z);
		transform.position = Mathfx.Sinerp(pos, newPos, Time.deltaTime);
	}
}
