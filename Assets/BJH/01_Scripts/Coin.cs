using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// ����뿡 ������ �˸°� ����Ǹ�
// ���� +28
// �����̴� ä����
// ��ǥ �ݾ��� �ϴ�.. 280? 10�� �����ϱ�� ����
public class Coin : MonoBehaviourPun
{
    // Coin Text
    public GameObject scoreText;
    Text text;
    public int getTotalCoin = 0;
    public int totalCompleteOrder = 0;

    // Coin Slider
    public GameObject sliderUI;
    Slider slider;

    // �ν��Ͻ�
    public static Coin coinInstance;

    private void Awake()
    {
        if(coinInstance == null)
        {
            coinInstance = this;
        }
    }


    void Start()
    {
        // UI Component ��������

        // text
        text = scoreText.GetComponent<Text>();
        text.text = getTotalCoin.ToString();
        
        // slider
        slider = sliderUI.GetComponent<Slider>();


    }

    // Update is called once per frame
    void Update()
    {
        // test
        if (Input.GetKeyDown(KeyCode.Z))
        {
            FillBar();
            GetCoin();
        }
        
        // ����뿡 ������ Ȯ�εǸ� 

        
    }
    public void FillBar()
    {
        RPCFillBar();
    }

    // ����Ǹ� �� ä���
    [PunRPC]
    public void RPCFillBar()
    {
        print("coin bar�� ä��ϴ�.");
        slider.value += 0.1f;
    }

    // ����Ǹ� ���� �ø���

    public void GetCoin()
    {
        //RPCGetCoin();
        photonView.RPC(nameof(RPCGetCoin), RpcTarget.AllBuffered);
    }
    
    [PunRPC]
    public void RPCGetCoin()
    {
        getTotalCoin += 28;
        print("coin�� ������ϴ�.");
        text.text = getTotalCoin.ToString();

        totalCompleteOrder += 1;
    }

}
