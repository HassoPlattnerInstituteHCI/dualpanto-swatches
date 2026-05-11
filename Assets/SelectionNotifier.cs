using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionNotifier : MonoBehaviour
{

    public string selectionName = "generic selection";
    GameObject parentObject;
    // Start is called before the first frame update
    void Start()
    {
        parentObject = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        parentObject.SendMessage("SetSelection", selectionName);
    }
}
