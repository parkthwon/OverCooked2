using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slidertimer : MonoBehaviour
{
    Slider slTimer;
    float f_sliderBarTime;

    // Start is called before the first frame update
    void Start()
    {
        slTimer = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (slTimer.value > 0.0f)
        {
            slTimer.value -= Time.deltaTime;
        }
        else if(slTimer.value == 0.0f)
        {
            Time.timeScale = 0f;
        }
    }
}
