    using UnityEngine;
    using System.Collections.Generic;
    using UnityEngine.AI;

    public class Pathfinding : MonoBehaviour
    {
        public GameObject path;
        private List<Transform> wayPoints = new List<Transform>();

        private NavMeshAgent agent;
        private int currentWaypoint = 0;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            if (agent == null)
            {
                Debug.LogError("NavMeshAgent component is missing from this GameObject.");
                return;
            }

            if (path != null)
            {
                for (int i = 0; i < path.transform.childCount; i++)
                {
                    wayPoints.Add(path.transform.GetChild(i));
                }

                if (wayPoints.Count == 0)
                {
                    Debug.LogError("The Path GameObject has no child objects. Add child waypoints to the Path.");
                }
            }
            else
            {
                Debug.LogError("Path GameObject is not assigned. Please assign a Path GameObject in the Inspector.");
            }
        }

        void Update()
        {
            if (wayPoints.Count > 0)
            {
                SetDestination();
            }
        }

        private void SetDestination()
        {
            if (wayPoints.Count == 0)
                return;

            float distanceToWaypoint = Vector3.Distance(wayPoints[currentWaypoint].position, transform.position);

            if (distanceToWaypoint < 2f)
            {
                if (currentWaypoint < wayPoints.Count - 1)
                {
                    currentWaypoint++;
                }
                else
                {
                    currentWaypoint = 0;
                }
            }

            agent.SetDestination(wayPoints[currentWaypoint].position);
        }
    }