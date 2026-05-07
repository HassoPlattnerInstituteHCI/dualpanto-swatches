using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoToolkit;

public class ItFollowsMe : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject player;
    private LowerHandle lowerHandle;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        lowerHandle = GameObject.Find("Panto").GetComponent<LowerHandle>();
    }

    public async void StartFollowing()
    {
        Debug.Log("it start following me");

        await lowerHandle.SwitchTo(player, 100.0f);
    }
    public void StopFollowing()
    {
        Debug.Log("it stop following me");
        lowerHandle.Free();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
