using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject homecore;

    private List<GameObject> _homecores;
    public List<GameObject> homecores => _homecores;

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

        _homecores = new List<GameObject>();
        GameObject initialHomecore = Instantiate(homecore, new Vector3(1, 1, 1), Quaternion.identity);
        initialHomecore.transform.position = new Vector3(initialHomecore.transform.position.x, initialHomecore.transform.position.y, 0);
        _homecores.Add(initialHomecore);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
