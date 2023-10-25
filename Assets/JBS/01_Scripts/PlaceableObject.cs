using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlaceableObject : ObjectPlace, IPunObservable
{
    //놓을 수 있는 상태 일때 해당 객체의 재료 놓기 함수를 실행 시키고 싶다.
    //놓을 수 있는 상태
    [Tooltip("테이블이나 바닥에 놓을 수 있는 상태 여부")]
    public bool canPlace = false;
    //오브젝트 번호
    public int objIndex;


    //리지드 바디
    Rigidbody rb;

    //XXX 접시용 스크립트
    PlateIngredient pi;
    //XXX 식재료용 스크립트
    Ingredient ing;

    //방장기준 위치
    [SerializeField]Vector3 receivePos;
    //방장기준 회전
    [SerializeField]Quaternion receiveRot;

    private void Awake() {
        //초기값 설정
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        canPlace = false;

        //접시면 추가 스크립트 가져오기
        if(gameObject.CompareTag("Plate"))
        {
            pi = GetComponent<PlateIngredient>();
        }
        else if(gameObject.CompareTag("Ingredient"))
        {
            ing = GetComponent<Ingredient>();
        }
    }

    private void Start() {
        SetOutlineMat();
    }

    private void Update() {
        //@@ 시리얼에서 집적 하는중
        ////방장이 아니면 위치 동기화
        //if(!PhotonNetwork.IsMasterClient)
        //{
        //    transform.position = receivePos;
        //    transform.rotation = receiveRot;
        //}
    }

    [PunRPC]
     void RpcPickTing(int parentViewId, Vector3 fixedPos)
    {
        //parentViewId 를 가지고있는 Player 찾자
        Transform player = PlayerManager.instance.allPlayer[parentViewId].transform;
        GameObject go = PickThing(player.GetChild(0), fixedPos);

        PlayerPickDrop pickDrop = player.GetComponent<PlayerPickDrop>();
        pickDrop.pickObj = go;
    }


    //집어지거나 놓여짐
    public override GameObject PickThing(Transform parent, Vector3 fixedPos)
    {
        base.PickThing(parent, fixedPos);
        //접시 스택이 있으면 들릴때 자신 대신 접시를 만들어서 들리게 하고 싶다.
        if(gameObject.CompareTag("Plate") && pi.plateStack > 1)
        {
            print("접시 스택 들려줌");
            //스택 감소
            pi.plateStack--;
            //fixme
            if(PhotonNetwork.IsMasterClient)
            {
                //새 접시 생성
                //GameObject newPlate = Instantiate(pi.plateF);
                GameObject newPlate = PhotonNetwork.Instantiate(
                    "Prefabs/Placeable/NormalPlate",Vector3.zero,Quaternion.identity
                );
                //새 접시를 해당 객체에게 들려줌
                newPlate.GetComponent<ObjectPlace>().PickThing(parent, fixedPos);

            }
            //아래 함수는 취소
            return null;
        }
        else
        {
            print("고정됨");
            //부모가 parent로 된다.
            transform.parent = parent;
            //리지드 바디 키네메틱 킴
            rb.isKinematic = true;
            //놓을 수 없다.
            canPlace = false;
            //위치가 fixedPos가 된다
            transform.SetPositionAndRotation(fixedPos, Quaternion.Euler(transform.rotation.x,0,transform.rotation.z));
            transform.GetChild(0).GetChild(0).gameObject.layer = 16;

            if(gameObject.CompareTag("Ingredient"))
            {
                switch(ing.CUR_FORM_INDEX)
                {
                    case 0:
                        transform.GetChild(0).GetChild(0).gameObject.layer = 16;
                        break;
                    case 1:
                        transform.GetChild(1).GetChild(0).gameObject.layer = 16;
                        break;
                }
            }

            return gameObject;

        }

    }

    

    //놓아지거나, 던져짐 져서 테이블에 놓거나 바닥에 굴러다닐것
    public override bool PlaceThing(GameObject self = null)
    {
        //부모가 LayedIngredients 가 된다.
        transform.parent = GameObject.FindWithTag("IngredientParent").transform;
        //리지드 바디 키네메틱 끔
        rb.isKinematic = false;
        //놓을 수 있는 상태가 된다.
        canPlace = true;
        //콜라이더가 붙은 객체의 레이어를 pleacable로 변경
        Transform meshTr = gameObject.CompareTag("Ingredient") 
            ? transform.GetChild(ing.CUR_FORM_INDEX) : transform.GetChild(0);
        meshTr.GetChild(0).gameObject.layer = 15;

        return canPlace;
    }


    //위치 마스터 플레이어 기준으로 동기화
    
    //위치 보내기
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //방장이면 위치값 보내기
        if (stream.IsWriting)
        {
            //XXX 두 명일때만 작동됨
            //print("asdda");
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        //다른 플레이어는 위치, 회전값 받기
        else
        {
            print("너 주인아니잖아");
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
