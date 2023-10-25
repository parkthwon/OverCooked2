using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 재료 아이콘 UI 리스트를 만들고
// 재료가 0에서 1로 변경되면
// 알맞는 재료의 머리 위에 재료 아이콘 UI가 표시되게 만든다.

public class IngredientUI : MonoBehaviour
{
    // 재료 아이콘 UI 리스트
    public List<GameObject> ingredientIconList;

    public GameObject iconUI;

    public GameObject canvas;

    PlateIngredient plateIngredient;

    Ingredient ingredient;


    // 버튼 눌렸는지 아닌지
    bool isKeyBtnDown;

    private void Start()
    {
        if (gameObject.CompareTag("Plate"))
        {
            plateIngredient = GetComponent<PlateIngredient>();
        }
        else if (gameObject.CompareTag("Ingredient"))
        {
            ingredient = GetComponent<Ingredient>();
        }

        //iconUI.SetActive(false);

        // bool isKeyBtnDown = false;



    }
    // Update is called once per frame
    void Update()
    {

        // 현재 상태 인덱스

        // 재료에 올라갈 아이콘인지 접시에 올라갈 아이콘인지 판별
        if (gameObject.CompareTag("Ingredient"))
        {
            int curFormIdx = ingredient.CUR_FORM_INDEX;
            if (Input.GetKeyDown(KeyCode.Alpha9) || curFormIdx == 1)
            {
                //if (isKeyBtnDown == false)
                iconUI.SetActive(true);
                isKeyBtnDown = true;
            }
            else
            {
                iconUI.SetActive(false);
                isKeyBtnDown = false;
            }
        }

        else if (gameObject.CompareTag("Plate"))
        {
            // 접시에 놓여진 식재료 리스트를 가져와서
            // 리스트에 놓인 재료의 번호를 확인하고
            // 번호와 일치하는 재료를 Ingredient Ui List에서 가져와서
            // 접시 상단에 배치하라

            if(iconList.Count >= plateIngredient.platedIngredientsIDList.Count)
            {
                return;
            }

            // count가 0이 아니면 for문 시작
            if (plateIngredient.platedIngredientsIDList.Count != 0 && iconList.Count <= plateIngredient.platedIngredientsIDList.Count)
            {
                //@@ JBS 수정 아이콘 UI 제거 함수로 변경
                ClearIconUI();

                for (int i = 0; i < plateIngredient.platedIngredientsIDList.Count; i++)
                {
                    int id = plateIngredient.platedIngredientsIDList[i];
                    GameObject currentIdIconGameObject = Instantiate(ingredientIconList[id], canvas.transform);
                    currentIdIconGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0.5f, 0.5f);
                    iconList.Add(currentIdIconGameObject);
                }
            }
        }
    }

    //@@ JBS 수정 아이콘 ui와 아이콘 객체 전부 제거 함수
    public void ClearIconUI()
    {
        foreach(GameObject ingUI in iconList)
        {
            Destroy(ingUI);
        }
        iconList.Clear();
    }

    //아이콘 UI 리스트
    public List<GameObject> iconList;
}