using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MenuRoom : MonoBehaviour
{

    public string instruction = "";
    SelectionManager selectionManager;

    // Start is called before the first frame update
    void Start()
    {
        selectionManager = GetComponentsInChildren<SelectionManager>()[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private async Task OnTriggerEnter(Collider other)
    {
        await selectionManager.Speak(instruction);
        selectionManager.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {   
        selectionManager.StopSpeech();
        selectionManager.enabled = false;
    }


}
