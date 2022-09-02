using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    private SpriteRenderer renderer;
    //public NavMeshSurface Surface2D; // keep lookout
    //List<GameObject> enemies;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        //enemy = gameObject.AddComponent<EnemyController>();
        //enemy.transform.position = new Vector3(0, 0, 0);
        Instantiate(enemyPrefab, GetRandomEnemyLocation(), Quaternion.identity);
        Instantiate(enemyPrefab, GetRandomEnemyLocation(), Quaternion.identity);
        Instantiate(enemyPrefab, GetRandomEnemyLocation(), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        //Surface2D.BuildNavMeshAsync();
    }

    Vector3 GetRandomEnemyLocation()
    {
        float xBoundary = renderer.bounds.size.x / 2 * 0.95f;
        float yBoundary = renderer.bounds.size.y / 2;
        float randomMultiplier = (Random.value * 2) - 1;
        if (Random.value > 0.5f)
        {
            // set enemy on top or bottom 
            float xLocation = renderer.bounds.size.x / 2 * randomMultiplier;
            float yLocation = yBoundary * ((Random.value > 0.5f) ? -1 : 1);
            return new Vector3(xLocation, yLocation, 0);
        } else
        {
            // set enemy on left or right
            float xLocation = xBoundary * ((Random.value > 0.5f) ? -1 : 1);
            float yLocation = renderer.bounds.size.x / 2 * randomMultiplier;
            return new Vector3(xLocation, yLocation, 0);
        }
    }
}
