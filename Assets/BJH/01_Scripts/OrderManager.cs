using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Unity.VisualScripting;

// RecipeInfo에서 주문서가 생성되면
// 해당 주문서를 Orders UI에 좌측정렬한다.
public class OrderManager : MonoBehaviourPunCallbacks
{
    // 메뉴 프리팹 리스트
    public List<GameObject> recipe = new List<GameObject>();

    // OrderUI GameObejct
    public GameObject orderUI;
    // public Vector3 ordersUIPosition;

    RecipeInfo recipeInfo;

    public GameObject sliderUI;
    Slider slider;

    Coin coin;



    // instance
    public static OrderManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        recipeInfo = GetComponent<RecipeInfo>();
        // coin = coinManager.GetComponent<Coin>();

        //text = scoreText.GetComponent<Text>();
        //slider = sliderUI.GetComponent<Slider>();
    }




    void Start()
    {
        coin = Coin.coinInstance;
    }

    void Update()
    {

    }


    // 제출된 요리와 주문서 일치여부를 확인하는 메서드
    public bool CompareFoodAndOrder(int foodIndex)
    {
        if (photonView.IsMine == true)
        {
            for (int i = 0; i < recipeInfo.orderList.Count; i++)
            {
                // print((int)recipeInfo.orderList[i].name[1] + "번 주문서를 삭제 하겠다");

                int a = recipeInfo.orderList[i].name[1] - '0';
                //print("변지환 : " + recipeInfo.orderList[i].name[1]);
                //print("변지환 : " + a);


                if (a == foodIndex)
                {
                    print("주문서와 요리가 일치합니다.");
                    // 주문서와 제출된 요리가 일치하면
                    // 주문서 삭제, 코인 증가, 코인바 채우기
                    print("a : " + a + " i : " + i);
                    StartCoroutine(ChangeColorDeletedOrder(a, i));
                    
                    
                    //photonView.RPC(nameof(coin.FillBar), RpcTarget.AllBuffered);
                    //photonView.RPC(nameof(coin.GetCoin), RpcTarget.AllBuffered);



                    return true;
                }

                

            }
            
        }

        return false;

    }

    IEnumerator ChangeColorDeletedOrder(int a, int i)
    {
        // 색을 변경하고
        print("색을 초록색으로 변경합니다.");

        //GameObject go = recipeInfo.orderList[a];
        GameObject go = recipeInfo.orderList[i];
        Image img = go.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        print(img.name);
        img.color = Color.green;

        // 몇 초 이따가
        print("1초를 대기합니다.");
        yield return new WaitForSeconds(0.6f);

        // 주문서 삭제
        print("주문서를 삭제합니다.");
        photonView.RPC(nameof(DeleteOrder), RpcTarget.AllBuffered, i);

        coin.FillBar();
        coin.GetCoin();

    }

    // 주문서 삭제
    // CompareFoodAndOrder()에서 일치하는 번호 Order를 삭제하는 메서드
    [PunRPC]
    void DeleteOrder(int n)
    {
        Destroy(recipeInfo.orderList[n]); 
        recipeInfo.orderList.RemoveAt(n);
        
        Debug.Log(n + "번 주문서가 제출되어 삭제하였습니다.");
    }


}
