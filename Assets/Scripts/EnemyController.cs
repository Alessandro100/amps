using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * Going to have an enemy manager object
 * Enemy manager is going to have the homecore and pass it to the enemy
 * EnemyController is going to be spawned from the Enemy manager
 */

public class EnemyController : MonoBehaviour
{
    private Transform target;
    private NavMeshPath path;

    private float speed = 0.5f;
    private int currentPathIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        target = GameManager.Instance.homecores[0].transform; // TODO: find the closest homecore
        path = new NavMeshPath();
        bool found = NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPathIndex < path.corners.Length)
        {
            if (transform.position == path.corners[currentPathIndex])
            {
                currentPathIndex++;
            }
            Vector3 vectorToTarget = path.corners[currentPathIndex] - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
            transform.position = Vector3.MoveTowards(transform.position, path.corners[currentPathIndex], speed * Time.deltaTime);
        }
    }

    void closestHomecore()
    {
        
    }
}
