using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// progress bar
// ��� ��Ⱑ ���۵Ǹ�
// ��� ���� ��ġ�Ѵ�.

// ��Ḧ ������ Ƚ����ŭ
// ��� �ٸ� �÷��ش�.

// ��ᰡ �� �丮��
// ���ٰ� �������.
public class ProgressBar : MonoBehaviourPun
{
    GameObject proBar;
    Slider slider;

    void Awake()
    {
        // object ��������
        proBar = GameObject.Find("ProgressBar_Group");
        slider = GameObject.Find("Progress_Bar").GetComponent<Slider>();

        // proBar ��Ȱ��ȭ
        proBar.SetActive(false);
    }


    // Update is called once per frame
    //void Update()
    //{

    //}

    // ���� ��Ȱ��ȭ
    // ���� ä���
    //[PunRPC]
    public void FillProgressBar(float processValue)
    {
        if (processValue == 0)
        {
            // pro Bar Ȱ��ȭ
            proBar.SetActive(false);
        }
        else
        {
            proBar.SetActive(true);
        }

        // slider value = slice progress �� / 100
        slider.value = processValue / 100;
        
    }
}
