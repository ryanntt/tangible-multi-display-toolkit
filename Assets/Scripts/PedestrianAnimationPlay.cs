using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianAnimationPlay : MonoBehaviour
{
    public GameObject[] ContextIndicators;
    Vector3 originalPos;
    private void Start()
    {
        originalPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
    }

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
                    if (this.transform.position.z >  22.5f)
                    {
                        transform.position = originalPos;
                    }
                }
                else
                {
                    GetComponent<Animator>().Play("HumanoidIdle");
                    transform.position = new Vector3(2f, 2.004f, 24.92f);
                }
            }
        }
    }
}
