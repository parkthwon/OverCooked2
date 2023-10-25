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
        // RPC ȣ�� ��
        PhotonNetwork.SendRate = 60;

        // OnPhotonSerializedView ȣ�� ��
        PhotonNetwork.SerializationRate = 60;

        //SetSpawnPos();

        // ���� ��ġ�ؾ� �ϴ� idx ������
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
