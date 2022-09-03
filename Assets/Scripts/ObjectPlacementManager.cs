using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectPlacementManager : MonoBehaviour
{
    public NavMeshSurface Surface2D;
    public GameObject circleSpawnerPrefab;
    public GameObject wallPrefab;

    private bool isPlacingObject = false;
    private GameObject currentPlaceObject;

    // Start is called before the first frame update
    void Start()
    {
        Surface2D.BuildNavMeshAsync();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        if (Input.GetKeyDown(KeyCode.E) && !isPlacingObject)
        {
            PlaceCircleSpawner(mousePosition);
        }

        if (Input.GetKeyDown(KeyCode.W) && !isPlacingObject)
        {
            PlaceWall(mousePosition);
        }

        if (currentPlaceObject != null)
        {
            currentPlaceObject.transform.position = mousePosition;
        }

        if (isPlacingObject && Input.GetMouseButtonDown(0)) // left click
        {
            if (currentPlaceObject.tag == "line_circle_spawner")
            {
                Instantiate(circleSpawnerPrefab, mousePosition, Quaternion.identity);
            }
            if(currentPlaceObject.tag == "wall")
            {
                Instantiate(wallPrefab, mousePosition, Quaternion.identity);
                Surface2D.UpdateNavMesh(Surface2D.navMeshData);
            }
            Destroy(currentPlaceObject);
            isPlacingObject = false;
        }
    }

    void PlaceCircleSpawner(Vector3 mousePosition)
    {
        isPlacingObject = true;
        currentPlaceObject = Instantiate(circleSpawnerPrefab, mousePosition, Quaternion.identity);
        currentPlaceObject.GetComponent<CircleSpawnerController>().enabled = false;
        currentPlaceObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
    }

    void PlaceWall(Vector3 mousePosition)
    {
        isPlacingObject = true;
        currentPlaceObject = Instantiate(wallPrefab, mousePosition, Quaternion.identity);
        currentPlaceObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
    }
}
