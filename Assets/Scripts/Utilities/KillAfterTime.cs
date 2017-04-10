using UnityEngine;
using System.Collections;

public class KillAfterTime : MonoBehaviour {

	public float lifetime = 1;

	void Start () {
		Destroy(this.gameObject, lifetime);
	}
}
