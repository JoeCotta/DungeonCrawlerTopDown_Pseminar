using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class Enemy : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float nextWaypointDistance;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    bool follow = true;

    void Start()
    {   
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        // updates the Path every half seconds
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        
    }

    void UpdatePath(){
        if(seeker.IsDone()) seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    // if the next part of the path is generated
    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {

        if(path == null) return;

        // checks if the end of the path is reached
        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndPath = true;
            return;
        }else
        {
            reachedEndPath = false;
        }

        // calculates the force to follow the path
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime * 10;

        rb.AddForce(force);

        // updates the current Waypoint        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

    }




}
