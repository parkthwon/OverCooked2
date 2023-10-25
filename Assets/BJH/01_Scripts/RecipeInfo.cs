//using JetBrains.Annotations;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Photon.Pun;
//using Photon.Realtime;

//// 주문서 Prefab을 리스트에 담는다.
//public class RecipeInfo : MonoBehaviourPunCallbacks
//{

//    // 주문서 Prefab 담는 List
//    public List<GameObject> recipeList = new List<GameObject>();

//    // 현재 주문 목록을 담는 List
//    public List<GameObject> orderList = new List<GameObject>();

//    // 방금 들어온 주문 오브젝트 1개를 담는 변수
//    GameObject order;

//    // OrdersUI 담는 변수
//    public GameObject ordersUI;

//    // 랜덤 최대 범위
//    int maxRange = 5;

//    int numberOfRecipe;

//    int cnt = -1;

//    int maxOrderList = 5;

//    // Start is called before the first frame update
//    void Start()
//    {
//        // 클라이언트를 기준으로 주문서를 생성
//        if(PhotonNetwork.IsMasterClient == false)
//        {
//            return;
//        }
//        // 게임을 시작하면 두 개의 레시피 생성
//        photonView.RPC(nameof(CreateOrder), RpcTarget.All, ReturnRecipeNumber());
//        photonView.RPC(nameof(CreateOrder), RpcTarget.All, ReturnRecipeNumber());

//        // 일정 시간이 지나면 레시피 생성
//        // 레시피 리스트 최대 5개
//        photonView.RPC(nameof(IECreateOrder), RpcTarget.All);

//        // 기존 코드
//        // 게임을 시작하면 두 개의 레시피 생성
//        //CreateOrder(ReturnRecipeNumber());
//        //CreateOrder(ReturnRecipeNumber());

//        // 기존 코드
//        // coroutine 실행
//        //StartCoroutine(IECreateOrder());

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        // 테스트
//        // 주문서 생성
//        if(Input.GetKeyDown(KeyCode.O))
//        {
//            photonView.RPC(nameof(CreateOrder), RpcTarget.All, 1);

//            // 기존 코드
//            CreateOrder(1);
//        }

//    }

//    // 만약 일정 시간이 지나면
//    // 랜덤으로 레시피 번호를 return
//    int  ReturnRecipeNumber()
//    {
//        // 최종 랜덤 코드
//        // int a = Random.Range(0, maxRange);
//        int a = Random.Range(0, 3);

//        return a;
//    }

//    // coroutine
//    // 8초, 12초 주기로 주문서 생성
//    [PunRPC]
//    IEnumerator IECreateOrder()
//    {
//        if( orderList.Count > maxOrderList )
//        {
//            yield return null;
//        }

//        // 선택된 주문서 Id가 뭔지?
//        int orderId = ReturnRecipeNumber();
//        Debug.Log("선택된 주문서 ID : " + orderId);

//        // 몇 초 기다리게 할건지?
//        WaitForSeconds delay;

//        if (orderId >= 2)
//        {
//            delay = new WaitForSeconds(12f);
//            Debug.Log("주문서 생성 시간 : 12초");
//        }
//        else
//        {
//            delay = new WaitForSeconds(8f);
//            Debug.Log("주문서 생성 시간 : 8초");
//        }

//        // delay 적용
//        yield return delay;

//        CreateOrder(orderId);

//        // 재귀
//        StartCoroutine(IECreateOrder());
//    }


//    // 주문서 생성하는 메서드
//    // RPC를 사용하여 모든 PC에서 동작하도록 설정
//    [PunRPC]
//    public void CreateOrder(int orderId)
//    {
//        if(orderList.Count < 5)
//        {
//            // 주문서 리스트에서 주문을 선택하고
//            // GameObject order에 Instantiate로 대입
//            order = Instantiate(recipeList[orderId], ordersUI.transform);
//            Debug.Log(order + "주문서를 생성 했습니다.");
//            print(orderList.Count);

//            orderList.Add(order);

//            // 리스트에서 해당하는 gameobject의 index를 찾고
//            // 위치를 UI로 이동
//            int num = orderList.IndexOf(order);
//            orderList[num].transform.position = ordersUI.transform.position;
//        }

//        else
//        {
//            Debug.Log("최대 주문서 개수를 초과했습니다.");
//        }


//    }

