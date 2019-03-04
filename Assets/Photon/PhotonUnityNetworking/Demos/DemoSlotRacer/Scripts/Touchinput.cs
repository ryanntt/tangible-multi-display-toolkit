using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class Touchinput : MonoBehaviourPunCallbacks
{

    public LayerMask touchInputMask;

    float DistanceObjectLong1 = 425;
    float DistanceObjectLong2 = 425;
    float DistanceObjectMin = 275;

    // Start is called before the first frame update
    void Update()
    {

        /*if (Input.GetMouseButtonDown(0))
        {

            print("mouseclick");

            Ray ray = this.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject recipient = hit.transform.gameObject;
                print("hit" + recipient);

                GameObject car = GameObject.FindWithTag("CarPlayer");
                car.GetComponent<PhotonView>().RequestOwnership();
                car.transform.position = hit.point;

                recipient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
            }
        }

        int num = 0;

        foreach (Touch touch in Input.touches)
        {
            print("Touch");
            print(touch.position);
            print(num);
            num++;

            Ray ray = this.GetComponent<Camera>().ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit))
            {

                print(hit.point);

                GameObject recipient = hit.transform.gameObject;
                print("hit" + recipient);

                GameObject car = GameObject.FindWithTag("CarPlayer");

                car.GetComponent<PhotonView>().RequestOwnership();
                car.transform.position = hit.point;


                if (touch.phase == TouchPhase.Began)
                {
                    recipient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
                }
            }

        }*/

        if (Input.touches.Length > 2)
        {
            print("more touches");

            int i = 0;
            int ii = 0;

            float sumX = 0;
            float sumY = 0;

            var distances = new float[3];
            Dictionary<float, Touch[]> distancesRef = new Dictionary<float, Touch[]>();

            foreach (Touch touch in Input.touches)
            {
                sumX += touch.position.x;
                sumY += touch.position.y;

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
            float distanceObjectLong1 = 0;
            float distanceObjectLong2 = 0;

            foreach (float d in distances)
            {
                print(d);

                if (d < DistanceObjectLong1 + 75 && d > DistanceObjectLong1 - 75 && !distance1Dec)
                {
                    distance1Dec = true;
                    distanceObjectLong1 = d;
                }
                else if (d < DistanceObjectLong2 + 75 && d > DistanceObjectLong2 - 75 && !distance2Dec)
                {
                    distance2Dec = true;
                    distanceObjectLong2 = d;

                }
                else if (d < DistanceObjectMin + 75 && d > DistanceObjectMin - 75)
                {
                    distance3Dec = true;

                }

            }

            if (distance1Dec && distance2Dec && distance3Dec)
            {
                print("recognized");

                Vector2 p = new Vector2(0, 0);
                p.x = sumX / 3;
                p.y = sumY / 3;

                Ray ray = this.GetComponent<Camera>().ScreenPointToRay(p);
                RaycastHit hit;
                GameObject car = GameObject.FindWithTag("CarPlayer");

                distancesRef.TryGetValue(distanceObjectLong1, out Touch[] touch1);
                distancesRef.TryGetValue(distanceObjectLong2, out Touch[] touch2);

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

                    print(hit.point);

                    GameObject recipient = hit.transform.gameObject;
                    print("hit" + recipient);


                    car.GetComponent<PhotonView>().RequestOwnership();
                    car.transform.position = Utils.ChangeZ(car.transform.position, hit.point.z);
                    car.transform.position = Utils.ChangeX(car.transform.position, hit.point.x); // change x
                    //car.transform.position = Utils.ChangeY(car.transform.position, hit.point.y); // change x


                    car.transform.eulerAngles = new Vector3(0, angle * (180 / Mathf.PI), 0);


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