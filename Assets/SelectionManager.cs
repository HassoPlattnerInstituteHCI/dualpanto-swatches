using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks; //note that you need to include this if you want to use Task.Delay.
using SpeechIO;
using UnityEngine;
using DualPantoToolkit;

public class SelectionManager : MonoBehaviour
{

    public bool enabled = false;
    public string preliminarySelection = "";
    public string finalSelection = "";
    string lastAnnouncedSelection = "";
    float timeLastAnnounced = 0;
    public bool confirmationRequired = true;
    bool finalSelectionMade = false;

    SpeechOut speechOut;

    // Start is called before the first frame update
    void Start()
    {
        speechOut = new SpeechOut();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastAnnouncedSelection != preliminarySelection && Time.time > timeLastAnnounced + 0.5)
        {
            lastAnnouncedSelection = preliminarySelection;
            Speak(preliminarySelection);
        }

        if (enabled && confirmationRequired)
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.M))
                {
                    finalSelection = preliminarySelection;
                    finalSelectionMade = true;
                    Speak("Final selection made: " + finalSelection);
                }
        }
    }

    void SetSelection(string newSelection)
    {
        if (enabled)
        {
            if (!confirmationRequired)
            {
                preliminarySelection = newSelection;
                finalSelection = newSelection;
            } else if (!finalSelectionMade)
            {
                preliminarySelection = newSelection;
            }
        }
    }

    public async Task Speak(string announcement)
    {
        await speechOut.Speak(announcement);
    }

    public void StopSpeech()
    {
        speechOut.Stop();
    }

    void OnApplicationQuit()
    {
        speechOut.Stop(); //Windows: do not remove this line.
    }
}
