using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {

	public static float distanceTraveled;

	public float acceleration;
	public Vector3 jumpVelocity;
	public int maxJumps;
	public float gameOverY = -10;
	public int minWeight = 180;
	public int maxWeight = 600;
	public int flyWeight = 400;
	public float minSpeed = 0.0f;
	
	public Transform playerSkin;

	private bool touchingPlatform;
	private int jumpCount = 0;
	private int slamCount = 0;
	private Vector3 startPosition;
	private int weight = 0;
	
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
			
			GUIManager.SetWeightText(weight);
			
			if(transform.localPosition.y < gameOverY){
				GameEventManager.TriggerGameOver();
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
			Debug.Log ("SLAMAJAMA!");
			Super (10);
			rigidbody.AddForce(new Vector3(10.0f, -20.0f, 0.0f), ForceMode.VelocityChange);
			slamCount += 1;
		} else {
			Debug.Log("D'Oh");
			//Jump fail
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
	
	void LoseWeight(int amt) {
		this.weight -= amt;
		if(this.weight < this.minWeight) {
			this.weight = this.minWeight;
		}
	}

	void FixedUpdate () {
		if(playerState == playerStates.Running) {
			if(touchingPlatform){
				rigidbody.AddForce(acceleration / (1.0f + weightProportion * 2), 0f, 0f, ForceMode.Acceleration);
				
				Debug.Log(Time.deltaTime);
				LoseWeight((int)(100.0f * Time.deltaTime));
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