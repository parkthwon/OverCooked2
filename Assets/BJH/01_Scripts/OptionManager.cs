using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine.TextCore.Text;

// ESC를 누르면 옵션창이 활성화된다.(단, 게임은 진행되며 ESC를 누른 플레이어만 옵션창이 활성화된다.)
// ESC를 한번 더 누르거나 계속하기를 누르면 옵션창이 닫힌다.
// 다시 시작을 누르면 게임을 종료하고 해당 맵을 다시 시작한다.
// 그만하기를 누르면 게임을 종료한다.

// 한번 더 물어봅니다.

public enum OptionState 
{
    Open,
    Close
}

public enum RestartConState
{
    Open,
    Close
}

public enum QuiteConState
{
    Open,
    Close
}

public class OptionManager : MonoBehaviourPunCallbacks
{
    GameObject option;
    OptionState os;

    GameObject character01;
    GameObject character02;

    GameObject restartConfirm;
    GameObject quiteConfirm;

    RestartConState rcs;
    QuiteConState qcs;



    void Start()
    {
        // 옵션창, 옵션 확인창 비활성화
        option = GameObject.Find("Option-UI");
        option.SetActive(false);
        os = OptionState.Close;

        //character01 = GameObject.FindGameObjectsWithTag("Character")[0];
        //character02 = GameObject.FindGameObjectsWithTag("Character")[1];


        restartConfirm = GameObject.FindGameObjectsWithTag("ConTag")[0];
        quiteConfirm = GameObject.FindGameObjectsWithTag("ConTag")[1];

        restartConfirm.SetActive(false);
        quiteConfirm.SetActive(false);
    }

    void Update()
    {
        // ESC키를 누른다면
        // 옵션 상태를 판별하여
        // 옵션을 활성화하거나 비활성화한다.
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(os == OptionState.Close)
            {
                print("변지환 : OpenOption();을 실행합니다.");
                OpenOption();
                
            }
            else if(os == OptionState.Open)
            {
            print("변지환 : CloseOption()을 실행합니다.");
            CloseOption();
        }
            else if(os == OptionState.Open && ((rcs == RestartConState.Open) || (qcs == QuiteConState.Open)))
            {
                return;
            }
        }
    }
    
    // 옵션창 여는 메서드
    public void OpenOption()
    {
        print("변지환 : OpenOption()");
        option.SetActive(true);
        os = OptionState.Open;

        //if (PhotonNetwork.IsMasterClient == true)
        //{
        //    character02.SetActive(false);
        //}
        //else
        //{
        //    character01.SetActive(false);
        //}
    }


    // Option창 비활성화
    // 조건 : 활성 상태에서 ESC를 누름 or 계속하기 버튼을 누름
    public void CloseOption()
    {
        print("변지환 : CloseOption()");
        option.SetActive(false);
        os = OptionState.Close;
    }

    // 다시시작
    public void Restart()
    {
       restartConfirm.SetActive(true);
        rcs = RestartConState.Open;
    }

    public void RestartYesBtn()
    {
        photonView.RPC(nameof(RestartRPC), RpcTarget.AllBuffered);
        rcs = RestartConState.Close;
        restartConfirm.SetActive(false);
    }



    public void ConfirmNoBtn()
    {
        restartConfirm.SetActive(false);
        quiteConfirm.SetActive(false);
        rcs = RestartConState.Close;
        qcs = QuiteConState.Close;
    }

    public void Quite()
    {
        quiteConfirm.SetActive(true);
        qcs = QuiteConState.Open;
    }

    public void QuitYesBtn()
    {
        quiteConfirm.SetActive(false);
        photonView.RPC(nameof(QuitRPC), RpcTarget.AllBuffered);
        qcs = QuiteConState.Close;

    }


    [PunRPC]
    void RestartRPC()
    {
        // 옵셩창 비활성화
        option.SetActive(false);
        os = OptionState.Close;

        // 게임 다시 시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Debug.Log("다시 시작");
    }

    public void Quit()
    {
        photonView.RPC(nameof(QuitRPC), RpcTarget.AllBuffered);
    }

    [PunRPC]
    void QuitRPC()
    {
        // 옵셩창 비활성화
        option.SetActive(false);
        os = OptionState.Close;

        // 어플리케이션 종료
        Application.Quit();
        Debug.Log("그만하기");
    }

}
