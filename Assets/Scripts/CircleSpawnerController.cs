using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpawnerController : MonoBehaviour
{
    private List<LineCircle> lineCircles;
    public float period = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "line_circle";
        lineCircles = new List<LineCircle>();
    }

    void Update()
    {
        if (period > 3f)
        {
            GameObject lineCircleHolder = new GameObject();
            lineCircleHolder.name = "lineCircleHolder";
            lineCircleHolder.tag = "line_circle_holder"; //should be holder
            lineCircleHolder.transform.position = gameObject.transform.position;
            Rigidbody2D rb = lineCircleHolder.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.useFullKinematicContacts = true;
            LineCircle circle = lineCircleHolder.AddComponent<LineCircle>();
            lineCircles.Add(circle);
            period = 0;
        }
        period += UnityEngine.Time.deltaTime;
    }
}
