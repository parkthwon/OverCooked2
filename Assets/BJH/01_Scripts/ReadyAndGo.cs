using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 게임씬으로 이동하면 Ready?, Go! UI생성
// UI가 발동하는 동안 게임은 일시정지
// Audio Source 추가

public class ReadyAndGo : MonoBehaviour
{
    // Ready
    GameObject ready;

    // Go
    GameObject go;

    // test
    public string test;

    void Start()
    {
        StartCo();
    }

    void Update()
    {

    }

    public void StartCo()
    {
        StartCoroutine(StartReadyAndGoUI());
    }

    IEnumerator StartReadyAndGoUI()
    {
        print("변지환 : StartRadyAndGoUI 코루틴을 실행합니다.");
        ready = GameObject.Find("Ready");
        go = GameObject.Find("Go");

        go.SetActive(false);

        yield return new WaitForSeconds(2.5f) ;

        ready.SetActive(false);

        go.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        go.SetActive(false);

        TimerManager.instance.Timer();
    }


}
