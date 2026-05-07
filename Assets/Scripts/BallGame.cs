using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using DualPantoToolkit;

public class BallGame : MonoBehaviour
{

    // Start is called before the first frame update
    private LowerHandle lowerHandle;
    private UpperHandle meHandle;

    public BoxCollider roomCollider;
    bool scriptActive = false;

    private Vector3 ballVelocity = Vector3.zero;
    public float ballSpeed = 20f;
    public float deceleration = 8f;
    public float stopThreshold = 0.05f;
    public float wallBounceDamping = 0.9f;
    public float fixedY = 0f;

    public bool inverseReciolDirection = false;

    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float recoilStrength = 3f;

    [SerializeField]
    [Range(1.0f, 10.0f)]
    private float recoilSpeed = 5f;


    void Start()
    {
        lowerHandle = GameObject.Find("Panto").GetComponent<LowerHandle>();
        meHandle = GameObject.Find("Panto").GetComponent<UpperHandle>();
    }
    public async void StartGame()
    {
        await lowerHandle.SwitchTo(this.gameObject, 100.0f);
        scriptActive = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var collisionPoint = other.ClosestPoint(transform.position);
            ApplyRecoil(collisionPoint);
            Vector3 currentPosition = transform.position;

            // Push ball away from player contact point on the XZ plane.
            Vector3 shootDirection = currentPosition - collisionPoint;
            shootDirection.y = 0f;

            // Fallback if closest point is too close to center.
            if (shootDirection.sqrMagnitude < 0.0001f)
            {
                shootDirection = currentPosition - other.transform.position;
                shootDirection.y = 0f;
            }

            if (shootDirection.sqrMagnitude < 0.0001f)
            {
                shootDirection = transform.forward;
                shootDirection.y = 0f;
            }

            ShootBall(shootDirection);
            Debug.Log("ball hit player, shoot in direction: " + shootDirection);
        }
    }
    // applies recoil to the player away from the collision point
    async void ApplyRecoil(Vector3 collisionPoint){
       
        Vector3 currentPosition = meHandle.GetPosition();
        // calculate the direction of the recoil
        Vector3 recoilDirection = (collisionPoint.normalized - currentPosition.normalized).normalized;   
        // apply the recoil to meHandle
        if (inverseReciolDirection){
            recoilDirection = -recoilDirection;
        }  
        await meHandle.MoveToPosition(currentPosition + (recoilDirection * recoilStrength), recoilSpeed);
    }
    public void EndGame()
    {
        scriptActive = false;
        FreeHandle();
    }


    void FreeHandle()
    {
        lowerHandle.Free();
    }

    void FixedUpdate()
    {
        if (scriptActive)
        {
            if (ballVelocity.sqrMagnitude > 0f)
            {
                ballVelocity = Vector3.MoveTowards(ballVelocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
                if (ballVelocity.magnitude < stopThreshold)
                {
                    ballVelocity = Vector3.zero;
                }
            }

            Vector3 newPosition = transform.position + ballVelocity * Time.fixedDeltaTime;
            newPosition.y = fixedY;

            if (roomCollider != null)
            {
                Bounds b = roomCollider.bounds;
                bool hitX = false;
                bool hitZ = false;

                if (newPosition.x < b.min.x)
                {
                    newPosition.x = b.min.x;
                    hitX = true;
                }
                else if (newPosition.x > b.max.x)
                {
                    newPosition.x = b.max.x;
                    hitX = true;
                }

                if (newPosition.z < b.min.z)
                {
                    newPosition.z = b.min.z;
                    hitZ = true;
                }
                else if (newPosition.z > b.max.z)
                {
                    newPosition.z = b.max.z;
                    hitZ = true;
                }

                if (hitX)
                {
                    ballVelocity.x = -ballVelocity.x * wallBounceDamping;
                }
                if (hitZ)
                {
                    ballVelocity.z = -ballVelocity.z * wallBounceDamping;
                }
            }

            transform.position = newPosition;
        }
    }

    public void ShootBall(Vector3 shootDirection)
    {
        SoundManager.Instance.Play("ball");
        // Normalize and ensure Y is 0
        shootDirection.y = 0f;
        if (shootDirection.sqrMagnitude < 0.0001f)
            return;

        shootDirection = shootDirection.normalized;

        // Set ball velocity in the shoot direction
        ballVelocity = shootDirection * ballSpeed;

        Vector3 pos = transform.position;
        pos.y = fixedY;
        transform.position = pos;
    }
}
