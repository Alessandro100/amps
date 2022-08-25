using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LineManipulation;

//[RequireComponent(typeof(LineCollider))]
[RequireComponent(typeof(LineCollider))]
public class CircleSpawnerController : MonoBehaviour
{
    //private LineCollider lc;
    private LineCollider lineCircle;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero;
        //rb.isKinematic = true;
        rb.useFullKinematicContacts = true;
        lineCircle = GetComponent<LineCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //float timeChange = Time.deltaTime;
        // find a way to spawn a lineCircle every 2 seconds
    }
}
