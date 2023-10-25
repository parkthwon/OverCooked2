using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BlueHighlight : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
