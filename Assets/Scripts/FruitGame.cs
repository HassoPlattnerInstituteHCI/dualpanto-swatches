using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using DualPantoToolkit;

public class FruitGame : MonoBehaviour
{
    private LowerHandle lowerHandle;
    private UpperHandle meHandle;

    public Transform fruitStartPosition;
    public Transform fruitEndPosition;

    bool scriptActive = false;

    public float fruitMinSpeed = 20f;
    public float fruitMaxSpeed = 30f;

    public float parabelaMinHeightZ = 1f;
    public float parabelaMaxHeightZ = 3f;

    public float currentSpeed;
    public float currentParabla;
    public float offsetX = 0.5f;

    public int startDelay = 1000;

    public float fixedY = 0f;

    public bool foodSliced = false;

    private Vector3 flightStartPosition;
    private Vector3 flightEndPosition;
    private float flightDistance;
    private float travelledDistance;
    private bool isFlying;

    void Start()
    {
        lowerHandle = GameObject.Find("Panto").GetComponent<LowerHandle>();
        meHandle = GameObject.Find("Panto").GetComponent<UpperHandle>();
    }

    public async void StartGame()
    {
        await lowerHandle.SwitchTo(this.gameObject, 100.0f);
        InitializeFlight();
        await Task.Delay(startDelay);
        scriptActive = true;
    }

    private async void RestartFlightWithDelay()
    {
        isFlying = false;
        transform.position = flightStartPosition;
        await Task.Delay(startDelay);
        InitializeFlight();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foodSliced = true;
            SoundManager.Instance.Play("cut");
            RestartFlightWithDelay();
        }
    }

    public void EndGame()
    {
        scriptActive = false;
        isFlying = false;
        FreeHandle();
    }

    void FreeHandle()
    {
        lowerHandle.Free();
    }

    void FixedUpdate()
    {
        if (!scriptActive || !isFlying)
        {
            return;
        }

        travelledDistance += currentSpeed * Time.fixedDeltaTime;
        float progress = flightDistance <= 0.0001f ? 1f : travelledDistance / flightDistance;
        progress = Mathf.Clamp01(progress);

        Vector3 nextPosition = EvaluateFlightPoint(progress);
        nextPosition.y = fixedY;
        transform.position = nextPosition;

        if (progress >= 1f)
        {
            SoundManager.Instance.Play("loose");
            RestartFlightWithDelay();
        }
    }

    private void InitializeFlight()
    {
        flightStartPosition = fruitStartPosition != null ? fruitStartPosition.position : transform.position;
        flightEndPosition = fruitEndPosition != null ? fruitEndPosition.position : transform.position;

        flightStartPosition.y = fixedY;
        flightEndPosition.y = fixedY;

        transform.position = flightStartPosition;

        currentSpeed = Random.Range(fruitMinSpeed, fruitMaxSpeed);
        currentParabla = Random.Range(parabelaMinHeightZ, parabelaMaxHeightZ);
        if (Random.value < 0.5f)
        {
            currentParabla = -currentParabla;
        }
        offsetX = Mathf.Clamp01(offsetX);

        Vector3 startXZ = new Vector3(flightStartPosition.x, 0f, flightStartPosition.z);
        Vector3 endXZ = new Vector3(flightEndPosition.x, 0f, flightEndPosition.z);
        flightDistance = Vector3.Distance(startXZ, endXZ);
        travelledDistance = 0f;
        isFlying = flightDistance > 0.0001f;
    }

    private Vector3 EvaluateFlightPoint(float t)
    {
        Vector3 start = flightStartPosition;
        Vector3 end = flightEndPosition;

        Vector3 direction = end - start;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f)
        {
            return start;
        }

        Vector3 control = Vector3.Lerp(start, end, offsetX);
        Vector3 perpendicular = new Vector3(-direction.z, 0f, direction.x).normalized;
        control += perpendicular * currentParabla;

        float oneMinusT = 1f - t;
        return (oneMinusT * oneMinusT * start) + (2f * oneMinusT * t * control) + (t * t * end);
    }
}
