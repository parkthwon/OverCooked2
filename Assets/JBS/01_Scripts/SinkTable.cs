using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SinkTable : MonoBehaviourPun
{
    public static SinkTable instance;
    
    //접시 스택
    public int plateStack = 0;
    //세척 수치
    [Range(0,100)]
    [SerializeField]float washProcess = 0;
    public float WASH_PROCESS
    {
        get { return washProcess; }
        set
        {
            washProcess = value;
            fillPB(value);
        }
    }
    //접시 프리팹
    [SerializeField] GameObject plateF;
    //접시 생성 사운드
    AudioSource returnPlateSound;
    //테이블 놓기 스크립트
    PlaceableTable poot;
    //세척 접시 메시 객체
    [SerializeField] GameObject washingPlate;

    //진척도 바 채우는 함수
    Action<float> fillPB;

    private void Awake() {
        instance = this;
        poot = GetComponent<PlaceableTable>();
        returnPlateSound = GetComponent<AudioSource>();
        fillPB = GetComponent<ProgressBar>().FillProgressBar;
    }

    private void Update() {
        //세척 진행도가 100이 되면 0 으로하고 새 접시 생성
        UpdateCheckWashProcess();
        //세척 접시 활/비
        UpdateActivePlateMesh();
    }

    //세척 진행도가 100이 되면 0 으로하고 새 접시 생성
    void UpdateCheckWashProcess()
    {
        //세척할 접시가 있는 지 확인
        if(plateStack > 0)
        {
            //@@ 접시 중복 잡히는것 임시 봉책
            if(WASH_PROCESS >= 100 && !poot.isPlaced)
            {
                WASH_PROCESS = 0;
                //새 접시 생성
                CreateNewPlate();
            }
        }
    }

    //플레이어가 더러운 접시를 놓으면 받고 싶다
    public bool GetDirtyPlate(GameObject dPlate)
    {
        if(dPlate.CompareTag("DirtyPlate"))
        {
            //접시의 스택 수치를 자신에게 더하기
            plateStack += dPlate.GetComponent<DirtyPlate>().plateStack;
            //받은 접시 삭제
            Destroy(dPlate);
            //if(PhotonNetwork.IsMasterClient)
            //    PhotonNetwork.Destroy(dPlate);

            return true;
        }
        else 
            return false;
    }
    
    //접시 스택-- 하고 깨끗한 새접시 생성
    void CreateNewPlate()
    {
        //접시 스택 감소
        plateStack--;
        //GameObject plate = Instantiate(plateF);
        GameObject plate = PhotonNetwork.Instantiate("Prefabs/Placeable/NormalPlate", Vector3.zero, Quaternion.identity);
        //자신에게 이미 놓인 접시가 있으면 그 접시의 스택함수 실행
        if(poot.isPlaced)
        {
            poot.PLACED_OBJECT
                .GetComponent<PlateIngredient>().StackPlate(plate);
        }
        //없으면 자신에게 놓기
        else
        {
            poot.PlaceThing(plate);
        }
        //접시 반환 사운드 재생
        returnPlateSound.Play();
    }
    
    //접시 스택 >0 이면 닦일 접시 메시 활/비
    void UpdateActivePlateMesh()
    {
        washingPlate.SetActive(plateStack > 0);
    }
}
