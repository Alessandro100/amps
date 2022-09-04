using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public NavMeshSurface Surface2D;
    public GameObject enemyPrefab;

    private float spawnTime = 2.0f;
    private SpriteRenderer renderer;


    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        GameSizeChangeCheck();
        Instantiate(enemyPrefab, GetRandomEnemyLocation(), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        GameSizeChangeCheck();
        if (spawnTime > 3f)
        {
            Instantiate(enemyPrefab, GetRandomEnemyLocation(), Quaternion.identity);
            spawnTime = 0;
        }
        spawnTime += UnityEngine.Time.deltaTime;
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

    public void GameSizeChangeCheck()
    {
        Vector2 localScale = transform.localScale;
        if (localScale != GameManager.Instance.gameSize)
        {
            transform.localScale = GameManager.Instance.gameSize;
            Surface2D.UpdateNavMesh(Surface2D.navMeshData);
        }
    }
}
