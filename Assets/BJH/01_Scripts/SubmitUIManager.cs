using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// 리턴대에 재료만 들어오면 에러
public class SubmitUIManager : MonoBehaviourPunCallbacks
{
    //@@ JBS 수정 인스턴스 추가
    public static SubmitUIManager instance;
    
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


    public GameObject plate;

    RectTransform plateRt;

    [SerializeField] float speed;

    void Start()
    {
        plateRt = plate.GetComponent<RectTransform>();

        plate.SetActive(false);
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) 
        {
            PlateCoroutine();
        }
    }

    // 재료가 접시에 담겨있지 않으면 
    // 해당 함수를 실행시켜주세요.
    // 썰린 상태, 썰리지 않은 상태 모두 동일합니다.
    [PunRPC]
    public void PlateCoroutine()
    {
        StartCoroutine(MissingPlate());
    }


    [SerializeField] float currentTime;
    IEnumerator MissingPlate()
    {
        
        for (float i = 0; i <  currentTime; i+= Time.deltaTime)
        { 
            plate.SetActive(true);
            plateRt.anchoredPosition += Vector2.up * speed * Time.deltaTime;
            yield return null;
        }
        plate.SetActive(false);
        plateRt.anchoredPosition = new Vector2(0, 0);

    }

}