using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    // 자신을 담을 static 변수
    public static GameManager instance = null;

    // 모든 Player들의 PhotonView를 가지는 List
    public List<PhotonView> listPlayer = new List<PhotonView>();

    //public Transform[] trSpawnPos;
    //내가 몇번째 들어왔는지
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

        // 나의 Player 생성
        GameObject player = PhotonNetwork.Instantiate("PSW/Player", spawnPosList[joinOrder], Quaternion.identity);
        
        //GameObject player = PhotonNetwork.Instantiate("PSW/Player", trSpawnPos[idx].position, trSpawnPos[idx].rotation);
        
        // 내가 위치해야 하는 idx 구하자
        int idx = GameManager.instance.joinOrder;

        // 얼굴 변화
        PlayerMove pm = player.GetComponent<PlayerMove>();
        pm.SetFace(idx);

    }

    // 캐릭터 스폰포인트
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
