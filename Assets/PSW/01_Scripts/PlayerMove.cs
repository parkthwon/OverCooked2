using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun, IPunObservable
{
    // 속력
    public float speed = 5;

    // 대시 힘 가하는 것
    public float power = 8f;
    
    // 리지드 바디
    Rigidbody rb;
   
    // 대시
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
    // particle 위치
    public Transform particlePos;

    // 서버에서 넘어오는 위치값
    Vector3 receivePos;
    // 서버에서 넘어오는 회전값
    Quaternion receiveRot = Quaternion.identity;
    // 보정하는 속력
    public float lerpSpeed = 50f;

    // 가로 방향을 결정하는 애
    float h;
    // 세로 방향을 결정하는 애
    float v;

    // 현재 시간
    float currentTime;

    //얼굴 모양
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

//@@ JBS 수정 회전 속도값
    [SerializeField] float rotSpeed;

    // Update is called once per frame
    void Update()
    {
        // 내가 만든 플레이어라면
        if (photonView.IsMine)
        {
            // W, S, A, D 키를 누르면 앞뒤좌우로 움직이고 싶다.
            // 사용자의 입력을 받자.
            h = Input.GetAxis("Horizontal");
            //print(h);
            v = Input.GetAxis("Vertical");
            Vector3 dir = new Vector3(h, 0, v);
            dir.Normalize();

            // 이동 애니메이션 발생
            //photonView.RPC(nameof(SetFloatRpc), RpcTarget.All, "Horizontal", h);
            //photonView.RPC(nameof(SetFloatRpc), RpcTarget.All, "Vertical", v);
           
            //@@ JBS 수정 임시 주석 처리
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
            // 대시 설정
            if (currentTime > dashTime)
            {
                if (Input.GetButtonDown("Dash"))
                {
                    rb.AddForce(body.forward * power, ForceMode.Impulse);
                    //rb.AddForce(body.Rotate * power, ForceMode.Impulse);
                    // Audio Sound
                    GetComponent<PlayerSound>().PlayerAudio(dashSound);
                    //파티클 효과
                    GameObject pa = Instantiate(particle, body);
                    pa.transform.position = particlePos.position;
                    print("야 되냐?????????");
                    currentTime = 0;
                }
            }
        }
        // 나의 플레이어가 아니라면
        else
        {
            // 위치 보정
            //transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);
            // 회전 보정
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
        // 내 Player면
        if(stream.IsWriting)
        {
            // 나의 위치 값을 보낸다
            //stream.SendNext(transform.position);
            // 나의 회전 값을 보낸다
            stream.SendNext(body.transform.rotation);
            // h 값 보낸다.
            stream.SendNext(h);
            // v 값 보낸다.
            stream.SendNext(v);
        }
        // 내 Player 아니라면
        else 
        {
            // 위치, 회전을 받자
            //receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion) stream.ReceiveNext();
            // h 값을 받자.
            h = (float)stream.ReceiveNext();
            // v 값을 받자.
            v = (float)stream.ReceiveNext();
        }
    }
}
