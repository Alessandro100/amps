using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Fix mess of hirearchy and collision
// It's a learning experience :)

public class CircleSpawnerController : MonoBehaviour
{
    private List<LineCircle> lineCircles;
    public float period = 3.0f;
    //private LineCollider lineCircle;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "line_circle";
        //Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        //rb.gravityScale = 0;
        //rb.bodyType = RigidbodyType2D.Kinematic;

        //rb.useFullKinematicContacts = true;
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
            //LineCircle circle2 = Instantiate(LineCircle)
            //LineCircle circle = gameObject.AddComponent<LineCircle>();
            lineCircles.Add(circle);
            period = 0;
        }
        period += UnityEngine.Time.deltaTime;
    }
}
