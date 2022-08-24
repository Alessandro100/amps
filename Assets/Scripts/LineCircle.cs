using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NEXT STEP -> DETECTING COLLISIONS!

class LineData
{
    private int _from;
    private int _to;
    private float _speed = 0.3f;
    private float _distanceTravelled = 0f;
    private float _opacity = 1;
    private float _opacityDecay = 0.1f;
    LineRenderer _line;

    // getters
    public int from => _from;
    public int to => _to;
    public float speed => _speed;
    public float distanceTravelled => _distanceTravelled;
    public float opacity => _opacity;
    public LineRenderer line => _line;


    public LineData(int from, int to, LineRenderer line, float distanceTravelled, float opacity, float speed = 0.3f)
    {
        this._from = from;
        this._to = to;
        this._line = line;
        this._distanceTravelled = distanceTravelled;
        this._speed = speed;
        this._opacity = opacity;
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

    public void SetLine(LineRenderer line)
    {
        this._line = line;
    }
}


namespace LineManipulation
{

    public class LineCircle : MonoBehaviour
    {
        public float speed = 0.3f; // 0.3f per second

        private List<LineData> linesData;
        private int numberOfPoints = 50;
        private float initialDistance = 1f;
        private List<CircleLineSegment> lineSegments;

        //TO REMOVE
        float distanceTest = 0;
        bool hasHappened = false;

        // Start is called before the first frame update
        void Awake()
        {
            LineData initialCircle = GenerateSegments(0, numberOfPoints + 1, initialDistance, 1f);
            initialCircle.line.loop = true;
            linesData = new List<LineData> { initialCircle };
        }

        // Update is called once per frame
        void LateUpdate()
        {
            //float timeChange = Time.deltaTime;
            //float distanceToTravel = timeChange * speed;
            
            //MoveCircle(distanceToTravel);
            float timeChange = Time.deltaTime;
            float distance = timeChange * 0.3f;
            distanceTest += distance;

            // this is all a test here
            if(distanceTest > 1f && !hasHappened)
            {
                hasHappened = true;
                //SIM W/POITN
                List<double> killNodes = new List<double>();
                Vector2 SIMULATEDCOLLISION = new Vector2(-3, -3); // will get this from the collision
                double index = FindIndexOfCollision(SIMULATEDCOLLISION);
                killNodes.Add(index);
                List<LineData> newLinesData = GenerateLinesPostCollision(killNodes);
                Destroy(linesData[0].line.gameObject);
                linesData.Clear();
                linesData = new List<LineData>();
                linesData.AddRange(newLinesData);
            }
            MoveSegments();
        }

        double FindIndexOfCollision(Vector2 collisionPoint)
        {
            float xDiff = collisionPoint.x - transform.position.x;
            float yDiff = collisionPoint.y - transform.position.y;
            double angle = Math.Atan2(yDiff, xDiff) + (2 * Math.PI);
            double anglePerIndex = 2 * Math.PI / numberOfPoints;
            return angle / anglePerIndex;
        }

        List<LineData> GenerateLinesPostCollision(List<double> killNodes)
        {
            List<LineData> lineDataSegments = new List<LineData>();
            int from = 0;
            int to;
            foreach (double killNode in killNodes)
            {
                to = (int)Math.Floor(killNode);
                if(from != to)
                {
                    LineData lineDataSegment = GenerateSegments(from, to, linesData[0].distanceTravelled, linesData[0].opacity);
                    lineDataSegments.Add(lineDataSegment);
                }
                from = (int)Math.Ceiling(killNode);
            }
            LineData finalLineDataSegment = GenerateSegments(from, numberOfPoints + 1, linesData[0].distanceTravelled, linesData[0].opacity);
            lineDataSegments.Add(finalLineDataSegment);
            return lineDataSegments;
        }

