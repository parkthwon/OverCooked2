using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun, IPunObservable
{
    // �ӷ�
    public float speed = 5;

    // ��� �� ���ϴ� ��
    public float power = 8f;
    
    // ������ �ٵ�
    Rigidbody rb;
   
    // ���
    public bool isDashing = false;

     //player body
    public Transform body;

    // animator
    Animator anim;

    // Audio Souce
    public AudioClip moveSound;
    // Audio Souce
    public AudioClip dashSound;

    // particle
    public GameObject particle;
    // particle ��ġ
    public Transform particlePos;

    // �������� �Ѿ���� ��ġ��
    Vector3 receivePos;
    // �������� �Ѿ���� ȸ����
    Quaternion receiveRot = Quaternion.identity;
    // �����ϴ� �ӷ�
    public float lerpSpeed = 50f;

    // ���� ������ �����ϴ� ��
    float h;
    // ���� ������ �����ϴ� ��
    float v;

    // ���� �ð�
    float currentTime;

    //�� ���
    public GameObject[] faceObject;

    public void SetFace(int index)
    {
        photonView.RPC(nameof(RpcSetFace), RpcTarget.AllBuffered, index);
    }

    [PunRPC]
    void RpcSetFace(int index)
    {
        faceObject[index].SetActive(true);
    }


    public float dashTime = 0.8f;

    private void Awake()
    {
        if(PlayerManager.instance != null)
            PlayerManager.instance.AddPlayer(photonView);
    }

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        anim = GetComponentInParent<Animator>();
        isDashing = false;
    }

//@@ JBS ���� ȸ�� �ӵ���
    [SerializeField] float rotSpeed;

    // Update is called once per frame
    void Update()
    {
        // ���� ���� �÷��̾���
        if (photonView.IsMine)
        {
            // W, S, A, D Ű�� ������ �յ��¿�� �����̰� �ʹ�.
            // ������� �Է��� ����.
            h = Input.GetAxis("Horizontal");
            //print(h);
            v = Input.GetAxis("Vertical");
            Vector3 dir = new Vector3(h, 0, v);
            dir.Normalize();

            // �̵� �ִϸ��̼� �߻�
            //photonView.RPC(nameof(SetFloatRpc), RpcTarget.All, "Horizontal", h);
            //photonView.RPC(nameof(SetFloatRpc), RpcTarget.All, "Vertical", v);
           
            //@@ JBS ���� �ӽ� �ּ� ó��
            //GetComponent<PlayerSound>().PlayerAudio(moveSound);
            float angle = Vector3.Angle(dir, body.right); //dir body.right
            if (dir.magnitude != 0)
            {
                if (angle < 89)
                {
                    body.Rotate(0, rotSpeed * Time.deltaTime, 0);
                }
                else if (angle > 90)
                {
                    body.Rotate(0, -rotSpeed * Time.deltaTime, 0);
                }
            }


            Vector3 velocity = dir * speed;
            transform.position += velocity * Time.deltaTime;

            currentTime += Time.deltaTime;
            // ��� ����
            if (currentTime > dashTime)
            {
                if (Input.GetButtonDown("Dash"))
                {
                    rb.AddForce(body.forward * power, ForceMode.Impulse);
                    //rb.AddForce(body.Rotate * power, ForceMode.Impulse);
                    // Audio Sound
                    GetComponent<PlayerSound>().PlayerAudio(dashSound);
                    //��ƼŬ ȿ��
                    GameObject pa = Instantiate(particle, body);
                    pa.transform.position = particlePos.position;
                    print("�� �ǳ�?????????");
                    currentTime = 0;
                }
            }
        }
        // ���� �÷��̾ �ƴ϶��
        else
        {
            // ��ġ ����
            //transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);
            // ȸ�� ����
            body.transform.rotation = Quaternion.Lerp(body.transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
        }
        anim.SetFloat("Horizontal", h);
        anim.SetFloat("Vertical", v);
    }

    public void MoveParticle()
    {
        GameObject pa = Instantiate(particle, body);
        pa.transform.position = particlePos.position;
    }

    [PunRPC]
    void SetFloatRpc(string eventName, float parameter)
    {
        anim.SetFloat(eventName, parameter);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // �� Player��
        if(stream.IsWriting)
        {
            // ���� ��ġ ���� ������
            //stream.SendNext(transform.position);
            // ���� ȸ�� ���� ������
            stream.SendNext(body.transform.rotation);
            // h �� ������.
            stream.SendNext(h);
            // v �� ������.
            stream.SendNext(v);
        }
        // �� Player �ƴ϶��
        else 
        {
            // ��ġ, ȸ���� ����
            //receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion) stream.ReceiveNext();
            // h ���� ����.
            h = (float)stream.ReceiveNext();
            // v ���� ����.
            v = (float)stream.ReceiveNext();
        }
    }
}
