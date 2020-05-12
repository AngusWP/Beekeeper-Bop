using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Death : MonoBehaviour {

    float score = 0, time = 0, best = 0, modifier = 1;

    public GameObject scoreText, modifierText, timeText, bestText;

    void Start() {
        getStats();
        updateStats();
    }

    void getStats() {
        score = GameManager.score;
        time = GameManager.time;
        best = getBestScore();
        modifier = (float) GameManager.modifier;

        Destroy(GameObject.FindGameObjectWithTag("GameManager"));
    }


    void updateStats() {
        scoreText.GetComponent<Text>().text = "Score: " + score;
        modifierText.GetComponent<Text>().text = "Modifier: " + modifier;
        timeText.GetComponent<Text>().text = "Time: " + time;
        bestText.GetComponent<Text>().text = "Best: " + getBestScore();
    }

    float getBestScore() {
        return PlayerPrefs.GetFloat("Best");
    }
}
