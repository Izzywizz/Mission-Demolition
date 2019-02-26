using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public GameObject launchPoint;

    private void Awake()
    {
        //Find the Child transform attached to the parent Sligshot and return its tranform to intiall deactive the slingshot highlight
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
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
}
