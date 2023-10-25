using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ingredient : MonoBehaviour
{
    //손질 퍼센트
    [Tooltip("0% 시작 100%시 손질된 식재료로 변경")][Range(0,100)]
    [SerializeField]float sliceProcess = 0;
    public float SLICE_PROCESS
    {
        get{return sliceProcess;}
        set
        {
            sliceProcess = value;
            fillPB(value);
        }
    }

    //진척도 UI
    ProgressBar pb;
    

    //식재료 형태 리스트
    [SerializeField] List<GameObject> ingredientFormList;
    //식재료 현재 형태
    [SerializeField] GameObject curForm;
    //식재료 현재 형태 인덱스
    [SerializeField] int curFormIndex;
    public int CUR_FORM_INDEX
    {
        get{return curFormIndex;}
        set
        {
            //변경된 객체 활성화
            ingredientFormList[curFormIndex].SetActive(false);
            ingredientFormList[value].SetActive(true);
            //인덱스 변경시 현재 형태 캐시도 변경
            curFormIndex = value;
            curForm = ingredientFormList[value];
            //형태 변경시 도마 위에 고정되야 하므로 레이어 변경
            ingredientFormList[value].transform.GetChild(0).gameObject.layer = 16;
            op.objRenderer = ingredientFormList[value].transform.GetChild(0).GetComponent<Renderer>();
        }
    }

    //상호작용 스크립트
    PlaceableObject op;
    //진척도 바 채우는 함수
    Action<float> fillPB;


    void Awake() {
        fillPB = GetComponent<ProgressBar>().FillProgressBar;
        op = GetComponent<PlaceableObject>();
    }

    private void Start() {
        //XXX 스크립트 로딩후 함수를 불러야 null 오류 미발생
        SLICE_PROCESS = 0;
        CUR_FORM_INDEX = 0;
    }


    private void Update() {
        UpdateCheckSliceProcess();
    }

    //슬라이스 진행도가 100이되면 0으로 하고 손질된 형태로 변경
    void UpdateCheckSliceProcess()
    {
        if(SLICE_PROCESS >= 100)
        {
            SLICE_PROCESS = 0;
            ChangeForm(1);
        }
    }

    //형태 변경
    void ChangeForm(int index)
    {
        print($"형태 {curFormIndex} 에서 {index}로 변경");
        CUR_FORM_INDEX = index; 
    }
    
}
