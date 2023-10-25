using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// �κ� ȭ��

// 1. �÷��̾ ť�꿡�� ���� �÷��̾� ���������� ����

// 2. ��ư Ȱ��ȭ
//  2.1. �����͸� ���� ��ư Ȱ��ȭ
//  2.2. �÷��̾�� ���� ��ư ��Ȱ��ȭ

// 3. �÷��̾� ����
//  3.1. �÷��̾ �����ϸ� �̹��� ����

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // ���� ��ư
    public GameObject startBtn;
    Image startBtnImg;

    // ���� ������
    public GameObject quitBtn;


    // Start is called before the first frame update
    void Start()
    {
        startBtn = GameObject.FindGameObjectsWithTag("RoomOption")[0];
        quitBtn = GameObject.FindGameObjectsWithTag("RoomOption")[1];

        startBtnImg = startBtn.GetComponent<Image>();

        // ������ �ƴϸ� ���� ���� ��ư ��Ȱ��ȭ
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
            print("���� �����մϴ�.");
            // game scene���� �̵�
            PhotonNetwork.LoadLevel(2);
        }
        else
        {
            print("�����͸� ���ӽ����� ���� �� �ֽ��ϴ�.");
        }
        
    }

    public void OnQuitButton()
    {
        Application.Quit();
        print("������ �����մϴ�.");
    }
}
