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
    private SpriteRenderer renderer;
    List<int> hitByIndexes;

    private double health = 100;
    private float speed = 0.5f;
    

    // Start is called before the first frame update
    void Start()
    {
        hitByIndexes = new List<int>();
        renderer = GetComponent<SpriteRenderer>();
        path = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
        int currentPathIndex = 0;
        SetTargetHomecore();
        if(target)
        {
            NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
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
    }

    public float DoDamageAmount()
    {
        return (float)health * 0.2f;
    }

    // Getting damaged by line
    void GetDamaged(double damageAmount)
    {
        health -= damageAmount;
        float opacity = (float)(health / 100);
        renderer.color = new Color(1f, 1f, 1f, opacity);
        if (health < 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        int collisionIndex = col.gameObject.GetInstanceID();
        if (col.gameObject.tag == "line_circle_holder" && !hitByIndexes.Contains(collisionIndex))
        {
            hitByIndexes.Add(collisionIndex);
            LineCircle lineCircle = col.gameObject.GetComponent<LineCircle>();
            GetDamaged(lineCircle.GetDamage());
        }
    }

    void SetTargetHomecore()
    {
        float closestTargetDistance = float.MaxValue;
        NavMeshPath Path = new NavMeshPath();
        foreach (GameObject homecore in GameManager.Instance.homecores)
        {
            if(homecore == null)
            {
                continue;
            }
            if(NavMesh.CalculatePath(transform.position, homecore.transform.position, NavMesh.AllAreas, Path))
            {
                float distance = Vector3.Distance(transform.position, Path.corners[0]);
                for(int i = 1; i < Path.corners.Length; i++)
                {
                    distance += Vector3.Distance(Path.corners[i - 1], Path.corners[i]);
                }

                if(distance < closestTargetDistance)
                {
                    target = homecore.transform;
                    closestTargetDistance = distance;
                }
            }
        }
    }
}
