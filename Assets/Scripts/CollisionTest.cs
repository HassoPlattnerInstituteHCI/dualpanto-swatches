using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoToolkit;

public class CollisionTest : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with " + other.gameObject.name + "Tag: " + other.gameObject.tag);
    }
}