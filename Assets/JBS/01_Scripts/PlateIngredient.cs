using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlateIngredient : MonoBehaviourPun
{
    //식재료나 식재료가 하나 든 접시를 들고 상호작용하면 접시에 식재료가 놓여짐.

    //물건 놓이는 위치
    Transform placePos;

    //접시 스택
    public int plateStack = 1;

    //접시 프리팹
    public GameObject plateF;

    

    //놓여진 식재료 번호 리스트
    public List<int> platedIngredientsIDList;

    //로드된 놓인 식재료 프리팹
    [SerializeField]GameObject platedIngredientF;

    //요리 번호
    [Tooltip("9999는 없는것")]
    public int foodIndex = 9999;

    private void Awake() {
        placePos = transform.Find("PlacePos");
    }

    private void Update() {
        //접시에 담긴 요리 유형 확인
        CheckFood();
    }

    //접시에 담긴 요리 유형 확인 
    /*
    식재료 번호 양상추 0 , 토마토 1, 오이 2, 생선 3, 쌀 4, 김 5
    */
    void CheckFood()
    {
        //양상추 샐러드
        if(platedIngredientsIDList.Count == 1 
                && platedIngredientsIDList[0] == 0)
        {
            foodIndex = 0;
        }
        //양상추-토마토 샐러드
        else if(platedIngredientsIDList.Count == 2
                && platedIngredientsIDList.Contains(0)
                && platedIngredientsIDList.Contains(1))
        {
            foodIndex = 1;
        }
        //양상추-토마토-오이 샐러드
        else if(platedIngredientsIDList.Count == 3
                && platedIngredientsIDList.Contains(0)
                && platedIngredientsIDList.Contains(1)
                && platedIngredientsIDList.Contains(2))
        {
            foodIndex = 2;
        }
        //@@ 생선 스시, 오이 스시
        else
        {
            foodIndex = 9999;
        }
    }

    //요청되면 식재료 객체와 리스트를 비우고 싶다
    public void ClearIng()
    {
        print("접시 : 내부 식재료, 리스트 비움");
        //@@ 해당 접시의 ui 리스트도 제거
        GetComponent<IngredientUI>().ClearIconUI();
        //해당 접시의 placePos안의 식재료 오브젝트를 제거한다.
        Transform[] pushObjPlacePosChildArray = transform.Find("PlacePos").gameObject.GetComponentsInChildren<Transform>();
        for(int i = 1; i<pushObjPlacePosChildArray.Length;i++)
        {
            if(pushObjPlacePosChildArray[i] != transform.Find("PlacePos"))
            {
                Destroy(pushObjPlacePosChildArray[i].gameObject);
            }
        }
        //해당 접시의 list값을 비운다.
        platedIngredientsIDList = new List<int>();
    }
    
    //요청되면 접시위에 접시를 쌓고싶다.
    public void StackPlate(GameObject newPlate)
    {
        if(newPlate.CompareTag("Plate"))
        {
            //접시 스택 증가
            plateStack++;
            //받은 접시 제거
            Destroy(newPlate);
            //if(PhotonNetwork.IsMasterClient)
            //    PhotonNetwork.Destroy(newPlate);
        }
    }
    
    
    //해당 식재료를 접시 위에 올림.
    public bool PlaceIngredient(GameObject pushObj)
    {
        //접시 스택이 있는지 확인
        if(plateStack > 1)
        {
            print("접시: 접시 스택이 있으면 올릴 수 없음");
            return false;
        }

        //식재료인지 검증
        if(pushObj.CompareTag("Ingredient"))
        {
            //손질된 식재료인지 검증
            if(pushObj.GetComponent<Ingredient>().CUR_FORM_INDEX == 1)
            {
                //해당 식재료의 놓기 스크립트 가져오기
                PlaceableObject op = pushObj.GetComponent<PlaceableObject>();
                int ingIndex = op.objIndex;
                //해당 식재료의 id가 리스트에 중복인지 확인
                foreach(int idx in platedIngredientsIDList)
                {
                    //중복이면 return
                    if(idx == ingIndex)
                    {
                        print("이미 같은 식재료가 들어있음");
                        return false;
                    }
                }
                //XXX 검증 끝 식재료를 접시에 올리기
                //식재료가 가진 id를 리스트에 추가
                platedIngredientsIDList.Add(ingIndex);
                //해당 식재료 번호에 맞는 놓인 식재료 로드
                LoadPlatedIngredient(op.objIndex);
                //로드한 식재료 생성
                //XXX 포톤뷰 안붙으니 굳이 포톤으로 생성안함
                GameObject platedIngredient = Instantiate(platedIngredientF);
                //식재료 초기 세팅
                platedIngredient.transform.parent = placePos;
                platedIngredient.transform.position = placePos.position;

                //받은 식재료 제거
                Destroy(pushObj);
                //if(PhotonNetwork.IsMasterClient)
                //    PhotonNetwork.Destroy(pushObj);

                return true;
            }
            else
            {
                print("접시 : 손질된 식재료가 아님");
                return false;
            }
        }
        //접시인지 확인
        else if(pushObj.CompareTag("Plate"))
        {
            //중복된 식재료가 있는지 확인
            PlateIngredient pushObjPI = pushObj.GetComponent<PlateIngredient>();
            //해당 접시의 식재료 번호 리스트 가져오기
            List<int> pushObjIngIDList = pushObjPI.platedIngredientsIDList;
            //두 리스트의 교집합 구하기
            var exceptLeft = platedIngredientsIDList.Except(pushObjIngIDList);
            var exceptRight = pushObjIngIDList.Except(platedIngredientsIDList);
            var intersection = pushObjIngIDList.Intersect(platedIngredientsIDList);
            //교집합 리스트
            List<int> interList = intersection.ToList();
            //없으면
            if(interList.Count == 0)
            {
                //해당 접시 식재료를 내 접시에 생성한다.
                foreach(int num in pushObjIngIDList)
                {
                    //해당 식재료 번호에 맞는 놓인 식재료 로드
                    LoadPlatedIngredient(num);
                    //로드한 식재료 생성
                    //XXX 포톤뷰 안붙으니 굳이 포톤으로 생성안함
                    GameObject platedIngredient = Instantiate(platedIngredientF);
                    //식재료 초기 세팅
                    platedIngredient.transform.parent = placePos;
                    platedIngredient.transform.position = placePos.position;
                }
                //해당 접시 식재료 번호 리스트와 내 리스트를 합친다.
                platedIngredientsIDList.AddRange(pushObjIngIDList);
                //해당 접시의 ui 리스트도 제거
                pushObj.GetComponent<IngredientUI>().ClearIconUI();
                //해당 접시의 placePos안의 식재료 오브젝트를 제거한다.
                Transform[] pushObjPlacePosChildArray = pushObj.transform.Find("PlacePos").gameObject.GetComponentsInChildren<Transform>();
                for(int i = 1; i<pushObjPlacePosChildArray.Length;i++)
                {
                    if(pushObjPlacePosChildArray[i] != pushObj.transform.Find("PlacePos"))
                    {
                        Destroy(pushObjPlacePosChildArray[i].gameObject);
                    }
                }
                //해당 접시의 list값을 비운다.
                pushObjPI.platedIngredientsIDList = new List<int>();
                print("접시 : 접시의 식재료 합쳐짐");
                return true;
            }
            //이게 있으면 안됨.
            else
            {
                print("접시 : 접시의 식재료 중복됨.");
                return false;
            }
        }
        else
        {
            print("접시 : 식재료 아님");
            return false;
        }
    }

    void LoadPlatedIngredient(int objIndex)
    {
        platedIngredientF = null;
        //해당 식재료 번호에 맞는 놓인 식재료 로드
        switch(objIndex)
        {
            case 0:
                platedIngredientF = Resources.Load<GameObject>("Prefabs/PlacedIngredient/Salad_Lettuce");
                break;
            case 1:
                platedIngredientF = Resources.Load<GameObject>("Prefabs/PlacedIngredient/Salad_Tomato");
                break;
            case 2:
                platedIngredientF = Resources.Load<GameObject>("Prefabs/PlacedIngredient/m_plated_sushi_cucumber_01");
                break;
            case 3:
                //platedIngredientF = Resources.Load<GameObject>("Prefabs/PlacedIngredient/Salad_Lettuce");
                break;
            case 4:
                //platedIngredientF = Resources.Load<GameObject>("Prefabs/PlacedIngredient/Salad_Lettuce");
                break;
        }
    }
}
