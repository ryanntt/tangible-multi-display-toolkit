using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LEDControl : MonoBehaviour
{
    public Material LEDON;
    public Material LEDOFF;
    public Color LEDColor;

    public float offsetTime = 1; // speed of blinking
    public List<Transform> LEDObjects; 

    // Start is called before the first frame update
    void Start()
    {
        GameObject car = GameObject.FindGameObjectsWithTag("CarPlayer")[0];
        this.transform.SetParent(car.transform);

        LEDObjects = new List<Transform>();
        foreach (Transform child in transform) 
        {
            LEDObjects.Add(child);
        }
        LEDObjects.OrderBy(go => go.name);

        LEDColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0f, 1f);

        LEDON.SetColor("_Color", LEDColor);
        LEDON.SetColor("_EmissionColor", LEDColor);

        StartCoroutine(Blink());
        
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    IEnumerator Blink()
    {
        while (true) {
            for (int i = 0; i < LEDObjects.Count; i++) {
                LEDObjects[i].GetComponent<Renderer>().material = LEDON;
                LEDObjects[i].GetChild(0).GetComponent<Light>().enabled = true;
                yield return new WaitForSeconds(offsetTime);
                LEDObjects[i].GetComponent<Renderer>().material = LEDOFF;
                LEDObjects[i].GetChild(0).GetComponent<Light>().enabled = false;
            }
        }
    }
}
