using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Tank : Unit
{
    Seeker seeker;

    public Path path;
    public float movementSpeed;
    public float nextWaypointDistance;
    private int currentWaypoint = 0;
    public bool reachedEndOfPath = false;
    public override void Start()
    {
        base.Start();
        seeker = GetComponent<Seeker>();
    }

    // Update is called once per frame
    public override void  Update()
    {
        base.Update();
        if (path != null)
        {
            reachedEndOfPath = false;
            float distanceToWaypoint;
            while (true)
            {
                distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
                if (distanceToWaypoint < nextWaypointDistance)
                {
                    if (currentWaypoint + 1 < path.vectorPath.Count)
                    {
                        currentWaypoint++;
                    }
                    else
                    {
                        reachedEndOfPath = true;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            float speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;
            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            Vector3 velocity = dir * speedFactor * speedFactor;
            transform.position += velocity * Time.deltaTime;
        }
    }
    public override void Move(Vector3 _destination)
    {
        seeker.StartPath(transform.position, _destination, OnPathComlete);
    }
    public void OnPathComlete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
