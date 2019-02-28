using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodySleep : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //This will assume at least initially that the wall should not be moving
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (rigidbody != null) rigidbody.Sleep();
    }
}
