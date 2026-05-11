using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks; //note that you need to include this if you want to use Task.Delay.
using SpeechIO;
using UnityEngine;
using DualPantoToolkit;

public class DialManager : MonoBehaviour
{
    PantoHandle upperHandle;
    int prevSectionID = -1;
    public string[] optionStrings = new string[]{"option 1", "option 2", "option 3"};
    SpeechOut speechOut;
    bool isInRoom = false;

    string finalSelection = "";
    bool finalSelectionMade = false;
    
    void Start()
    {
        speechOut = new SpeechOut();
        upperHandle = GameObject.Find("Panto").GetComponent<UpperHandle>();
        if (upperHandle == null)
        {
            Debug.LogError("[DualPanto] UpperHandle not found on Panto GameObject!");
        }
    }

    void Update()
    {
        if (isInRoom && !finalSelectionMade)
        {
            int newSectionID = (int) (upperHandle.GetRotation() / 360.0 * optionStrings.Length);
            if (newSectionID != prevSectionID) {
                prevSectionID = newSectionID;
                Speak(optionStrings[newSectionID]);
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.M))
                {
                    finalSelection = optionStrings[newSectionID];
                    finalSelectionMade = true;
                    Speak("Final selection made: " + finalSelection);
                }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        isInRoom = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isInRoom = false;
    }

    public async void Speak(string announcement)
    {
        await speechOut.Speak(announcement);
    }

    void OnApplicationQuit()
    {
        speechOut.Stop(); //Windows: do not remove this line.
    }
}