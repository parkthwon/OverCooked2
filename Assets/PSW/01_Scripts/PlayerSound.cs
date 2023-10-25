using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSound : MonoBehaviour
{
    public AudioSource audioSource;
    // AudioSource
    public AudioClip cutSound;
    // AudioSource
    public AudioClip washSound;

    // cutting ��ƼŬ
    public Action onCutting;
    // cutting  ����
    public Action onCut;
    // washing ����
    public Action onWashing;
   
    // Start is called before th����!e first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        onCutting = GetComponent<PlayerCutWash>().OnCutting;
        onCut = GetComponent<PlayerCutWash>().Cutting;
        onWashing = GetComponent<PlayerCutWash>().Washing;
    }

    // Update is called once per frame
    void Update()
    {

    }

    
    void PlayerCut()
    {
        print("�Ҹ� ����?");
        audioSource.clip = cutSound;
        audioSource.Play();
        onCutting();
        onCut();
    }

    void PlayerWash()
    {
        audioSource.clip = washSound;
        audioSource.Play();
        onWashing();
    }   
    void PlayerMove()
    {
       GetComponent<PlayerMove>().MoveParticle();
    }

    [PunRPC]
    public void PlayerAudio(AudioClip audio)
    {
        audioSource.clip = audio;
        audioSource.Play();
    }

}
