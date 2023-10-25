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

    // cutting 파티클
    public Action onCutting;
    // cutting  행위
    public Action onCut;
    // washing 행위
    public Action onWashing;
   
    // Start is called before th아하!e first frame update
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
        print("소리 나니?");
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
