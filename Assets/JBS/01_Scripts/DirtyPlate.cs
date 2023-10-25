using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DirtyPlate : MonoBehaviourPun
{
    //접시 스택
    public int plateStack = 1;

    //요청되면 접시위에 접시를 쌓고싶다.
    public void StackPlate(GameObject newPlate)
    {
        if(newPlate.CompareTag("DirtyPlate"))
        {
            //접시 스택 증가
            plateStack++;
            //받은 접시 제거
            Destroy(newPlate);
            //if(PhotonNetwork.IsMasterClient)
            //    PhotonNetwork.Destroy(newPlate);
        }
    }
}
