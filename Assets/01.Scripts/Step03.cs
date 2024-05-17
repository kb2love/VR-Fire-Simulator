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

    [Header("Animator")]
    [SerializeField] Animator[] kidsAni;
    [SerializeField] Animator teacherAni;

    [Header("Transform")]
    [SerializeField] Transform[] kidsTr;    //애들의 위치를 리스트로 가져옴
    [SerializeField] Transform[] targetTr;  // 이동할 위치
    [SerializeField] Transform teacherTr;   // 선생님의 위치
    [SerializeField] Transform playerTr;    // 플레이어의 위치
    [SerializeField] GameObject nextStep;   // 다음 스탭으로 넘어가기 위한 오브젝트

    AudioManager audioManager;              // 오디오 매니저를 사용하기 위한 변수
    TweenManager tweenManager;              // 트윈 매니저를 사용하기 위한 변수

    float startVal = 0.0f;
    float endVal = 1.0f;
    float timeTp = 0.5f;
    void OnEnable()
    {
        tweenManager = new TweenManager();
        tweenManager.PopUpUI(transform);

        audioManager = gameObject.AddComponent<AudioManager>();
        audioManager.NarrationPlay(stepSource, narration03, 1.0f);

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
        if (other.gameObject.CompareTag("LeftCol") || other.gameObject.CompareTag("RightCol"))
        {
            OnClickMethods();
        }
    }

    public void OnClickMethods()
    {
        stepSource.PlayOneShot(clikClip, 1.0f);
        tweenManager.CloseUI(transform);

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f)
           .AppendCallback(() =>
           {
               teacherAni.SetTrigger("TalkTrigger");
               teacherSource.PlayOneShot(teacherGuid04, 1.0f);
           })
           .AppendInterval(4.0f)
           .AppendCallback(() => CharacterMove());
    }

    private void CharacterMove()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(teacherTr.DORotate(new Vector3(0, 90f, 0), 1.5f))
           .AppendCallback(() => StartMove())
           .AppendInterval(timeTp)
           .AppendCallback(() => MoveCharacters(targetTr[7], targetTr[6], targetTr[5], targetTr[4], targetTr[3]))
           .AppendInterval(timeTp)
           .AppendCallback(() => SeconMove())
           .AppendInterval(timeTp)
           .AppendCallback(() => ThirdMove())
           .AppendInterval(timeTp)
           .AppendCallback(() => FinalMove())
           .AppendInterval(timeTp)
           .AppendCallback(() => EndMove());
    }

    private void StartMove()            
    {
        teacherAni.SetTrigger("moveTrigger");
        Tween floatTween = DOTween.To(() => startVal, x => startVal = x, endVal, 1.0f).SetUpdate(true)
            .OnUpdate(() =>
            {
                teacherAni.SetFloat("moveSpeed", startVal);
                foreach (var kidAni in kidsAni)
                {
                    kidAni.SetFloat("moveSpeed", startVal);
                }
            });

        MoveCharacters(targetTr[6], targetTr[5], targetTr[4], targetTr[3], targetTr[2]);
    }
    private void SeconMove()
    {
        kidsTr[0].DOMove(targetTr[8].position, 1.0f);
        kidsTr[1].DOMove(targetTr[6].position, 1.0f);
        kidsTr[2].DOMove(targetTr[5].position, 1.0f);
        playerTr.DOMove(targetTr[4].position, 1.0f);
    }
    private void MoveCharacters(Transform teacherTarget, Transform kid1Target, Transform kid2Target, Transform kid3Target, Transform playerTarget)
    {
        teacherTr.DOMove(teacherTarget.position, 1.0f);
        kidsTr[0].DOMove(kid1Target.position, 1.0f);
        kidsTr[1].DOMove(kid2Target.position, 1.0f);
        kidsTr[2].DOMove(kid3Target.position, 1.0f);
        playerTr.DOMove(playerTarget.position, 1.0f);
    }

    private void ThirdMove()
    {
        kidsTr[1].DOMove(targetTr[9].position, 1.0f);
        kidsTr[2].DOMove(targetTr[6].position, 1.0f);
        playerTr.DOMove(targetTr[5].position, 1.0f);
    }
    private void FinalMove()
    {
        teacherTr.DORotate(Vector3.zero, 1.0f);
        kidsTr[2].DOMove(targetTr[10].position, 1.0f);
        playerTr.DOMove(targetTr[6].position, 1.0f);
    }

    private void EndMove()
    {
        playerTr.DOMove(targetTr[11].position, 1.0f);
        endVal = 0.0f;
        Tween floatTween = DOTween.To(() => startVal, x => startVal = x, endVal, 1.0f).SetUpdate(true)
            .OnUpdate(() =>
            {
                teacherAni.SetFloat("moveSpeed", startVal);
                foreach (var kidAni in kidsAni)
                {
                    kidAni.SetFloat("moveSpeed", startVal);
                }
            });
    }
}