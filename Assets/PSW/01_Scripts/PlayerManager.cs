using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    // spawnPosGroup Transform
    public Transform [] trSpawnPos;

    public Dictionary<int, PhotonView> allPlayer = new Dictionary<int, PhotonView>();

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // RPC 호출 빈도
        PhotonNetwork.SendRate = 60;

        // OnPhotonSerializedView 호출 빈도
        PhotonNetwork.SerializationRate = 60;

        //SetSpawnPos();

        // 내가 위치해야 하는 idx 구하자
        int idx = GameManager.instance.joinOrder;
        
        GameObject player = PhotonNetwork.Instantiate("PSW/Player", trSpawnPos[idx].position, trSpawnPos[idx].rotation);

        PlayerMove pm = player.GetComponent<PlayerMove>();
        pm.SetFace(idx);

    }

    public void AddPlayer(PhotonView pv)
    {
        allPlayer[pv.ViewID] = pv;
    }

}
