using UnityEngine;
using System.Collections;

public class Player_movement : MonoBehaviour {
    // track the direction of gravity
    private enum GravityDir {
        Up,
        Right,
        Down,
        Left
    }

    public float jumpFactor = 0.5f;
    public Transform groundCheck;
    private Rigidbody2D rb2d;

    public float moveForce = 365f;
    public float maxSpeed = 5f;

    private bool grounded = false;
    private bool jump = false;
    private bool move = false;
    private GravityDir grav;
    public GameObject portalLinePrefab;
    private GameObject portalLine;
    private bool cameOutOfAFloorPortal;

    void Awake() {
        portalLine = GameObject.Instantiate(portalLinePrefab);
        rb2d = GetComponent<Rigidbody2D>();
        grav = GravityDir.Down;
    }

    void Update() {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Platform"));

        DrawAimLine(); // draw the aiming line

        if (grounded) {
            move = true;
            cameOutOfAFloorPortal = false;
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

    private void DrawAimLine() {
        int platform = LayerMask.GetMask("PortalPlatform");

        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pos = transform.position;
        Vector2 direction = target - pos;

        RaycastHit2D hit = Physics2D.Raycast(pos, direction, Mathf.Infinity, platform);
        Vector3 hit3D = new Vector3(hit.point.x, hit.point.y, 0);
        Vector3[] points = { transform.position, hit3D };

        var renderer = portalLine.GetComponent<LineRenderer>();

        renderer.SetPositions(points);
    }

    void FixedUpdate() {
        // movement controls for when gravity is up/down
        if ((move || cameOutOfAFloorPortal) && (grav == GravityDir.Down || grav == GravityDir.Up)) {
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
        else if ((move || cameOutOfAFloorPortal) && (grav == GravityDir.Left || grav == GravityDir.Right)) {
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
        // collisions with portals
        if (coll.gameObject.tag == "Portal") {
            GameObject collidedPortal = coll.gameObject;
            var PortalScript = GetComponent<PortalScript>();
            float offset = 0.6f;
            if (collidedPortal == PortalScript.Portal1)
            {
                if (PortalScript.Portal2.activeSelf) {
                    Vector2 newPos = PortalScript.PPos.p2;
                    PortalScript.WallOrientation orientation = PortalScript.PPos.p2Or;
                    if (orientation == PortalScript.WallOrientation.Left)
                        newPos.x += offset;
                    else if (orientation == PortalScript.WallOrientation.Right)
                        newPos.x -= offset;
                    else if (orientation == PortalScript.WallOrientation.Ceiling)
                        newPos.y -= offset;
                    else
                        newPos.y += offset;
                    transform.position = newPos;
                    rb2d.velocity = NewVelocity(2);
                }
            }
            else {
                if (PortalScript.Portal1.activeSelf) {
                    Vector2 newPos = PortalScript.PPos.p1;
                    PortalScript.WallOrientation orientation = PortalScript.PPos.p1Or;
                    if (orientation == PortalScript.WallOrientation.Left)
                        newPos.x += offset;
                    else if (orientation == PortalScript.WallOrientation.Right)
                        newPos.x -= offset;
                    else if (orientation == PortalScript.WallOrientation.Ceiling)
                        newPos.y -= offset;
                    else
                        newPos.y += offset;
                    transform.position = newPos;
                    rb2d.velocity = NewVelocity(1);
                }
            }
        }
    }

    Vector2 NewVelocity(int exitPortal) {
        Vector2 result;
        var PortalScript = GetComponent<PortalScript>();
        PortalScript.WallOrientation orient;
        if (exitPortal == 1)
            orient = PortalScript.PPos.p1Or;
        else
            orient = PortalScript.PPos.p2Or;
        float velocity = rb2d.velocity.magnitude;
        if (orient == PortalScript.WallOrientation.Left) {
            result = new Vector2(velocity, 0);
        }
        else if (orient == PortalScript.WallOrientation.Right) {
            result = new Vector2(-velocity, 0);
        }
        else if (orient == PortalScript.WallOrientation.Ceiling) {
            result = new Vector2(0, -velocity);
        }
        else {
            result = new Vector2(0, velocity);
            cameOutOfAFloorPortal = true;
        }

        return result;
    }
}