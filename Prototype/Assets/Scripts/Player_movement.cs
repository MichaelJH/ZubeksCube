using UnityEngine;
using System.Collections;

public class Player_movement : MonoBehaviour {
    private enum GravityDir {
        Up,
        Right,
        Down,
        Left
    }

    public float jumpFactor = 0.5f;
    public Transform groundCheck;
    private Rigidbody2D rb2d;
    private Transform selfTrans;

    public float moveForce = 365f;
    public float maxSpeed = 5f;

    private bool grounded = false;
    private bool jump = false;
    private bool move = false;
    private GravityDir grav;

    void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        selfTrans = GetComponent<Transform>();
        grav = GravityDir.Down;
    }

    void Update() {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Platform"));

        if (grounded) {
            move = true;
            if (Input.GetButtonDown("Jump")) {
                jump = true;
            }
        }
        else
            move = false;
        if (Input.GetKeyDown(KeyCode.I)) {
            transform.rotation = Quaternion.Euler(0, 0, 180);
            grav = GravityDir.Up;
        }
        if (Input.GetKeyDown(KeyCode.J)) {
            transform.rotation = Quaternion.Euler(0, 0, 270);
            grav = GravityDir.Left;
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            grav = GravityDir.Down;
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            transform.rotation = Quaternion.Euler(0, 0, 90);
            grav = GravityDir.Right;
        }
    }

    void FixedUpdate() {
        // movement controls for when gravity is up/down
        if ((grav == GravityDir.Down || grav == GravityDir.Up)) {
            // get the horizontal input
            float h = Input.GetAxis("Horizontal");

            // increase speed
            if (h * rb2d.velocity.x < maxSpeed) {
                rb2d.AddForce(Vector2.right * h * moveForce);
            }

            // cut excess speed to not exceed max speed
            if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
                rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
        }

        // movement controls for when gravity is right/left
        if ((grav == GravityDir.Left || grav == GravityDir.Right)) {
            // get the horizontal input
            float v = Input.GetAxis("Vertical");

            // increase speed
            if (v * rb2d.velocity.y < maxSpeed) {
                rb2d.AddForce(Vector2.up * v * moveForce);
            }

            // cut excess speed to not exceed max speed
            if (Mathf.Abs(rb2d.velocity.y) > maxSpeed)
                rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Sign(rb2d.velocity.y) * maxSpeed);
        }

        // if the spacebar was pressed in the last Update frame, jump
        if (jump) {
            jump = false;
            Vector2 jumpVector = Physics2D.gravity * -jumpFactor;
            rb2d.AddForce(jumpVector, ForceMode2D.Force);
        }
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.tag == "Platform") {
            grounded = true;
        }
    }
}