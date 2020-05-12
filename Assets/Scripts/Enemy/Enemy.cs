using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private GameObject hive;
    
    private string direction = "";
    private bool fleeing = false;
    private bool active = false;

    private bool attacked = false;

    private GameObject player;
    private GameObject gameManager;

    public float minSpeed;
    public float maxSpeed;
    public float minAddedModifier, maxAddedModifier;

    private float speed;

    bool scoreRecieved = false;

    private SpriteRenderer sr;

    void Start() {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        hive = GameObject.FindGameObjectWithTag("Hive");
        gameManager = GameObject.FindGameObjectWithTag("GameManager");

        speed = Random.Range(minSpeed, maxSpeed);
    }

    IEnumerator showStats() {
        yield return new WaitForSeconds(1);
        gameManager.GetComponent<GameManager>().showStats();
    }

    void Update() {

        if (gameManager.GetComponent<GameManager>().gameOver) {
            return;
        }

        if (transform.position.x == hive.transform.position.x) {
            gameManager.GetComponent<GameManager>().gameOver = true;
            gameManager.GetComponent<GameManager>().play("Death"); // sound

            gameManager.GetComponent<GameManager>().shake(1, 0.3f);
            gameManager.GetComponent<GameManager>().GetComponent<Fade>().fadeIn();

            gameManager.GetComponent<GameManager>().removeStatGUI();

            StartCoroutine(showStats());
            return;
        }

        if (!fleeing) { 
            if (transform.position.x >= hive.transform.position.x) {
                direction = "left";
                sr.flipX = true;
            }
            else {
                direction = "right";
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (active && !fleeing) {
                player.GetComponent<PlayerMovement>().attacking = true;
                Vector3 newLoc = transform.position;
                newLoc += new Vector3(0, 0, 0);
                attacked = true;
                gameManager.GetComponent<GameManager>().play("Hit");
                gameManager.GetComponent<GameManager>().shake(0.2f, 0.1f);

                float addedModifier = Random.Range(minAddedModifier, maxAddedModifier);

                GameManager.modifier += (decimal) System.Math.Round((addedModifier / 100), 2);

                if ((float) GameManager.modifier > GameManager.bestModifier) {
                    GameManager.bestModifier = (float) GameManager.modifier;
                }

                gameManager.GetComponent<GameManager>().updateModifierUI();

                player.transform.position = newLoc;
                StartCoroutine(stopBuzzing());
            }
        }

        if (!attacked) {
            transform.position = Vector3.MoveTowards(transform.position, hive.transform.position, speed * Time.deltaTime);
        }

        if (fleeing) {
            float position = direction == "left" ? 500 : -500;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(position, transform.position.y, transform.position.z), (speed * 2) * Time.deltaTime);
        }
    }

    IEnumerator stopBuzzing() {
        yield return new WaitForSeconds(0.4f);
        player.GetComponent<PlayerMovement>().attacking = false;
        fleeing = true;


        if (!scoreRecieved) {
            float score = (speed * 100) * (float) GameManager.modifier;

            GameManager.score += (int)score;
        }

        scoreRecieved = true;

        gameManager.GetComponent<GameManager>().updateScoreUI();

        sr.flipX = !sr.flipX;
        StartCoroutine(cleanup());
    }

    IEnumerator cleanup() {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            active = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            active = false;
        }
    }
}
