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

    [Header("AudioClip")]

    [SerializeField] AudioClip teacherGuid;        // 클릭 오디오 클립

    [SerializeField] AudioClip narration04;     // 나레이션 오디오 클립

    [SerializeField] AudioClip fireClip;     // 추가적인 나레이션 오디오 클립
    

    [Header("Transform")]

    [SerializeField] Transform playerTr;

    [SerializeField] Transform arrowImage;      // 소화기가 있는 위치를 알려줄 화살표 Image

    [SerializeField] Transform playerFirePos;   // 플레이어가 소화기를 쏠 위치

    [Header("GameObject")]

    [SerializeField] GameObject playerFire;     // 플레이어의 손에있는 소화기

    [SerializeField] GameObject fireextinguisher;   // 바닥에 설치되어있는 소화기

    [SerializeField] GameObject fire;           // 불

    [SerializeField] GameObject nextStep;       // 다음 스탭으로 넘어가기 위한 오브젝트

    [SerializeField] GameObject digestion;      // 소화기가 내뿜을 소화제 파티클

    [SerializeField] Animator fireAni;          // 소화기를 움직이게 해줄 애니메이터


    AudioManager audioManager;                  // 오디오 매니저를 사용하기 위한 변수

    TweenManager tweenManager;                  // 트윈 매니저를 사용하기 위한 변수
    void OnEnable()
    {
        tweenManager = new TweenManager();                  // 모노 비헤이버를 상속받지 않아도 되기 때문에 클라스를 생성 시켜 준다
        tweenManager.PopUpUI(transform);

        audioManager = gameObject.AddComponent<AudioManager>();                        // 네임 스페이스로 제작 해 놓은 클래스(모노비헤이버상속)
        audioManager.NarrationPlay(audioSource, narration04, 1.0f);                    //생성된 오디오 매니저 클라스의 나레이션 메서드를 사용 할 수 있다
        float originPos = arrowImage.position.y;
        float downPos = originPos - 0.5f;
        Sequence seq = DOTween.Sequence();
        seq.Append(arrowImage.DOMoveY(downPos, 1.0f));
        seq.Append(arrowImage.DOMoveY(originPos, 1.0f));
        seq.SetLoops(-1);
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
        playerFire.SetActive(true);
        fireextinguisher.SetActive(false);
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            teacherSource.PlayOneShot(teacherGuid, 1.0f);
            tweenManager.CloseUI(transform);
        });
        seq.AppendInterval(5.0f);
        seq.AppendCallback(() =>
        {
            playerTr.DOMove(playerFirePos.position, 1.5f);
            playerTr.DORotate(Vector3.zero, 1.0f);
        });
        seq.AppendCallback(() =>
        {
            fireAni.SetTrigger("OpenTrigger");
        });
        seq.AppendInterval(2.0f);
        seq.AppendCallback(() =>
        {
            digestion.SetActive(true);
            audioSource.PlayOneShot(fireClip, 1.0f);
        });
        seq.AppendInterval(6.0f);
        seq.AppendCallback(() =>
        {
            fireAni.SetTrigger("CloseTrigger");
            digestion.SetActive(false);
            fire.SetActive(false);
        });

    }
}
