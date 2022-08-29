using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    //public NavMeshSurface Surface2D; // keep lookout
    //List<GameObject> enemies;

    // Start is called before the first frame update
    void Start()
    {
        print("enemy manager start prefab");
        //enemy = gameObject.AddComponent<EnemyController>();
        //enemy.transform.position = new Vector3(0, 0, 0);
        Instantiate(enemyPrefab, new Vector3(-1.5f,-4f, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        //Surface2D.BuildNavMeshAsync();
    }
}
