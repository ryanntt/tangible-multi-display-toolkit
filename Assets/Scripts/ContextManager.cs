using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextManager : MonoBehaviour
{
    public GameObject[] ContextIndicators;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateContext(int i, string text) {
        if (i <= ContextIndicators.Length) {
            this.ContextIndicators[i - 1].GetComponent<ContextScript>().UpdateText(text);
        }
    }
}

