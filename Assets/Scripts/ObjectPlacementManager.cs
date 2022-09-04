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
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // will have to add a check if the player can afford
        if (Input.GetKeyDown(KeyCode.E) && !isPlacingObject)
        {
            PlaceCircleSpawner(mousePosition);
        }

        // will have to add a check if the player can afford
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
                GameManager.Instance.PurchaseObject("line_circle_spawner");
            }
            if(currentPlaceObject.tag == "wall")
            {
                Instantiate(wallPrefab, mousePosition, Quaternion.identity);
                GameManager.Instance.PurchaseObject("wall");
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
