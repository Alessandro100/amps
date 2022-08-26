using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LineManipulation;

public class CircleSpawnerController : MonoBehaviour
{
    private List<LineCircle> lineCircles;
    public float period = 0.0f;
    //private LineCollider lineCircle;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "line_circle";
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        rb.useFullKinematicContacts = true;
        lineCircles = new List<LineCircle>();
    }

    void Update()
    {
        if (period > 3f)
        {
            LineCircle circle = gameObject.AddComponent<LineCircle>();
            lineCircles.Add(circle);
            period = 0;
        }
        period += UnityEngine.Time.deltaTime;
    }
}
