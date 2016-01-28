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
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Linecast(transform.position, direction);
            Debug.Log("Mouse: " + Input.mousePosition);
            Debug.Log("direction: " + direction);
            Debug.Log("hitpoint: " + hit.point);

            if (hit.collider.gameObject.tag == "Platform" || hit.collider.gameObject.tag == "Portal")
            {
                Debug.Log("You are casting ray");
                Debug.DrawRay(transform.position, Vector2.up, Color.black);
                
                Quaternion rotation = GetPortalRotation(hit);

                if (Input.GetButtonDown("Fire1")) //If right mouse click
                {
                    //Instantiate(portalEnter, hit.point, rotation);
                    Portal1.SetActive(true);
                    Portal1.transform.position = hit.point;
                    Portal1.transform.rotation = rotation;
                    PPos.p1 = hit.point;
                    Debug.Log("transform: " + Portal1.transform.position);
                }
                else if (Input.GetButtonDown("Fire2")) //if left mouse click
                {
                    //Instantiate(portalExit, hit.point, rotation);
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
