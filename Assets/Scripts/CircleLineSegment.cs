using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLineSegment : MonoBehaviour
{
    Vector2 point0;
    Vector2 point1;
    double travelAngle0;
    double travelAngle1;
    float speed = 0.3f;
    LineRenderer line;
    PolygonCollider2D polygonCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        line = InitializeLine();
    }

    void LateUpdate()
    {
        float timeChange = Time.deltaTime;
        float distanceToTravel = timeChange * speed;
        MoveLine(distanceToTravel);
        List<Vector2> colliderPoints = CalculateColliderPoints();
        polygonCollider2D.SetPath(0, colliderPoints.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        print("LINE SEGMENT COLLIDELINE SEGMENT COLLIDE");
        // if wall
        //GameObject.Destroy(line);
        // if other line
        //sss
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("TRIGGERR");
    }

    void MoveLine(float distanceToTravel)
    {
        float y0 = line.GetPosition(0).y + ((float)Math.Sin(travelAngle0) * distanceToTravel);
        float x0 = line.GetPosition(0).x + ((float)Math.Cos(travelAngle0) * distanceToTravel);
        Vector2 newPosition0 = new Vector2(x0, y0);
        line.SetPosition(0, newPosition0);
        float y1 = line.GetPosition(1).y + ((float)Math.Sin(travelAngle1) * distanceToTravel);
        float x1 = line.GetPosition(1).x + ((float)Math.Cos(travelAngle1) * distanceToTravel);
        Vector2 newPosition1 = new Vector2(x1, y1);
        line.SetPosition(1, newPosition1);
    }

    LineRenderer InitializeLine()
    {
        GameObject line = new GameObject();
        line.AddComponent<LineRenderer>();

        Rigidbody2D rb = line.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero;
        //rb.isKinematic = true;
        //rb.useFullKinematicContacts = true;
        polygonCollider2D = line.AddComponent<PolygonCollider2D>();
        LineRenderer lr = line.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.numCapVertices = 4;
        float alpha = 1f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lr.colorGradient = gradient;
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0.0f, 0.1f);
        lr.widthCurve = curve;
        lr.SetPosition(0, point0);
        lr.SetPosition(1, point1);
        return lr;
    }

    public void InitializeValues(Vector2 p1, Vector2 p2, double angle1, double angle2)
    {
        point0 = p1;
        point1 = p2;
        travelAngle0 = angle1;
        travelAngle1 = angle2;
    }

    public Vector3[] GetPositions()
    {
        Vector3[] positions = new Vector3[line.positionCount];
        line.GetPositions(positions);
        return positions;
    }

    public float GetWidth()
    {
        return line.startWidth;
    }

    private List<Vector2> CalculateColliderPoints()
    {
        //Get The Width of the Line
        float width = GetWidth();
        Vector3[] positions = GetPositions();

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
}
