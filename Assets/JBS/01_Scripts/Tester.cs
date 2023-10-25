using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public static Tester instance;
    
    
    //키보드로 상황 만드는 테스트기
    
    //테스트1
    [SerializeField] GameObject obj;
    //테스트2
    [SerializeField] GameObject storage;
    [SerializeField] GameObject plate;
    [SerializeField] GameObject receivePlate;
    //[SerializeField] GameObject player;
    //[SerializeField] GameObject playerPickObj;
    //주문 생성 스크립트
    [SerializeField] RecipeInfo ri;

    //PlayerPickDrop ppd;

    private void Awake() {
        instance = this;

        //player = GameObject.Find("Player");
        //ppd = player.GetComponent<PlayerPickDrop>();
    }
    
    
    
    private void Update() {
        //playerPickObj = ppd.pickObj;



        //if(Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    print("물건 떨어뜨리기");
        //    obj.GetComponent<ObjectPlace>().BePlace();
        //    //obj = null;
        //}
        //if(Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    print("물건 놓기");
        //    storage.GetComponent<ObjectPlace>().PlaceThing(obj);
        //    obj = null;
        //}
        //if(Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    print("물건 집기");
        //    obj = storage.GetComponent<ObjectPlace>().PickThing(transform, transform.position);
        //}
        //if(Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    print("물건 가져오기");
        //    obj = storage.GetComponent<IngredientStrorage>().GetIngredient(transform, transform.position);
        //}
        //if(Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    //주문 생성
        //    ri.CreateNewOrder(0);
        //}
        //if(Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    print("플레이어의 pick obj 식재료 손질하기");
        //    if(playerPickObj.CompareTag("Ingredient"))
        //    {
        //        playerPickObj.GetComponent<Ingredient>().SLICE_PROCESS = 100;
        //    }
        //    else
        //    {
        //        print("식재료 아님");
        //    }
        //}
        //if(Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    print("손질된 물건 접시에 놓기");
        //    plate.GetComponent<PlateIngredient>().PlaceIngredient(playerPickObj);
        //    player.GetComponent<PlayerPickDrop>().pickOn = false;
        //}
        //if(Input.GetKeyDown(KeyCode.Alpha7))
        //{
        //    print("두번째 접시에 첫번째 접시에 놓인것 교체하기");
        //    receivePlate.GetComponent<PlateIngredient>().PlaceIngredient(plate);
        //}
        //if(Input.GetKeyDown(KeyCode.Alpha8))
        //{
        //    print("접시 제출");
        //    PlateReceiver.instance.CanReceivePlate(plate);
        //}
        //if(Input.GetKeyDown(KeyCode.Alpha9))
        //{
        //    print("접시 스택 쌓기");
        //    plate.GetComponent<PlateIngredient>().plateStack++;
        //}
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            print("접시 생성");
            PlateReturnTable.instance.CreatePlate();
        }
        //if(Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    print("접시 고정");
        //    obj = plate.GetComponent<ObjectPlace>().PickThing(transform,transform.position);
            
        //}
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            print("시간 80초로 변경");
            TimerManager.instance.time = 80;
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            print("시간 1분 13초로 변경");
            TimerManager.instance.time = 73;
        }

        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            print("시간 15초로 변경");
            TimerManager.instance.time = 15;
        }
           
    }
    
}
