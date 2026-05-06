using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DualPantoToolkit;
using System;

public class PlayerRecoil : MonoBehaviour
{
   
    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float recoilStrength = 3f;

    [SerializeField]
    [Range(1.0f, 10.0f)]
    private float recoilSpeed = 5f;

    [SerializeField]
    private bool inverseReciolDirection = false;

    private UpperHandle meHandle;

    void Start()
    {
        meHandle = GameObject.Find("Panto").GetComponent<UpperHandle>();
    }

    // checks for collision with enemy and applies recoil
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            var collisionPoint = other.ClosestPoint(transform.position);
            ApplyRecoil(collisionPoint);
        }
    }
    
    // applies recoil to the player away from the collision point
    async void ApplyRecoil(Vector3 collisionPoint){
       
        Vector3 currentPosition = meHandle.GetPosition();
        // calculate the direction of the recoil
        Vector3 recoilDirection = (collisionPoint.normalized - currentPosition.normalized).normalized;   
        if (inverseReciolDirection){
            recoilDirection = -recoilDirection;
        } 
        // apply the recoil to meHandle
        await meHandle.MoveToPosition(currentPosition + (recoilDirection * recoilStrength), recoilSpeed);
        
    }
    
}