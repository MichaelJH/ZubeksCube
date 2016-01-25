using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public Transform playerTrans;

    void Update() {
        Vector3 cameraPos = new Vector3(playerTrans.position.x, playerTrans.position.y, -10);
        transform.position = cameraPos;
    }
}
