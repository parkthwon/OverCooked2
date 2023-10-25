using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// 로비 화면

// 1. 플레이어를 큐브에서 실제 플레이어 프리팹으로 변경

// 2. 버튼 활성화
//  2.1. 마스터만 시작 버튼 활성화
//  2.2. 플레이어는 시작 버튼 비활성화

// 3. 플레이어 보드
//  3.1. 플레이어가 입장하면 이미지 변경

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // 시작 버튼
    public GameObject startBtn;
    Image startBtnImg;

    // 게임 나가기
    public GameObject quitBtn;


    // Start is called before the first frame update
    void Start()
    {
        startBtn = GameObject.FindGameObjectsWithTag("RoomOption")[0];
        quitBtn = GameObject.FindGameObjectsWithTag("RoomOption")[1];

        startBtnImg = startBtn.GetComponent<Image>();

        // 방장이 아니면 게임 시작 버튼 비활성화
        startBtnImg = startBtn.GetComponent<Image>();
        if (PhotonNetwork.IsMasterClient == false)
        {
            startBtnImg.color = Color.gray;
        }
    }

    public void OnStartButton()
    {
        if(PhotonNetwork.IsMasterClient == true)
        {
            print("씬을 변경합니다.");
            // game scene으로 이동
            PhotonNetwork.LoadLevel(2);
        }
        else
        {
            print("마스터만 게임시작을 누를 수 있습니다.");
        }
        
    }

    public void OnQuitButton()
    {
        Application.Quit();
        print("게임을 종료합니다.");
    }
}
