using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {

	public static float distanceTraveled;

	public float acceleration;
	public Vector3 jumpVelocity;
	public int maxJumps;

	private bool touchingPlatform;
	private int jumpCount = 0;
	
	void Start() {
		jumpCount = 0;
	}

	void Update () {
		if(Input.GetButtonDown("Jump")){
			Jump();
		}
		distanceTraveled = transform.localPosition.x;
	}
	
	private void Jump() {
		var canJump = false;
		if(touchingPlatform) {
			canJump = true;
		} else if(jumpCount < maxJumps) {
			canJump = true;
			//TODO super power
		}
		if(canJump) {
			Debug.Log("JUMPING");
			rigidbody.AddForce(jumpVelocity, ForceMode.VelocityChange);
			jumpCount += 1;
		} else {
			Debug.Log("D'Oh");
		}
	}

	void FixedUpdate () {
		if(touchingPlatform){
			rigidbody.AddForce(acceleration, 0f, 0f, ForceMode.Acceleration);
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