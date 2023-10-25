using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    // 2분 10초에 타이머 정지하기 위해서 OptionManager GameObject 가져오기
    public GameObject optionManager;

    // 타이머 텍스트 유아이
    public GameObject timeText;
    Text timer;

    // 슬라이더 유아이
    public GameObject fill;
    Image barColor;

    // 타이머 활성 상태
    bool isTimerStart;

    // 시간 표시
    public Text[] textTime;

    // 시간
    float maxTime;
    public float time;
    int min;
    int sec;

    // 슬라이더
    [SerializeField]Slider slTimer;

    OptionManager option;
    
    //@@ JBS 수정 인스턴스 만듬
    public static TimerManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        // 타이머 컴포넌트 가져오기
        timer = timeText.GetComponent<Text>();
        timer.text = "02 : 10";
        Debug.Log("시간을 2분 10초로 초기화 했습니다.");

        // 색깔 컴포넌트 가져오기
        barColor = fill.GetComponent<Image>();

        time = 130f;
        maxTime = time;
    }
    // Start is called before the first frame update
    void Start()
    {
        isTimerStart = true;

        option = optionManager.GetComponent<OptionManager>();

        
    } 

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(DelayTimer());
    }

    IEnumerator DelayTimer()
    {
        yield return new WaitForSeconds(4.5f);

        Timer();
    }

    // 타이머 메서드
    public void Timer()
    {
        if (isTimerStart == true)
        {
            // 시간이 흘러감
            time -= Time.deltaTime;
            //print(time);

            min = (int)(time / 60); // 분
            sec = ((int)(time % 60)); // 초

            timer.text = string.Format("{0:D2} : {1:D2}", min, sec);

            slTimer.value = time / maxTime;

            // change bar color ver2
            if (0.7 < time / 100)
            {
                barColor.color = new Color(0.5982f, 1, 0);
            }
            else if (0.4 < time / 100)
            {
                barColor.color = new Color(1f, 0.7592f, 0.023f);
            }
            else
            {
                barColor.color = Color.red;
            }
        }
    }

    // 타이머 유아이
    public GameObject TimerUI;

    // 옵션창 상태
    bool OptionState = false;

    // 옵션 창이 열리면 타이머 유아이 사라지기
    void StopTimer()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(OptionState == true)
            {
                OptionState = false;
                TimerUI.SetActive(true);
                // Time.timeScale = 1;
            }
            else if(OptionState == false)
            {
                OptionState = true;
                TimerUI.SetActive(false);
                // Time.timeScale = 0;
            }

        }

    }
}
    