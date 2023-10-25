using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// progress bar
// 재료 썰기가 시작되면
// 재료 위에 위치한다.

// 재료를 다지는 횟수만큼
// 재료 바를 올려준다.

// 재료가 다 썰리면
// 재료바가 사라진다.
public class ProgressBar : MonoBehaviourPun
{
    GameObject proBar;
    Slider slider;

    void Awake()
    {
        // object 가져오기
        proBar = GameObject.Find("ProgressBar_Group");
        slider = GameObject.Find("Progress_Bar").GetComponent<Slider>();

        // proBar 비활성화
        proBar.SetActive(false);
    }


    // Update is called once per frame
    //void Update()
    //{

    //}

    // 재료바 비활성화
    // 재료바 채우기
    //[PunRPC]
    public void FillProgressBar(float processValue)
    {
        if (processValue == 0)
        {
            // pro Bar 활성화
            proBar.SetActive(false);
        }
        else
        {
            proBar.SetActive(true);
        }

        // slider value = slice progress 값 / 100
        slider.value = processValue / 100;
        
    }
}
