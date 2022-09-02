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
    private int currentPathIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        hitByIndexes = new List<int>();
        renderer = GetComponent<SpriteRenderer>();
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

        void closestHomecore()
    {
        
    }
}
