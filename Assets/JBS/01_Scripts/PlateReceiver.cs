using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlateReceiver : MonoBehaviourPun
{
    public static PlateReceiver instance;

    //무빙워크 머티리얼
    [SerializeField] Material matMovingWalk;
    //오프셋 변경 속도
    [SerializeField]float offsetSpeed;

    //시작 오프셋 값
    Vector2 startOffsetValue;
    //현재 오프셋 값
    [SerializeField]Vector2 curOffestValue;

    //서비스 벨 소리
    [SerializeField]AudioSource bellSound;
    //접시 제출 성공 소리
    [SerializeField]AudioSource receiveSuccessSound;
    //제출 거부 소리
    [SerializeField]AudioSource receiveRefuse;


    private void Awake() {
        instance = this;
        //시작 오프셋 값으로 변경
        startOffsetValue = new Vector2(0, -0.27f);
        curOffestValue = startOffsetValue;
        matMovingWalk.SetTextureOffset("_MainTex", startOffsetValue);
    }

    private void Update() {
        //-오프셋 스피드*시간 만큼 오프셋 값 변경
        curOffestValue += new Vector2(-Time.deltaTime, 0);
        matMovingWalk.SetTextureOffset("_MainTex", curOffestValue);
    }

    //플레이어가 나에게 접시를 주면 받을 수 있는지 검사
    public bool CanReceivePlate(GameObject plate)
    {
        //접시 인지 검사
        if(plate.CompareTag("Plate"))
        {
            //@@ 받을 수 있으니 놓아지는 연출 넣기 이후 아래 스크립트 실행하기
            //접시에 식재료가 들어있는지 검사
            PlateIngredient pi = plate.GetComponent<PlateIngredient>();
            if(pi.platedIngredientsIDList.Count > 0)
            {
                print($"접시 제출됨 : 요리 번호 {pi.foodIndex}");
                GetPlateInfo(plate, pi);
            }
            //빈 접시 제출됨
            else
            {
                print("빈 접시 제출됨");
                GetPlateInfo(plate);
            }
            return true;
        }
        //해당 안되면 false 리턴
        //접시 거부 사운드 재생
        PlaySound(receiveRefuse);
        SubmitUIManager.instance.PlateCoroutine();
        //fixme 병합 후에 rpc 실행으로 변경
        print("접시 아님");
        return false;
    }

    //빈 접시 제출됨
    void GetPlateInfo(GameObject plate)
    {
        //서비스 벨 사운드 재생
        PlaySound(bellSound);
        //반환테이블에 접시 생성 요청
        PlateReturnTable.instance.ReturnPlate();

        //받은 접시는 제거
        Destroy(plate);
        //if(PhotonNetwork.IsMasterClient)
        //    PhotonNetwork.Destroy(plate);
    }

    //받은 접시 정보 확인 후 접시 제거
    void GetPlateInfo(GameObject plate, PlateIngredient pi)
    {
        //서비스 벨 사운드 재생
        PlaySound(bellSound);
        //얻은 요리 번호를 OrderManager에게 넘김
        //넘겼을때 bool 값 받아서 성공하면 사운드 재생
        if(OrderManager.instance.CompareFoodAndOrder(pi.foodIndex))
        {
            PlaySound(receiveSuccessSound);
            
        }

        //반환테이블에 접시 생성 요청
        PlateReturnTable.instance.ReturnPlate();

        //받은 접시는 제거
        Destroy(plate);
        //if(PhotonNetwork.IsMasterClient)
        //    PhotonNetwork.Destroy(plate);
    }

    //해당 사운드 재생
    void PlaySound(AudioSource audioSource)
    {
        audioSource.Play();
    }
}
