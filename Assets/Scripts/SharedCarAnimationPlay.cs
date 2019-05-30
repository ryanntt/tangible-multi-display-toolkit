using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedCarAnimationPlay : MonoBehaviour
{
    public GameObject[] ContextIndicators;

    // Update is called once per frame
    void Update()
    {
        // Check which context is activated


        // Play the animation according to which context is activated, also according to the car's relative distance to context indicator.

        for (int i=0; i< this.ContextIndicators.Length; i++)
        {
            ContextScript ctxScript = this.ContextIndicators[i].GetComponent<ContextScript>();
            if (ctxScript.isActivated == true)
            {
                if (ctxScript.ID == 1)
                {
                    GetComponent<Animator>().Play("Shared_CarGiveWay");
                } 
                else if (ctxScript.ID == 2)
                {
                    GetComponent<Animator>().Play("Shared_CarStart");

                }
                else if (ctxScript.ID == 3)
                {
                    GetComponent<Animator>().Play("Shared_CarTurnLeft");
                }
                else if (ctxScript.ID == 4)
                {
                    GetComponent<Animator>().Play("Shared_CarWaiting");
                }                    
                //else
                //{
                //    GetComponent<Animator>().Play("CarDefault");
                //}
                // We commented this to avoid freezing the car if we put car in other context
            }
        }
    }
}
