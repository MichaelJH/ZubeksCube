using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour {
    // track the position of each portal
    public struct PortalPosition {
        public Vector2 p1;
        public Vector2 p2;

        public PortalPosition(Vector2 p1, Vector2 p2) {
            this.p1 = p1;
            this.p2 = p2;
        }
    }

    //public GameObject portalPrefab;
    public GameObject P1Prefab, P2Prefab;
    public GameObject Portal1, Portal2;
    public PortalPosition PPos;

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
        //Debug.Log(Input.mousePosition);
        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //Mouse clicked
        //Create ray cast from player position to the platform
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
        {
            // LayerMask is a bitmap. NameToPlayer("Platform") returns and int, and then we shift 1 to get a bitmask
            int platform = LayerMask.GetMask("Platform");

            Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Linecast(transform.position, target, platform);

            if (hit.collider.gameObject.tag == "Platform")
            {
                Quaternion rotation = GetPortalRotation(hit);

                if (Input.GetButtonDown("Fire1")) //If right mouse click
                {
                    Portal1.SetActive(true);
                    Portal1.transform.position = hit.point;
                    Portal1.transform.rotation = rotation;
                    PPos.p1 = hit.point;
                }
                else if (Input.GetButtonDown("Fire2")) //if left mouse click
                {
                    Portal2.SetActive(true);
                    Portal2.transform.position = hit.point;
                    Portal2.transform.rotation = rotation;
                    PPos.p2 = hit.point;
                }
            }
        }
     }


    //Find the correct orientation of the portal
    private Quaternion GetPortalRotation(RaycastHit2D hit)
    {
        Quaternion rot = Quaternion.identity;
        return rot;
    }

    //void OnCollisonEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Portal")
    //    {
    //        //transform.position = portals.p2.transform.position;
    //    }
    //}

}
