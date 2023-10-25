using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class IngredientStrorage : MonoBehaviourPun
{

    //공급할 식재료 프리팹
    [SerializeField]GameObject ingredientF;
    //공급할 식재료 프리팹 경로
    string ingredientFPath;
    //공급할 식재료 상자 렌더러
    [SerializeField] Renderer ingIconRen;
    //공급할 식재료 번호
    [SerializeField]int supplyIngID;

    //물건 놓기 스크립트
    PlaceableTable po;

    private void Awake() {
        po = GetComponent<PlaceableTable>();
        //번호에 맞는 식재료 프리팹 로드
        LoadIngredientF();
    }

    //식재료 프리팹 로드
    void LoadIngredientF()
    {
        ingredientF = null;
        //번호에 맞는 식재료 프리팹 로드
        switch(supplyIngID)
        {
            case 0:
                //식재료 프리팹 로드
                ingredientF = Resources.Load<GameObject>("Prefabs/Placeable/Lettuce");
                ingredientFPath = "Prefabs/Placeable/Lettuce";
                //식재료 아이콘 로드
                ingIconRen.material = Resources.Load<Material>("Prefabs/Materials/CrateIcon/M_IconLettuce");
                break;
            case 1:
                //식재료 프리팹 로드
                ingredientF = Resources.Load<GameObject>("Prefabs/Placeable/Tomato");
                ingredientFPath = "Prefabs/Placeable/Tomato";
                //식재료 아이콘 로드
                ingIconRen.material = Resources.Load<Material>("Prefabs/Materials/CrateIcon/M_IconTomato");
                break;
            case 2:
                //식재료 프리팹 로드
                ingredientF = Resources.Load<GameObject>("Prefabs/Placeable/Cucumber");
                ingredientFPath = "Prefabs/Placeable/Cucumber";
                //식재료 아이콘 로드
                ingIconRen.material = Resources.Load<Material>("Prefabs/Materials/CrateIcon/M_IconCucumber");
                break;
            //case 3:
            //    ingredientF = Resources
            //        .Load<GameObject>("Prefabs/Placeable/Lettuce");
            //    break;
        }
    }


    //정해진 식재료를 플레이어에게 주기
    public GameObject GetIngredient(PhotonView player, Vector3 pickPos)
    {
        //위에 놓인 물건이 없음
        if(!po.isPlaced)
        {
            print("식재료 획득");
            //식재료 생성
            //GameObject ingredient = Instantiate(ingredientF);
            GameObject ingredient = PhotonNetwork.Instantiate(ingredientFPath, Vector3.zero, Quaternion.identity);
            //식재료 방향은 플레이어와 같게
            ingredient.transform.forward = player.transform.forward;
            //식재료를 플레이어에게 줌.
            ingredient.GetPhotonView().RPC("RpcPickTing", RpcTarget.All, player.ViewID, pickPos);
            //ingredient.GetComponent<ObjectPlace>().PickThing(player, pickPos);

            return ingredient;
        }
        else
        {
            print("식재료 획득 불가함");

            return null;
        }
    }
}
