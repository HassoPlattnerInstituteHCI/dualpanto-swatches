using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using DualPantoToolkit;

public class HandleToGameObject : MonoBehaviour
{
    public bool onUpperHandle = true;
    public bool onLowerHandle = false;
    // Start is called before the first frame update

    private UpperHandle meHandle;
    private LowerHandle lowerHandle;
    void Start()
    {
        meHandle = GameObject.Find("Panto").GetComponent<UpperHandle>();
        lowerHandle = GameObject.Find("Panto").GetComponent<LowerHandle>();

    }
    
    public async void HandleMoveToGameObjectPosition(){
        if (onUpperHandle){
            await meHandle.SwitchTo(this.gameObject, 100.0f);
        } else if (onLowerHandle){
            await lowerHandle.SwitchTo(this.gameObject, 100.0f);
        }
    }
    public void FreeHandle(){
        if (onUpperHandle){
             meHandle.Free();
        } else if (onLowerHandle){
            lowerHandle.Free();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
