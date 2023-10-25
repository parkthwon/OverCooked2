using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    // �ڽ��� ���� static ����
    public static GameManager instance = null;

    // ��� Player���� PhotonView�� ������ List
    public List<PhotonView> listPlayer = new List<PhotonView>();

    //public Transform[] trSpawnPos;
    //���� ���° ���Դ���
    public int joinOrder;

    // Player
    // public GameObject player;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        spawnPos();

        joinOrder = PhotonNetwork.CurrentRoom.PlayerCount - 1;

        // ���� Player ����
        GameObject player = PhotonNetwork.Instantiate("PSW/Player", spawnPosList[joinOrder], Quaternion.identity);
        
        //GameObject player = PhotonNetwork.Instantiate("PSW/Player", trSpawnPos[idx].position, trSpawnPos[idx].rotation);
        
        // ���� ��ġ�ؾ� �ϴ� idx ������
        int idx = GameManager.instance.joinOrder;

        // �� ��ȭ
        PlayerMove pm = player.GetComponent<PlayerMove>();
        pm.SetFace(idx);

    }

    // ĳ���� ��������Ʈ
    public Vector3[] spawnPosList;
    int x = 0;
    void spawnPos()
    {
        spawnPosList = new Vector3[PhotonNetwork.CurrentRoom.MaxPlayers];

        for (int i = 0; i < spawnPosList.Length; i++)
        {
            spawnPosList[i] = new Vector3(x, 0, 0);
            x -= 5;
        }
    }

    public GameObject go2;
    public GameObject view;
    private void Update()
    {
        if(go2 != null && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            go2.SetActive(true);
        }
        
    }
}
