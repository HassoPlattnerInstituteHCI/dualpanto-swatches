using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using DualPantoToolkit;

public class ShootingGame : MonoBehaviour
{

    // Start is called before the first frame update

    private UpperHandle meHandle;
    private LowerHandle lowerHandle;

    bool scriptActive = false;
    void Start()
    {
        meHandle = GameObject.Find("Panto").GetComponent<UpperHandle>();
        lowerHandle = GameObject.Find("Panto").GetComponent<LowerHandle>();

    }
    public async void StartGame()
    {
        await lowerHandle.SwitchTo(this.gameObject, 100.0f);
        scriptActive = true;
    }

    void ShootBall()
    {
        Debug.Log("shoot ball");

        Vector3 origin = meHandle.GetPosition();
        float angle = meHandle.GetRotation();
        Quaternion rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 direction = rotation * Vector3.forward;
        direction.y = 0f;
        direction.Normalize();

        float maxDistance = 100f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance))
        {
            Debug.Log("Hit: " + hit.collider.name);

            // Prüfen: wurde genau dieses Objekt getroffen?
            if (hit.collider.gameObject == gameObject)
            {
                EndGame();
                SoundManager.Instance.Play("done");
            }
            else
            {
                SoundManager.Instance.Play("loose");
            }
        }

        Debug.DrawRay(origin, direction * maxDistance, Color.red, 2f);
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
    // Update is called once per frame
    void Update()
    {
        if (scriptActive)
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.M))
            {
                ShootBall();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                ShootBall();
            }
        }

    }
}
