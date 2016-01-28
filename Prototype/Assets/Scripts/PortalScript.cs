using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour {
    // track the position of each portal
    public enum WallOrientation {
        Ceiling,
        Right,
        Floor,
        Left
    }

    public struct PortalPosition {
        public Vector2 p1;
        public Vector2 p2;
        public WallOrientation p1Or;
        public WallOrientation p2Or;

        public PortalPosition(Vector2 p1, Vector2 p2) {
            this.p1 = p1;
            this.p2 = p2;
            this.p1Or = WallOrientation.Left;
            this.p2Or = WallOrientation.Left;
        }
    }

    //public GameObject portalPrefab;
    public GameObject P1Prefab, P2Prefab;
    public GameObject Portal1, Portal2;
    public PortalPosition PPos;
    private WallOrientation shotOr;

    // Use this for initialization
    void Awake()
    {
        Portal1 = GameObject.Instantiate(P1Prefab);
        Portal2 = GameObject.Instantiate(P2Prefab);
        Portal1.SetActive(false);
        Portal2.SetActive(false);
        PPos = new PortalPosition(Portal1.transform.position, Portal2.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //Create ray cast from player position to the platform
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
        {
            // LayerMask is a bitmap. NameToPlayer("Platform") returns and int, and then we shift 1 to get a bitmask
            int platform = LayerMask.GetMask("PortalPlatform");

            Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Linecast(transform.position, target, platform);

            if (hit) {
                Quaternion rotation = GetPortalRotation(hit);
                Debug.Log("wall: " + shotOr);

                if (Input.GetButtonDown("Fire1")) //If right mouse click
                {
                    Portal1.SetActive(true);
                    Portal1.transform.position = hit.point;
                    Portal1.transform.rotation = rotation;
                    PPos.p1 = hit.point;
                    PPos.p1Or = shotOr;
                }
                else if (Input.GetButtonDown("Fire2")) //if left mouse click
                {
                    Portal2.SetActive(true);
                    Portal2.transform.position = hit.point;
                    Portal2.transform.rotation = rotation;
                    PPos.p2 = hit.point;
                    PPos.p2Or = shotOr;
                }
            }
            else
                Debug.Log("This failed :(");
        }
     }


    //Find the correct orientation of the portal
    private Quaternion GetPortalRotation(RaycastHit2D hit)
    {
        Quaternion rot;

        Debug.Log("beforeTag " + hit.collider.gameObject.tag);
        string collTag = hit.collider.gameObject.tag;
        Debug.Log("afterTag: " + collTag);

        if (collTag == "Floor")
            shotOr = WallOrientation.Floor;
        if (collTag == "Ceiling")
            shotOr = WallOrientation.Ceiling;
        if (collTag == "Right_wall")
            shotOr = WallOrientation.Right;
        if (collTag == "Left_wall")
            shotOr = WallOrientation.Left;
        if (shotOr == WallOrientation.Left || shotOr == WallOrientation.Right)
            rot = Quaternion.identity;
        else
            rot = Quaternion.Euler(0, 0, 90);
        return rot;
    }
}
