using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SimpleConnectionMgr : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        // resource에 작성한 환경설정을 기반으로 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    // master room에 연결
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(nameof(OnConnectedToMaster));

        // 로비 진입
        JoinLobby();
    }

    // 로비 진입하는 함수
    // OnConnectedToMaster()에서 실행
    void JoinLobby()
    {
        // 기본 Lobby 입장
        PhotonNetwork.JoinLobby();
    }

    // 로비 진입 완료
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(nameof(OnJoinedLobby));

        // 방 생성
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 2;

        // PhotonNetwork.CreateRoom("OverCooked2", roomOption, TypedLobby.Default);
        PhotonNetwork.JoinOrCreateRoom("OverCooked2", roomOption, TypedLobby.Default);
    }

    // 방 생성 완료
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print(nameof(OnCreatedRoom));
    }


    // 방 생성 실패
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print(nameof(OnCreateRoomFailed));
    }

    // 방 참여
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print(nameof(OnJoinedRoom));

        print("1번 씬으로 전환합니다.");
        PhotonNetwork.LoadLevel(1);
    }
}
