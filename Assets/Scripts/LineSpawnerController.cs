using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LineManipulation;

[RequireComponent(typeof(LineCircle))]
public class LineSpawnerController : MonoBehaviour
{
    private float time = 0.0f;
    public float interpolationPeriod = 1f;

    private Vector3 start;
    private Vector3 mid;
    private Vector3 end;

    private float speed = 0.3f; //0.5f per second; 


    private LineRenderer line;
    // 
    private LineCircle lc;


    /*
     * Programming Ideas
     * Create a LineCircle Class
     */

    // Start is called before the first frame update
    void Start()
    {
        //LineCircle lineCircle = gameObject.AddComponent(typeof(LineCircle)) as LineCircle;
        lc = GetComponent<LineCircle>();
        start = new Vector3(transform.position.x, transform.position.y - 0.5f, 0);
        mid = new Vector3(transform.position.x + 0.5f, transform.position.y, 0);
        end = new Vector3(transform.position.x, transform.position.y + 0.5f, 0);
        //

        //line = DrawLine(start, mid, end, Color.blue);
    }

    // Update is called once per frame
    void Update()
    {
        time = Time.deltaTime;
        float distanceToTravel = time * speed;
        float newXPos = 0;
        if (line.GetPosition(0).x > 3f)
        {
            newXPos = -3f;
        } else
        {
            newXPos = line.GetPosition(0).x + distanceToTravel;
        }

        Vector3 newPositionTop = new Vector3(newXPos, line.GetPosition(0).y, 0);
        Vector3 newPositionMid = new Vector3(newXPos + 0.5f, line.GetPosition(1).y, 0);
        Vector3 newPositionEnd = new Vector3(newXPos, line.GetPosition(2).y, 0);
        //line.SetPosition(0, newPositionTop);
        //line.SetPosition(1, newPositionMid);
        //line.SetPosition(2, newPositionEnd);
    }

    LineRenderer DrawLine(Vector3 start, Vector3 mid, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.SetVertexCount(3);
        //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        //lr.SetColors(color, color);
        float alpha = 0.75f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lr.colorGradient = gradient;
        // lr.SetWidth(0.1f, 0.1f);
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0.0f, 0.1f);
        lr.widthCurve = curve;

        lr.SetPosition(0, start);
        lr.SetPosition(1, mid);
        lr.SetPosition(2, end);
        //GameObject.Destroy(myLine, duration);
        return lr;
    }
}

