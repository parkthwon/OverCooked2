using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlateReturnTable : MonoBehaviourPun
{
    public static PlateReturnTable instance;
    //XXX 플레이어 쪽에서 들기만 할 수 있도록 해둘것

    //접시 프리팹
    [SerializeField] GameObject plateF;
    //테이블 물건 놓기 스크립트
    PlaceableTable poot;
    //접시 생성 사운드
    AudioSource returnPlateSound;


    private void Awake() {
        instance = this;
        poot = GetComponent<PlaceableTable>();
        returnPlateSound = GetComponent<AudioSource>();
    }


    //접시 반환 함수
    public void ReturnPlate()
    {
        StartCoroutine(IECreatePlate());
        //접시 생성 코루틴을 실행한다.
    }

    //접시 생성 코루틴 
    IEnumerator IECreatePlate()
    {
        //10초 대기
        yield return new WaitForSeconds(10);
        //새 접시 생성
        //CreatePlate();
        if(PhotonNetwork.IsMasterClient)
            photonView.RPC(nameof(CreatePlate), RpcTarget.All);
    }

    //더러운 접시 생성 함수
    [PunRPC]
    public void CreatePlate()
    {
        GameObject dirtyPlate = Instantiate(plateF);
        //GameObject dirtyPlate = PhotonNetwork.Instantiate("Prefabs/Placeable/DirtyPlate", Vector3.zero, Quaternion.identity);
        //자신에게 이미 놓인 접시가 있으면 그 접시의 스택함수 실행
        if(poot.isPlaced)
        {
            poot.PLACED_OBJECT
                .GetComponent<DirtyPlate>().StackPlate(dirtyPlate);
        }
        //없으면 자신에게 놓기
        else
        {
            poot.PlaceThing(dirtyPlate);
        }
        //접시 반환 사운드 재생
        returnPlateSound.Play();
    }

    
}
