using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; //Singleton pattern

    [Header("Set in Inspector")]
    public float minDist = 0.1f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    private void Awake()
    {
        S = this; // Set the singleton
        // Get a reference to the LineRenderer
        line = GetComponent<LineRenderer>();
        // Disable the LineRender until it's needed
        line.enabled = false;
        // Initialize the points List
        points = new List<Vector3>();
    }

    // This is a property (that is , a method masquerading as a field)
    public GameObject poi
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                // when _poi is set to something new, it resets everything
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    // this can be used to clear the line directly
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        //This is called to add a point to the line
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0  && (pt - lastPoint).magnitude < minDist)
        {
            //If the point isn't far enough from the last point, it returns
            return; //bc it hasn't gone far enough to add a point
        }
        if (points.Count == 0) //if this is the launch point (initial)
        {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS; //TO be defined
            // ...it adds an extra bit of line to aid aiming later
            points.Add(pt + launchPosDiff); //to negate the intial line rednering of the oiinter shooting mechanic
            points.Add(pt);
            line.positionCount = 2;
            // sets the first two points
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            //Enables the LineRender
            line.enabled = true;
        }
        else
        {
            //Normal behaviour of adding a point
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }

    //Returns the location of the most recently added point
    public Vector3 lastPoint
    {
        get
        {
            if(points == null)
            {
                // If there are no points, return Vector3.zero
                return (Vector3.zero);
            }
            return points[points.Count - 1]; //return last point in the list
        }
    }

    private void FixedUpdate()
    {
        if (poi == null)
        {
            //If there are no points, search for one
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                }
                else
                {
                    return; //return if we didn't find a poi
                }
            }
            else
            {
                return; //return if we didn't find a poi
            }
        }
        //if there is a poi, it's location is added every FixedUpdate
        AddPoint();
        if (FollowCam.POI == null)
        {
            //Once FollowCam.POI is null, make the local pooi null to, so it doesn't't just added lines to it
            poi = null;
        }
    }

}
