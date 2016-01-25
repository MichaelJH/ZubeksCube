using UnityEngine;
using System.Collections;

public class GravityController : MonoBehaviour {
    public float gravityDefault = 9.81f;
    private float gravityCurrent;

	void Start () {
        gravityCurrent = gravityDefault;
        Physics2D.gravity = new Vector2(0f, -gravityCurrent);
    }
	
	void Update () {
        // gravity switching
        if (Input.GetKeyDown(KeyCode.I)) {
            Physics2D.gravity = new Vector2(0f, gravityCurrent);
        }
        if (Input.GetKeyDown(KeyCode.J)) {
            Physics2D.gravity = new Vector2(-gravityCurrent, 0f);
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            Physics2D.gravity = new Vector2(0f, -gravityCurrent);
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            Physics2D.gravity = new Vector2(gravityCurrent, 0f);
        }
    }
}
