using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFireSim;
using DG.Tweening;
public class Step02 : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;   // 나레이션을 플레이할 오디오 소스

    [SerializeField] AudioClip clikClip;        // 클릭 오디오 클립

    [SerializeField] AudioClip narration;     // 나레이션 오디오 클립

    [SerializeField] GameObject nextStep;       // 다음 스탭으로 넘어가기 위한 오브젝트


    [SerializeField] Transform playerTr;

    

    AudioManager audioManager;                  // 오디오 매니저를 사용하기 위한 변수

    TweenManager tweenManager;                  // 트윈 매니저를 사용하기 위한 변수
    void OnEnable()
    {
        tweenManager = new TweenManager();                  // 모노 비헤이버를 상속받지 않아도 되기 때문에 클라스를 생성 시켜 준다
        tweenManager.PopUpUI(transform);

        audioManager = gameObject.AddComponent<AudioManager>();                        // 네임 스페이스로 제작 해 놓은 클래스(모노비헤이버상속)
        audioManager.NarrationPlay(audioSource, narration, 1.0f);                    //생성된 오디오 매니저 클라스의 나레이션 메서드를 사용 할 수 있다

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
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => tweenManager.CloseUI(transform));     // 창을 닫고 다음 스텝을 여는 트윈 메서드 실행
        seq.Append(playerTr.DOMove(new Vector3(-1.0f, playerTr.position.y, -1.5f), 5.0f));  //플레이어를 이동킨다
        seq.Append(playerTr.DORotate(new Vector3(0f, 90f, 0f), 1.0f).OnComplete(() => tweenManager.CloseUI(transform, nextStep, 1.0f))); //플레이어를 회전시키고 다음스탭으로 넘어간다
    }
}
