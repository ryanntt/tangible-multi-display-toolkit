using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianCamera : MonoBehaviour
{
    public Transform carForTouch;
    public Transform carAnimation;
    public Transform target;
    public GameObject leds;
    public GameObject pedestrianModel;
    public GameObject contexts;

    bool TangibleModeEnabled = true;



    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        // Keep the camera pointed at car animation position
        if (TangibleModeEnabled == false)
        {
            transform.LookAt(target);

        }
    }

    public void TangibleMode_Changed(bool newValue)
    {
        TangibleModeEnabled = newValue;
        GameObject car = carForTouch.GetChild(0).gameObject;
        GameObject carclone = this.carAnimation.GetChild(0).gameObject;
        GameObject leds = this.leds;
        if (TangibleModeEnabled == false)
        {
            pedestrianModel.SetActive(false);


            car.SetActive(false);
            carclone.SetActive(true);
            leds.transform.SetParent(carclone.transform);
            transform.LookAt(target);

            foreach (Transform child in contexts.transform)
            {
                foreach (Transform subchild in child)
                {
                    subchild.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            pedestrianModel.SetActive(true);
            car.SetActive(true);
            carclone.SetActive(false);
            leds.transform.SetParent(car.transform);
            foreach (Transform child in contexts.transform)
            {
                foreach (Transform subchild in child)
                {
                    subchild.gameObject.SetActive(true);
                }
            }
        }
    }
}
