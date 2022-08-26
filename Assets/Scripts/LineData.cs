using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineData
{
    private int _from;
    private int _to;
    private float _speed = 0.4f;
    private float _distanceTravelled = 0f;
    private float _opacity = 1;
    private float _opacityDecay = 0.30f;
    private LineRenderer _line;
    private PolygonCollider2D _polygonCollider2D;

    // getters
    public int from => _from;
    public int to => _to;
    public float speed => _speed;
    public float distanceTravelled => _distanceTravelled;
    public float opacity => _opacity;
    public LineRenderer line => _line;
    public PolygonCollider2D polygonCollider2D => _polygonCollider2D;


    public LineData(int from, int to, LineRenderer line, float distanceTravelled, float opacity, float speed = 0.5f)
    {
        this._from = from;
        this._to = to;
        this._line = line;
        this._distanceTravelled = distanceTravelled;
        this._speed = speed;
        this._opacity = opacity;
        this._polygonCollider2D = _line.gameObject.AddComponent<PolygonCollider2D>();
    }

    public void AddDistanceTravelled(float distance)
    {
        _distanceTravelled += distance;
    }

    public void SetOpacityDecay(float timeChange)
    {
        _opacity -= timeChange * _opacityDecay;
        Color c = new Color(1f, 1f, 1f, _opacity);
        line.startColor = c;
        line.endColor = c;
    }

    public float GetLineWidth()
    {
        return _line.startWidth;
    }

    public Vector3[] GetLinePositions()
    {
        Vector3[] positions = new Vector3[_to - _from];
        line.GetPositions(positions);
        return positions;
    }

    public void SetLine(LineRenderer line)
    {
        this._line = line;
    }

    public void UpdateCollider()
    {
        Vector3[] positions = GetLinePositions();
        if (positions.Length >= 2)
        {
            int numberOfLines = positions.Length - 1;
            _polygonCollider2D.pathCount = numberOfLines;

            for (int i = 0; i < numberOfLines; i++)
            {
                List<Vector2> currentPositions = new List<Vector2>
                {
                    positions[i],
                    positions[i + 1]
                };

                List<Vector2> currentColliderPoints = CalculateColliderPoints(currentPositions, GetLineWidth());
                _polygonCollider2D.SetPath(i, currentColliderPoints);
            }
        }
        else
        {
            _polygonCollider2D.pathCount = 0;
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
}

