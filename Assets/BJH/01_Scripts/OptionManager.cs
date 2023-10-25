using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine.TextCore.Text;

// ESC�� ������ �ɼ�â�� Ȱ��ȭ�ȴ�.(��, ������ ����Ǹ� ESC�� ���� �÷��̾ �ɼ�â�� Ȱ��ȭ�ȴ�.)
// ESC�� �ѹ� �� �����ų� ����ϱ⸦ ������ �ɼ�â�� ������.
// �ٽ� ������ ������ ������ �����ϰ� �ش� ���� �ٽ� �����Ѵ�.
// �׸��ϱ⸦ ������ ������ �����Ѵ�.

// �ѹ� �� ����ϴ�.

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
        // �ɼ�â, �ɼ� Ȯ��â ��Ȱ��ȭ
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
        // ESCŰ�� �����ٸ�
        // �ɼ� ���¸� �Ǻ��Ͽ�
        // �ɼ��� Ȱ��ȭ�ϰų� ��Ȱ��ȭ�Ѵ�.
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(os == OptionState.Close)
            {
                print("����ȯ : OpenOption();�� �����մϴ�.");
                OpenOption();
                
            }
            else if(os == OptionState.Open)
            {
            print("����ȯ : CloseOption()�� �����մϴ�.");
            CloseOption();
        }
            else if(os == OptionState.Open && ((rcs == RestartConState.Open) || (qcs == QuiteConState.Open)))
            {
                return;
            }
        }
    }
    
    // �ɼ�â ���� �޼���
    public void OpenOption()
    {
        print("����ȯ : OpenOption()");
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


    // Optionâ ��Ȱ��ȭ
    // ���� : Ȱ�� ���¿��� ESC�� ���� or ����ϱ� ��ư�� ����
    public void CloseOption()
    {
        print("����ȯ : CloseOption()");
        option.SetActive(false);
        os = OptionState.Close;
    }

    // �ٽý���
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
        // �ɼ�â ��Ȱ��ȭ
        option.SetActive(false);
        os = OptionState.Close;

        // ���� �ٽ� ����
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Debug.Log("�ٽ� ����");
    }

    public void Quit()
    {
        photonView.RPC(nameof(QuitRPC), RpcTarget.AllBuffered);
    }

    [PunRPC]
    void QuitRPC()
    {
        // �ɼ�â ��Ȱ��ȭ
        option.SetActive(false);
        os = OptionState.Close;

        // ���ø����̼� ����
        Application.Quit();
        Debug.Log("�׸��ϱ�");
    }

}
