using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LineManipulation;

//[RequireComponent(typeof(LineCollider))]
[RequireComponent(typeof(LineCircle))]
public class CircleSpawnerController : MonoBehaviour
{
    //private LineCollider lc;
    private LineCircle circc;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        rb.useFullKinematicContacts = true;
        //lc = GetComponent<LineCollider>();
        circc = GetComponent<LineCircle>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D()
    {
        print("COLLIDE $$$$$");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("TRIGGER TRIGGER TRIGGER");
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            print("Point of contact: " + hit.point);
            Debug.Log("Point of contact: " + hit.point);
        }
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
