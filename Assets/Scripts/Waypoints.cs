using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SingleWaypoint
{
    public Vector3 positionXYZ { get; private set; }
    public SingleWaypoint nextWaypoint { get; set; }

    public SingleWaypoint(Transform tr)
    {
        positionXYZ = tr.position;
        positionXYZ = new Vector3(positionXYZ.x, positionXYZ.y, 0);
    }

}

public class Waypoints : MonoBehaviour
{
    public static Waypoints instance { get; private set; }

    public Transform[] points;
    public List<SingleWaypoint> singleWaypoints = new List<SingleWaypoint>();

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }

        foreach (Transform point in points)
        {
            singleWaypoints.Add(new SingleWaypoint(point));
        }
        for(int i = 0; i < singleWaypoints.Count - 2; i++)
        {
            singleWaypoints[i].nextWaypoint = singleWaypoints[i + 1];
        }
    }

    public SingleWaypoint FirstWayPoint()
    {
        return singleWaypoints[0];
    }
}
