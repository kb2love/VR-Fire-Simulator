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

    [SerializeField] AudioClip fireWarningClip;     //화재경고음 클립

    [SerializeField] AudioClip clikClip;            // 클릭 오디오 클립

    [SerializeField] GameObject nextStep;           // 다음 스탭으로 넘어가기 위한 오브젝트

    [Header("Animator")]

    [SerializeField] Animator teacherAni;           // 선생님의 애니메이터

    [SerializeField] Animator kidAni01;             // 아이 1의 애니메이터

    [SerializeField] Animator kidAni02;

    [SerializeField] Animator kidAni03;

    [Header("Character")]

    [SerializeField] Transform player;              // 플레이어를 이동시킬 트랜스폼

    [SerializeField] Transform teacher;             // 선생님을 이동시킬 트랜스폼

    [SerializeField] Transform kid01;               //아이 1을 움직일 트랜스폼

    [SerializeField] Transform kid02;

    [SerializeField] Transform kid03;

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

            kidAni01.SetTrigger("StandUp");
            kidAni02.SetTrigger("StandUp");
            kidAni03.SetTrigger("StandUp");

            kid01.DOMoveZ(1.11f, 1.0f);
            kid02.DOMoveZ(1.11f, 1.0f);
            kid03.DOMoveZ(1.11f, 1.0f);
        });
        seq.AppendInterval(2.0f);
        seq.AppendCallback(() => FireWarningClipChange());  //화재경고음의 시작부분을 바꿀 시퀀스
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
        seq.AppendCallback(() => teacherSource.PlayOneShot(teacherGuid03, 1.0f));       //선생님의 3번째 지시 오디오 실행
        seq.AppendInterval(8.0f);
        // 다음 단계로 이동하기 위한 콜백 함수를 추가합니다.
        seq.AppendCallback(() => CharacterMove());

        // 시퀀스를 시작합니다.
        seq.Play();
    }

    private void CharacterMove()
    {
        Sequence seq01 = DOTween.Sequence();
        seq01.Append(teacher.DORotate(new Vector3(0f, 90f, 0f), 0.5f));
        seq01.Join(teacher.DOMove(new Vector3(4.9f, 0.129f, -1.45f), 2.0f));
        seq01.Append(teacher.DORotate(new Vector3(0f, -90f, 0f), 2f));
        Sequence seq02 = DOTween.Sequence();
        seq02.Append(kid01.DORotate(new Vector3(0f, 90f, 0f), 2.0f));
        seq02.Join(kid01.DOMoveZ(2f, 5f));
    }
    private void FireWarningClipChange()
    {
        float startTime = 2.0f;

        // 오디오 클립의 새로운 길이 (시작 시간부터 끝까지)
        int samplesToCopy = fireWarningClip.samples - (int)(startTime * fireWarningClip.frequency);

        // 새로운 AudioClip을 생성하고 오디오 데이터를 복사합니다.
        AudioClip newClip = AudioClip.Create("NewClip", samplesToCopy, fireWarningClip.channels, fireWarningClip.frequency, false);
        float[] data = new float[samplesToCopy * fireWarningClip.channels];
        fireWarningClip.GetData(data, (int)(startTime * fireWarningClip.frequency));
        newClip.SetData(data, 0);

        // 새로운 AudioClip을 AudioSource에 할당하여 재생합니다.
        warningAudioSource.clip = newClip; 
        audioManager.FireWarningPlay(warningAudioSource, newClip);
    }
}
