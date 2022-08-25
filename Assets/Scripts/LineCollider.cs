
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LineManipulation;

[RequireComponent(typeof(LineCircle), typeof(PolygonCollider2D))]
public class LineCollider : MonoBehaviour
{
    LineCircle lc;
    PolygonCollider2D polygonCollider2D;

    void Awake()
    {
        print("HELLO FROM LINE COLLIDER");
        lc = GetComponent<LineCircle>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        foreach (LineData lineData in lc.publicLinesData)
        {
            Vector3[] positions = lineData.GetLinePositions();
            if (positions.Length >= 2)
            {
                int numberOfLines = positions.Length - 1;
                polygonCollider2D.pathCount = numberOfLines;

                for (int i = 0; i < numberOfLines; i++)
                {
                    List<Vector2> currentPositions = new List<Vector2>
                {
                    positions[i],
                    positions[i + 1]
                };

                    List<Vector2> currentColliderPoints = CalculateColliderPoints(currentPositions, lineData.GetLineWidth());
                    polygonCollider2D.SetPath(i, currentColliderPoints.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));
                }
            }
            else
            {
                polygonCollider2D.pathCount = 0;
            }
        }
        
    }

    private List<Vector2> CalculateColliderPoints(List<Vector2> positions, float width)
    {
        // m = (y2 - y1) / (x2 - x1)
        float m = (positions[1].y - positions[0].y) / (positions[1].x - positions[0].x);
        float deltaX = (width / 2f) * (m / Mathf.Pow(m * m + 1, 0.5f));
        float deltaY = (width / 2f) * (1 / Mathf.Pow(1 + m * m, 0.5f));

        //Calculate Vertex Offset from Line Point
        Vector2[] offsets = new Vector2[2];
        offsets[0] = new Vector2(-deltaX, deltaY);
        offsets[1] = new Vector2(deltaX, -deltaY);

        List<Vector2> colliderPoints = new List<Vector2> {
            positions[0] + offsets[0],
            positions[1] + offsets[0],
            positions[1] + offsets[1],
            positions[0] + offsets[1]
        };

        return colliderPoints;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        print("COLLIDE bing bong");
    }
}