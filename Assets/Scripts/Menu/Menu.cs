using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    // menu = 0, controls = 1, credits = 2, game = 3, 4 = leaderboard

    public List<GameObject> choices;
    public string current;
    public int currentNumber;
    public int scene;

    public AudioSource menuMove;
    public AudioSource select;
    public GameObject gameManager;

    void Start() {
        current = choices[0].ToString();
        choices[currentNumber].GetComponent<Image>().color = new Color(0.409f, 0.811f, 0.790f, 1.000f);
        currentNumber = 0;
        scene = SceneManager.GetActiveScene().buildIndex;
    }

    int getScene() {
        return scene;
    }

    void setScene(int scene) {
        this.scene = scene;
    }

    void Update() {

        if (getScene() == 3) {
            if (gameManager != null) {
                if (!gameManager.GetComponent<GameManager>().isDead()) {
                    return;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {

            AudioSource.PlayClipAtPoint(select.clip, Camera.main.transform.position);
            runSelected();
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            setSelected(currentNumber - 1);
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            setSelected(currentNumber + 1);
        }
    }

    public void setSelected(int number) {
        if (number < 0 || number > 3) {
            return;
        }

        current = choices[number].ToString();
        choices[number].GetComponent<Image>().color = new Color(0.409f, 0.811f, 0.790f, 1.000f);
        currentNumber = number;

        AudioSource.PlayClipAtPoint(menuMove.clip, Camera.main.transform.position);

        foreach (GameObject go in choices) {
            if (go != choices[number]) {
                go.GetComponent<Image>().color = Color.white;
            }
        }
    }

    public string getSelected() {
        return current;
    }

    void runSelected() {

        if (getSelected().Contains("Start")) {
            startGame();
            return;
        }

        if (getSelected().Contains("Credits")) {
            currentNumber = 0;
            showCredits();
            return;
        }

        if (getSelected().Contains("Controls")) {
            currentNumber = 0;
            showControls();
            return;
        }

        if (getSelected().Contains("Exit")) {
            exitGame();
            return;
        }

        if (getSelected().Contains("Retry")) {
            currentNumber = 0;
            startGame();
            return;
        }

        if (getSelected().Contains("Return")) {
            currentNumber = 0;
            goBackToMenu();
            return;
        }

        if (getSelected().Contains("Menu")) {
            currentNumber = 0;
            goBackToMenu();
            return;
        }
    }

    public void startGame() {
        setScene(3);
        SceneManager.LoadScene(3);
    }

    public void showCredits() {
        setScene(2);
        SceneManager.LoadScene(2);
        current = "return";
    }

    public void goBackToDeathScreen() {
        setScene(5);
        SceneManager.LoadScene(4);
        current = "retry";
    }

    public void showControls() {
        setScene(1);
        SceneManager.LoadScene(1);
        current = "return";
    }

    public void goBackToMenu() {
        setScene(0);
        SceneManager.LoadScene(0);
        current = "start";
    }
    
    public void exitGame() {
        Application.Quit();
        // this only works if the game is built, doesn't work in the Unity editor.
    }
}
