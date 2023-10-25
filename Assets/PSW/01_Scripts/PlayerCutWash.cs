using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCutWash : MonoBehaviourPun
{
    // PlayerPickDrop에 있는 스크립트 가져오기
    PlayerPickDrop ppd;
    // sliceProcess 를 float 화 시키기
    public bool cutting = false;
    // washProcess 를 float 화 시키기
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
            //GetComponent<PlayerSound>().PlayerAudio(washSound); // 성공
            print("애니메이션되니?");
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
        //내것이 아닐때 함수를 나가자
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
    // 자르는 상태
    void beCut()
    {
        // 감지되는 물체와 멀어질 때, Cur From Index가 0이 아닐 때
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
        //@@ JBS 수정 placedObject 가 식재료인지 먼저 검사
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
        // 오브젝트가 테이블에 있을 때, 감지되는 물체가 CuttingTable일 때, 왼쪽 컨트롤 키를 누를 때
        //@@ JBS 수정 1 프레임이라도 잘못 작동되면 안되므로 true는 가장 나중에 검사
        else if (ppd.obj.CompareTag("CuttingTable") && ppd.obj.GetComponent<PlaceableTable>().PLACED_OBJECT != null && Input.GetButtonDown("Cut"))
        {
            print("되니?");
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
        // 감지되는 물체가 싱크 테이블 일 때
        if (ppd.obj.CompareTag("SinkTable") && ppd.obj.GetComponent<SinkTable>().plateStack > 0 && Input.GetButtonDown("Cut") && !ppd.pickOn
            && !ppd.obj.GetComponent<PlaceableTable>().isPlaced)
        {
            print("그릇 설거지할 준비");
            photonView.RPC(nameof(RpcShowWash), RpcTarget.All, true);
        }
    }

    // 자르는 중
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
                print("그릇아 씻겨라");
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
