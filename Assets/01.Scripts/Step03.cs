using DG.Tweening;
using UnityEngine;
using VrFireSim;

public class Step03 : MonoBehaviour
{
    [Header("AudioSource")]

    [SerializeField] AudioSource stepSource;   // 나레이션을 플레이할 오디오 소스

    [SerializeField] AudioSource[] kidsSource;

    [SerializeField] AudioSource teacherSource;

    [Header("AudioClip")]

    [SerializeField] AudioClip clikClip;        // 클릭 오디오 클립

    [SerializeField] AudioClip narration03;     // 나레이션 오디오 클립

    [SerializeField] AudioClip teacherGuid04;   // 선생님의 네번째 지시 클립

    [SerializeField] AudioClip coughtClip01;    // 기침하는 오디오 클립

    [SerializeField] AudioClip coughtClip02;

    [SerializeField] AudioClip coughtClip03;

    [Header("Animator")]

    [SerializeField] Animator[] kidsAni;

    [SerializeField] Animator teacherAni;

    [Header("Transform")]

    [SerializeField] Transform[] kidsTr;    //애들의 위치를 리스트로 가져옴

    [SerializeField] Transform[] targetTr;      // 이동할 위치

    [SerializeField] Transform teacherTr;       // 선생님의 위치

    [SerializeField] Transform playerTr;        // 플레이어의 위치

    [SerializeField] GameObject nextStep;       // 다음 스탭으로 넘어가기 위한 오브젝트
    AudioManager audioManager;                  // 오디오 매니저를 사용하기 위한 변수

    TweenManager tweenManager;                  // 트윈 매니저를 사용하기 위한 변수


    float startVal = 0.0f;

    float endVal = 1.0f;
    void OnEnable()
    {
        tweenManager = new TweenManager();                  // 모노 비헤이버를 상속받지 않아도 되기 때문에 클라스를 생성 시켜 준다
        tweenManager.PopUpUI(transform);

        audioManager = gameObject.AddComponent<AudioManager>();                        // 네임 스페이스로 제작 해 놓은 클래스(모노비헤이버상속)
        audioManager.NarrationPlay(stepSource, narration03, 1.0f);                    //생성된 오디오 매니저 클라스의 나레이션 메서드를 사용 할 수 있다

        targetTr = GameObject.Find("TargetPosition").GetComponentsInChildren<Transform>();  
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
        stepSource.PlayOneShot(clikClip, 1.0f);            // 중복이 되는 단발성 오디오 플레이
        tweenManager.CloseUI(transform);
        Vector3 rot = new Vector3(0, 0, 0);
        Sequence seq = DOTween.Sequence();

        //tweenManager.CloseUI(transform, nextStep, 1.0f);    // 창을 닫고 다음 스텝을 여는 트윈 메서드 실행
        seq.AppendInterval(0.5f);
        seq.AppendCallback(() =>
        {
            teacherAni.SetTrigger("TalkTrigger");
            teacherSource.PlayOneShot(teacherGuid04, 1.0f);
        });
        seq.AppendInterval(4.0f);
        charactermove();
    }

    private void charactermove()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(teacherTr.DORotate(new Vector3(0, 90f, 0), 1.5f));
        seq.AppendCallback(() =>
        {
            teacherAni.SetTrigger("moveTrigger");
            Tween floatTween = DOTween.To(() => startVal, x => startVal = x, endVal, 1.0f).SetUpdate(true)  // startVal을 1로 만드는걸 DOTween.To 를써서 보간시켜 1로 만들어주는 트윈
            .OnUpdate(() =>
            {
                teacherAni.SetFloat("moveSpeed", startVal);
                kidsAni[0].SetFloat("moveSpeed", startVal);
                kidsAni[1].SetFloat("moveSpeed", startVal);
                kidsAni[2].SetFloat("moveSpeed", startVal);
            });
            teacherTr.DOMoveX(6.5f, 1.0f);
            kidsTr[0].DOMoveX(5.0f, 1.0f);
            kidsTr[1].DOMoveX(3.5f, 1.0f);
            kidsTr[2].DOMoveX(2.0f, 1.0f);
            playerTr.DOMoveX(0.5f, 1.0f);

            teacherTr.DOMove(targetTr[6].position ,1.0f);   
            kidsTr[0].DOMove(targetTr[5].position, 1.0f);
            kidsTr[1].DOMove(targetTr[4].position, 1.0f);
            kidsTr[2].DOMove(targetTr[3].position, 1.0f);
            playerTr.DOMove(targetTr[2].position, 1.0f);
        });
        seq.AppendInterval(1.0f);
        seq.AppendCallback(() =>
        {
            teacherTr.DORotate(Vector3.zero, 1.0f);
            teacherTr.DOMove(targetTr[7].position, 1.0f);
            kidsTr[0].DOMove(targetTr[6].position, 1.0f);
            kidsTr[1].DOMove(targetTr[5].position, 1.0f);
            kidsTr[2].DOMove(targetTr[4].position, 1.0f);
            playerTr.DOMove(targetTr[3].position, 1.0f);
        });
        seq.AppendInterval(1.0f);
        seq.AppendCallback(() =>
        {
            kidsTr[0].DORotate(Vector3.zero, 1.0f);
            kidsTr[0].DOMove(targetTr[8].position, 1.0f);
            kidsTr[1].DOMove(targetTr[6].position, 1.0f);
            kidsTr[2].DOMove(targetTr[5].position, 1.0f);
            playerTr.DOMove(targetTr[4].position, 1.0f);
        });
        seq.AppendInterval(1.0f);
        seq.AppendCallback(() =>
        {

            kidsTr[1].DORotate(Vector3.zero, 1.0f);
            kidsTr[1].DOMove(targetTr[9].position, 1.0f);
            kidsTr[2].DOMove(targetTr[6].position, 1.0f);
            playerTr.DOMove(targetTr[5].position, 1.0f);
        });
        seq.AppendInterval(1.0f);
        seq.AppendCallback(() =>
        {
            kidsTr[2].DORotate(Vector3.zero, 1.0f);
            kidsTr[2].DOMove(targetTr[10].position, 1.0f);
            playerTr.DOMove(targetTr[6].position, 1.0f);
        });
        
        seq.AppendInterval(1.0f);
        seq.AppendCallback(() =>
        {
            playerTr.DOMove(targetTr[11].position, 1.0f);
            endVal = 0.0f;
            Tween floatTween = DOTween.To(() => startVal, x => startVal = x, endVal, 1.0f).SetUpdate(true)  // startVal을 1로 만드는걸 DOTween.To 를써서 보간시켜 1로 만들어주는 트윈
            .OnUpdate(() =>
            {
                teacherAni.SetFloat("moveSpeed", startVal);
                kidsAni[0].SetFloat("moveSpeed", startVal);
                kidsAni[1].SetFloat("moveSpeed", startVal);
                kidsAni[2].SetFloat("moveSpeed", startVal);
            });
        });
    }
}
