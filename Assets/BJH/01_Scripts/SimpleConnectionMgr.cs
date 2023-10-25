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
        // resource�� �ۼ��� ȯ�漳���� ������� ����
        PhotonNetwork.ConnectUsingSettings();
    }

    // master room�� ����
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(nameof(OnConnectedToMaster));

        // �κ� ����
        JoinLobby();
    }

    // �κ� �����ϴ� �Լ�
    // OnConnectedToMaster()���� ����
    void JoinLobby()
    {
        // �⺻ Lobby ����
        PhotonNetwork.JoinLobby();
    }

    // �κ� ���� �Ϸ�
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(nameof(OnJoinedLobby));

        // �� ����
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 2;

        // PhotonNetwork.CreateRoom("OverCooked2", roomOption, TypedLobby.Default);
        PhotonNetwork.JoinOrCreateRoom("OverCooked2", roomOption, TypedLobby.Default);
    }

    // �� ���� �Ϸ�
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print(nameof(OnCreatedRoom));
    }


    // �� ���� ����
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print(nameof(OnCreateRoomFailed));
    }

    // �� ����
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print(nameof(OnJoinedRoom));

        print("1�� ������ ��ȯ�մϴ�.");
        PhotonNetwork.LoadLevel(1);
    }
}
