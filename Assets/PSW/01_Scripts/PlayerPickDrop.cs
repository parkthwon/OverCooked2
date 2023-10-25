using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerPickDrop : MonoBehaviourPun
{
    // ���� ����
    public GameObject obj;
    // ���� ����
    public GameObject pickObj;
    // ������ ���� ��ġ
    public Transform pickPosition;
    // ���� �Ÿ�
    public float distacne = 2;
    // ȸ�� ��
    public Transform body;
    // pickOn = �ֿ��� ��
    public bool pickOn = false;
    // tablePick = ���̺� ��ü�� �ֿ� �� ���� ��
    [SerializeField]bool tablePick = false;
    // Animator
    Animator anim;
    
    // Audio Source
    public AudioClip pickSound;
    // Audio Source
    public AudioClip dropSound;
    // Audio Source
    public AudioClip throwSound;

    // �����Ǿ��� �� ��
    public Color detectedColor;

    // ���� ��ü�� �� ���ϴ� ��
    public float power = 8f;
    public bool PICK
    {
        get { return pickOn; }
        set
        {
            pickOn = value;
            anim.SetBool("Pick", value);
        }
    }
    public bool TABLEPICK
    {
        get { return tablePick; }
        set
        {
            tablePick = value;
            anim.SetBool("Pick", value);
        }
    }

    void Start()
    {
        body = transform.Find("Body");
        anim = GetComponentInParent<Animator>();
    }

    // ������ ��´�.
    [PunRPC]
    public void Pick()
    {
        // ���� �� �ִ� ���·� Ȱ��ȭ
        PICK = true;
        // ���� ������ obj
        // pickObj �� ���� �� �ִ� ��ũ��Ʈ
        //fixme JBS ���� Befixed�� ��ü�� ��ȯ��
        pickObj = obj.GetComponent<ObjectPlace>().PickThing(body, pickPosition.position);
        //pickObj.transform.SetParent(body);

        // Audio Sound
        GetComponent<PlayerSound>().PlayerAudio(pickSound); // ����
    }

    [PunRPC]
    // ������ ���´�.
    public void Drop()
    {
        //pickObj =  ����� && ������ �׸��� obj.tag = plate
        if (pickObj.CompareTag("Ingredient") && obj != null && obj.CompareTag ("Plate"))
        {
            //������ ����� ���ϴ¹�
            //pickObj�� ��ũ��Ʈ Ingredient�� �����ͼ�, CUR_FORM_INDEX ������ 1���� Ȯ��
            //1�̸� ������. 0�̸� �����
            if(pickObj.GetComponent<Ingredient>().CUR_FORM_INDEX == 1)
            {
                //obj.compareTag("Plate") == true �� obj�� ��ũ��Ʈ PlateIngredient�� �����ͼ� �Լ� PlaceIngredient(����� ���ӿ�����Ʈ)�� �����Ų��.
                if (obj.CompareTag("Plate"))
                {
                    obj.GetComponent<PlateIngredient>().PlaceIngredient(gameObject);
                }
            }
        }
        //�׷��� ������
        else
        {
            PICK = false;
            pickObj.GetComponent<ObjectPlace>().PlaceThing();
            pickObj = null;
        }
    }
  
    // ���̺� �ִ� ��ü�� �ֿ� �� �ִ�
    [PunRPC]
    void TablePick()
    {
        PICK = true;
        // ���� ������ obj
        pickObj = obj.GetComponent<PlaceableTable>().PickThing(body.transform, pickPosition.position);
        // pickObj �� ���� �� �ִ� ��ũ��Ʈ
        pickObj.GetComponent<ObjectPlace>().PickThing(transform, pickPosition.position);
        // ���� ��ü�� body �� ��ġ ��Ű�� 
        pickObj.transform.SetParent(body);
        // Audio Sound
        GetComponent<PlayerSound>().PlayerAudio(pickSound); // ����
    }

    [PunRPC]
    // ���̺� ��ü�� ���� �� �ִ�.
    void TableDrop()
    {
        // ��ȯ�뿡 ����ᰡ �ִ� �׸��� ���� �� �ִ�.
        if (obj.CompareTag("PlateReceiver"))
        {
            PICK = !(obj.GetComponent<PlateReceiver>().CanReceivePlate(pickObj)); 
            if (PICK == false)
            {
                pickObj = null;
            }
            return;
        }
        else if(obj.CompareTag("TrashBinTable"))
        {
            obj.GetComponent<PlaceableTable>().PlaceThing(pickObj, gameObject);
        }
        else if (obj.CompareTag("SinkTable") && !pickObj.CompareTag("DirtyPlate"))
        {
            return;
        }
        obj.GetComponent<PlaceableTable>().PlaceThing(pickObj);
        PICK = false;
        pickObj = null;
        // Audio Sound
        GetComponent<PlayerSound>().PlayerAudio(dropSound); // ����
    }

    [PunRPC]
    // �׸��� ������ ����Ḧ ���� �� �ִ�.
    void DishFoodDrop()
    {
        obj.GetComponent<PlateIngredient>().PlaceIngredient(pickObj);
        // Audio Sound
        GetComponent<PlayerSound>().PlayerAudio(dropSound); // ����
    }

    [PunRPC]
    // �׸��� ��� �ִ� ���¿����� ������ ����Ḧ �׸��� ���� �� �ִ�.
    void DishPickFoodDrop()
    {
        if(pickObj.GetComponent<PlateIngredient>().PlaceIngredient(obj.GetComponent<PlaceableTable>().PLACED_OBJECT))
        {
            obj.GetComponent<PlaceableTable>().PLACED_OBJECT = null;
            TableDrop();
        }
        // pickObj.GetComponent<PlateIngredient>().PlaceIngredient(obj.GetComponent<PlaceableTable>().PickThing(body.transform, pickPosition.position));
    }
    // ���̺� ���� �ִ� �׸��� ������ ����Ḧ ���� �� �ִ�.
    [PunRPC]
    void TableFoodDrop()
    {
        obj.GetComponent<PlaceableTable>().PLACED_OBJECT.GetComponent<PlateIngredient>().PlaceIngredient(pickObj);
        // Audio Sound
        GetComponent<PlayerSound>().PlayerAudio(dropSound); // ����
    }
    // ä�� ���� ���� ������ �ִ� ���ǵ� ����
    // �÷��̾ ��� �ִ� ������ ������ ä�Ҹ� ���� �� �ִ�.
    
    void ISTablePick()
    {
        pickObj = obj.GetComponent<IngredientStrorage>().GetIngredient(photonView, pickPosition.position);
        // Audio Sound
        photonView.RPC("PlayPickSound", RpcTarget.All);
        //GetComponent<PlayerSound>().PlayerAudio(pickSound); // ����
    }

    [PunRPC]
    void PlayPickSound()
    {
        GetComponent<PlayerSound>().PlayerAudio(pickSound); // ����
    }

    [PunRPC]
    void Throw(Vector3 pos, Vector3 forward)
    {
        pickObj.transform.position = pos;

        Rigidbody objRigidbody = pickObj.GetComponent<Rigidbody>();
        Drop();
        objRigidbody.AddForce(forward * power, ForceMode.Impulse);
        anim.SetTrigger("Throw");
        // Audio Sound
        GetComponent<PlayerSound>().PlayerAudio(throwSound); // ����
    }
    void Update()
    {
        OnSense();

        //�տ� �� ��ü�� �ִ��� Ȯ��
        //�ִ�
        if (pickObj != null)
        {
            PICK = true;
        }
        //����
        else
        {
            PICK = false;
        }
        //������ �ƴҶ� �Լ��� ������
        if (photonView.IsMine == false) return;

        // layer == 15 : ����ᳪ �׸��� ��ü�� ���� �� �ִ�
        if (Input.GetButtonDown("Pick") && !pickOn && obj.layer == 15)
        {
            //Pick();
            photonView.RPC(nameof(Pick), RpcTarget.All);
            print("�̾���111111");
        }
        // layer == 15 : ����ᳪ �׸��� ��ü�� ���� �� �ִ�
        else if (Input.GetButtonDown("Pick") && pickOn && obj == null)
        {
            //Drop();
            photonView.RPC(nameof(Drop), RpcTarget.All);
            print("�̾���222222");
        }
        // layer == 12 : ���̺� ���� ��ü�� ���� �� �ִ�
        else if (Input.GetButtonDown("Pick") && tablePick && !pickOn && obj.layer == 12)
        {
            print("�̾���3333333!!");
            //TablePick();
            photonView.RPC(nameof(TablePick), RpcTarget.All);
        }
        // layer == 12 : ���̺� ���� ��ü�� ���� �� �ִ� & �� �׸����� ������ �Ǵ� ���� �׸��� �ֿ� �� ����.
        else if (Input.GetButtonDown("Pick") && !tablePick && pickOn && obj.layer == 12 && !obj.CompareTag("PlateReturner"))
        {
            print("�̾���4444444!");
            //TableDrop();
            photonView.RPC(nameof(TableDrop), RpcTarget.All);
        }
        else if (Input.GetButtonDown("Pick") && pickOn && obj.CompareTag("Plate"))
        {
            //DishFoodDrop();
            photonView.RPC(nameof(DishFoodDrop), RpcTarget.All);
        }
        // ���̺� ���ð� �ְ� ������ �� ������ ���� ��
        else if (Input.GetButtonDown("Pick") && obj.layer == 12 && obj.GetComponent<PlaceableTable>().isPlaced && pickOn)
        {
            //���� ������ �������� Ȯ��
            if(obj.GetComponent<PlaceableTable>().PLACED_OBJECT.CompareTag("Plate"))
            {
                //TableFoodDrop();
                photonView.RPC(nameof(TableFoodDrop), RpcTarget.All);
            }
            // �� �׸��� ��� �ִ� ���¿��� ������ ����� �տ� ���� �׸��� ����ᰡ �������.
            else if (obj.GetComponent<PlaceableTable>().PLACED_OBJECT.CompareTag("Ingredient"))
            {
                print("�ֶ�̾Ʒ��;�Ʒ����ٷ�;����");
                // DishPickFoodDrop();
                photonView.RPC(nameof(DishPickFoodDrop), RpcTarget.All);
            }
        }
        else if (Input.GetButtonDown("Pick") && !pickObj && !obj.GetComponent<PlaceableTable>().isPlaced)
        {
            //@@ JBS �߰� �ش� ��ü �±װ� ����� â�� ���� Ȯ��
            if(obj.CompareTag("IngredientStorageTable"))
            {
                ISTablePick();
                //photonView.RPC(nameof(ISTablePick), RpcTarget.All);
            }
        }
        else if (Input.GetButtonDown("Cut") && pickOn)
        {
            print("�з�����");
            //Throw();
            photonView.RPC(nameof(Throw), RpcTarget.All, pickObj.transform.position, body.forward);
        }
    }

    // ���� ����
    void OnSense()
    {
        SetObjHighlight(prevObj, false);
        // ���̾� ��ũ�� Placeable �̰ų� Table �� ��
        int layerMask = 1 << LayerMask.NameToLayer("Placeable") | 1 << LayerMask.NameToLayer("Table");
        //int layerMask = LayerMask.GetMask("Placeable", "Table");
        // ���� �ν��� �� �迭�� ����
        Collider[] cols = Physics.OverlapSphere(body.position, distacne, layerMask);

        if (cols.Length > 0)
        {
            //���� ����� object ���� ����
            Collider nearest = cols[0];
            for (int i = 1; i < cols.Length; i++)
            {
                //nearest �� body.position �� �Ÿ��� ����
                float dist1 = Vector3.Distance(nearest.transform.position, body.transform.position);

                //cols[i] �� body.position �� �Ÿ��� ����
                float dist2 = Vector3.Distance(cols[i].transform.position, body.transform.position);

                //dist1 �� dist2 ���� ũ�ٸ�
                if (dist1 > dist2)
                {
                    //nearest �� cols[i] �� ����
                    nearest = cols[i];
                }

                //print(i + " --- " + cols[i].gameObject.name + " : " + Vector3.Distance(body.position, cols[i].transform.position));
            }

            //����� �϶� �θ��Ǻθ�
            if (nearest.gameObject.layer == 15)
            {
                obj = nearest.transform.parent.parent.gameObject;
            }
            //���̺� �϶� �θ�
            else if (nearest.gameObject.layer == 12)
            {
                obj = nearest.transform.parent.gameObject;

                //���̺� ������ ������  tablepick true
                if (!obj.CompareTag("PlateReceiver")
                    && obj.GetComponent<PlaceableTable>().isPlaced)
                {
                    //print("ssssssssssssssssss");
                    tablePick = true;
                }
                else
                {
                    tablePick = false;
                }
            }
            //print($"������ ��ü : {obj}");
            SetObjHighlight(obj, true);
            prevObj = obj;
        }
        else
        {
            obj = null;
        }
    }

    ///<summary>
    ///XXX JBS �߰� �ش� ��ü �ƿ����� Ȱ/��
    ///</summary>
    void SetObjHighlight(GameObject gameObject, bool isHL)
    {
        if(photonView.IsMine)
        {
            if(gameObject != null && gameObject.GetComponent<ObjectPlace>() != null)
            {
                gameObject.GetComponent<ObjectPlace>().isHighlight = isHL;
            }
        }
    }
    //���� obj
    GameObject prevObj;

    // ���� ���� ���� Ȯ��
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(body != null)
        {
            Gizmos.DrawWireSphere(body.position, distacne); 
        }
    }
}
