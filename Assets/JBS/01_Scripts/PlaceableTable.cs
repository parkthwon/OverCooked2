using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableTable : ObjectPlace
{
    //물건 놓기
    //물건이 집혔을때 관계 끊기
    //1. 일정거리 내 놓을 수 있는 물건이 있고
    //2. 그 물건이 던져진 물건일때
    //3. 나에게 놓이게 만들기

    //물건 놓이는 위치
    Transform placePos;
    //놓인 물건 존재 여부
    public bool isPlaced = false;

    //놓인 물건
    [Tooltip("놓인 물건 확인용, 어사인 금지")]
    [SerializeField]GameObject placedObject;
    public GameObject PLACED_OBJECT
    {
        get{return placedObject;}
        set
        {
            placedObject = value;
            //isPlaced = value != null || value.activeSelf;
            isPlaced = value != null;
        }
        
    }

    private void Awake() {
        placePos = transform.Find("PlacePos");
    }

    private void Start() {
        SetOutlineMat();
    }

    //쓰레기통 물건 놓여지기
    public override bool PlaceThing(GameObject thing, GameObject player)
    {
        //놓인 물건이 없음 && 물건의 레이어가 "Placeable" && 물건이 놓일 수 있음
        if(!isPlaced && thing.layer.Equals(15)
            && !thing.CompareTag("DirtyPlate") && gameObject.CompareTag("TrashBinTable"))
        {
            //놓는 물건이 더러운 접시가 아니고 자신이 쓰레기통이면 쓰레기 버리기
            print("조건 맞음");
            GetComponent<TrashBinTable>().ThrowTrash(thing, player);

            return true;
        }
        else
        {
            return false;
        }

    }


    //물건 놓여지기
    public override bool PlaceThing(GameObject thing)
    {
        //놓인 물건이 없음 && 물건의 레이어가 "Placeable" && 물건이 놓일 수 있음
        if(!isPlaced && thing.layer.Equals(15))
        {
            print("조건 맞음");
            //놓는 물건이 더러운 접시고 자신이 싱크대면 스택 쌓기
            if(thing.CompareTag("DirtyPlate") && gameObject.CompareTag("SinkTable"))
            {
                GetComponent<SinkTable>().GetDirtyPlate(thing);

                return true;
            }
            else
            {
                //물건의 놓기 확인 스크립트
                ObjectPlace ip = thing.GetComponent<ObjectPlace>();
                //물건을 고정함
                ip.PickThing(transform, placePos.position);
                //놓인 물건 변경
                PLACED_OBJECT = thing;

                return true;
            }
        }
        else
        {
            return false;
        }
    }

    //물건을 집기
    public override GameObject PickThing(Transform self, Vector3 fixedPos)
    {
        //줄 물건이 있음
        if(isPlaced)
        {
            //물건을 요청된 트랜스폼에게 줌.
            PLACED_OBJECT.GetComponent<ObjectPlace>().PickThing(self, fixedPos);
            //반환할 물건
            GameObject pickedObject = PLACED_OBJECT;
            //놓은 물건 없어짐
            PLACED_OBJECT = null;
            print("물건을 집음");
            return pickedObject;
        }
        else
        {
            print("줄 물건이 없음");

            return null;

        }
    }
    

}
