using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VrFireSim;

public class Step05 : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;   // 나레이션을 플레이할 오디오 소스

    [Header("AudioClip")]

    [SerializeField] AudioClip clikClip;        // 클릭 오디오 클립

    [SerializeField] AudioClip narration01;     // 나레이션 오디오 클립

    [Header("Transform")]
    [SerializeField] Transform[] kidsTr;    //애들의 위치를 리스트로 가져옴
    [SerializeField] Transform targetTr;  // 이동할 위치
    [SerializeField] Transform teacherTr;   // 선생님의 위치
    [SerializeField] Transform playerTr;    // 플레이어의 위치

    [Header("Animator")]
    [SerializeField] Animator[] kidsAni;
    [SerializeField] Animator teacherAni;

    [SerializeField] Image fadeImage;
    [SerializeField] GameObject nextStep;   // 다음 스탭으로 넘어가기 위한 오브젝트
    float startVal = 0.0f;
    float endVal = 1.0f;

    AudioManager audioManager;                  // 오디오 매니저를 사용하기 위한 변수

    TweenManager tweenManager;                  // 트윈 매니저를 사용하기 위한 변수
    void OnEnable()
    {
        tweenManager = new TweenManager();                  // 모노 비헤이버를 상속받지 않아도 되기 때문에 클라스를 생성 시켜 준다
        tweenManager.PopUpUI(transform);

        audioManager = gameObject.AddComponent<AudioManager>();                        // 네임 스페이스로 제작 해 놓은 클래스(모노비헤이버상속)
        audioManager.NarrationPlay(audioSource, narration01, 1.0f);                    //생성된 오디오 매니저 클라스의 나레이션 메서드를 사용 할 수 있다

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
        audioSource.PlayOneShot(clikClip, 1.0f);            // 중복이 되는 단발성 오디오 플레이

        tweenManager.CloseUI(transform);    // 창을 닫고 다음 스텝을 여는 트윈 메서드 실행
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            playerTr.DOMove(targetTr.position, 5.0f);
            StartMove();
        });
        seq.AppendInterval(1.0f);
        seq.Append(fadeImage.DOFade(1, 2.0f).OnComplete(() =>
        {
            DOTween.KillAll();
            playerTr.position = new Vector3(0, 0, 1.664f);
            playerTr.localEulerAngles = new Vector3(0, 180, 0);
        }));
        seq.Append(fadeImage.DOFade(0, 2.0f).OnComplete(() => tweenManager.CloseUI(transform, nextStep, 1.0f)));
        
    }
    private void StartMove()
    {
        teacherAni.SetBool("IsTalk", false);
        Tween floatTween = DOTween.To(() => startVal, x => startVal = x, endVal, 1.0f).SetUpdate(true)
            .OnUpdate(() =>
            {
                teacherAni.SetFloat("moveSpeed", startVal);
                foreach (var kidAni in kidsAni)
                {
                    kidAni.SetFloat("moveSpeed", startVal);
                }
            });

        MoveCharacters();
    }
    private void MoveCharacters()
    {
        teacherTr.DOMove(targetTr.position, 10.0f);
        kidsTr[0].DOMove(targetTr.position, 10.0f);
        kidsTr[1].DOMove(targetTr.position, 10.0f);
        kidsTr[2].DOMove(targetTr.position, 10.0f);
    }

}
