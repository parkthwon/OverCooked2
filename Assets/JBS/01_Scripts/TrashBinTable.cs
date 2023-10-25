using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TrashBinTable : MonoBehaviourPun
{
    Transform placePos;

    private void Awake() {
        placePos = transform.Find("PlacePos");
    }

    public void ThrowTrash(GameObject thing, GameObject player)
    {
        //해당 플레이어 집기/놓기 스크립트 가져오기
        PlayerPickDrop ppd = player.GetComponent<PlayerPickDrop>();
        //버려진 물건이 접시면 접시 내부 식재료 비우기
        if(thing.CompareTag("Plate"))
        {
            StartCoroutine(IEThrowPlate(thing, player));
        }
        //식재료 버려짐
        else
        {
            //테이블과 thing의 관계 끊기
            GetComponent<PlaceableTable>().PLACED_OBJECT = null;
            //물건의 놓기 확인 스크립트
            ObjectPlace ip = thing.GetComponent<ObjectPlace>();
            //물건을 고정함
            ip.PickThing(transform, placePos.position);
            //나한테 뭔갈 놓으면 회전을 0 으로하고 오른쪽으로 3초간 돌면서 떨어진다.
            thing.transform.rotation = Quaternion.Euler(0,0,0);
            StartCoroutine(IEThrowIngredient(thing));   
            //이후 삭제한다.
        }
    }

    //버려진 물건이 접시면 접시 내부 식재료 비우기
    IEnumerator IEThrowPlate(GameObject thing, GameObject player)
    {
        //한 프레임 쉬어야 정상 작동
        yield return null;
        //비우기
        thing.GetComponent<PlateIngredient>().ClearIng();
        //
        PlayerPickDrop ppd = player.GetComponent<PlayerPickDrop>();
        //그리고 접시를 플레이어에게 pick 시킨다
        ppd.obj = thing;
        ppd.Pick();
    }

    //버려진 식재료 회전하며 사라지기
    IEnumerator IEThrowIngredient(GameObject thing)
    {
        
        //프레임 당 회전값
        Vector3 rotSpeed = new Vector3(0,2,0);
        for(float i = 0; i < 1; i += Time.deltaTime)
        {
            thing.transform.Rotate(rotSpeed);
            thing.transform.localScale = Vector3.Lerp(thing.transform.localScale, Vector3.zero, Time.deltaTime);
            yield return null;    
        }
        //테이블과 thing의 관계 끊기
        GetComponent<PlaceableTable>().PLACED_OBJECT = null;
        Destroy(thing);
        //PhotonNetwork.Destroy(thing);
        
        yield return null;
    }

}
