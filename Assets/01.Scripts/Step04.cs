using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFireSim;
using DG.Tweening;
public class Step04 : MonoBehaviour
{
    [Header("AudioSource")]
    
    [SerializeField] AudioSource audioSource;   // 나레이션을 플레이할 오디오 소스

    [SerializeField] AudioSource teacherSource; // 선생님의 지시를 플레이할 오디오 소스

    [SerializeField] AudioSource fireSource;    // 불이 꺼지고나서 바뀔 오디오클립을 플레이할 오디오 소스

    [Header("AudioClip")]

    [SerializeField] AudioClip teacherGuid;        // 클릭 오디오 클립

    [SerializeField] AudioClip narration04;     // 나레이션 오디오 클립

    [SerializeField] AudioClip fireClip;     // 추가적인 나레이션 오디오 클립

    [SerializeField] AudioClip littleFireClip;  // 불이 꺼지고나서 바뀔 오디오 클립

    [Header("Transform")]

    [SerializeField] Transform playerTr;

    [SerializeField] Transform arrowImage;      // 소화기가 있는 위치를 알려줄 화살표 Image

    [SerializeField] Transform playerFirePos;   // 플레이어가 소화기를 쏠 위치

    [Header("GameObject")]

    [SerializeField] GameObject playerFire;     // 플레이어의 손에있는 소화기

    [SerializeField] GameObject fireextinguisher;   // 바닥에 설치되어있는 소화기

    [SerializeField] GameObject fire;           // 불이 완전히 끄게 할 오브젝트

    [SerializeField] GameObject nextStep;       // 다음 스탭으로 넘어가기 위한 오브젝트

    [SerializeField] GameObject digestion;      // 소화기가 내뿜을 소화제 파티클 오브젝트

    [SerializeField] ParticleSystemRenderer fireParticle;   // 불의 파티클 렌더러의 메테리얼을 가져올 파티클렌더러

    [SerializeField] Animator fireAni;          // 소화기를 움직이게 해줄 애니메이터


    AudioManager audioManager;                  // 오디오 매니저를 사용하기 위한 변수

    TweenManager tweenManager;                  // 트윈 매니저를 사용하기 위한 변수

    Material fireMat;          // 불을 서서히 끄게 해줄 메테리얼
    void OnEnable()
    {
        tweenManager = new TweenManager();                  // 모노 비헤이버를 상속받지 않아도 되기 때문에 클라스를 생성 시켜 준다
        tweenManager.PopUpUI(transform);

        audioManager = gameObject.AddComponent<AudioManager>();                        // 네임 스페이스로 제작 해 놓은 클래스(모노비헤이버상속)
        audioManager.NarrationPlay(audioSource, narration04, 1.0f);                    //생성된 오디오 매니저 클라스의 나레이션 메서드를 사용 할 수 있다
        float originPos = arrowImage.position.y;                    // 화살표를 원래 위치로 돌리기 위한 값
        float downPos = originPos - 0.5f;                           // 화살표를 아래로 내리기 위한 값
        fireMat = fireParticle.material;                            // fireMat 에 파티클렌더러의 메테리얼 값을 저장
        Sequence seq = DOTween.Sequence();                          // 새로운 시퀀스 생성
        seq.Append(arrowImage.DOMoveY(downPos, 1.0f));              // 화살표를 아래로 다운
        seq.Append(arrowImage.DOMoveY(originPos, 1.0f));            // 화살표를 원래위치로 업
        seq.SetLoops(-1);       // 반복
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
        playerFire.SetActive(true);         // 플레이어가 들고있는 소화기를 켜준다
        fireextinguisher.SetActive(false);  // 바닥에있는 소화기를 꺼준다
        Sequence seq = DOTween.Sequence();  // 새로운 시퀀스 생성
        seq.AppendCallback(() =>
        {
            teacherSource.PlayOneShot(teacherGuid, 1.0f);   // 선생님의 지시 오디오 클립을 켜준다
            tweenManager.CloseUI(transform);                // Ui창을 꺼준다
        });
        seq.AppendInterval(5.0f);
        seq.AppendCallback(() =>
        {
            playerTr.DOMove(playerFirePos.position, 1.5f);  // 플레이어의 위치를 불이 있는곳으로 옮긴다
            playerTr.DORotate(Vector3.zero, 1.0f);          // 플레이어의 방향을 불이 있는곳으로 향한다
        });
        seq.AppendCallback(() => fireAni.SetTrigger("OpenTrigger"));    // 소화기를 사용하는 애니메이션을 활성화한다
        seq.AppendInterval(2.0f);
        seq.AppendCallback(() =>
        {
            digestion.SetActive(true);          // 소화제 파티클을 킨다
            audioSource.PlayOneShot(fireClip, 1.0f);    //소화기 뿌리는 소리를 킨다
            Color color = fireMat.GetColor("_Color");   // fireMat의 
            DOTween.To(() => color.a, x => {
                color.a = x;
                fireMat.SetColor("_Color", color);
            }, 0, 6.0f);
        });
        seq.AppendInterval(6.0f);
        seq.AppendCallback(() =>
        {
            fireAni.SetTrigger("CloseTrigger");
            digestion.SetActive(false);
            fire.SetActive(false);
            audioManager.LoopAudioPlay(fireSource, littleFireClip);
        });
        seq.AppendInterval(1.0f);
        seq.AppendCallback(() => tweenManager.CloseUI(transform, nextStep, 1.0f));

    }
}
