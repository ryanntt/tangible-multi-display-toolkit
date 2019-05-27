using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextScript : MonoBehaviour
{
    public Transform target;

    [SerializeField]
    private Sprite inactive, activated;

    [SerializeField]
    private SpriteRenderer statusRenderer;

    [SerializeField]
    public TextMesh textMesh;

    public bool isActivated;

    public int ID;

    bool TangibleModeEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (TangibleModeEnabled == true)
        {
            var dist = Vector3.Distance(statusRenderer.transform.position, target.transform.position);
            //print("Distance to car is:"+ dist);

            if (dist <= 3.0)
            {
                ActivateContext();
                //UpdateText("Activated :)");
            }
            else
            {
                DeactivateContext();
                //UpdateText("Inactive");
            }
        }
    }

    // To change the visual of context indicator
    private void ActivateContext() 
    {
        statusRenderer.sprite = activated;
        if (!isActivated)
        {
            ContextManager ctxManager = GameObject.FindWithTag("Contexts").GetComponent<ContextManager>();
            ctxManager.ActivateContext(ID);
            isActivated = true;
        }
    }

    private void DeactivateContext() {
        statusRenderer.sprite = inactive;
        if (isActivated)
        {
            isActivated = false;
        }
    }

    // To change the display text
    public void UpdateText(string textdisplay)
    {
        textMesh.text = textdisplay;
    }

    public void TangibleMode_Changed(bool newValue)
    {
        TangibleModeEnabled = newValue;
    }
}
