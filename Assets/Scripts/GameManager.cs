using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // game impacting variables
    private Vector2 _gameSize = new Vector2(15F, 5f);
    private int currency = 0;

    private float currencyGainInterval = 0;

    // prefabs
    public NavMeshSurface Surface2D;
    public GameObject homecore;
    public GameObject homecoreLocation;
    public GameObject wall;

    private List<GameObject> _homecores;
    public List<GameObject> homecores => _homecores;
    public Vector2 gameSize => _gameSize;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        OnGameStart();
    }

    // Update is called once per frame  
    void Update()
    {
        if(currencyGainInterval > 1f)
        {
            currency += _homecores.Count * 10;
            currencyGainInterval = 0;
        }
        currencyGainInterval += UnityEngine.Time.deltaTime;
    }

    private void OnGameStart()
    {
        Surface2D.BuildNavMeshAsync();
        _homecores = new List<GameObject>();
        AddHomecore(new Vector3(0, 0, 0));
        // spawn random walls
        Vector3 randomWallPosition1 = GetRandomLocationInGame();
        Vector3 randomWallPosition2 = GetRandomLocationInGame();
        Instantiate(wall, randomWallPosition1, Quaternion.identity);
        Instantiate(wall, randomWallPosition2, Quaternion.identity);
        Surface2D.UpdateNavMesh(Surface2D.navMeshData);
    }

    private Vector3 GetRandomLocationInGame()
    {
        float xBoundary = _gameSize.x / 2 * 0.95f;
        float yBoundary = _gameSize.y / 2;
        float randomMultiplierX = (Random.value * 2) - 1;
        float randomMultiplierY = (Random.value * 2) - 1;
        return new Vector3(xBoundary * randomMultiplierX, yBoundary * randomMultiplierY, 0);
    }

    public void AddHomecore(Vector3 position)
    {
        GameObject homecoreGameObject = Instantiate(homecore, position, Quaternion.identity);
        _homecores.Add(homecoreGameObject);
        _gameSize += new Vector2(5, 5);
        Vector3 randomHomecoreLocationPosition = GetRandomLocationInGame();
        Instantiate(homecoreLocation, randomHomecoreLocationPosition, Quaternion.identity);
    }

    public void PurchaseObject(string tag)
    {
        if(tag == "wall")
        {
            currency -= 50;
        }

        if(tag == "line_circle_spawner")
        {
            currency -= 150;
        }

        if (tag == "homecore")
        {
            currency -= 400;
        }
    }
}
