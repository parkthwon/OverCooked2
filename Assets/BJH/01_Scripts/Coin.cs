using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// 제출대에 음식이 알맞게 제출되면
// 코인 +28
// 슬라이더 채워짐
// 목표 금액은 일단.. 280? 10개 제출하기로 하자
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

    // 인스턴스
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
        // UI Component 가져오기

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
        
        // 제출대에 제출이 확인되면 

        
    }
    public void FillBar()
    {
        RPCFillBar();
    }

    // 제출되면 바 채우기
    [PunRPC]
    public void RPCFillBar()
    {
        print("coin bar를 채웁니다.");
        slider.value += 0.1f;
    }

    // 제출되면 코인 올리기

    public void GetCoin()
    {
        //RPCGetCoin();
        photonView.RPC(nameof(RPCGetCoin), RpcTarget.AllBuffered);
    }
    
    [PunRPC]
    public void RPCGetCoin()
    {
        getTotalCoin += 28;
        print("coin을 얻었습니다.");
        text.text = getTotalCoin.ToString();

        totalCompleteOrder += 1;
    }

}
