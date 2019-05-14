using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Photon.Pun.UtilityScripts;

public class LEDControl : MonoBehaviour
{
    public Material LEDON;
    public Material LEDOFF;
    public Color LEDColor;
    public Color backColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);
    public float offsetTime = 1; // speed of blinking
    public List<Transform> LEDObjects;
    public List<Material> LEDMaterials;

    string str = "";
    string oldStr = "";
    string[] strings;

    bool lockThread = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject carclone = GameObject.FindGameObjectsWithTag("CarClone")[0].transform.GetChild(0).gameObject;


        if (carclone.activeInHierarchy == false)
        {
            GameObject car = GameObject.FindGameObjectsWithTag("CarPlayer")[0];
            this.transform.SetParent(car.transform);
        }
        else 
        {
            this.transform.SetParent(carclone.transform);
        }

        LEDObjects = new List<Transform>();
        foreach (Transform child in transform)
        {
            LEDObjects.Add(child);
        }
        LEDObjects.OrderBy(go => go.name);

        LEDColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.75f, 1f);

        LEDON.SetColor("_Color", new Color(0.5f, 0.5f, 0.5f, 0.2f));
        LEDON.SetColor("_EmissionColor", LEDColor);
        LEDON.EnableKeyword("_EMISSION");

        ChangeColor(new string[] { "#00FF00", "#FF0000", "#0000FF", "#00FF00", "#FF0000", "#0000FF" });

        //StartCoroutine(Blink());

    }

    // Update is called once per frame
    void Update()
    {
        if (!lockThread)
        {
            lockThread = true;
            //quick fix to avoid flickering of the LEDs (if we later send dynamic light patterns, we should check for each single LED if the value changed to avoid flickering
            if (str != oldStr)
            {
                for (int i = 0; i < LEDObjects.Count; i++)
                {
                    if (i < strings.Length)
                    {
                        Color newCol;
                        ColorUtility.TryParseHtmlString(strings[i], out newCol);
                        LEDMaterials[i].SetColor("_Color", backColor);
                        LEDMaterials[i].SetColor("_EmissionColor", newCol);
                    }

                    else
                    {
                        //LEDMaterials[i].SetColor("_Color", Random.ColorHSV(0f, 1f, 1f, 1f, 0.75f, 1f));
                        //LEDON.SetColor("_EmissionColor", Random.ColorHSV(0f, 1f, 1f, 1f, 0.75f, 1f));
                    }
                    LEDObjects[i].GetComponent<Renderer>().material = LEDMaterials[i];
                    LEDObjects[i].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                }

                oldStr = str;
            }
            lockThread = false;
        }
    }

    IEnumerator Blink()
    {
        while (true) {
            for (int i = 0; i < LEDObjects.Count; i++) {
                //LEDObjects[i].GetComponent<Renderer>().material = LEDON;
                LEDObjects[i].GetChild(0).GetComponent<Light>().enabled = true;
                yield return new WaitForSeconds(offsetTime);
                //LEDObjects[i].GetComponent<Renderer>().material = LEDOFF;
                LEDObjects[i].GetChild(0).GetComponent<Light>().enabled = false;
            }
        }
    }

    public void ChangeColor(string[] stringsSend)
    {
        if (!lockThread)
        {
            lockThread = true;
            str = "";
            for (int i = 0; i < stringsSend.Length; i++)
            {
                str += stringsSend[i];
            }
            print(str);
            strings = stringsSend;
            lockThread = false;
        }

    }
}
