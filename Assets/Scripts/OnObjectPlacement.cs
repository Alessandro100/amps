using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OnObjectPlacement : MonoBehaviour
{
    SpriteRenderer sr;
    private bool isCurrentlyColliding = false;
    private int collisionCount = 0;
    public bool isAllowedToPlace => !isCurrentlyColliding;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(0f, 0.7f, 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCurrentlyColliding)
        {
            sr.color = new Color(0.8f, 0.3f, 0.1f, 0.5f);
        }
        else
        {
            sr.color = new Color(0f, 0.7f, 0f, 0.5f);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        isCurrentlyColliding = true;
        collisionCount++;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        collisionCount--;
        if(collisionCount == 0)
        {
            isCurrentlyColliding = false;
        }
    }
}
