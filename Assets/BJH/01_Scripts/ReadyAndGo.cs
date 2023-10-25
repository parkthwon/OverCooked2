using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���Ӿ����� �̵��ϸ� Ready?, Go! UI����
// UI�� �ߵ��ϴ� ���� ������ �Ͻ�����
// Audio Source �߰�

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
        print("����ȯ : StartRadyAndGoUI �ڷ�ƾ�� �����մϴ�.");
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
