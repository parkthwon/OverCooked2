using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ��� ������ UI ����Ʈ�� �����
// ��ᰡ 0���� 1�� ����Ǹ�
// �˸´� ����� �Ӹ� ���� ��� ������ UI�� ǥ�õǰ� �����.

public class IngredientUI : MonoBehaviour
{
    // ��� ������ UI ����Ʈ
    public List<GameObject> ingredientIconList;

    public GameObject iconUI;

    public GameObject canvas;

    PlateIngredient plateIngredient;

    Ingredient ingredient;


    // ��ư ���ȴ��� �ƴ���
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

        // ���� ���� �ε���

        // ��ῡ �ö� ���������� ���ÿ� �ö� ���������� �Ǻ�
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
            // ���ÿ� ������ ����� ����Ʈ�� �����ͼ�
            // ����Ʈ�� ���� ����� ��ȣ�� Ȯ���ϰ�
            // ��ȣ�� ��ġ�ϴ� ��Ḧ Ingredient Ui List���� �����ͼ�
            // ���� ��ܿ� ��ġ�϶�

            if(iconList.Count >= plateIngredient.platedIngredientsIDList.Count)
            {
                return;
            }

            // count�� 0�� �ƴϸ� for�� ����
            if (plateIngredient.platedIngredientsIDList.Count != 0 && iconList.Count <= plateIngredient.platedIngredientsIDList.Count)
            {
                //@@ JBS ���� ������ UI ���� �Լ��� ����
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

    //@@ JBS ���� ������ ui�� ������ ��ü ���� ���� �Լ�
    public void ClearIconUI()
    {
        foreach(GameObject ingUI in iconList)
        {
            Destroy(ingUI);
        }
        iconList.Clear();
    }

    //������ UI ����Ʈ
    public List<GameObject> iconList;
}