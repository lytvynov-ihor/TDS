using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
public class pathfinding : MonoBehaviour
{
    public List<Transform> wayPoints;

    private NavMeshAgent agent;
    
    public int currentWaypoint = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.Log("Nav mesh agent missing");
        }
        else
        {
            agent = GetComponent<NavMeshAgent>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        SetDestination();
    }

    private void SetDestination()
    {
        if (wayPoints.Count == 0)
            return;

        float distanceToWaypoint = Vector3.Distance(wayPoints[currentWaypoint].position,transform.position);
        
        if (distanceToWaypoint < 2 && wayPoints.Count-1 !=currentWaypoint)
        {
            currentWaypoint++;
        }
            agent.SetDestination(wayPoints[currentWaypoint].position);
                
    }
}
