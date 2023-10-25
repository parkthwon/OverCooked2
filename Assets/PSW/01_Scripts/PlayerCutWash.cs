using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCutWash : MonoBehaviourPun
{
    // PlayerPickDrop�� �ִ� ��ũ��Ʈ ��������
    PlayerPickDrop ppd;
    // sliceProcess �� float ȭ ��Ű��
    public bool cutting = false;
    // washProcess �� float ȭ ��Ű��
    public bool washing = false;
    // cut speed 
    public float cutSpeed = 5;
    // Knife
    public GameObject knife;
    //player body
    public Transform body;
    // cutParicle
    public GameObject particle;
   

    public bool CUTTING
    {
        get { return cutting; }
        set
        {
            cutting = value;
            anim.SetBool("Cut", value);
        }
    }

    public bool WASHING
    {
        get { return washing; }
        set
        {
            washing = value;
            anim.SetBool("Wash", value);
            // Audio Sound
            //GetComponent<PlayerSound>().PlayerAudio(washSound); // ����
            print("�ִϸ��̼ǵǴ�?");
        }
    }

    // Animator
    Animator anim;
    private void Awake()
    {
        ppd = GetComponent<PlayerPickDrop>();
    }

    private void Start()
    {
        anim = GetComponentInParent<Animator>();
    }
    void Update()
    {
        //������ �ƴҶ� �Լ��� ������
        if (photonView.IsMine == false) return;

        if (ppd.obj != null)
        {
            beCut();
            beWash();
            
            //photonView.RPC(nameof(beWash), RpcTarget.All);
        }
        else
        {
            cutting = false;
            washing = false;
        }
        //Cutting();
        //Washing();
        //photonView.RPC(nameof(Cutting), RpcTarget.All);
        //photonView.RPC(nameof(Washing), RpcTarget.All);
    }

    [PunRPC]
    void RpcShowKnife(bool isShow)
    {
        knife.gameObject.SetActive(isShow);
    }
    [PunRPC]
    void RpcSetCutting(bool isCut)
    {
        CUTTING = isCut;
    }
    [PunRPC]
    void RpcSLICE_PROCESS()
    {
        Ingredient ing = ppd.obj.GetComponent<PlaceableTable>().PLACED_OBJECT.GetComponent<Ingredient>();
        ing.SLICE_PROCESS = 100;
    }

    [PunRPC]
    void RpcShowWash(bool isWashShow)
    {
        WASHING = isWashShow;
    }

    [PunRPC]
    void RpcWASH_PROCESS()
    {
        SinkTable sinkT = ppd.obj.GetComponent<SinkTable>();
        sinkT.WASH_PROCESS = 100;
    }
    // �ڸ��� ����
    void beCut()
    {
        // �����Ǵ� ��ü�� �־��� ��, Cur From Index�� 0�� �ƴ� ��
        if (!ppd.obj.CompareTag("CuttingTable"))
        {
            if (CUTTING)
            {
                //CUTTING = false;
                photonView.RPC(nameof(RpcSetCutting), RpcTarget.All, false);
                print("111");
            }
        }
        else if(ppd.obj.CompareTag("CuttingTable") && ppd.obj.GetComponent<PlaceableTable>()
                .PLACED_OBJECT == null)
        {
            //.GetComponent<Ingredient>().CUR_FORM_INDEX != 0
            if (CUTTING)
            {
                //CUTTING = false;
                photonView.RPC(nameof(RpcSetCutting), RpcTarget.All, false);
                print("222");
            }
        }
        //@@ JBS ���� placedObject �� ��������� ���� �˻�
        else if(ppd.obj.CompareTag("CuttingTable") && ppd.obj.GetComponent<PlaceableTable>()
                .PLACED_OBJECT != null && !ppd.obj.GetComponent<PlaceableTable>()
                .PLACED_OBJECT.CompareTag("Ingredient"))
        {
            if (CUTTING)
            {
                //CUTTING = false;
                photonView.RPC(nameof(RpcSetCutting), RpcTarget.All, false);
                print("333");
            }
        }
        else if(ppd.obj.CompareTag("CuttingTable") && ppd.obj.GetComponent<PlaceableTable>()
                .PLACED_OBJECT != null && ppd.obj.GetComponent<PlaceableTable>()
                .PLACED_OBJECT.GetComponent<Ingredient>().CUR_FORM_INDEX != 0)
        {
            if (CUTTING)
            {
                //CUTTING = false;
                photonView.RPC(nameof(RpcSetCutting), RpcTarget.All, false);
                print("444");
            }
        }
        // ������Ʈ�� ���̺� ���� ��, �����Ǵ� ��ü�� CuttingTable�� ��, ���� ��Ʈ�� Ű�� ���� ��
        //@@ JBS ���� 1 �������̶� �߸� �۵��Ǹ� �ȵǹǷ� true�� ���� ���߿� �˻�
        else if (ppd.obj.CompareTag("CuttingTable") && ppd.obj.GetComponent<PlaceableTable>().PLACED_OBJECT != null && Input.GetButtonDown("Cut"))
        {
            print("�Ǵ�?");
            if(CUTTING == false)
            {
                //CUTTING = false;
                photonView.RPC(nameof(RpcSetCutting), RpcTarget.All, true);
                photonView.RPC(nameof(RpcShowKnife), RpcTarget.All, true);
            }
            //knife.gameObject.SetActive(true);
        }
    }

    [PunRPC]
    void beWash()
    {
        // �����Ǵ� ��ü�� ��ũ ���̺� �� ��
        if (ppd.obj.CompareTag("SinkTable") && ppd.obj.GetComponent<SinkTable>().plateStack > 0 && Input.GetButtonDown("Cut") && !ppd.pickOn
            && !ppd.obj.GetComponent<PlaceableTable>().isPlaced)
        {
            print("�׸� �������� �غ�");
            photonView.RPC(nameof(RpcShowWash), RpcTarget.All, true);
        }
    }

    // �ڸ��� ��
    public void Cutting()
    {
        if (CUTTING == true)
        {
            Ingredient ing = ppd.obj.GetComponent<PlaceableTable>().PLACED_OBJECT.GetComponent<Ingredient>();
            ing.SLICE_PROCESS += 20;
            if(ing.SLICE_PROCESS > 99)
            {
                photonView.RPC(nameof(RpcSLICE_PROCESS), RpcTarget.All);
                //ing.SLICE_PROCESS = 100;
                photonView.RPC(nameof(RpcShowKnife), RpcTarget.All, false);
                //knife.gameObject.SetActive(false); 
            }
        }
    }

    [PunRPC]
    public void Washing()
    {
        if (WASHING == true)
        {
            SinkTable sinkT = ppd.obj.GetComponent<SinkTable>();
            sinkT.WASH_PROCESS += 20;
            if (sinkT.WASH_PROCESS > 99)
            {
                print("�׸��� �İܶ�");
                photonView.RPC(nameof(RpcWASH_PROCESS), RpcTarget.All);
                //sinkT.WASH_PROCESS = 100;     
                photonView.RPC(nameof(RpcShowWash), RpcTarget.All, false);
                //WASHING = false;
            }
        }
    }

   // [PunRPC]
    public void OnCutting()
    {
        GameObject pa = Instantiate(particle);
        pa.transform.position = body.transform.position + body.transform.forward;
    }
}
