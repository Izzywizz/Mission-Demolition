using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S; //a private singleton (only accessible from within this classs)
    // fields set in the Unity Inspector pane, this is known as a compiler attribute
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velcoityMul = 8f;

    // fields set dynamically
    [Header("Set Dynamically")]
    public GameObject launchPoint; 
    public Vector3 launchPos;// 3D position of the LaunchPoint
    public GameObject projectile;
    public bool aimingMode; //refers to aiming mode when the user has pressed the mouse button over the slingshot
    private Rigidbody projectileRigidbody;

    private void Awake()
    {
        S = this;
        //Find the Child transform attached to the parent Sligshot and return its tranform to intiall deactive the slingshot highlight
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }

    private void Update()
    {
        //If Slingshot is not in amingMode, don't run this code
        if (!aimingMode) return;

        //Get the current mouse position in 2D screen coordintes
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z; //remember that the camera z axis is set to -10, this will set the mousePos to +10
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);//this will make z axis 0 in the world because -10 + 10 = 0, it pushes it on to the 3d world

        //Find the delta from the launch to the mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        //Limit mouseDElta to the radius of theSlingshot SphereCollider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize(); //this essentially sets the vector length to 1 (it keeps it direction)
            mouseDelta *= maxMagnitude; //then we multiply by the MaxMagnitude (which is set to 3m in the Sphere collider componet) (1 * 3)
        }

        //Move the projectile to this new position, however limited to a specific radius (see above)
        Vector3 projectilePos = launchPos + mouseDelta;
        projectile.transform.position = projectilePos;

        if (Input.GetMouseButtonUp(0))
        {
            //The mouse has been released 
            aimingMode = false;
            projectileRigidbody.isKinematic = false; //Move due to vecloty and gravity again
            projectileRigidbody.velocity = -mouseDelta * velcoityMul; //the projectile is given a velocity that is propotional to the distance that its launched from,
            // its negative becase we want it to fly in the opposite direction from where its clicked, giving it the illusion that its being pulled back and fired much like a noraml slingshot.
            FollowCam.POI = projectile; //ensures the camera follows the projectile
            projectile = null; //doesn't delete the gameObject but allows us to fill it again with another projectile instance.

            MissionDemolition.ShotFired(); //updated the shots fire UI TExt
            ProjectileLine.S.poi = projectile; //follow the new projectile's line, rmemebr a new instance of the projectile is created
        }


    }
    private void OnMouseEnter()
    {
        //When you activate or deactivate an GameObject, it stops rendering on the screen and doesn't get any updated
        // func calls from Updated() or OnCollisionEnter(), ususally we would just on/ off the Halo component but for some reason 
        // it isn't accisible from c# code so we just deactive/ active the entire gameObject
        //print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        launchPoint.SetActive(false);
        //print("Slingshot:OnMouseExit()");
    }

    //This method will only be called when the user presses the mouse down over the slingshot Collider component
    private void OnMouseDown()
    {
        //The player has pressed the mouse button while over Slingshot
        aimingMode = true;
        //Instantiate a projectile
        projectile = Instantiate(prefabProjectile) as GameObject;
        //start it at the launch point
        projectile.transform.position = launchPos;
        // set it to is isKinematic for now
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        //projectileRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete; //prevents runtime warning
        projectileRigidbody.isKinematic = true;

    }

    /// <summary>
    /// This static publicproperty uses the static private Slingshot instance S to allow publuc acces to read teh value of the slingshots launchPos.
    /// If somehow S is null then it returns [0,0,0]
    /// </summary>
    /// <value>The launch position.</value>
    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null) return Vector3.zero;
            return S.launchPos;
        }
    }
}
