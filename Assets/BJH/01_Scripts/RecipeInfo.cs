//using JetBrains.Annotations;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Photon.Pun;
//using Photon.Realtime;

//// �ֹ��� Prefab�� ����Ʈ�� ��´�.
//public class RecipeInfo : MonoBehaviourPunCallbacks
//{

//    // �ֹ��� Prefab ��� List
//    public List<GameObject> recipeList = new List<GameObject>();

//    // ���� �ֹ� ����� ��� List
//    public List<GameObject> orderList = new List<GameObject>();

//    // ��� ���� �ֹ� ������Ʈ 1���� ��� ����
//    GameObject order;

//    // OrdersUI ��� ����
//    public GameObject ordersUI;

//    // ���� �ִ� ����
//    int maxRange = 5;

//    int numberOfRecipe;

//    int cnt = -1;

//    int maxOrderList = 5;

//    // Start is called before the first frame update
//    void Start()
//    {
//        // Ŭ���̾�Ʈ�� �������� �ֹ����� ����
//        if(PhotonNetwork.IsMasterClient == false)
//        {
//            return;
//        }
//        // ������ �����ϸ� �� ���� ������ ����
//        photonView.RPC(nameof(CreateOrder), RpcTarget.All, ReturnRecipeNumber());
//        photonView.RPC(nameof(CreateOrder), RpcTarget.All, ReturnRecipeNumber());

//        // ���� �ð��� ������ ������ ����
//        // ������ ����Ʈ �ִ� 5��
//        photonView.RPC(nameof(IECreateOrder), RpcTarget.All);

//        // ���� �ڵ�
//        // ������ �����ϸ� �� ���� ������ ����
//        //CreateOrder(ReturnRecipeNumber());
//        //CreateOrder(ReturnRecipeNumber());

//        // ���� �ڵ�
//        // coroutine ����
//        //StartCoroutine(IECreateOrder());

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        // �׽�Ʈ
//        // �ֹ��� ����
//        if(Input.GetKeyDown(KeyCode.O))
//        {
//            photonView.RPC(nameof(CreateOrder), RpcTarget.All, 1);

//            // ���� �ڵ�
//            CreateOrder(1);
//        }

//    }

//    // ���� ���� �ð��� ������
//    // �������� ������ ��ȣ�� return
//    int  ReturnRecipeNumber()
//    {
//        // ���� ���� �ڵ�
//        // int a = Random.Range(0, maxRange);
//        int a = Random.Range(0, 3);

//        return a;
//    }

//    // coroutine
//    // 8��, 12�� �ֱ�� �ֹ��� ����
//    [PunRPC]
//    IEnumerator IECreateOrder()
//    {
//        if( orderList.Count > maxOrderList )
//        {
//            yield return null;
//        }

//        // ���õ� �ֹ��� Id�� ����?
//        int orderId = ReturnRecipeNumber();
//        Debug.Log("���õ� �ֹ��� ID : " + orderId);

//        // �� �� ��ٸ��� �Ұ���?
//        WaitForSeconds delay;

//        if (orderId >= 2)
//        {
//            delay = new WaitForSeconds(12f);
//            Debug.Log("�ֹ��� ���� �ð� : 12��");
//        }
//        else
//        {
//            delay = new WaitForSeconds(8f);
//            Debug.Log("�ֹ��� ���� �ð� : 8��");
//        }

//        // delay ����
//        yield return delay;

//        CreateOrder(orderId);

//        // ���
//        StartCoroutine(IECreateOrder());
//    }


//    // �ֹ��� �����ϴ� �޼���
//    // RPC�� ����Ͽ� ��� PC���� �����ϵ��� ����
//    [PunRPC]
//    public void CreateOrder(int orderId)
//    {
//        if(orderList.Count < 5)
//        {
//            // �ֹ��� ����Ʈ���� �ֹ��� �����ϰ�
//            // GameObject order�� Instantiate�� ����
//            order = Instantiate(recipeList[orderId], ordersUI.transform);
//            Debug.Log(order + "�ֹ����� ���� �߽��ϴ�.");
//            print(orderList.Count);

//            orderList.Add(order);

//            // ����Ʈ���� �ش��ϴ� gameobject�� index�� ã��
//            // ��ġ�� UI�� �̵�
//            int num = orderList.IndexOf(order);
//            orderList[num].transform.position = ordersUI.transform.position;
//        }

//        else
//        {
//            Debug.Log("�ִ� �ֹ��� ������ �ʰ��߽��ϴ�.");
//        }


//    }

