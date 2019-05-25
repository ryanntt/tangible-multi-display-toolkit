using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianAnimationPlay : MonoBehaviour
{
    public GameObject[] ContextIndicators;

    // Update is called once per frame
    void Update()
    {
        // Check which context is activated


        // Play the animation according to which context is activated, also according to the car's relative distance to context indicator.

        for (int i = 0; i < this.ContextIndicators.Length; i++)
        {
            ContextScript ctxScript = this.ContextIndicators[i].GetComponent<ContextScript>();
            if (ctxScript.isActivated == true)
            {
                if (ctxScript.ID == 1)
                {
                    GetComponent<Animator>().Play("HumanoidWalk");
                }
                else
                {
                    GetComponent<Animator>().Play("HumanoidIddle");
                }
            }
        }
    }
}
