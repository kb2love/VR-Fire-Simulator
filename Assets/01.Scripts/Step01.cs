using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFireSim;

public class Step01 : MonoBehaviour
{
    [Header("AudioSource")]
    [SerializeField] AudioSource teacherSource;     // 선생님의 지시를 플레이할 오디오 소스

    [SerializeField] AudioSource stepAudioSource;   // 나레이션 오디오 소스

    [SerializeField] AudioSource warningAudioSource;    //화재 경고음이 울릴 오디오 소스

    [Header("AudioClip")]

    [SerializeField] AudioClip narration;           // 나레이션 오디오 클립

    [SerializeField] AudioClip teacherGuid01;       // 선생님의 지시 오디오 클립 01

    [SerializeField] AudioClip teacherGuid02;

    [SerializeField] AudioClip teacherGuid03;

    [SerializeField] AudioClip fireWarningClip;         // 화재경고음 클립

    [SerializeField] AudioClip fireWarningClipChange;   // 화재 경고음을 반복시키기 위한 클립 

    [SerializeField] AudioClip clikClip;            // 클릭 오디오 클립


    [Header("Animator")]

    [SerializeField] Animator teacherAni;           // 선생님의 애니메이터

    [SerializeField] Animator[] kidAnimations;

    [Header("Transform")]

    [SerializeField] Transform playerTr;              // 플레이어를 이동시킬 트랜스폼

    [SerializeField] Transform teacherTr;             // 선생님을 이동시킬 트랜스폼

    [SerializeField] Transform[] kidsTr;

    [SerializeField] Transform[] targetTr;      // 이동할 위치

    [SerializeField] GameObject nextStep;           // 다음 스탭으로 넘어가기 위한 오브젝트

    [SerializeField] GameObject fire;

    AudioManager audioManager;                  // 오디오 매니저를 사용하기 위한 변수

