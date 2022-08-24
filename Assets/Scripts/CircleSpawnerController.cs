using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LineManipulation;

//[RequireComponent(typeof(LineCollider))]
[RequireComponent(typeof(LineCircle))]
public class CircleSpawnerController : MonoBehaviour
{
    //private LineCollider lc;
    private LineCircle lineCircle;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        rb.useFullKinematicContacts = true;
        lineCircle = GetComponent<LineCircle>();
    }

    // Update is called once per frame
    void Update()
    {
        //float timeChange = Time.deltaTime;
        // find a way to spawn a lineCircle every 2 seconds
    }

    void OnCollisionEnter2D()
    {
        print("COLLIDE $$$$$");
    }

    void OnCollisionEnter2D(Collision collision)
    {
        print("%%%%%%%%%%%%%%%%%%%%%%%%");
        foreach (ContactPoint contact in collision.contacts)
        {
            print(contact.point);
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
    }
}
