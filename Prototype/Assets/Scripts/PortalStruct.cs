using UnityEngine;
using System.Collections;

public struct PortalStruct {

	public GameObject p1, p2;

    public PortalStruct(GameObject p1, GameObject p2)
    {
        this.p1 = p1;
        this.p2 = p2;

    }

    public GameObject getPair(GameObject portal)
    {
        if (this.p1.Equals(portal))
        {
            return p2;
        } else if (this.p2.Equals(portal))
        {
            return p1;
        } else
        {
            return null;
        }
    }
}
