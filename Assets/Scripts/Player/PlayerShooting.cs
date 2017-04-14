using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {
	private PlayerMain main;
	private Rigidbody rigid;

	public AimMode aimMode = AimMode.Joystick;
	public LayerMask aimMask;

	public Bullet bullet;
	public Transform gunPosition;
	public float fireRate = 0.02f;
	private float fireRateTimer = 0f;

	public AudioSource shootSound;

	void Awake() {
		main = GetComponent<PlayerMain>();
		rigid = GetComponent<Rigidbody>();
	}

	void Update() {
		InputData input = VirtualControlManager.SampleInput(main.playerNumber);

		//Aim 360 degrees around
		if(aimMode == AimMode.Joystick) {
			if(input.AimHorizontal != 0 || input.AimVertical != 0) {
				Vector3 aimInput = new Vector3(input.AimHorizontal, 0f, input.AimVertical);
				Quaternion aimDirection = Quaternion.LookRotation(aimInput, Vector3.up);
				transform.rotation = aimDirection;
			}
		}
		else if(aimMode == AimMode.Mouse){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if( Physics.Raycast(ray, out hit, Mathf.Infinity, aimMask) ){
				Vector3 aimVector = hit.point - transform.position;
				aimVector = Vector3.ProjectOnPlane(aimVector, Vector3.up);
				Quaternion aimDirection = Quaternion.LookRotation(aimVector, Vector3.up);
				transform.rotation = aimDirection;
			}
		}

		//Shoot
		if(fireRateTimer > 0)
			fireRateTimer -= Time.deltaTime;

		if(input.Fire1 || (Input.GetKey(KeyCode.Alpha1) && main.playerNumber != 0)) {
			if(fireRateTimer <= 0f) {
				FireBullet();
				fireRateTimer = fireRate;
			}
		}
	}

	public void FireBullet() {
		//Vector3 slightSpread = transform.right * Random.Range(-slightSpreadAmount, slightSpreadAmount);
		//Bullet bulletObj = Instantiate(bullet, gunPosition.position + slightSpread, Quaternion.identity) as Bullet;
		Bullet bulletObj = Instantiate(bullet, gunPosition.position, Quaternion.identity) as Bullet;
		bulletObj.transform.position += (transform.forward * 0.25f) + (transform.forward * bulletObj.transform.localScale.y * 0.5f);
		bulletObj.transform.rotation = transform.rotation;
		bulletObj.transform.Rotate(90, 0, 0, Space.Self);
		bulletObj.playerNumber = main.playerNumber;
		bulletObj.GetComponent<Renderer>().material.SetColor("_TintColor", main.playerColor);
		Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), bulletObj.GetComponent<Collider>(), true);
		ShootSound();
		//NetworkServer.Spawn(bulletObj.gameObject);
	}

	public void ShootSound() {
		shootSound.Stop();
		shootSound.Play();
	}
}
