using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The goal of the castle needs to react when hit by athe projectile
/// </summary>
public class Goal : MonoBehaviour
{
    //A static filed accessible by code anywhere
    static public bool goalMet = false;

    private void OnTriggerEnter(Collider other)
    {
        //When the trigger is hit by something
        // check to see if it's a Projectile. Remeber to 'check' the box for GameObject
        if (other.gameObject.tag == "Projectile")
        {
            //if so, set goalMet to true
            Goal.goalMet = true;
            //Also set the alpha of the color to higher opacity, turning the GOAL bright green when hit
            Material mat = GetComponent<Renderer>().material;
            Color colour = mat.color;
            colour.a = 1; //alpha
            mat.color = colour;
        }
    }
}
