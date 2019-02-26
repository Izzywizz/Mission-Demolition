using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // The static point of interest (ie. the projectile) can be access anywhere inc Slingshot script

    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero; // [0,0]

    [Header("Set Dynamically")]
    public float camZ; // The desired Z pos of the camera

    private void Awake()
    {
        camZ = this.transform.position.z; //intial camera position
    }

    //remembered called a fix number of times 50
    //Fixed update was chosen as the projectile is being moved by PhysX physics engine which updates and syc with FixedUpdate
private void FixedUpdate()
    {
        if (POI == null) return; //if there is no POI then return (default behaviour)

        //Get the position of the poi
        Vector3 destination = POI.transform.position;
        // Limit the X & Y to minimum values
        destination.x = Mathf.Max(minXY.x, destination.x); //note when the projectile is fired, it moves -ve in the X direction, this prevents the canmera from moving to left  of x = 0
        destination.y = Mathf.Max(minXY.y, destination.y); //the same principle applies but to the y axis, so the projectiles doesn't go below y = 0

        //Set the OrthographucSize of the camera to keep ground in view
        Camera.main.orthographicSize = destination.y + 10; //we know from below that the minimum destination.y can't be below 0 so adding 10 (mimnimum otherogprahic size).
        //This valye will nicely expland as the projectile moves upwards.

        //Interpolate for the current Camera positon toward destination, Lerp basically tries to select a point between the first 2 parameters based on the 3rd value,
        // I set it to 5%, doing this will tell Unity to move the camera about 5% of the way from its current location to the location of the POI every fixedUpdated
        // bc POI is constanly moving, the transition should be smooth.
        destination = Vector3.Lerp(transform.position, destination, easing); 
        //Force destination.z to be camZ to keep the camera far enough away. Otherwise it would be zoomed up
        destination.z = camZ;
        //set the camera to the destination
        transform.position = destination;
    }
}
