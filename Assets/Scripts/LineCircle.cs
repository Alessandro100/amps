using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class NodeData
{
    public Vector2 position;
    public double travelingAngle;
    public NodeData(Vector2 position, double travelingAngle)
    {
        this.position = position;
        this.travelingAngle = travelingAngle; // in radiens
    }

}

namespace LineManipulation
{

    public class LineCircle : MonoBehaviour
    {
        public float speed = 0.3f; // 0.3f per second

        private LineRenderer circleLine;
        //private Vector3[] circlePoints; // I'm pretty sure I'm going to have to save position and direction
        private List<NodeData> circlePoints;
        private List<CircleLineSegment> lineSegments;

        // Start is called before the first frame update
        void Awake()
        {
            ///circlePoints = GenerateCirclePoints(transform.position);
            //circleLine = DrawCircle();
            lineSegments = DrawCircleV2();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            //float timeChange = Time.deltaTime;
            //float distanceToTravel = timeChange * speed;
            //MoveCircle(distanceToTravel);
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

        LineRenderer DrawCircle()
        {
            print("DRAW $$$$$$$$$$");
            GameObject line = new GameObject();
            line.AddComponent<LineRenderer>();
            Rigidbody2D rb = line.AddComponent<Rigidbody2D>();
            //rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            rb.useFullKinematicContacts = true;
            LineRenderer lr = line.GetComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.positionCount = circlePoints.Count;
            float alpha = 0f;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.black, 0.0f), new GradientColorKey(Color.white, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
            lr.colorGradient = gradient;
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(0.0f, 0.1f);
            lr.widthCurve = curve;
            for (int i = 0; i < circlePoints.Count; i++)
            {
                //print(i.ToString() + ": " + circlePoints[i].ToString());
                lr.SetPosition(i, circlePoints[i].position);
            }
            //lr.loop = true;
            //GameObject.Destroy(myLine, duration);
            return lr;
        }

        List<NodeData> GenerateCirclePoints(Vector3 objectPosition)
        {
            int numberOfPoints = 25;
            List<NodeData> pointCoordinates = new List<NodeData>();
            double angle = 2 * Math.PI / numberOfPoints; // in radians
            float initialLength = 1f;
            // iteration
            for (int i = 0; i < numberOfPoints; i++)
            {
                double pointAngle = angle * i;
                float y = objectPosition.y + ((float)Math.Sin(pointAngle) * initialLength);
                float x = objectPosition.x + ((float)Math.Cos(pointAngle) * initialLength);
                //print("ITERATE");
                //print("angle:" + pointAngle +", x: " + x.ToString() + ", y:" + y.ToString());
                Vector3 point = new Vector3(x, y, 0);
                NodeData pointData = new NodeData(point, pointAngle);
                pointCoordinates.Add(pointData);
            }
            return pointCoordinates;
        }

        void MoveCircle(float distanceToTravel)
        {
            //double angle = 2 * Math.PI / circlePoints.Length; // iteration angle in radians
            for (int i = 0; i < circlePoints.Count; i++)
            {
                double pointAngle = circlePoints[i].travelingAngle;
                float y = circlePoints[i].position.y + ((float)Math.Sin(pointAngle) * distanceToTravel);
                float x = circlePoints[i].position.x + ((float)Math.Cos(pointAngle) * distanceToTravel);
                Vector3 newPosition = new Vector2(x, y);
                circlePoints[i].position = newPosition;
                circleLine.SetPosition(i, newPosition);
            }
        }

        public Vector3[] GetPointPositions()
        {
            Vector3[] positions = new Vector3[circleLine.positionCount];
            circleLine.GetPositions(positions);
            return positions;
        }

        public float GetWidth()
        {
            return circleLine.startWidth;
        }

        int GetNodeClosestToPointIndex(Vector2 point)
        {
            float minDistance = float.PositiveInfinity;
            int smallestNodeIndex = 0;
            for(int i = 0; i < circlePoints.Count; i++)
            {
                float distance = Vector2.Distance(circlePoints[i].position, point);
                if(distance < minDistance)
                {
                    minDistance = distance;
                    smallestNodeIndex = i;
                }
            }
            return smallestNodeIndex;
        }

        public void CollisionCalculate(Collision2D col)
        {
            print("CollisionCalculate CollisionCalculate CollisionCalculate CollisionCalculate");
            //col.

            foreach (ContactPoint2D contact in col.contacts)
            {
                print(contact.point); // with this point find the closest point in the array
                int index = GetNodeClosestToPointIndex(contact.point);
                print(index);
                //circlePoints[index+1].travelingAngle += Math.PI /2;
                //circlePoints[index].travelingAngle += Math.PI /2;
                //circlePoints[index-1].travelingAngle += Math.PI /2;

                circlePoints.RemoveAt(index);

                print(contact.normal);
                double angleOfContact = Math.Atan2(contact.normal.y, contact.normal.x);
                ///// line segment stuff
                ///
            }
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            print("OnCollisionEnter2D");
            //col.

            foreach (ContactPoint2D contact in col.contacts)
            {
                //print(contact.point); // with this point find the closest point in the array
                //circlePoints[GetNodeClosestToPointIndex(contact.point)].travelingAngle += Math.PI;
                //print(contact.normal);
                double angleOfContact = Math.Atan2(contact.normal.y, contact.normal.x);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            print("On trigger enter 2d");
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                print("Point of contact: " + hit.point);
            }
            //print("aaaaaaa: " + collision.attachedRigidbody.GetPointVelocity().ToString());
        }
    }
}