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
    [SerializeField] AudioClip fireClip;

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

    float startVal = 0.0f;
    float endVal = 1.0f;

    void OnEnable()
    {
        tweenManager = new TweenManager();
        tweenManager.PopUpUI(transform);

        audioManager = gameObject.AddComponent<AudioManager>();
        audioManager.NarrationPlay(stepAudioSource, narration, 1.0f);

        targetTr = GameObject.Find("TargetPosition").GetComponentsInChildren<Transform>();

        
    }

    void OnDisable()
    {
        tweenManager = null;
        audioManager = null;
        Destroy(gameObject.GetComponent<AudioManager>());
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
        teacherSource.PlayOneShot(clikClip, 1.0f);

        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => tweenManager.CloseUI(transform));
        seq.AppendInterval(1.0f);
        seq.AppendCallback(HandleFireWarning);
        seq.AppendInterval(2.0f);
        seq.AppendCallback(() => audioManager.LoopAudioPlay(warningAudioSource, fireWarningClipChange));
        seq.AppendInterval(4.0f);
        seq.AppendCallback(() => PlayTeacherGuidance(teacherGuid01, "IsTalk"));
        seq.AppendInterval(7.0f);
        seq.AppendCallback(() => teacherSource.PlayOneShot(teacherGuid02, 1.0f));
        seq.AppendInterval(7.0f);
        seq.AppendCallback(CharacterMove);
    }

    private void HandleFireWarning()
    {
        audioManager.LoopAudioPlay(warningAudioSource, fireWarningClip);
        teacherAni.SetTrigger("LookAround");
        fire.SetActive(true);

        foreach (var kidAnimation in kidAnimations)
        {
            kidAnimation.SetTrigger("StandUp");
        }

        foreach (var kidTr in kidsTr)
        {
            kidTr.DOMoveZ(1.11f, 1.0f);
        }
    }

    public void CharacterMove()
    {
        Sequence seq = DOTween.Sequence();

        seq.AppendCallback(() => MoveTeacher());
        seq.AppendInterval(4.0f);
        seq.Append(teacherTr.DORotate(new Vector3(0f, -90f, 0f), 1.5f));
        seq.AppendCallback(() => StopCharacter());
        seq.AppendInterval(1.0f);
        seq.AppendCallback(() => PlayTeacherGuidance(teacherGuid03, "IsTalk"));
        seq.AppendInterval(7.0f);
        seq.AppendCallback(() => StartKidsMove(startVal, endVal));
    }

    private void MoveTeacher()
    {
        teacherAni.SetBool("IsTalk", false);

        Tween floatTween = DOTween.To(() => startVal, x => startVal = x, endVal, 1.0f).SetUpdate(true)
            .OnUpdate(() => teacherAni.SetFloat("moveSpeed", startVal));
        Sequence moveSeq = DOTween.Sequence();
        moveSeq.Append(floatTween);
        moveSeq.Join(teacherTr.DORotate(new Vector3(0f, 90f, 0f), 1.0f));
        moveSeq.Join(teacherTr.DOMove(targetTr[4].position, 5.0f));
    }

    private void StopCharacter()
    {
        endVal = 0.0f;
        Tween floatTween = DOTween.To(() => startVal, x => startVal = x, endVal, 1.0f).SetUpdate(true)
            .OnUpdate(() => teacherAni.SetFloat("moveSpeed", startVal));
    }


    private void PlayTeacherGuidance(AudioClip clip, string _bool)
    {
        teacherAni.SetBool(_bool, true);
        teacherSource.PlayOneShot(clip, 1.0f);
    }
    private void StartKidsMove(float startVal, float endVal)
    {
        endVal = 1.0f;
        audioManager.LoopAudioPlay(warningAudioSource, fireClip);
        Sequence kidsSeq = DOTween.Sequence();

        Tween lerpTween = DOTween.To(() => startVal, x => startVal = x, endVal, 1.0f)
            .SetUpdate(true)
            .OnUpdate(() =>
            {
                foreach (var kidAnimation in kidAnimations)
                {
                    kidAnimation.SetFloat("moveSpeed", startVal);
                }
            });

        kidsSeq.Append(lerpTween);

        for (int i = 0; i < kidsTr.Length; i++)
        {
            kidsSeq.Join(kidsTr[i].DORotate(new Vector3(0f, 90f, 0f), 5.0f));
            kidsSeq.Join(kidsTr[i].DOMove(targetTr[3 - i].position, 5.0f));
        }

        kidsSeq.AppendCallback(() => StopKidsMove(startVal, endVal));
        kidsSeq.AppendCallback(() => tweenManager.CloseUI(transform, nextStep, 1.0f));
    }

    private void StopKidsMove(float startVal, float endVal)
    {
        teacherAni.SetBool("IsTalk", false);
        endVal = 0.0f;
        Tween lerpTween = DOTween.To(() => startVal, x => startVal = x, endVal, 1.0f)
            .SetUpdate(true)
            .OnUpdate(() =>
            {
                foreach (var kidAnimation in kidAnimations)
                {
                    kidAnimation.SetFloat("moveSpeed", startVal);
                }
            });
    }
}