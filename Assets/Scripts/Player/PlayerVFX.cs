using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour {

	public ParticleSystem
		AimBeam,
		OrbCollectField,
		HoverPulses;

	public void SetVFXColors(Color color) {
		AimBeam.startColor = color;
		OrbCollectField.startColor = color;
		HoverPulses.startColor = color;
	}

}
