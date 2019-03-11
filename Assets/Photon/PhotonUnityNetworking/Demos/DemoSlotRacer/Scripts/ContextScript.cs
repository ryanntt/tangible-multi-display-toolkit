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
    private TextMesh textMesh;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var dist = Vector3.Distance(statusRenderer.transform.position, target.transform.position);
        print("Distance to car is:"+ dist);

        if ( dist <= 3.0) 
        {
            ActivateContext();
            UpdateText("Activated :)");
        } else
        {
            DeactivateContext();
            UpdateText("Inactive");
        }
    }

    // To change the visual of context indicator
    public void ActivateContext() 
    {
        statusRenderer.sprite = activated;
    }

    public void DeactivateContext() {
        statusRenderer.sprite = inactive; 
    }

    // To change the display text
    public void UpdateText(string textdisplay)
    {
        textMesh.text = textdisplay;
    }
}
