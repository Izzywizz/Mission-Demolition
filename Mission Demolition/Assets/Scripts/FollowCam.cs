using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // The static point of interest (ie. the projectile) can be access anywhere inc Slingshot scri[t

    [Header("Set Dynamically")]
    public float camZ; // The desired Z pos of the camera

    private void Awake()
    {
        camZ = this.transform.position.z; //intial camera position
        print("Z: " + camZ);
    }

    //remembered called a fix number of times 50
    //Fixed update was chosen as the projectile is being moved by PhysX physics engine which updates and syc with FixedUpdate
private void FixedUpdate()
    {
        if (POI == null) return; //if there is no POI then return (default behaviour)

        //Get the position of the poi
        Vector3 destination = POI.transform.position;
        //Force destination.z to be camZ to keep the camera far enough away. Otherwise it would be zoomed up
        destination.z = camZ;
        //set the camera to the destination
        transform.position = destination;
    }
}
