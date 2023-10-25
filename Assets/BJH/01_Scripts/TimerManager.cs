using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    // 2�� 10�ʿ� Ÿ�̸� �����ϱ� ���ؼ� OptionManager GameObject ��������
    public GameObject optionManager;

    // Ÿ�̸� �ؽ�Ʈ ������
    public GameObject timeText;
    Text timer;

    // �����̴� ������
    public GameObject fill;
    Image barColor;

    // Ÿ�̸� Ȱ�� ����
    bool isTimerStart;

    // �ð� ǥ��
    public Text[] textTime;

    // �ð�
    float maxTime;
    public float time;
    int min;
    int sec;

    // �����̴�
    [SerializeField]Slider slTimer;

    OptionManager option;
    
    //@@ JBS ���� �ν��Ͻ� ����
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
        // Ÿ�̸� ������Ʈ ��������
        timer = timeText.GetComponent<Text>();
        timer.text = "02 : 10";
        Debug.Log("�ð��� 2�� 10�ʷ� �ʱ�ȭ �߽��ϴ�.");

        // ���� ������Ʈ ��������
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

    // Ÿ�̸� �޼���
    public void Timer()
    {
        if (isTimerStart == true)
        {
            // �ð��� �귯��
            time -= Time.deltaTime;
            //print(time);

            min = (int)(time / 60); // ��
            sec = ((int)(time % 60)); // ��

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

    // Ÿ�̸� ������
    public GameObject TimerUI;

    // �ɼ�â ����
    bool OptionState = false;

    // �ɼ� â�� ������ Ÿ�̸� ������ �������
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
    