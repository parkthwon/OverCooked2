using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderTimer : MonoBehaviour
{
    // Ÿ�̸� �ð�
    float maxTime;
    [SerializeField] float time;

    // �����̴�
    public Slider slider;

    // �����̴� ä���
    public GameObject fill;
    Image fillColor;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        // �ֹ��� n�� ����
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
            //Debug.Log("���� Ÿ�̸Ӵ� �ʷϻ��Դϴ�.");
        }
        else if(time <= ((maxTime)/3)*2 && time >= ((maxTime)/3)) 
        {
            fillColor.color = Color.yellow;
            //Debug.Log("���� Ÿ�̸Ӵ� ������Դϴ�.");
        }
        else if(time <= ((maxTime) / 3))
        {
            fillColor.color = Color.red;
            //Debug.Log("���� Ÿ�̸Ӵ� �������Դϴ�.");
        }

        if(time <= 1)
        {
            ResetTime();
        }
    }

    // Ÿ�̸� �ʱ�ȭ ��Ű��
    void ResetTime()
    {
        time = maxTime;
    }
}
