using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {

	public static float distanceTraveled;

	public float acceleration;
	public Vector3 jumpVelocity;
	public int maxJumps;
	public float gameOverY = -10;
	public float minWeight = 180.0f;
	public float maxWeight = 600.0f;
	public float flyWeight = 400.0f;
	public float weightLossPerSecond = 20.0f;
	public float minSpeed = 0.0f;
	
	public Transform playerSkin;

	private bool touchingPlatform;
	private int jumpCount = 0;
	private int slamCount = 0;
	private Vector3 startPosition;
	private float weight = 0.0f;
	
	private enum playerStates {Idle, Running, Dead};
	private playerStates playerState;
	private Vector3 initialScale;
	private float weightProportion = 1.0f;

	void Start () {
		GameEventManager.GameStart += GameStart;
		startPosition = transform.localPosition;
		playerState = playerStates.Idle;
		initialScale = playerSkin.localScale;
	}	
		
	private void GameStart () {
		distanceTraveled = 0f;
		transform.localPosition = startPosition;
		rigidbody.isKinematic = false;
		enabled = true;
		jumpCount = 0;
		slamCount = 0;
		weight = minWeight;
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
			if(Input.GetButtonDown("Vertical")){	
				if(Input.GetAxis("Vertical") < 0) {
					Slam ();
				} else if(Input.GetAxis("Vertical") > 0) {
					Jump ();
				}
			}
			distanceTraveled = transform.localPosition.x;
			int score = (int)(distanceTraveled * 100 / 100);
			GUIManager.SetScoreText(score);
			
			GUIManager.SetWeightText((int)weight);
			
			if(transform.localPosition.y < gameOverY){
				GameEventManager.TriggerGameOver();
			}
			
			if(touchingPlatform) {
				LoseWeight();
			}
			
			weightProportion = ((float)(weight - minWeight) / (maxWeight - minWeight));
			
			playerSkin.localScale = Vector3.Scale(initialScale, new Vector3((1.0f + weightProportion), (1.0f + weightProportion * 0.20f), (1.0f + weightProportion)));
		}
	}
	
	private void Slam() {
		
		var canSlam = false;
		if(touchingPlatform == false && slamCount < 1) {
			canSlam = true;
		}
		if(canSlam) {
			Super (50);
			rigidbody.AddForce(new Vector3(10.0f, -20.0f, 0.0f), ForceMode.VelocityChange);
			slamCount += 1;
		} else {
			Debug.Log("D'Oh");
			//Slam fail
		}
	}
	
	private void Jump() {
		if(distanceTraveled < 0.5) {
			
			//Too early to jump
			return;
		}
		var canJump = false;
		Vector3 jumpVel = this.jumpVelocity;
		if(touchingPlatform) {
			canJump = true;
		} else if(jumpCount < maxJumps) {
			canJump = true;
			Super (100);
			jumpVel = Vector3.Scale(new Vector3(1.05f, 1.4f, 1.0f), jumpVelocity);
		}
		if(canJump) {
			rigidbody.AddForce(jumpVel, ForceMode.VelocityChange);
			jumpCount += 1;
		} else {
			Debug.Log("D'Oh");
			//Jump fail
		}
	}
	
	void Super(int cost) {
		this.weight += cost;
		if(this.weight > this.maxWeight) {
			this.weight = this.maxWeight;
		}
	}
	
	void LoseWeight() {
		float amt = weightLossPerSecond * Time.deltaTime;
		
		this.weight -= amt;
		if(this.weight < this.minWeight) {
			this.weight = this.minWeight;
		}
	}

	void FixedUpdate () {
		if(playerState == playerStates.Running) {
			if(touchingPlatform){
				rigidbody.AddForce(acceleration / (1.0f + weightProportion * 2), 0f, 0f, ForceMode.Acceleration);
			}
		}
	}

	void OnCollisionEnter (Collision collision) {
		Vector3 normal = collision.contacts[0].normal;
		
		//Don't reset the jump if the collision is with the side or the bottom of the collider
		if(normal.x > -1.0f && normal.y > -1.0f) {
	    	touchingPlatform = true;
			jumpCount = 0;
			slamCount = 0;
		}
	}

	void OnCollisionExit () {
		touchingPlatform = false;
	}	

}