using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerPickDrop : MonoBehaviourPun
{
    // 잡을 물건
    public GameObject obj;
    // 잡은 물건
    public GameObject pickObj;
    // 물건을 잡을 위치
    public Transform pickPosition;
    // 감지 거리
    public float distacne = 2;
    // 회전 몸
    public Transform body;
    // pickOn = 주웠을 때
    public bool pickOn = false;
    // tablePick = 테이블에 물체를 주울 수 있을 때
    [SerializeField]bool tablePick = false;
    // Animator
    Animator anim;
    
    // Audio Source
    public AudioClip pickSound;
    // Audio Source
    public AudioClip dropSound;
    // Audio Source
    public AudioClip throwSound;

    // 감지되었을 때 색
    public Color detectedColor;

    // 던질 물체에 힘 가하는 것
    public float power = 8f;
    public bool PICK
    {
        get { return pickOn; }
        set
        {
            pickOn = value;
            anim.SetBool("Pick", value);
        }
    }
    public bool TABLEPICK
    {
        get { return tablePick; }
        set
        {
            tablePick = value;
            anim.SetBool("Pick", value);
        }
    }

    void Start()
    {
        body = transform.Find("Body");
        anim = GetComponentInParent<Animator>();
    }

    // 물건을 잡는다.
    [PunRPC]
    public void Pick()
    {
        // 잡을 수 있는 상태로 활성화
        PICK = true;
        // 잡은 물건이 obj
        // pickObj 를 잡을 수 있는 스크립트
        //fixme JBS 수정 Befixed가 객체를 반환함
        pickObj = obj.GetComponent<ObjectPlace>().PickThing(body, pickPosition.position);
        //pickObj.transform.SetParent(body);

        // Audio Sound
        GetComponent<PlayerSound>().PlayerAudio(pickSound); // 성공
    }

    [PunRPC]
    // 물건을 놓는다.
    public void Drop()
    {
        //pickObj =  식재료 && 손질됨 그리고 obj.tag = plate
        if (pickObj.CompareTag("Ingredient") && obj != null && obj.CompareTag ("Plate"))
        {
            //손질된 식재료 구하는법
            //pickObj의 스크립트 Ingredient를 가져와서, CUR_FORM_INDEX 변수가 1인지 확인
            //1이면 손질됨. 0이면 원재료
            if(pickObj.GetComponent<Ingredient>().CUR_FORM_INDEX == 1)
            {
                //obj.compareTag("Plate") == true 면 obj의 스크립트 PlateIngredient를 가져와서 함수 PlaceIngredient(식재료 게임오브젝트)를 실행시킨다.
                if (obj.CompareTag("Plate"))
                {
                    obj.GetComponent<PlateIngredient>().PlaceIngredient(gameObject);
                }
            }
        }
        //그런거 없을때
        else
        {
            PICK = false;
            pickObj.GetComponent<ObjectPlace>().PlaceThing();
            pickObj = null;
        }
    }
  
    // 테이블에 있는 물체를 주울 수 있다
    [PunRPC]
    void TablePick()
    {
        PICK = true;
        // 잡은 물건이 obj
        pickObj = obj.GetComponent<PlaceableTable>().PickThing(body.transform, pickPosition.position);
        // pickObj 를 잡을 수 있는 스크립트
        pickObj.GetComponent<ObjectPlace>().PickThing(transform, pickPosition.position);
        // 잡은 물체를 body 에 위치 시키자 
        pickObj.transform.SetParent(body);
        // Audio Sound
        GetComponent<PlayerSound>().PlayerAudio(pickSound); // 성공
    }

    [PunRPC]
    // 테이블에 물체를 놓을 수 있다.
    void TableDrop()
    {
        // 반환대에 식재료가 있는 그릇을 놓을 수 있다.
        if (obj.CompareTag("PlateReceiver"))
        {
            PICK = !(obj.GetComponent<PlateReceiver>().CanReceivePlate(pickObj)); 
            if (PICK == false)
            {
                pickObj = null;
            }
            return;
        }
        else if(obj.CompareTag("TrashBinTable"))
        {
            obj.GetComponent<PlaceableTable>().PlaceThing(pickObj, gameObject);
        }
        else if (obj.CompareTag("SinkTable") && !pickObj.CompareTag("DirtyPlate"))
        {
            return;
        }
        obj.GetComponent<PlaceableTable>().PlaceThing(pickObj);
        PICK = false;
        pickObj = null;
        // Audio Sound
        GetComponent<PlayerSound>().PlayerAudio(dropSound); // 성공
    }

    [PunRPC]
    // 그릇에 손질된 식재료를 놓을 수 있다.
    void DishFoodDrop()
    {
        obj.GetComponent<PlateIngredient>().PlaceIngredient(pickObj);
        // Audio Sound
        GetComponent<PlayerSound>().PlayerAudio(dropSound); // 성공
    }

    [PunRPC]
    // 그릇을 들고 있는 상태에서도 손질된 식재료를 그릇에 놓을 수 있다.
    void DishPickFoodDrop()
    {
        if(pickObj.GetComponent<PlateIngredient>().PlaceIngredient(obj.GetComponent<PlaceableTable>().PLACED_OBJECT))
        {
            obj.GetComponent<PlaceableTable>().PLACED_OBJECT = null;
            TableDrop();
        }
        // pickObj.GetComponent<PlateIngredient>().PlaceIngredient(obj.GetComponent<PlaceableTable>().PickThing(body.transform, pickPosition.position));
    }
    // 테이블 위에 있는 그릇에 손질된 식재료를 놓을 수 있다.
    [PunRPC]
    void TableFoodDrop()
    {
        obj.GetComponent<PlaceableTable>().PLACED_OBJECT.GetComponent<PlateIngredient>().PlaceIngredient(pickObj);
        // Audio Sound
        GetComponent<PlayerSound>().PlayerAudio(dropSound); // 성공
    }
    // 채소 상자 위에 놓여져 있는 물건도 없고
    // 플레이어가 들고 있는 물건이 없으면 채소를 꺼낼 수 있다.
    
    void ISTablePick()
    {
        pickObj = obj.GetComponent<IngredientStrorage>().GetIngredient(photonView, pickPosition.position);
        // Audio Sound
        photonView.RPC("PlayPickSound", RpcTarget.All);
        //GetComponent<PlayerSound>().PlayerAudio(pickSound); // 성공
    }

    [PunRPC]
    void PlayPickSound()
    {
        GetComponent<PlayerSound>().PlayerAudio(pickSound); // 성공
    }

    [PunRPC]
    void Throw(Vector3 pos, Vector3 forward)
    {
        pickObj.transform.position = pos;

        Rigidbody objRigidbody = pickObj.GetComponent<Rigidbody>();
        Drop();
        objRigidbody.AddForce(forward * power, ForceMode.Impulse);
        anim.SetTrigger("Throw");
        // Audio Sound
        GetComponent<PlayerSound>().PlayerAudio(throwSound); // 성공
    }
    void Update()
    {
        OnSense();

        //손에 든 물체가 있는지 확인
        //있다
        if (pickObj != null)
        {
            PICK = true;
        }
        //없다
        else
        {
            PICK = false;
        }
        //내것이 아닐때 함수를 나가자
        if (photonView.IsMine == false) return;

        // layer == 15 : 식재료나 그릇인 물체를 들을 수 있다
        if (Input.GetButtonDown("Pick") && !pickOn && obj.layer == 15)
        {
            //Pick();
            photonView.RPC(nameof(Pick), RpcTarget.All);
            print("미안해111111");
        }
        // layer == 15 : 식재료나 그릇인 물체를 놓을 수 있다
        else if (Input.GetButtonDown("Pick") && pickOn && obj == null)
        {
            //Drop();
            photonView.RPC(nameof(Drop), RpcTarget.All);
            print("미안해222222");
        }
        // layer == 12 : 테이블 위에 물체를 들을 수 있다
        else if (Input.GetButtonDown("Pick") && tablePick && !pickOn && obj.layer == 12)
        {
            print("미안해3333333!!");
            //TablePick();
            photonView.RPC(nameof(TablePick), RpcTarget.All);
        }
        // layer == 12 : 테이블 위에 물체를 놓을 수 있다 & 새 그릇으로 생성이 되는 곳에 그릇은 주울 수 없다.
        else if (Input.GetButtonDown("Pick") && !tablePick && pickOn && obj.layer == 12 && !obj.CompareTag("PlateReturner"))
        {
            print("미안해4444444!");
            //TableDrop();
            photonView.RPC(nameof(TableDrop), RpcTarget.All);
        }
        else if (Input.GetButtonDown("Pick") && pickOn && obj.CompareTag("Plate"))
        {
            //DishFoodDrop();
            photonView.RPC(nameof(DishFoodDrop), RpcTarget.All);
        }
        // 테이블에 접시가 있고 나에게 든 물건이 있을 때
        else if (Input.GetButtonDown("Pick") && obj.layer == 12 && obj.GetComponent<PlaceableTable>().isPlaced && pickOn)
        {
            //놓인 물건이 접시인지 확인
            if(obj.GetComponent<PlaceableTable>().PLACED_OBJECT.CompareTag("Plate"))
            {
                //TableFoodDrop();
                photonView.RPC(nameof(TableFoodDrop), RpcTarget.All);
            }
            // 빈 그릇을 들고 있는 상태에서 손질된 식재료 앞에 가면 그릇에 식재료가 담겨진다.
            else if (obj.GetComponent<PlaceableTable>().PLACED_OBJECT.CompareTag("Ingredient"))
            {
                print("밍라미아루미;아루메재다루;ㅁㅇ");
                // DishPickFoodDrop();
                photonView.RPC(nameof(DishPickFoodDrop), RpcTarget.All);
            }
        }
        else if (Input.GetButtonDown("Pick") && !pickObj && !obj.GetComponent<PlaceableTable>().isPlaced)
        {
            //@@ JBS 추가 해당 객체 태그가 식재료 창고 인지 확인
            if(obj.CompareTag("IngredientStorageTable"))
            {
                ISTablePick();
                //photonView.RPC(nameof(ISTablePick), RpcTarget.All);
            }
        }
        else if (Input.GetButtonDown("Cut") && pickOn)
        {
            print("밀려지렴");
            //Throw();
            photonView.RPC(nameof(Throw), RpcTarget.All, pickObj.transform.position, body.forward);
        }
    }

    // 물건 감지
    void OnSense()
    {
        SetObjHighlight(prevObj, false);
        // 레이어 마크가 Placeable 이거나 Table 일 때
        int layerMask = 1 << LayerMask.NameToLayer("Placeable") | 1 << LayerMask.NameToLayer("Table");
        //int layerMask = LayerMask.GetMask("Placeable", "Table");
        // 물건 인식한 걸 배열로 정리
        Collider[] cols = Physics.OverlapSphere(body.position, distacne, layerMask);

        if (cols.Length > 0)
        {
            //제일 가까운 object 담을 변수
            Collider nearest = cols[0];
            for (int i = 1; i < cols.Length; i++)
            {
                //nearest 와 body.position 의 거리를 담자
                float dist1 = Vector3.Distance(nearest.transform.position, body.transform.position);

                //cols[i] 와 body.position 의 거리를 담자
                float dist2 = Vector3.Distance(cols[i].transform.position, body.transform.position);

                //dist1 이 dist2 보다 크다면
                if (dist1 > dist2)
                {
                    //nearest 에 cols[i] 를 담자
                    nearest = cols[i];
                }

                //print(i + " --- " + cols[i].gameObject.name + " : " + Vector3.Distance(body.position, cols[i].transform.position));
            }

            //식재료 일땐 부모의부모
            if (nearest.gameObject.layer == 15)
            {
                obj = nearest.transform.parent.parent.gameObject;
            }
            //테이블 일땐 부모
            else if (nearest.gameObject.layer == 12)
            {
                obj = nearest.transform.parent.gameObject;

                //테이블에 물건이 있을때  tablepick true
                if (!obj.CompareTag("PlateReceiver")
                    && obj.GetComponent<PlaceableTable>().isPlaced)
                {
                    //print("ssssssssssssssssss");
                    tablePick = true;
                }
                else
                {
                    tablePick = false;
                }
            }
            //print($"감지된 물체 : {obj}");
            SetObjHighlight(obj, true);
            prevObj = obj;
        }
        else
        {
            obj = null;
        }
    }

    ///<summary>
    ///XXX JBS 추가 해당 객체 아웃라인 활/비
    ///</summary>
    void SetObjHighlight(GameObject gameObject, bool isHL)
    {
        if(photonView.IsMine)
        {
            if(gameObject != null && gameObject.GetComponent<ObjectPlace>() != null)
            {
                gameObject.GetComponent<ObjectPlace>().isHighlight = isHL;
            }
        }
    }
    //직전 obj
    GameObject prevObj;

    // 물건 감지 범위 확인
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(body != null)
        {
            Gizmos.DrawWireSphere(body.position, distacne); 
        }
    }
}
