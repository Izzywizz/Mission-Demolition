using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    // fields set in the Unity Inspector pane
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;

    // fields set dynamically
    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    private void Awake()
    {
        //Find the Child transform attached to the parent Sligshot and return its tranform to intiall deactive the slingshot highlight
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
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

    private void OnMouseDown()
    {
        //The player has pressed the mouse button while over Slingshot
        aimingMode = true;
        //Instantiate a projectile
        projectile = Instantiate(prefabProjectile) as GameObject;
        //start it at the launch point
        projectile.transform.position = launchPos;
        // set it to is isKinematic for now
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }
}
