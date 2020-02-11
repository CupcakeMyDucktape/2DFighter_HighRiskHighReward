using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To Add
//Hold shift + A OR D to run
//Small attack with damage min + max
//Big attack with damage min + max
//Block; to block incoming damage

public class PlayerControl : MonoBehaviour {

    [Tooltip("This is the maximum health of the player. While in play mode it will show the current health.")]
    public int playerHealth;
    [Tooltip("This indicated the player speed. On 0 it doesn't move.")]
    [SerializeField] float playerSpeed;
    [Tooltip("This indicated the player sprint speed. On 0 it doesn't move.")]
    [SerializeField] float playerSprintSpeed;

    private Rigidbody2D rb;
    private float moveDirection;
    private bool facingRight = false;
    private bool sprint = false;
    private float minSpeed;

    void Awake() {
       rb = GetComponent<Rigidbody2D>();
        minSpeed = playerSpeed;
    }

    void Update() {
        InputMovement();

        Animate();

        Move();
    }

    private void InputMovement() {
        //Use A and D to move side to side / Get inputs
        moveDirection = Input.GetAxis("Horizontal");

        Sprint();
    }

    private void Sprint() {
        if (Input.GetKey(KeyCode.LeftShift)) {
            sprint = true;
            playerSpeed = playerSprintSpeed;
        }
        else {
            sprint = false;
            playerSpeed = minSpeed;
        }
    }

    private void Move() {
        //Player movement
        rb.velocity = new Vector2(moveDirection * playerSpeed, rb.velocity.y);
    }

    private void Animate() {
        //Animate to the correct direction
        if (moveDirection > 0 && facingRight){
            FlipCharacter();
        }
        else if (moveDirection < 0 && !facingRight) {
            FlipCharacter();
        }
    }

    private void FlipCharacter() {
        //Used Brackeys for the flip.
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void PlayerTakesDamage() {

    }

    void Death() {

    }
}
