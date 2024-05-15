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

    [SerializeField] GameObject nextStep;           // 다음 스탭으로 넘어가기 위한 오브젝트

    [Header("Animator")]

    [SerializeField] Animator teacherAni;           // 선생님의 애니메이터

    [SerializeField] List<Animator> kidAnimations = new List<Animator>();

    [Header("Character")]

    [SerializeField] Transform playerTr;              // 플레이어를 이동시킬 트랜스폼

    [SerializeField] Transform teacherTr;             // 선생님을 이동시킬 트랜스폼

    [SerializeField] List<Transform> kidsTr = new List<Transform>();

    AudioManager audioManager;                  // 오디오 매니저를 사용하기 위한 변수

    TweenManager tweenManager;                  // 트윈 매니저를 사용하기 위한 변수
    void OnEnable()
    {
        tweenManager = new TweenManager();                      // 모노 비헤이버를 상속받지 않아도 되기 때문에 클라스를 생성 시켜 준다
        tweenManager.PopUpUI(transform);

        audioManager = gameObject.AddComponent<AudioManager>(); // 네임 스페이스로 제작 해 놓은 클래스(모노비헤이버상속)
        audioManager.NarrationPlay(stepAudioSource, narration, 1.0f);      //생성된 오디오 매니저 클라스의 나레이션 메서드를 사용 할 수 있다


        teacherAni.SetTrigger("TalikTrigger");
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
        // 창을 닫는 트윈 메서드를 추가합니다.
        seq.AppendCallback(() => tweenManager.CloseUI(transform));
        seq.AppendInterval(1.0f);
        seq.AppendCallback(() =>
        {
            Debug.Log("asd");
            // 화재 경고음 재생과 선생님 및 학생 애니메이션 실행
            audioManager.FireWarningPlay(warningAudioSource, fireWarningClip);
            teacherAni.SetTrigger("IsSurprise");

            for (int i = 0; i < kidAnimations.Count; i++)
            {
                kidAnimations[i].SetTrigger("StandUp");
                kidsTr[i].DOMoveZ(1.11f, 1.0f);
            }
        });
        seq.AppendInterval(2.0f);
        seq.AppendCallback(() => warningAudioSource.clip = fireWarningClipChange);  //화재경고음의 시작부분을 바꿀 시퀀스
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
        // 다음 단계로 이동하기 위한 콜백 함수를 추가합니다.
        seq.AppendCallback(() => CharacterMove());

        // 시퀀스를 시작합니다.
        seq.Play();
    }

    public void CharacterMove()
    {
        Sequence seq = DOTween.Sequence();
        float startVal = 0.0f;
        float endVal = 1.0f;

        seq.AppendCallback(() =>
        {
            teacherAni.SetTrigger("moveTrigger");
            Tween floatTween = DOTween.To(() => startVal, x => startVal = x, endVal, 1.0f).SetUpdate(true)
            .OnUpdate(() => teacherAni.SetFloat("moveSpeed", startVal));

            Sequence seq01 = DOTween.Sequence();
            seq01.Append(floatTween);
            seq01.Join(teacherTr.DORotate(new Vector3(0f, 90f, 0f), 0.5f));
            seq01.Join(teacherTr.DOMove(new Vector3(5f, 0.05f, -1.5f), 5.0f));
        });
        seq.AppendInterval(1.0f);
        seq.AppendCallback(() =>
        {
            Tween floatTween = DOTween.To(() => startVal, x => startVal = x, endVal, 0.0f).SetUpdate(true)
            .OnUpdate(() => teacherAni.SetFloat("moveSpeed", startVal));
        });
        seq.AppendInterval(1.0f);
        seq.Append(teacherTr.DORotate(new Vector3(0f, -90f, 0f), 1.5f));
        seq.AppendCallback(() =>
        {
            teacherAni.SetTrigger("TalkTrigger");
            teacherSource.PlayOneShot(teacherGuid03, 1.0f);
        });
        seq.AppendInterval(7.0f);
        seq.AppendCallback(() =>
        {
            teacherAni.SetFloat("moveSpeed", 0.0f);
            teacherAni.SetTrigger("moveTrigger");
            startVal = 0.0f;
            endVal = 1.0f;
            Sequence seq01 = DOTween.Sequence();
            for (int i = 0; i < kidAnimations.Count; i++)
            {
                Tween floatTween = DOTween.To(() => startVal, x => startVal = x, endVal, 1.0f).SetUpdate(true)
                .OnUpdate(() => kidAnimations[i].SetFloat("moveSpeed", startVal));
                seq01.Append(floatTween);
                seq01.Join(kidsTr[i].DORotate(new Vector3(0f, 90f, 0f), 5.0f));
            }

            seq01.AppendCallback(() =>
            {
                kidsTr[0].DOMove(new Vector3(3.5f, 0.05f, -1.5f), 5.0f);
                kidsTr[1].DOMove(new Vector3(2.0f, 0.05f, -1.5f), 5.0f);
                kidsTr[2].DOMove(new Vector3(0.5f, 0.05f, -1.5f), 5.0f);
            });

            seq01.AppendCallback(() =>tweenManager.CloseUI(transform, nextStep, 1.0f));    // 창을 닫고 다음 스텝을 여는 트윈 메서드 실행
        });
    }
}