    TweenManager tweenManager;                  // 트윈 매니저를 사용하기 위한 변수
    void OnEnable()
    {
        tweenManager = new TweenManager();                      // 모노 비헤이버를 상속받지 않아도 되기 때문에 클라스를 생성 시켜 준다
        tweenManager.PopUpUI(transform);

        audioManager = gameObject.AddComponent<AudioManager>(); // 네임 스페이스로 제작 해 놓은 클래스(모노비헤이버상속)
        audioManager.NarrationPlay(stepAudioSource, narration, 1.0f);      //생성된 오디오 매니저 클라스의 나레이션 메서드를 사용 할 수 있다

        targetTr = GameObject.Find("TargetPosition").GetComponentsInChildren<Transform>();

        teacherAni.SetTrigger("TalkTrigger");
    }
    void OnDisable()
    {
        tweenManager = null;                                // 생성했던 트윈 매니저를 비워준다
        audioManager = null;                                // 생성했던 오디오 매니저를 지워준다
        Destroy(gameObject.GetComponent<AudioManager>());   // 추가했던 콤포넌트를 삭제 해 준다
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LeftCol" || other.gameObject.tag == "RightCol")
        {
            OnClickMethods();

        }
    }
    public void OnClickMethods()
    {
        teacherSource.PlayOneShot(clikClip, 1.0f);            // 중복이 되는 단발성 오디오 플레이
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => tweenManager.CloseUI(transform));  //UI를 닫는다
        seq.AppendInterval(1.0f);
        seq.AppendCallback(() =>
        {
            Debug.Log("asd");
            audioManager.LoopAudioPlay(warningAudioSource, fireWarningClip);  // 화재 경고음 재생
            teacherAni.SetTrigger("LookAround");            // 선생님이 불이 난걸 알아챈다
            fire.SetActive(true);                           // 불을 켜준다
            for (int i = 0; i < kidAnimations.Length; i++)
            {
                kidAnimations[i].SetTrigger("StandUp");     // 아이들이 일어난다
                kidsTr[i].DOMoveZ(1.11f, 1.0f);             // 일어났을때 의자에 떨어지도록 약간 움직여준다
            }
        });
        seq.AppendInterval(2.0f);
        seq.AppendCallback(() => audioManager.LoopAudioPlay(warningAudioSource, fireWarningClipChange));  //화재경고음의 시작부분을 바꿀 시퀀스
        seq.AppendInterval(4.0f);
        seq.AppendCallback(() =>
        {
            // 대화 애니메이션 실행 및 선생님의 지시 오디오 재생
            teacherAni.SetTrigger("TalkTrigger");
            // 선생님의 지시 오디오를 재생합니다.
            teacherSource.PlayOneShot(teacherGuid01, 1.0f);
        });
        seq.AppendInterval(7.0f);
        seq.AppendCallback(() => teacherSource.PlayOneShot(teacherGuid02, 1.0f));       //선생님의 2번째 지시 오디오 실행
        seq.AppendInterval(7.0f);
        seq.AppendCallback(() => CharacterMove());              //캐릭터들의 움직임을 실행한다

    }

    public void CharacterMove()
    {
        Sequence seq = DOTween.Sequence();
        float startVal = 0.0f;
        float endVal = 1.0f;

        seq.AppendCallback(() =>
        {   
            teacherAni.SetTrigger("moveTrigger");   // 선생님의 무브블랜드를 켜준다
            Tween floatTween = DOTween.To(() => startVal, x => startVal = x, endVal, 1.0f).SetUpdate(true)  // startVal을 1로 만드는걸 DOTween.To 를써서 보간시켜 1로 만들어주는 트윈
            .OnUpdate(() => teacherAni.SetFloat("moveSpeed", startVal));

            Sequence seq01 = DOTween.Sequence();    // 새로운 시퀀스를 만든다
            seq01.Append(floatTween);               // 보간을 넣어준다
            seq01.Join(teacherTr.DORotate(new Vector3(0f, 90f, 0f), 0.5f));     // 동시에 움직인다
            seq01.Join(teacherTr.DOMove(targetTr[4].position, 5.0f));  // 동시에 회전한다
        });
        seq.AppendInterval(4.0f);
        seq.AppendCallback(() =>
        {
            endVal = 0.0f;  //endVal 을다시 0으로 초기화시켜 선생님이 걷는걸 멈추게한다
            Tween floatTween = DOTween.To(() => startVal, x => startVal = x, endVal, 1.0f).SetUpdate(true)
            .OnUpdate(() => teacherAni.SetFloat("moveSpeed", startVal));
        });
        seq.AppendInterval(1.0f);
        seq.Append(teacherTr.DORotate(new Vector3(0f, -90f, 0f), 1.5f));
        seq.AppendCallback(() =>
        {
            teacherAni.SetTrigger("TalkTrigger");           // 줄을 서라는 선생님의 지시 애니메이션 실행
            teacherSource.PlayOneShot(teacherGuid03, 1.0f); // 줄을 서라는 선생님의 지시 오디오 실행
        });
        seq.AppendInterval(7.0f);
        seq.AppendCallback(() =>
        {
            teacherAni.SetTrigger("moveTrigger");           // 말이끝나면 다시 Idle 상태로 바꿔준다
            endVal = 1.0f;                                  // 아이들의 움직임을 위해 endVal = 1.0으로 초기화 한다
            Sequence seq01 = DOTween.Sequence(); 

            Tween lerpTween = DOTween.To(() => startVal, x => startVal = x, endVal, 1.0f)
                .SetUpdate(true)
                .OnUpdate(() =>
                {
                    kidAnimations[0].SetFloat("moveSpeed", startVal);
                    kidAnimations[1].SetFloat("moveSpeed", startVal);
                    kidAnimations[2].SetFloat("moveSpeed", startVal);

                });

            seq01.Append(lerpTween);
            seq01.Join(kidsTr[0].DORotate(new Vector3(0f, 90f, 0f), 5.0f));
            seq01.Join(kidsTr[1].DORotate(new Vector3(0f, 90f, 0f), 5.0f));
            seq01.Join(kidsTr[2].DORotate(new Vector3(0f, 90f, 0f), 5.0f));

            seq01.Join(kidsTr[0].DOMove(targetTr[3].position, 5.0f));
            seq01.Join(kidsTr[1].DOMove(targetTr[2].position, 5.0f));
            seq01.Join(kidsTr[2].DOMove(targetTr[1].position, 5.0f));
            seq01.AppendCallback(() =>
            {
                endVal = 0.0f;
                Tween lerpTween = DOTween.To(() => startVal, x => startVal = x, endVal, 1.0f)
                .SetUpdate(true)
                .OnUpdate(() =>
                {
                    kidAnimations[0].SetFloat("moveSpeed", startVal);
                    kidAnimations[1].SetFloat("moveSpeed", startVal);
                    kidAnimations[2].SetFloat("moveSpeed", startVal);

                });
            });
            seq01.AppendCallback(() => tweenManager.CloseUI(transform, nextStep, 1.0f));    // 창을 닫고 다음 스텝을 여는 트윈 메서드 실행
        });
    }
}
