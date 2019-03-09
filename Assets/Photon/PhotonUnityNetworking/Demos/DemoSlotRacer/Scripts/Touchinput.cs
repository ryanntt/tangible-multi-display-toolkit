using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class Touchinput : MonoBehaviourPunCallbacks
{

    public LayerMask touchInputMask;

    float DistanceObject1Long1 = 400;
    float DistanceObject1Long2 = 400;
    float DistanceObject1Min = 250;

    float DistanceObject2Long1 = 175;
    float DistanceObject2Long2 = 175;
    float DistanceObject2Min = 100;

    float treshhold = 30;

    // Start is called before the first frame update
    void Update()
    {

        if (Input.touches.Length > 2)
        {
            //print("more touches");

            int i = 0;
            int ii = 0;

            var distances = new float[20];
            Dictionary<float, Touch[]> distancesRef = new Dictionary<float, Touch[]>();

            foreach (Touch touch in Input.touches)
            {
                for (int j = i; j < Input.touches.Length - 1; j++)
                {
                    var distance = Vector2.Distance(Input.touches[i].position, Input.touches[j + 1].position);
                    Touch[] touches = { Input.touches[i], Input.touches[j + 1] };
                    distancesRef.Add(distance, touches);
                    distances[ii] = distance;
                    ii++;
                }
                i++;
            }

            bool distance1Dec = false;
            bool distance2Dec = false;
            bool distance3Dec = false;
            float distanceObject1Long1 = 0;
            float distanceObject1Long2 = 0;
            float distanceObject1Min = 0;


            bool distance4Dec = false;
            bool distance5Dec = false;
            bool distance6Dec = false;
            float distanceObject2Long1 = 0;
            float distanceObject2Long2 = 0;
            float distanceObject2Min = 0;

            foreach (float d in distances)
            {
                print(d);

                if (d < DistanceObject1Long1 + treshhold && d > DistanceObject1Long1 - treshhold && !distance1Dec)
                {
                    distance1Dec = true;
                    distanceObject1Long1 = d;
                }
                else if (d < DistanceObject1Long2 + treshhold && d > DistanceObject1Long2 - treshhold && !distance2Dec)
                {
                    distance2Dec = true;
                    distanceObject1Long2 = d;

                }
                else if (d < DistanceObject1Min + treshhold && d > DistanceObject1Min - treshhold)
                {
                    distance3Dec = true;
                    distanceObject1Min = d;

                } else if (d < DistanceObject2Long1 + treshhold && d > DistanceObject2Long1 - treshhold && !distance4Dec)
                {
                    distance4Dec = true;
                    distanceObject2Long1 = d;
                }
                else if (d < DistanceObject2Long2 + treshhold && d > DistanceObject2Long2 - treshhold && !distance5Dec)
                {
                    distance5Dec = true;
                    distanceObject2Long2 = d;

                }
                else if (d < DistanceObject2Min + treshhold && d > DistanceObject2Min - treshhold)
                {
                    distance6Dec = true;
                    distanceObject2Min = d;

                }

            }

            if (distance1Dec && distance2Dec && distance3Dec)
            {
                print("recognized object1");

                float sumX = 0;
                float sumY = 0;

                distancesRef.TryGetValue(distanceObject1Long1, out Touch[] touch1);
                distancesRef.TryGetValue(distanceObject1Long2, out Touch[] touch2);

                sumX += touch1[0].position.x + touch1[1].position.x;
                sumY += touch1[0].position.y + touch1[1].position.y;

                float add1X = 0, add1Y = 0;

                foreach (Touch t2 in touch2)
                {
                    foreach (Touch t1 in touch1)
                    {
                        if (t1.position.x != t2.position.x && t1.position.y != t2.position.y)
                        {
                            add1X = t2.position.x;
                            add1Y = t2.position.y;
                        }
                    }
                }

                sumX += add1X;
                sumY += add1Y;

                Vector2 p = new Vector2(0, 0);
                p.x = sumX / 3;
                p.y = sumY / 3;
                
                Ray ray = this.GetComponent<Camera>().ScreenPointToRay(p);
                RaycastHit hit;
                GameObject car = GameObject.FindWithTag("CarPlayer");

                float angle = 0;

                foreach (Touch t1 in touch1)
                {
                    foreach (Touch t2 in touch2)
                    {
                        if (t1.position.x == t2.position.x && t1.position.y == t2.position.y)
                        {
                            angle = Mathf.Atan2(t2.position.x - p.x, t2.position.y - p.y);
                            //print(angle * (180 / Mathf.PI));
                        }
                    }
                }

                if (Physics.Raycast(ray, out hit))
                {
                
                    GameObject recipient = hit.transform.gameObject;


                    car.GetComponent<PhotonView>().RequestOwnership();
                    car.transform.position = Utils.ChangeZ(car.transform.position, hit.point.z);
                    car.transform.position = Utils.ChangeX(car.transform.position, hit.point.x); // change x


                    car.transform.eulerAngles = new Vector3(0, angle * (180 / Mathf.PI), 0);


                }
            }


            if (distance4Dec && distance5Dec && distance6Dec)
            {
                print("recognized object2");

                float sumX = 0;
                float sumY = 0;

                distancesRef.TryGetValue(distanceObject2Long1, out Touch[] touch1);
                distancesRef.TryGetValue(distanceObject2Long2, out Touch[] touch2);

                sumX += touch1[0].position.x + touch1[1].position.x;
                sumY += touch1[0].position.y + touch1[1].position.y;

                float add1X = 0, add1Y = 0;

                foreach (Touch t2 in touch2)
                {
                    foreach (Touch t1 in touch1)
                    {
                        if (t1.position.x != t2.position.x && t1.position.y != t2.position.y)
                        {
                            add1X = t2.position.x;
                            add1Y = t2.position.y;
                        }
                    }
                }

                sumX += add1X;
                sumY += add1Y;


                Vector2 p = new Vector2(0, 0);
                p.x = sumX / 3;
                p.y = sumY / 3;
                
                Ray ray = this.GetComponent<Camera>().ScreenPointToRay(p);
                RaycastHit hit;
                GameObject pedestrian = GameObject.FindWithTag("PedestrianPlayer");

                float angle = 0;

                foreach (Touch t1 in touch1)
                {
                    foreach (Touch t2 in touch2)
                    {
                        if (t1.position.x == t2.position.x && t1.position.y == t2.position.y)
                        {
                            angle = Mathf.Atan2(t2.position.x - p.x, t2.position.y - p.y);
                            print(angle * (180 / Mathf.PI));
                        }
                    }
                }

                if (Physics.Raycast(ray, out hit))
                {

                    GameObject recipient = hit.transform.gameObject;

                    pedestrian.GetComponent<PhotonView>().RequestOwnership();
                    pedestrian.transform.position = Utils.ChangeZ(pedestrian.transform.position, hit.point.z);
                    pedestrian.transform.position = Utils.ChangeX(pedestrian.transform.position, hit.point.x); // change x

                    pedestrian.transform.eulerAngles = new Vector3(0, angle * (180 / Mathf.PI), 0);


                }


            }

        }


    }


}


public static class Utils
{


    public static Vector3 ChangeX(Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }

    public static Vector3 ChangeY(Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }

    public static Vector3 ChangeZ(Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }

}