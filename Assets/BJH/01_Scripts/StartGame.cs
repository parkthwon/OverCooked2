using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// �����̽��� ������
// Start UI�� ���� �ε巴�� �̵��Ѵ�.
public class StartGame : MonoBehaviour
{
    public GameObject openingUIGroup;
    RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        openingUIGroup = GameObject.FindWithTag("StartGameTag");
        rt = openingUIGroup.GetComponent<RectTransform>();

        openingUIGroup.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(MoveStartUIGroup());
        }
    }

    [SerializeField] float speed = 600f;
    [SerializeField] float currentTime = 3f;
    IEnumerator MoveStartUIGroup()
    {
        print("�ڷ�ƾ�� ���� �Ǿ����ϴ�.");
        for(float ctime = 0; ctime < 3; ctime += Time.deltaTime)
        {
            rt.anchoredPosition += Vector2.up * speed * Time.deltaTime;
            yield return null;
        }
    }
}
