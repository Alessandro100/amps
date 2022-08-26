using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NEXT STEP -> OnCollisionEnter2D

namespace LineManipulation
{

    public class LineCircle : MonoBehaviour
    {
        public float speed = 0.3f; // 0.3f per second

        private List<LineData> linesData;
        private int numberOfPoints = 25;
        private float initialDistance = 1f;
        private List<CircleLineSegment> lineSegments;
        List<double> killNodes = new List<double>(); // list of nodes that were hit
        List<int> indexHit = new List<int>(); // concrete

        public List<LineData> publicLinesData => linesData;

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
            MoveSegments();
        }

        double FindIndexOfCollision(Vector2 collisionPoint)
        {
            float xDiff = collisionPoint.x - transform.position.x;
            float yDiff = collisionPoint.y - transform.position.y;
            double angle = (Math.Atan2(yDiff, xDiff) + (2 * Math.PI)) % (2 * Math.PI);
            double anglePerIndex = 2 * Math.PI / numberOfPoints;
            return angle / anglePerIndex; 
        }

        //issues
        List<LineData> GenerateLinesPostCollision(List<double> killNodes)
        {
            List<LineData> lineDataSegments = new List<LineData>();
            int from = 0;
            int to;
            killNodes.Sort();
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

        LineData GenerateSegments(int from, int to, float distanceTravelled, float initialOpacity) //drawcircleV3
        {
            GameObject line = new GameObject();
            line.tag = "line_circle";
            line.AddComponent<LineRenderer>();
            LineRenderer lr = line.GetComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Sprites/Default"));
            //print("GEN from: " + from + " To: " + to);
            lr.positionCount = to - from;
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(0.0f, 0.05f);
            lr.widthCurve = curve;
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
            LineData newLineData = new LineData(from, to, lr, distanceTravelled, initialOpacity);
            newLineData.line.transform.SetParent(this.transform);
            return newLineData;
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

                lineData.UpdateCollider();

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



        void OnCollisionEnter2D(Collision2D col) //OnCollisionStay2D
        {
            //ignore collisions with other line circles
            if (col.gameObject.tag == "line_circle")
            {
                return;
            }


            foreach (ContactPoint2D contact in col.contacts)
            {
                //print("CONTACT: " + contact.point.x + " " + contact.point.y);
                double index = FindIndexOfCollision(col.contacts[0].point);
                //print(index);
               // print("rounded: " + (int)index);
                if (!indexHit.Contains((int)index))
                {
                    indexHit.Add((int)index);
                    killNodes.Add(index); 
                }
            }
            new WaitForSeconds(1);
            List<LineData> newLinesData = GenerateLinesPostCollision(killNodes);
            foreach (LineData lineData in linesData)
            {
                Destroy(lineData.line.gameObject);
                Destroy(lineData.polygonCollider2D.gameObject);
            }
            linesData.Clear();
            linesData = new List<LineData>();
            linesData.AddRange(newLinesData);
        }

    }
}