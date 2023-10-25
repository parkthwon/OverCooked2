using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// ���ϴ뿡 ��Ḹ ������ ����
public class SubmitUIManager : MonoBehaviourPunCallbacks
{
    //@@ JBS ���� �ν��Ͻ� �߰�
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

    // ��ᰡ ���ÿ� ������� ������ 
    // �ش� �Լ��� ��������ּ���.
    // �丰 ����, �丮�� ���� ���� ��� �����մϴ�.
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