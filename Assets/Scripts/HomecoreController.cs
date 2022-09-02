using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomecoreController : MonoBehaviour
{
    private SpriteRenderer renderer;
    private double health = 100;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // give money
    }

    void GetDamaged(double damageAmount)
    {
        health -= damageAmount;
        float opacity = (float)(health / 100);
        renderer.color = new Color(1f, 1f, 1f, opacity);
        if (health < 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "enemy")
        {
            EnemyController enemy = col.gameObject.GetComponent<EnemyController>();
            float damageAmount = enemy.DoDamageAmount();
            GetDamaged(damageAmount);
            Destroy(col.gameObject);
        }
    }
}
