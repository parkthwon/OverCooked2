using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGManager : MonoBehaviour
{
    public static BGManager instance;
    //지상 머티리얼
    [SerializeField] Material matGround;
    //오프셋 변경 속도
    [SerializeField]float offsetSpeed;

    //시작 오프셋 값
    Vector2 startOffsetValue;
    //현재 오프셋 값
    [SerializeField]Vector2 curOffestValue;

    //구름 파티클 1,2
    [SerializeField]GameObject psCloud1,psCloud2;
    //블루 필터
    [SerializeField]GameObject blueFilter;

    //타이밍 종류
    public enum BGTiming
    {
        ChangeCloud,Thunder1,Thunder2,BigExpl,Ending
    }
    public BGTiming bgTiming;
    //타이밍 배열
    [SerializeField] float[] BGTimingAry;

    //브금 
    [SerializeField]AudioSource bgm;
    //브금 목록
    [SerializeField]AudioClip[] bgmList;
    //현재 브금 재생 인덱스
    int curBgmIdx;

    //환경음
    [SerializeField]AudioSource amb;


    //특수 사운드
    [SerializeField]AudioSource sfx;
    //특수 사운드 이펙트
    [SerializeField]AudioClip[] sfxList;

    //오디오 타입
    public enum soundType
    {
        Thunder1,Thunder2,BigThunder,BigExplIntro,BigExpl
    }



    //정지 상태 확인
    [SerializeField]bool isPaused;
    public bool IS_PAUSED
    {
        get{return isPaused;}
        set
        {
            isPaused = value;
        }
    }
    //음악 종료 확인
    bool isStop;

    //PD
    [SerializeField] GameObject PD;

    //연출용 패널
    [SerializeField] GameObject whitePanel,blankPanel;

    //번개 프리팹
    [SerializeField]GameObject thunderF;

    //번개1,2,3 위치
    Vector3 thunder1Pos,thunder2Pos,thunderBigPos;

    //vcam1
    [SerializeField]GameObject vcam1;

    //돌리 카트
    [SerializeField]Transform dollyCart;

    //카메라 흔들림 코루틴
    Coroutine cameraShakeCor;

    //강조선 파티클
    [SerializeField]GameObject psSpeedLines;
    

    
    private void Awake() {
        instance = this;
        //브금 실행
        PlayBGM(curBgmIdx);
        amb.Play();
        //시작 오프셋 값으로 변경
        startOffsetValue = new Vector2(0, 0);
        curOffestValue = startOffsetValue;
        matGround.SetTextureOffset("_MainTex", startOffsetValue);
        //연출용 패널 비활성화
        blankPanel.SetActive(false);
        //파티클 초기 활성화
        psCloud1.SetActive(true);
        psCloud2.SetActive(false);
        psSpeedLines.SetActive(false);
        //번개 위치 설정
        thunder1Pos = new Vector3(-13.32f,-2.44f,5.38f);
        thunder2Pos = new Vector3(7.17f,-2.13f, 5.92f);
        thunderBigPos = new Vector3(-1.19f, 6.54f, -5.76f);
    }

    private void Start() {
        //배경 연출 코루틴
        StartCoroutine(IEPlayBGSFX());
        //카메라 흔들림 코루틴
        cameraShakeCor = StartCoroutine(IECameraShake());
    }

    private void Update() {
        //지상 머티리얼의 offset을 천천히 감소 시키고 싶다
        //-오프셋 스피드*시간 만큼 오프셋 값 변경
        curOffestValue += new Vector2(0, -Time.deltaTime*offsetSpeed);
        matGround.SetTextureOffset("_MainTex", curOffestValue);

        
        //음악 끝나면 다음 음악 재생
        if(!bgm.isPlaying && !IS_PAUSED && isStop)
        {
            isStop = !PlayBGM(curBgmIdx+1);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            print("번개 테스트");
            StartCoroutine(IEThunder(new Vector3(-13.32f,-2.44f,5.38f), 2));
        }
        if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            print("카메라 정지");
            StopCoroutine(cameraShakeCor);
        }

        //게임 일시정지 확인해서 음악 일시정지
        UpdatePauseSound();
        


    }

    //시간 확인해서 배경 특수 연출 재생
    IEnumerator IEPlayBGSFX()
    {
        //1:22 구름 전환 연출 대기
        while(TimerManager.instance.time > BGTimingAry[(int) BGTiming.ChangeCloud])
        {
            yield return null;
        }
        StartCoroutine(IEChangeCloud());
        //1:18 천둥1 연출
        while(TimerManager.instance.time > BGTimingAry[(int) BGTiming.Thunder1])
        {
            yield return null;
        }
        StartCoroutine(IEThunder(thunder1Pos, 2));
        PlaySFX(soundType.Thunder1);
        //1:10 천둥2-빅 연출
        while(TimerManager.instance.time > BGTimingAry[(int) BGTiming.Thunder2])
        {
            yield return null;
        }
        StartCoroutine(IEBigThunder());
        //0:10 추락 연출
        while(TimerManager.instance.time > BGTimingAry[(int) BGTiming.BigExpl])
        {
            yield return null;
        }
        StartCoroutine(IEBigExpl());
        //0:00 엔딩 씬 전환
        while(TimerManager.instance.time > BGTimingAry[(int) BGTiming.Ending])
        {
            yield return null;
        }
        print("엔딩씬 전환");
        blankPanel.SetActive(false);
        Ending.instance.OpenEndingUI();
    }

    //카메라 흔들림
    IEnumerator IECameraShake()
    {
        //vcam의 x값 -.5f 의 min,max -1f, 0
        Vector3 vcamPos = vcam1.transform.position;
        Vector3 minPos = vcamPos + new Vector3(-.5f,0,0);
        Vector3 maxPos = vcamPos + new Vector3(0.5f,0,0);
        while(true)
        {

            for(float time = 0; time < 2f; time += Time.deltaTime)
            {
                vcam1.transform.position = Vector3.Lerp(vcam1.transform.position, minPos, Time.deltaTime);
                yield return null;
            }
            for(float time = 0; time < 2f; time += Time.deltaTime)
            {
                vcam1.transform.position = Vector3.Lerp(vcam1.transform.position, maxPos, Time.deltaTime);
                yield return null;
            }
        }
    }

    IEnumerator IECameraShake2()
    {
        while(true)
        {
            vcam1.transform.position = dollyCart.position;
            yield return null;
        }
    }

    //천천히 구름 2로 전환
    IEnumerator IEChangeCloud()
    {
        //구름1 위치 저장
        Vector3 cloudPos = psCloud1.transform.position;
        //구름1 비활성화
        psCloud1.SetActive(false);
        //구름2 활성화
        psCloud2.SetActive(true);
        //5초 동안 구름 이동
        for(float cTime = 0; cTime < 5; cTime += Time.deltaTime)
        {
            psCloud2.transform.position = Vector3.Lerp
                (psCloud2.transform.position
                , cloudPos
                , Time.deltaTime*.5f);
            yield return null;
        }
        
        yield return null;
    }

    //횟수만큼 번개 치기
    IEnumerator IEThunder(Vector3 pos, int times)
    {
        for(int i = 0; i < times; i++)
        {
            //번개 생성
            GameObject thunder = Instantiate(thunderF, transform.parent);
            //번개 위치 설정
            thunder.transform.position = pos;
            //번개 렌더러 가져오기
            Renderer rend = thunder.GetComponent<Renderer>();
            yield return StartCoroutine(IEFadeInOut(rend));
            yield return null;
            Destroy(thunder);

        }
    }

    //흰 패널 페이드 인/아웃
    IEnumerator IEWhitePanelFadeInOut(float fadeDuration = 0.12f)
    {
        //이미지 가져오기
        Image wpImg = whitePanel.GetComponent<Image>();
        //알파값
        float alpha = 0;
        //페이드 인
        for(float time = 0; time <= fadeDuration; time += Time.deltaTime)
        {
            alpha = time/fadeDuration;
            yield return null;
            wpImg.color = new Color(1,1,1,alpha);
        }
        yield return null;
        alpha = 1;
        wpImg.color = new Color(1,1,1,alpha);
        yield return new WaitForSeconds(0.12f);
        //페이드 아웃
        for(float time = fadeDuration; time >= 0; time -= Time.deltaTime)
        {
            alpha = time/fadeDuration;
            yield return null;
            wpImg.color = new Color(1,1,1,alpha);
        }
        yield return null;
        alpha = 0;
        wpImg.color = new Color(1,1,1,alpha);
        yield return null;
    }

    //해당 객체 기간 동안 페이드 인/아웃
    IEnumerator IEFadeInOut(Renderer rend ,float fadeDuration = 0.1f)
    {
        //알파값
        float alpha = 0;
        //페이드 인
        for(float time = 0; time <= fadeDuration; time += Time.deltaTime)
        {
            alpha = time/fadeDuration;
            yield return null;
            rend.material.color = new Color(1,1,1,alpha);
        }
        yield return null;
        alpha = 1;
        rend.material.color = new Color(1,1,1,alpha);
        //페이드 아웃
        for(float time = fadeDuration; time >= 0; time -= Time.deltaTime)
        {
            alpha = time/fadeDuration;
            yield return null;
            rend.material.color = new Color(1,1,1,alpha);
        }
        yield return null;
        alpha = 0;
        rend.material.color = new Color(1,1,1,alpha);
    }


    //번개2-큰 번개 연출
    IEnumerator IEBigThunder()
    {
        //번개2 이펙트
        StartCoroutine(IEThunder(thunder2Pos, 2));
        //번개2 사운드
        sfx.PlayOneShot(sfxList[(int)soundType.Thunder2]);
        yield return new WaitForSeconds(1);
        //흰 패널 -> 큰 번개 x2 -> 흰 패널 -> 맵 변경
        yield return StartCoroutine(IEWhitePanelFadeInOut());

        //큰 번개 사운드
        PlaySFX(soundType.BigThunder);

        StartCoroutine(IEWhitePanelFadeInOut());
        yield return new WaitForSeconds(0.06f);
        //큰 번개 이펙트
        yield return StartCoroutine(IEThunder(thunderBigPos, 2));

        StartCoroutine(IEWhitePanelFadeInOut(0.2f));

        //카메라이동 정지
        StopCoroutine(cameraShakeCor);
        //카메라이동2 시작
        cameraShakeCor = StartCoroutine(IECameraShake2());
        //강조선 파티클 활성화
        psSpeedLines.SetActive(true);
    }

    //추락 연출
    IEnumerator IEBigExpl()
    {
        //번개 이펙트 및 사운드
        StartCoroutine(IEThunder(thunderBigPos, 2));
        PlaySFX(soundType.Thunder1);
        //카메라 흔들림 정지
        StopCoroutine(cameraShakeCor);
        //추락 컷신 시작
        PD.SetActive(true);
        //2초 대기후 intro 재생
        yield return new WaitForSeconds(2);
        PlaySFX(soundType.BigExplIntro);
        //인트로 재생 대기
        yield return new WaitForSeconds(2);
        while(IS_PAUSED)
        {
            yield return null;
        }
        //배경 음악 종료
        bgm.Stop();
        //추락 사운드 재생
        PlaySFX(soundType.BigExpl);
        //검은 화면 활성화
        blankPanel.SetActive(true);
    }

    //게임 일시정지 확인해서 음악 일시정지
    void UpdatePauseSound()
    {
        if(Time.timeScale == 0f)
        {
            bgm.Pause();
            amb.Pause();
            sfx.Pause();
            IS_PAUSED = true;
        }
        else if(Time.timeScale == 1f && IS_PAUSED)
        {
            bgm.Play();
            amb.Play();
            sfx.Play();
            IS_PAUSED = false;
        }
    }

    

    //음악 종료시 다음 음악 재생
    bool PlayBGM(int idx)
    {
        try
        {
            curBgmIdx = idx;
            bgm.clip = bgmList[idx];
            bgm.Play();
            print("다음 음악 재생");

            return true;
        }
        catch
        {
            print("음악 종료");
            return false;
        }
    }

    void PlaySFX(soundType sType)
    {
        sfx.clip = sfxList[(int) sType];
        sfx.Play();
        print("sfx 재생" + sType.ToString());
    }

    //private void OnDrawGizmos() {
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(vcam1.transform.position, .5f);
    //}
}
