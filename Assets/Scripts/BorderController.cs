using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderController : MonoBehaviour
{
    Vector2 gameSize = GameManager.Instance.gameSize;

    // Update is called once per frame
    void Update()
    {
        if(gameSize != GameManager.Instance.gameSize)
        {
            // game size changed
            print("game size changed");
            float xRatio = GameManager.Instance.gameSize.x / gameSize.x; // 1.2
            float yRatio = GameManager.Instance.gameSize.y / gameSize.y; // 1.2
            float xScale = transform.localScale.x + (xRatio - 1);
            float yScale = transform.localScale.y + (yRatio - 1);
            transform.localScale = new Vector3(xScale, yScale, 0);
            gameSize = GameManager.Instance.gameSize;
        }
    }
}
