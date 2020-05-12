using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed;
    public float acceleration;
    public float maxSpeed;
    public float originalSpeed;

    private Vector3 change;
    private Rigidbody2D rb;

    public bool attacking = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = speed;
    }

    void FixedUpdate() {
        change = Vector3.zero;

        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        change.Normalize();

        updateMovement();
    }

    void updateMovement() {

        if (attacking) {
            return;
        }

        if (change == Vector3.zero) {
            speed = originalSpeed;


        } else {
            speed += acceleration;
        }

        if (speed > maxSpeed) {
            speed = maxSpeed;
        }

        rb.MovePosition(transform.position + (change * speed) * Time.deltaTime);
    }

}
