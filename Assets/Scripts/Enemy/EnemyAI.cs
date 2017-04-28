using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAIState { Roaming, ChaseFleeing }
public class EnemyAI : MonoBehaviour {

	private Rigidbody rigid;

	public float moveSpeed = 5f;

	public float fleeRange = 8f;

	public EnemyAIState aiState = EnemyAIState.Roaming;
	public bool chasing = false;

	public Vector2 roamRangeX;
	public Vector2 roamRangeZ;

	public float maxPlayerRange = 5;
	public float whiskerMaxRange = 3;
	public float whiskerAngle = 45f;
	public LayerMask whiskerMask;


	public float stopThreshold = 1f;
	public float waitTime = 5f;
	private float waitTimer = 0;
	private Vector3 moveTarget;
	private bool moving = false;

	private Vector3 cancelY = new Vector3(1,0,1);

	void Awake() {
		rigid = GetComponent<Rigidbody>();
	}

	void Update() {
		if(!moving) waitTimer -= Time.deltaTime;
		
		if(aiState == EnemyAIState.Roaming)
			RoamUpdate();
		else
			ChaseFleeUpdate();

		//Stop Roaming when player nearby
		PlayerMain nearest = GetClosestPlayer();
		if( nearest != null && PlayerWithinFleeRange(nearest))
			ChaseFleePlayer();	
	}

	public void ChaseFleeUpdate() {
		PlayerMain nearest = GetClosestPlayer();
		if( nearest != null) {
			if(PlayerWithinFleeRange(nearest)) {
				//Find a direction to player
				Vector3 moveDirection = (nearest.transform.position - transform.position).normalized;
				moveDirection = WhiskerPathCheck(moveDirection, whiskerAngle);
				moveDirection = Vector3.ProjectOnPlane (moveDirection, Vector3.up);

				//Am I flee or chasing?
				moveDirection = chasing ? moveDirection : -moveDirection;

				//Go in this direction.
				rigid.velocity = Vector3.Scale(moveDirection * moveSpeed, cancelY) + new Vector3(0,rigid.velocity.y,0);
				Debug.DrawLine(transform.position, transform.position + moveDirection * 5, Color.green);
			}
			//No player nearby. Resume roaming.
			else {
				StartRoaming();
			}
		}
	}

	public void RoamUpdate() {
		//Stopped. Find a new roam target.
		if (!moving && waitTimer <= 0) {
			moveTarget = GetRandomRoamPosition();
			moving = true;
		}

		//Moving to roam target
		if(moving) {
			Vector3 moveDirection = (moveTarget - transform.position).normalized;
			moveDirection = WhiskerPathCheck(moveDirection, whiskerAngle);
			rigid.velocity = Vector3.Scale(moveDirection * moveSpeed, cancelY) + new Vector3(0,rigid.velocity.y,0);//moveDirection * moveSpeed;
			StopWhenDestinationReached();
		}

		Debug.DrawLine(transform.position, moveTarget, Color.white);
	}

	//Helper methods
	Vector3 WhiskerPathCheck (Vector3 moveDirection, float whiskerAngle) {
		Debug.DrawLine(transform.position, transform.position + moveDirection*3, Color.green);
		
		//Is my path blocked?
		if (Physics.Raycast(transform.position, moveDirection, whiskerMaxRange, whiskerMask)) {
			Vector3 leftWhisker = Quaternion.AngleAxis (whiskerAngle, Vector3.up) * moveDirection.normalized;
			Vector3 rightWhisker = Quaternion.AngleAxis (whiskerAngle, Vector3.up) * moveDirection.normalized;

			Debug.DrawLine(transform.position, transform.position + leftWhisker*3, Color.green);
			Debug.DrawLine(transform.position, transform.position + rightWhisker*3, Color.green);

			//Feel the way ahead with the two whiskers
			RaycastHit leftWhiskerHit, rightWhiskerHit;
			bool leftWhiskerBlocked, rightWhiskerBlocked;
			leftWhiskerBlocked = Physics.Raycast(transform.position, leftWhisker, out leftWhiskerHit, whiskerMaxRange, whiskerMask);
			rightWhiskerBlocked = Physics.Raycast(transform.position, rightWhisker, out rightWhiskerHit, whiskerMaxRange, whiskerMask);

			//Select whichever direction is more clear.
			if(leftWhiskerBlocked && !rightWhiskerBlocked)
				return rightWhisker;
			else if(!leftWhiskerBlocked && rightWhiskerBlocked)
				return leftWhisker;
			else
				return (leftWhiskerHit.distance >= rightWhiskerHit.distance) ? leftWhisker : rightWhisker;
		}


		//Clear path! Go this way.
		return moveDirection.normalized;
	}

	PlayerMain GetClosestPlayer() {
		float distance = Mathf.Infinity;
		PlayerMain nearest = null;
		foreach (PlayerMain player in MultiplayerManagement.players) {
			float distToEnemy = (player.transform.position - transform.position).sqrMagnitude;
			if(distToEnemy < distance) {
				nearest = player;
				distance = distToEnemy;
			}
		}
		return nearest;
	}

	Vector3 GetRandomRoamPosition() {
		return new Vector3 (Random.Range (roamRangeX.x, roamRangeX.y), 0, Random.Range (roamRangeZ.x, roamRangeZ.y));
	}

	void StopWhenDestinationReached() {
		if( (transform.position - moveTarget).magnitude < stopThreshold ){
			moving = false;
			waitTimer = waitTime;
		}
	}

	bool PlayerWithinFleeRange(PlayerMain player) {
		return Vector3.Distance(player.transform.position, transform.position) <= fleeRange;
	}

	void ChaseFleePlayer() {
		moving = false;
		waitTimer = 0;
		aiState = EnemyAIState.ChaseFleeing;
	}

	void StartRoaming() {
		aiState = EnemyAIState.Roaming;
	}
}
