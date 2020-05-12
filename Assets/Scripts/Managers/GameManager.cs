using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {

    public float minimumTimer = 1f;
    public static float time, score, bestModifier;
    public static decimal modifier;

    public float spawnTimer, timeAdjuster;

    public bool gameOver = false;

    public GameObject enemy, player, deathScreen;
    public float elapsedTime;

    public GameObject scoreUI, modifierUI;
    public AudioSource hit, death;

    public GameObject scoreText, modifierText, timeText, bestText;

    private bool dead = false;

    void Start() {
        reset();
        InvokeRepeating("count", 0, 1);
    }

    public void play(string noise) {
        if (noise.Equals("Hit")) {
            AudioSource.PlayClipAtPoint(hit.clip, Camera.main.transform.position);
        }

        if (noise.Equals("Death")) {
            AudioSource.PlayClipAtPoint(death.clip, Camera.main.transform.position);
        }
    }

    public void updateScoreUI() {
        scoreUI.GetComponent<Text>().text = "Score " + score;
    }

    public void updateModifierUI() {
        modifierUI.GetComponent<Text>().text = "Modifier x" + modifier;
    }

    public void showStats() {
        dead = true;
        DontDestroyOnLoad(gameObject);

        if (score > PlayerPrefs.GetFloat("Best")) {
            PlayerPrefs.SetFloat("Best", score);
        }

        SceneManager.LoadScene(4);
    }

    public bool isDead() {
        return dead;
    }

    public void reset() {
        gameOver = false;
        dead = false;
        time = 0;
        score = 0;
        modifier = 1;
        spawnTimer = 2;

        updateScoreUI();
        updateModifierUI();
    }

    public void removeStatGUI() {
        scoreUI.GetComponent<Text>().text = "";
        modifierUI.GetComponent<Text>().text = "";
    }

    public void shake(float duration, float magnitude) {
        StartCoroutine(startShake(duration, magnitude));
    }

    public IEnumerator startShake(float duration, float magnitude) {
        Vector3 originalPos = Camera.main.transform.position;

        float elapsed = 0.0f;

        while (elapsed < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.position = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null; // wait until the next frame is drawn.
        }

        Camera.main.transform.position = originalPos;
    }

    void Update() {

        if (gameOver) {
            return;
        }

        elapsedTime += Time.deltaTime;

        if (elapsedTime > spawnTimer) {
            elapsedTime = 0;

            spawn();
        }
    }

    void count() {
        time++;

        spawnTimer += timeAdjuster;

        if (spawnTimer < minimumTimer) {
            spawnTimer = minimumTimer;
        }
    }

    void spawn() {

        string area;
        int areaNumber = Random.Range(1, 4);

        switch (areaNumber) {
            case 1:
                area = "left";
                break;
            case 2:
                area = "right";
                break;
            case 3:
                area = "up";
                break;
            default:
                area = "down";
                break;
        }

        float minX = 0, maxX = 0;
        float minY = 0, maxY = 0;

        switch (area) {
            case "left":
                minX = -14;
                maxX = -16;
                minY = -7;
                maxY = 7;
                break;
            case "right":
                minX = 14;
                maxX = 16;
                minY = -7;
                maxY = 7;
                break;
            case "up":
                minX = -11;
                maxX = 11;
                minY = 8;
                maxY = 10;
                break;
            case "down":
                minX = -11;
                maxX = 11;
                minY = -8;
                maxY = -10;
                break;
        }

        Vector3 spawnLoc = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
        Instantiate(enemy, spawnLoc, Quaternion.identity);
    }
}
