using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Touch theTouch;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 3, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            print("hello touch");
            theTouch = Input.GetTouch(0);
            print(theTouch.position.x);
            print(theTouch.position.y);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(theTouch.position);
            print(worldPosition.x);
            print(worldPosition.y);
            print(worldPosition.z);
            transform.position = new Vector3(worldPosition.x,worldPosition.y, 0);
        }
    }
}
