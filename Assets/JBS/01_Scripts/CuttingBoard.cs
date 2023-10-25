using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : MonoBehaviour
{
    //놓인 물건이 있으면 칼 비활성화
    PlaceableTable pot;

    [SerializeField] GameObject knife;

    private void Awake() {
        pot = GetComponent<PlaceableTable>();
    }

    // Update is called once per frame
    void Update()
    {
        //놓인 물건이 있으면 칼 비활성화
        knife.SetActive(!pot.isPlaced);
    }
}
