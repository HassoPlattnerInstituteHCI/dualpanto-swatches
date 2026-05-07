using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DualPantoToolkit;
using SpeechIO;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Plays speech when the PantoHandle enters a room. 
/// !!! Room should have a collider with "Is Trigger" enabled.
/// </summary>
public class PlayerHitTarget : MonoBehaviour
{
    public bool onUpperEnter = true;
    public bool onLowerEnter = false;
    public AudioClip finishClip;

    public bool PantoColliderDisableOnHit = true;

    private PantoCollider pantoCollider;
    private AudioSource audioSource;

    void Start()
    {
        pantoCollider = this.gameObject.GetComponent<PantoCollider>();
    }
    void Awake()
    {
        if (finishClip != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = finishClip;
        }
    }


    async Task OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.gameObject.name + "This GO" + gameObject.name);
        if(other.gameObject == null) return;
        if ((other.gameObject.tag == "MeHandle" && onUpperEnter) || (other.gameObject.tag == "ItHandle" && onLowerEnter))
        {
            await EntrySpeechSound();
            if(PantoColliderDisableOnHit){
                if (pantoCollider == null){
                    pantoCollider = other.gameObject.GetComponent<PantoCollider>();
                }
                if (pantoCollider != null){
                    pantoCollider.Disable();
                }
            }
        }
    }

    // async void OnTriggerExit(Collider other)
    // {
    //     if(other.gameObject == null) return;
    //     if ((other.gameObject.tag == "MeHandle" && onUpperEnter) || (other.gameObject.tag == "ItHandle" && onLowerEnter))
    //     {
    //     }
    // }

    public void ResetCollider(){
        if (pantoCollider != null){
            pantoCollider.Enable();
        }
    }

    private async Task EntrySpeechSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
