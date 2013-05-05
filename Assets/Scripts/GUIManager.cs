using UnityEngine;


public class GUIManager : MonoBehaviour {

	public GUIText gameOverText, instructionsText, titleText, scoreText, weightText;
	public GUITexture tweetImg;
	
	private static GUIManager instance;
	
	private enum GUIState { Started, Playing, Over };
	private GUIState guiState;
	
	void Start () {
		instance = this;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		gameOverText.enabled = false;
		scoreText.enabled = false;
		weightText.enabled = false;
		instructionsText.enabled = false;
		titleText.enabled = false;
		guiState = GUIState.Started;	
	}

	void Update () {
		if(Input.GetButtonDown("Jump")){
			if(guiState == GUIState.Started) {
				GameEventManager.TriggerGameStart();
			} else {
				GameEventManager.TriggerGameStart();
			}
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
		guiState = GUIState.Playing;
	}

	private void GameOver () {
		gameOverText.enabled = true;
		instructionsText.enabled = true;
		enabled = true;
		guiState = GUIState.Over;
	}
	
	public static void SetScoreText(float score){
		instance.scoreText.text = score.ToString("f0");
	}
	
	public static void SetWeightText(int weight){
		instance.weightText.text = weight.ToString("0 lbs");
	}
}