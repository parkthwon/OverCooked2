using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Unity.VisualScripting;

// RecipeInfo���� �ֹ����� �����Ǹ�
// �ش� �ֹ����� Orders UI�� ���������Ѵ�.
public class OrderManager : MonoBehaviourPunCallbacks
{
    // �޴� ������ ����Ʈ
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


    // ����� �丮�� �ֹ��� ��ġ���θ� Ȯ���ϴ� �޼���
    public bool CompareFoodAndOrder(int foodIndex)
    {
        if (photonView.IsMine == true)
        {
            for (int i = 0; i < recipeInfo.orderList.Count; i++)
            {
                // print((int)recipeInfo.orderList[i].name[1] + "�� �ֹ����� ���� �ϰڴ�");

                int a = recipeInfo.orderList[i].name[1] - '0';
                //print("����ȯ : " + recipeInfo.orderList[i].name[1]);
                //print("����ȯ : " + a);


                if (a == foodIndex)
                {
                    print("�ֹ����� �丮�� ��ġ�մϴ�.");
                    // �ֹ����� ����� �丮�� ��ġ�ϸ�
                    // �ֹ��� ����, ���� ����, ���ι� ä���
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
        // ���� �����ϰ�
        print("���� �ʷϻ����� �����մϴ�.");

        //GameObject go = recipeInfo.orderList[a];
        GameObject go = recipeInfo.orderList[i];
        Image img = go.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        print(img.name);
        img.color = Color.green;

        // �� �� �̵���
        print("1�ʸ� ����մϴ�.");
        yield return new WaitForSeconds(0.6f);

        // �ֹ��� ����
        print("�ֹ����� �����մϴ�.");
        photonView.RPC(nameof(DeleteOrder), RpcTarget.AllBuffered, i);

        coin.FillBar();
        coin.GetCoin();

    }

    // �ֹ��� ����
    // CompareFoodAndOrder()���� ��ġ�ϴ� ��ȣ Order�� �����ϴ� �޼���
    [PunRPC]
    void DeleteOrder(int n)
    {
        Destroy(recipeInfo.orderList[n]); 
        recipeInfo.orderList.RemoveAt(n);
        
        Debug.Log(n + "�� �ֹ����� ����Ǿ� �����Ͽ����ϴ�.");
    }


}
