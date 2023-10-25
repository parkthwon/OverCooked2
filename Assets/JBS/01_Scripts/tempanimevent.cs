using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempanimevent : MonoBehaviour
{
    PlayerCutWash pc;
    
    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlayerCutWash>();
    }

    //void OnKnifeChop()
    //{
    //    pc.OnKnifeChop();
    //}
}