        List<CircleLineSegment> DrawCircleV2()
        {
            int numberOfPoints = 25;
            List<CircleLineSegment> circleLineSegments = new List<CircleLineSegment>();
            double angle = 2 * Math.PI / numberOfPoints; // in radians
            float initialLength = 1f;
            GameObject circle = new GameObject();
            // iteration
            Vector2 prevPoint = new Vector2(0,0);
            double prevAngle = 0.0;
            for (int i = 0; i < numberOfPoints + 1; i++)
            {
                double pointAngle = angle * i;
                float y = transform.position.y + ((float)Math.Sin(pointAngle) * initialLength);
                float x = transform.position.x + ((float)Math.Cos(pointAngle) * initialLength);
                Vector2 point = new Vector2(x, y);

                if(i != 0) {
                    CircleLineSegment lineSegment = circle.AddComponent<CircleLineSegment>();
                    lineSegment.InitializeValues(prevPoint, point, prevAngle, pointAngle);
                    circleLineSegments.Add(lineSegment);
                }
                prevPoint = new Vector2(point.x, point.y);
                prevAngle = pointAngle;
            }

            return circleLineSegments;
        }

        LineData GenerateSegments(int from, int to, float distanceTravelled, float initialOpacity) //drawcircleV3
        {
            GameObject line = new GameObject();
            line.AddComponent<LineRenderer>();
            Rigidbody2D rb = line.AddComponent<Rigidbody2D>();
            //rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            rb.useFullKinematicContacts = true;
            LineRenderer lr = line.GetComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.positionCount = to - from;
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(0.0f, 0.1f);
            lr.widthCurve = curve;
            // logic of setting the points here
            double anglePerSegment = 2 * Math.PI / numberOfPoints; // in radians
            int indexCount = 0;
            for (int i = from; i < to; i++)
            {
                double pointAngle = anglePerSegment * i;
                float y = transform.position.y + ((float)Math.Sin(pointAngle) * distanceTravelled);
                float x = transform.position.x + ((float)Math.Cos(pointAngle) * distanceTravelled);
                Vector3 newPosition = new Vector3(x, y, 0);
                lr.SetPosition(indexCount, newPosition);
                indexCount++;
            }
            Color c = new Color(1f, 1f, 1f, initialOpacity);
            lr.startColor = c;
            lr.endColor = c;
            return new LineData(from, to, lr, distanceTravelled, initialOpacity);
        }

        public void MoveSegments()
        {
            double anglePerSegment = 2 * Math.PI / numberOfPoints; // in radians
            Boolean shouldDestroyLineDataList = false;
            foreach (LineData lineData in linesData)
            {
                float timeChange = Time.deltaTime;
                float distanceToTravel = timeChange * lineData.speed;
                lineData.SetOpacityDecay(timeChange);
                lineData.AddDistanceTravelled(distanceToTravel);
                int indexCounter = 0;


                for(int i = lineData.from; i < lineData.to; i++)
                {
                    double travelAngle = i * anglePerSegment;
                    float y = lineData.line.GetPosition(indexCounter).y + ((float)Math.Sin(travelAngle) * distanceToTravel);
                    float x = lineData.line.GetPosition(indexCounter).x + ((float)Math.Cos(travelAngle) * distanceToTravel);
                    
                    Vector2 newPosition0 = new Vector2(x, y);
                    lineData.line.SetPosition(indexCounter, newPosition0);
                    indexCounter++;
                }

                if (lineData.opacity < 0)
                {
                    shouldDestroyLineDataList = true;
                    Destroy(lineData.line.gameObject);
                }
            }
            if(shouldDestroyLineDataList)
            {
                linesData.Clear();
                // I think at this point the entire LineCircle object should be destroyed
            }
        }

        public Vector3[] GetPointPositions()
        {
            Vector3[] positions = new Vector3[numberOfPoints];
            //lineSegments2[0].GetPositions(positions);
            return positions;
        }

        public float GetWidth()
        {
            return 0;//lineSegments2[0].startWidth;
        }
        public void CollisionCalculate(Collision2D col)
        {
            print("CollisionCalculate CollisionCalculate CollisionCalculate CollisionCalculate test");
            //col.

            foreach (ContactPoint2D contact in col.contacts)
            {
                print(contact.point); // with this point find the closest point in the array

                print(contact.normal);
                double angleOfContact = Math.Atan2(contact.normal.y, contact.normal.x);
            }
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            print("OnCollisionEnter2D");
            //col.

            foreach (ContactPoint2D contact in col.contacts)
            {
                double angleOfContact = Math.Atan2(contact.normal.y, contact.normal.x);
            }
        }
    }
}