//    // �ֹ��� ����Ʈ ��ġ ����
//    void TransformPositonOrder(GameObject g)
//    {
//        // �Է� ���� ���� gameobject�� ��ġ��
//        // orders UI ��ġ�� �ű��.
//        // g.transform.position = OrderManager.instace.ordersUIPosition;
//    }

//    // �ֹ��� ����Ʈ�� �ֹ� ����
//    void DeleteOrder()
//    {
//        // ����Ʈ�� ������� Ž���Ͽ�
//        // ������ gameobject�� ����Ʈ�� gameobject �̸��� �����ϸ�
//        // �ش� ����Ʈ�� �����ϰ�
//        // ����Ʈ ������� ������


//    }

//}









using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// �ֹ��� Prefab�� ����Ʈ�� ��´�.
public class RecipeInfo : MonoBehaviourPunCallbacks
{

    // �ֹ��� Prefab ��� List
    public List<GameObject> recipeList = new List<GameObject>();

    // ���� �ֹ� ����� ��� List
    public List<GameObject> orderList = new List<GameObject>();

    // ��� ���� �ֹ� ������Ʈ 1���� ��� ����
    GameObject order;

    // OrdersUI ��� ����
    public GameObject ordersUI;

    // ���� �ִ� ����
    int maxRange = 5;

    int numberOfRecipe;

    int cnt = -1;

    int maxOrderList = 5;

    // Start is called before the first frame update
    void Start()
    {
        // Ŭ���̾�Ʈ�� �������� �ֹ����� ����
        if (PhotonNetwork.IsMasterClient == true)
        {
            // ������ �����ϸ� �� ���� ������ ����
            photonView.RPC(nameof(CreateOrder), RpcTarget.All, ReturnRecipeNumber());
            photonView.RPC(nameof(CreateOrder), RpcTarget.All, ReturnRecipeNumber());

            StartCoroutine(IECreateOrder());
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ���� ���� �ð��� ������
    // �������� ������ ��ȣ�� return
    int ReturnRecipeNumber()
    {
        // ���� ���� �ڵ�
        // int a = Random.Range(0, maxRange);
        int a = Random.Range(0, 3);

        return a;
    }

    // coroutine
    // 8��, 12�� �ֱ�� �ֹ��� ����
    IEnumerator IECreateOrder()
    {
        if (orderList.Count > maxOrderList)
        {
            yield return null;
        }

        // ���õ� �ֹ��� Id�� ����?
        int orderId = ReturnRecipeNumber();
        Debug.Log("���õ� �ֹ��� ID : " + orderId);

        // �� �� ��ٸ��� �Ұ���?
        WaitForSeconds delay;

        if (orderId >= 2)
        {
            delay = new WaitForSeconds(12f);
            Debug.Log("�ֹ��� ���� �ð� : 12��");
        }
        else
        {
            delay = new WaitForSeconds(8f);
            Debug.Log("�ֹ��� ���� �ð� : 8��");
        }

        // delay ����
        yield return delay;

        photonView.RPC(nameof(CreateOrder), RpcTarget.All, ReturnRecipeNumber());

        // ���
        StartCoroutine(IECreateOrder());
    }


    // �ֹ��� �����ϴ� �޼���
    // RPC�� ����Ͽ� ��� PC���� �����ϵ��� ����
    [PunRPC]
    public void CreateOrder(int orderId)
    {
        if (orderList.Count < 5)
        {
            // �ֹ��� ����Ʈ���� �ֹ��� �����ϰ�
            // GameObject order�� Instantiate�� ����
            order = Instantiate(recipeList[orderId], ordersUI.transform);
            Debug.Log(order + "�ֹ����� ���� �߽��ϴ�.");
            print(orderList.Count);

            orderList.Add(order);

            // ����Ʈ���� �ش��ϴ� gameobject�� index�� ã��
            // ��ġ�� UI�� �̵�
            int num = orderList.IndexOf(order);
            orderList[num].transform.position = ordersUI.transform.position;
        }

        else
        {
            Debug.Log("�ִ� �ֹ��� ������ �ʰ��߽��ϴ�.");
        }


    }

    // �ֹ��� ����Ʈ ��ġ ����
    void TransformPositonOrder(GameObject g)
    {
        // �Է� ���� ���� gameobject�� ��ġ��
        // orders UI ��ġ�� �ű��.
        // g.transform.position = OrderManager.instace.ordersUIPosition;
    }

    // �ֹ��� ����Ʈ�� �ֹ� ����
    void DeleteOrder()
    {
        // ����Ʈ�� ������� Ž���Ͽ�
        // ������ gameobject�� ����Ʈ�� gameobject �̸��� �����ϸ�
        // �ش� ����Ʈ�� �����ϰ�
        // ����Ʈ ������� ������


    }

}

