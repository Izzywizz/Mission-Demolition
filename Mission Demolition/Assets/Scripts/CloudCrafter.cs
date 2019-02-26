using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("SEt in Inspector")]
    public int numClouds = 40; // the number of clouds to make
    public GameObject cloudPrefab; // the prefab for the clouds
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1; //Min scale of each cloud
    public float cloudScaleMax = 3; //MAX scale of each cloud
    public float cloudSpeedMul = 0.5f; //adjusts speed of clouds
    private GameObject[] cloudInstances;

    private void Awake()
    {
        //make an array large enough to hold all the cloud instances
        cloudInstances = new GameObject[numClouds];
        //Find the CloudAnchor parent GameObject
        GameObject anchor = GameObject.Find("CloudAnchor");
        //Iterte through and make Cloud_s
        GameObject cloud;

        for (int i = 0; i < numClouds; i++)
        {
            //make an instance of cloudPrefab
            cloud = Instantiate<GameObject>(cloudPrefab);
            // position cloud
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            //scale cloud
            float scaleU = Random.value;
            float scaleValue = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //scale clouds(with samller scaleU) should be earer the ground
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            //sma;;er clouds should be farther away
            cPos.z = 100 - 90 * scaleU;
            // apply these transform to the cloud
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleValue; //ensures it is a vector
            //make cloud a child of the anchor
            cloud.transform.SetParent(anchor.transform);
            //add the cloud to cloudInstances
            cloudInstances[i] = cloud;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Iterate over each cloud that was created
        foreach (var cloud in cloudInstances)
        {
            //get the cloud scale and postion
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cloudPos = cloud.transform.position;
            //Move larger clouds faster
            cloudPos.x -= scaleVal * Time.deltaTime * cloudSpeedMul;

            //if a cloud has move too far to the left..
            if (cloudPos.x <= cloudPosMin.x)
            {
                //move it to the far right
                cloudPos.x = cloudPosMax.x;
            }
            //apply the new position to the ckoud
            cloud.transform.position = cloudPos;
        }

    }
}
