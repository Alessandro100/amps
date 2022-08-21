using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleLineSegment), typeof(PolygonCollider2D))]
public class CircleLineSegmentCollider : MonoBehaviour
{
    CircleLineSegment lineSegment;
    PolygonCollider2D polygonCollider2D;

    List<Vector2> colliderPoints = new List<Vector2>();

    // Start is called before the first frame update
    void Awake()
    {
        lineSegment = GetComponent<CircleLineSegment>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        colliderPoints = CalculateColliderPoints();
        polygonCollider2D.SetPath(0, colliderPoints.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));
    }

    public void InitializeValues(Vector2 p1, Vector2 p2, double angle1, double angle2)
    {
        lineSegment.InitializeValues(p1, p2, angle1, angle2);
    }

    private List<Vector2> CalculateColliderPoints()
    {
        //Get The Width of the Line
        float width = lineSegment.GetWidth();
        Vector3[] positions = lineSegment.GetPositions();

        // m = (y2 - y1) / (x2 - x1)
        float m = (positions[1].y - positions[0].y) / (positions[1].x - positions[0].x);
        float deltaX = (width / 2f) * (m / Mathf.Pow(m * m + 1, 0.5f));
        float deltaY = (width / 2f) * (1 / Mathf.Pow(1 + m * m, 0.5f));

        //Calculate Vertex Offset from Line Point
        Vector3[] offsets = new Vector3[2];
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
        print("COLLIDE FROM SEGMENT");
    }
}
