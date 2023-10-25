using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// ending scnen
// total score
public class Ending : MonoBehaviourPunCallbacks
{
    //@@ JBS 수정 인스턴스 생성
    public static Ending instance;
    
    private void Awake() {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ending UI
    // 이거 해결 방법 없나? 너무 오브젝트 많은데?
    GameObject endingUI;
    GameObject endingCnt;
    GameObject endingScore;
    GameObject total;
    bool endingUIState;
    Text totalCnt;
    Text score;
    Text totalScore;

    

    // complete star
    // 기본이 setactive false라서 assign으로 변경
    //GameObject star01;
    //GameObject star02;
    //GameObject star03;
    public GameObject star01;
    public GameObject star02;
    public GameObject star03;

    Coin coin;

    void Start()
    {
        endingUI = GameObject.Find("Ending_Group");
        endingCnt = GameObject.FindGameObjectsWithTag("Score")[0];
        endingScore = GameObject.FindGameObjectsWithTag("Score")[1];
        total = GameObject.FindGameObjectsWithTag("Score")[2];
        
        // coinSc = coinOb.GetComponent<Coin>();

        endingUIState = false;
        endingUI.SetActive(false);

        // complete star
        //star01 = GameObject.Find("Start01");
        //star02 = GameObject.Find("Start02");
        //star03 = GameObject.Find("Start03");

        coin = Coin.coinInstance;
    }

    // 점수에 따라 별 넣기
    public void FillStars()
    {
        StartCoroutine(DelayFillStar());
    }

    IEnumerator DelayFillStar()
    {
        // 실행되면 5분 딜레이
        if (coin.totalCompleteOrder <= 3)
        {
            print("최종 제출된 오더 : " + coin.totalCompleteOrder);
            yield return new WaitForSeconds(1f);
            star01.SetActive(true);
            print("별이 생성됐습니다.");
        }
        else if (coin.totalCompleteOrder <= 7)
        {
            print("최종 코인 : " + coin.totalCompleteOrder);
            yield return new WaitForSeconds(1f);
            star01.SetActive(true);
            yield return new WaitForSeconds(1f);
            star02.SetActive(true);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            star01.SetActive(true);
            yield return new WaitForSeconds(1f);
            star02.SetActive(true);
            yield return new WaitForSeconds(1f);
            star03.SetActive(true);
        }
        
    }

    public void OpenEndingUI()
    {
        ColScore();
        endingUI.SetActive(true);
        endingUIState = true;

        FillStars();
    }

    public void ColScore()
    {
        totalCnt = endingCnt.GetComponent<Text>();
        // totalCnt.text = Coin.coinInstance.totalCompleteOrder.ToString();
        // totalCnt.text = coinSc.totalCompleteOrder.ToString();
        totalCnt.text = Coin.coinInstance.totalCompleteOrder.ToString();
        print("전체 제출된 오더 : " + Coin.coinInstance.totalCompleteOrder);
        

        score = endingScore.GetComponent<Text>();
        // score.text = Coin.coinInstance.getTotalCoin.ToString();
        // score.text = coinSc.getTotalCoin.ToString();
        score.text = Coin.coinInstance.getTotalCoin.ToString();

        totalScore = total.GetComponent<Text>();
        // totalScore.text = Coin.coinInstance.getTotalCoin.ToString();
        // totalScore.text = coinSc.getTotalCoin.ToString();
        totalScore.text = Coin.coinInstance.getTotalCoin.ToString();
    }

    public void CloseEndingUI()
    {
        endingUI.SetActive(false);
        endingUIState = false;
    }

    void Update()
    {
        if (TimerManager.instance.time == 0 || Input.GetKeyDown(KeyCode.M))
        {
            if(endingUIState == false)
            {
                OpenEndingUI();
            }

            else
            {
                CloseEndingUI();
            }
            
        }
    }
}
