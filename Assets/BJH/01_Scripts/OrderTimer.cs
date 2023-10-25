using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderTimer : MonoBehaviour
{
    // 타이머 시간
    float maxTime;
    [SerializeField] float time;

    // 슬라이더
    public Slider slider;

    // 슬라이더 채우기
    public GameObject fill;
    Image fillColor;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        // 주문당 n초 제한
        maxTime = 80f;
        time = maxTime;

        fillColor = fill.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;

        slider.value = time / maxTime;

        if(time <= maxTime && time >= ((maxTime)/3)*2)
        {
            fillColor.color = Color.green;
            //Debug.Log("오더 타이머는 초록색입니다.");
        }
        else if(time <= ((maxTime)/3)*2 && time >= ((maxTime)/3)) 
        {
            fillColor.color = Color.yellow;
            //Debug.Log("오더 타이머는 노란색입니다.");
        }
        else if(time <= ((maxTime) / 3))
        {
            fillColor.color = Color.red;
            //Debug.Log("오더 타이머는 빨간색입니다.");
        }

        if(time <= 1)
        {
            ResetTime();
        }
    }

    // 타이머 초기화 시키기
    void ResetTime()
    {
        time = maxTime;
    }
}
