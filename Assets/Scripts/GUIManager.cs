using UnityEngine;


public class GUIManager : MonoBehaviour {

	public GUIText gameOverText, instructionsText, titleText, scoreText, weightText;
	public GUITexture tweetImg;
	
	private static GUIManager instance;
	
	void Start () {
		instance = this;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		gameOverText.enabled = false;
		scoreText.enabled = false;
		weightText.enabled = false;
	}

	void Update () {
		if(Input.GetButtonDown("Jump")){
			GameEventManager.TriggerGameStart();
		}
	}

	private void GameStart () {
		gameOverText.enabled = false;
		instructionsText.enabled = false;
		titleText.enabled = false;
		scoreText.enabled = true;
		weightText.enabled = true;
		enabled = false;
		tweetImg.enabled = false;
	}

	private void GameOver () {
		gameOverText.enabled = true;
		instructionsText.enabled = true;
		enabled = true;
	}
	
	public static void SetScoreText(float score){
		instance.scoreText.text = score.ToString("f0");
	}
	
	public static void SetWeightText(int weight){
		instance.weightText.text = weight.ToString("0 lbs");
	}
}