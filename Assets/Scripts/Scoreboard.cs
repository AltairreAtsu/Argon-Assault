using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scoreboard : MonoBehaviour {
	private int score = 0;

	private TextMeshProUGUI scoreDisplay;

	private void Start () {
		scoreDisplay = GetComponent<TextMeshProUGUI>();
		scoreDisplay.text = score.ToString();
	}
	
	private void Update () {
		scoreDisplay.text = score.ToString();
	}

	public void ScoreHit(int score)
	{
		this.score += score;
	}
}
