using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour {

	public Vector3 velocity;

	public float buildSpeedScale, maxSpeedToward, maxSpeedAway;
	public int playerNumber;

	public TextMesh text;

	void Awake() {
		Destroy(gameObject, 30f);
		velocity = Vector3.ProjectOnPlane(Vector3.zero - transform.position, Vector3.up).normalized * maxSpeedToward;
	}

	void Update () {
		if(playerNumber < 0)
			text.text = string.Empty;
		else
			text.text = string.Format("P{0}",playerNumber+1);

		Debug.DrawLine(transform.position, transform.position + velocity, Color.green);

		foreach(Repulsor repulsor in Repulsor.repulsors) {
			if(repulsor.gameObject.activeInHierarchy && repulsor is RadialRepulsor) {
				Vector3 force = transform.position - repulsor.transform.position;

				//Check if orb is within range
				RadialRepulsor radialRepulsor = (RadialRepulsor)repulsor;
				if (force.magnitude >= radialRepulsor.range)
					continue;

				//Continue with movement calculations
				force = Vector3.ProjectOnPlane(force, Vector3.up).normalized;
				velocity += force * repulsor.pushForceScale * buildSpeedScale * Time.deltaTime;
			}	
		}

		velocity = Vector3.ClampMagnitude(velocity, maxSpeedAway);
		transform.position += velocity * Time.deltaTime;
	}

	void OnTriggerEnter(Collider col) {
		CollectOrb();
	}
	
	public void CollectOrb() {
		OrbSFXManager.PlayOrbPickUpSound();
		Destroy(gameObject);
	}
}
