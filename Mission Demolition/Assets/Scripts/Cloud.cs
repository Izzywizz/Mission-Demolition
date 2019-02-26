using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Procedurally generating some clouds
/// </summary>
public class Cloud : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject cloudSphere;

    public int numSpheresMin = 6; //Number of clouds to be create Min/ Max
    public int numSpheresMax = 10;
    public Vector3 sphereOffsetScale = new Vector3(5, 2, 1); // the maxmium distance that a cloudsphere could be from the centre of the cloud
    public Vector3 sphereScaleRangeX = new Vector2(4, 8); //the ranges of scales, clouds are usually wider hence the larger x
    public Vector2 sphereScaleRangeY = new Vector2(3, 4);
    public Vector2 sphereScaleRangeZ = new Vector2(2, 4);
    public float scaleYMin = 2f; //This prevents super skinnny clouds.

    private List<GameObject> spheres;

    private void Start()
    {
        spheres = new List<GameObject>(); //Holds a reference to all the sphers created for this cloud

        int num = Random.Range(numSpheresMin, numSpheresMax); //number of spheres that should be attached to this one cloud
        for (int i = 0; i < num; i++)
        {
            GameObject sphere = Instantiate<GameObject>(cloudSphere);
            spheres.Add(sphere);
            Transform sphereTrans = sphere.transform;
            sphereTrans.SetParent(this.transform);//Set the spheres to be the child of this Cloud (the one in the hierachy) all set to [0,0,0]

            //Randomly assign a postion
            Vector3 offset = Random.insideUnitSphere; // a random point inside a sphere (within 1 unit of the [0,0,0] origin)
            offset.x *= sphereOffsetScale.x;
            offset.y *= sphereOffsetScale.y; //we then multiply it by our  offsets
            offset.z *= sphereOffsetScale.z;
            sphereTrans.localPosition = offset;// we use local position bc it takes in to account the centre of the parent as its centre NOT the global coords 
            // so its moves relative to the centre of its parent.

            //Randomly assigning scale
            Vector3 scale = Vector3.zero;// so for scale we have min and max value for each diimension, x holds min values and y holds max
            scale.x = Random.Range(sphereScaleRangeX.x, sphereScaleRangeX.y);
            scale.y = Random.Range(sphereScaleRangeY.x, sphereScaleRangeY.y);
            scale.z = Random.Range(sphereScaleRangeZ.x, sphereScaleRangeZ.y);

            //adjust y scale by x distance from core
            print("Offset X: " + offset.x);
            print("Sphere Offset X: " + sphereOffsetScale.x);
            scale.y *= 1 - (Mathf.Abs(offset.x) / sphereOffsetScale.x); //y scale is changed based on how far it is from the centre, the further out X is, the smaller the y scale  
            scale.y = Mathf.Max(scale.y, scaleYMin);

            sphereTrans.localScale = scale; //scale is always relative to the parents transform
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Restart();
        //}
    }

    private void Restart()
    {
        foreach (GameObject sphere in spheres)
        {
            Destroy(sphere);
        }
        Start();
    }
}