//    // 주문서 리스트 위치 선정
//    void TransformPositonOrder(GameObject g)
//    {
//        // 입력 받은 값의 gameobject의 위치를
//        // orders UI 위치로 옮긴다.
//        // g.transform.position = OrderManager.instace.ordersUIPosition;
//    }

//    // 주문서 리스트의 주문 삭제
//    void DeleteOrder()
//    {
//        // 리스트를 순서대로 탐색하여
//        // 제출한 gameobject와 리스트의 gameobject 이름이 동일하면
//        // 해당 리스트를 삭제하고
//        // 리스트 빈공간을 재정렬


//    }

//}









using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// 주문서 Prefab을 리스트에 담는다.
public class RecipeInfo : MonoBehaviourPunCallbacks
{

    // 주문서 Prefab 담는 List
    public List<GameObject> recipeList = new List<GameObject>();

    // 현재 주문 목록을 담는 List
    public List<GameObject> orderList = new List<GameObject>();

    // 방금 들어온 주문 오브젝트 1개를 담는 변수
    GameObject order;

    // OrdersUI 담는 변수
    public GameObject ordersUI;

    // 랜덤 최대 범위
    int maxRange = 5;

    int numberOfRecipe;

    int cnt = -1;

    int maxOrderList = 5;

    // Start is called before the first frame update
    void Start()
    {
        // 클라이언트를 기준으로 주문서를 생성
        if (PhotonNetwork.IsMasterClient == true)
        {
            // 게임을 시작하면 두 개의 레시피 생성
            photonView.RPC(nameof(CreateOrder), RpcTarget.All, ReturnRecipeNumber());
            photonView.RPC(nameof(CreateOrder), RpcTarget.All, ReturnRecipeNumber());

            StartCoroutine(IECreateOrder());
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 만약 일정 시간이 지나면
    // 랜덤으로 레시피 번호를 return
    int ReturnRecipeNumber()
    {
        // 최종 랜덤 코드
        // int a = Random.Range(0, maxRange);
        int a = Random.Range(0, 3);

        return a;
    }

    // coroutine
    // 8초, 12초 주기로 주문서 생성
    IEnumerator IECreateOrder()
    {
        if (orderList.Count > maxOrderList)
        {
            yield return null;
        }

        // 선택된 주문서 Id가 뭔지?
        int orderId = ReturnRecipeNumber();
        Debug.Log("선택된 주문서 ID : " + orderId);

        // 몇 초 기다리게 할건지?
        WaitForSeconds delay;

        if (orderId >= 2)
        {
            delay = new WaitForSeconds(12f);
            Debug.Log("주문서 생성 시간 : 12초");
        }
        else
        {
            delay = new WaitForSeconds(8f);
            Debug.Log("주문서 생성 시간 : 8초");
        }

        // delay 적용
        yield return delay;

        photonView.RPC(nameof(CreateOrder), RpcTarget.All, ReturnRecipeNumber());

        // 재귀
        StartCoroutine(IECreateOrder());
    }


    // 주문서 생성하는 메서드
    // RPC를 사용하여 모든 PC에서 동작하도록 설정
    [PunRPC]
    public void CreateOrder(int orderId)
    {
        if (orderList.Count < 5)
        {
            // 주문서 리스트에서 주문을 선택하고
            // GameObject order에 Instantiate로 대입
            order = Instantiate(recipeList[orderId], ordersUI.transform);
            Debug.Log(order + "주문서를 생성 했습니다.");
            print(orderList.Count);

            orderList.Add(order);

            // 리스트에서 해당하는 gameobject의 index를 찾고
            // 위치를 UI로 이동
            int num = orderList.IndexOf(order);
            orderList[num].transform.position = ordersUI.transform.position;
        }

        else
        {
            Debug.Log("최대 주문서 개수를 초과했습니다.");
        }


    }

    // 주문서 리스트 위치 선정
    void TransformPositonOrder(GameObject g)
    {
        // 입력 받은 값의 gameobject의 위치를
        // orders UI 위치로 옮긴다.
        // g.transform.position = OrderManager.instace.ordersUIPosition;
    }

    // 주문서 리스트의 주문 삭제
    void DeleteOrder()
    {
        // 리스트를 순서대로 탐색하여
        // 제출한 gameobject와 리스트의 gameobject 이름이 동일하면
        // 해당 리스트를 삭제하고
        // 리스트 빈공간을 재정렬


    }

}

