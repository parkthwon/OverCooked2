using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ObjectPlace : MonoBehaviourPun
{
    //메시 렌더러
    public Renderer objRenderer;
    //렌더러 초기 머티리얼 값
    Material[] objMatList;
    //아웃라인 머티리얼
    Material outline;
    //머티리얼 리스트
    [SerializeField]List<Material> materialList = new List<Material>();

    public bool isHighlight = false;

    public virtual GameObject PickThing(Transform objParent, Vector3 fixedPos)
    {
        return null;
    }

    public virtual bool PlaceThing(GameObject thing = null)
    {
        return false;
    }

    public virtual bool PlaceThing(GameObject thing, GameObject player)
    {
        return false;
    }

    ///<summary>
    /// 아웃라인 머티리얼 추가
    ///</summary>
    public void SetOutlineMat()
    {
        outline = new Material(Shader.Find("Mingyu/Outline"));
        objMatList = objRenderer.materials;
    }

    private void LateUpdate() {
        CheckHighlight();
    }

    ///<summary>
    /// isHighlight 값에 따라 아웃라인 활/비활성화
    ///</summary>
    public void CheckHighlight()
    {
        if(objMatList != null)
        {
            if(isHighlight)
            {
                materialList.Clear();
                materialList.AddRange(objMatList);
                materialList.Add(outline);

                objRenderer.materials = materialList.ToArray();
            }
            else
            {
                materialList.Clear();
                materialList.AddRange(objMatList);
                materialList.Remove(outline);

                objRenderer.materials = materialList.ToArray();
            }

        }
    }

    
}
