using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianCamera : MonoBehaviour
{
    public Transform target;
    public GameObject pedestrianModel;

    bool TangibleModeEnabled = true;

    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        if (TangibleModeEnabled == false)
        {
            pedestrianModel.SetActive(false);
            transform.LookAt(target);
        } else {
            pedestrianModel.SetActive(true);
        }
    }

    public void TangibleMode_Changed(bool newValue)
    {
        TangibleModeEnabled = newValue;
    }
}
