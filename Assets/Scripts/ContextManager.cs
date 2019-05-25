using System;
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

    public void UpdateContexts(string message) 
    {
        string jsonString = JsonHelper.FixJson(message);
        Context[] contexts = JsonHelper.FromJson<Context>(jsonString);

        int activeContext = 0;

        ContextManager ctxManager = GameObject.FindWithTag("Contexts").GetComponent<ContextManager>();
        print(contexts[0].name);
        for (int i = 0; i < contexts.Length; i++)
        {
            string contextName = contexts[i].name;

            if (contexts[i].activated) {
                activeContext = i;
            }

            string finalString = contextName;
            int ctxLength = contextName.Length;

            int linebreakPosition = 15;

            if (ctxLength > linebreakPosition)
            {
                bool found = false;
                int position = linebreakPosition;
                while (!found)
                {
                    if (contextName[position] == ' ')
                    {
                        finalString = contextName.Substring(0, position) + '\n' + contextName.Substring(position);
                        found = true;
                    }
                    else
                    {
                        position--;
                    }
                }
            }

            UpdateContext(i + 1, finalString);
        }


        /*if (tangibleOff) {
         * ActivateContext(i);
         * }
         */        
    }

    public void ActivateContext(int id)
    {
        print("instance ID");
        print(id);

        Context ctx = new Context();
        for(int i=0; i<this.ContextIndicators.Length; i++)
        {
            print(this.ContextIndicators[i].GetComponent<ContextScript>().ID);
            if(this.ContextIndicators[i].GetComponent<ContextScript>().ID == id)
            {
                ContextScript ctxScript = this.ContextIndicators[i].GetComponent<ContextScript>();
                ctx.name = ctxScript.textMesh.text;
                ctx.id = id;
                ctx.activated = ctxScript.isActivated;
                print("context");
                print(ctx.name);
                print(ctx.id);
                print(ctx.activated);

                string contextJson = JsonUtility.ToJson(ctx);
                print(contextJson);

                UDPReceive udp = GameObject.FindWithTag("UDPReceive").GetComponent<UDPReceive>();
                udp.writeSocket(contextJson);
            }
        }
    }
}

[Serializable]
internal class Context
{
    public string name;
    public int id;
    public bool activated;
}