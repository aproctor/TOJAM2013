using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {

	public static float distanceTraveled;

	public float acceleration;
	public Vector3 jumpVelocity;
	public int maxJumps;
	public float gameOverY;

	private bool touchingPlatform;
	private int jumpCount = 0;
	private Vector3 startPosition;
	
	private enum playerStates {Idle, Running, Dead};
	private playerStates playerState;

	void Start () {
		GameEventManager.GameStart += GameStart;
		startPosition = transform.localPosition;
		playerState = playerStates.Idle;
	}	
		
	private void GameStart () {
		distanceTraveled = 0f;
		transform.localPosition = startPosition;
		rigidbody.isKinematic = false;
		enabled = true;
		jumpCount = 0;
		playerState = playerStates.Running;
	}

	private void GameOver () {
		rigidbody.isKinematic = true;
		enabled = false;
		playerState = playerStates.Dead;
	}

	void Update () {
		if(playerState == playerStates.Running) {
			if(Input.GetButtonDown("Jump")){
				Jump();
			}		
			distanceTraveled = transform.localPosition.x;
			
			if(transform.localPosition.y < gameOverY){
				GameEventManager.TriggerGameOver();
			}			
		}
	}
	
	private void Jump() {
		if(distanceTraveled < 5) {
			//Too early to jump
			return;
		}
		var canJump = false;
		if(touchingPlatform) {
			canJump = true;
		} else if(jumpCount < maxJumps) {
			canJump = true;
			//TODO super power
		}
		if(canJump) {
			rigidbody.AddForce(jumpVelocity, ForceMode.VelocityChange);
			jumpCount += 1;
		} else {
			Debug.Log("D'Oh");
			//Jump fail
		}
	}

	void FixedUpdate () {
		if(playerState == playerStates.Running) {
			if(touchingPlatform){
				rigidbody.AddForce(acceleration, 0f, 0f, ForceMode.Acceleration);
			}
		}
	}

	void OnCollisionEnter () {
		touchingPlatform = true;
		jumpCount = 0;
	}

	void OnCollisionExit () {
		touchingPlatform = false;
	}	

}