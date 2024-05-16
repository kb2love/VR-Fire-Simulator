using DG.Tweening;
using UnityEngine;
using VrFireSim;

public class Step03 : MonoBehaviour
{
    [Header("AudioSource")]

    [SerializeField] AudioSource stepSource;   // �����̼��� �÷����� ����� �ҽ�

    [SerializeField] AudioSource[] kidsSource;

    [SerializeField] AudioSource teacherSource;

    [Header("AudioClip")]

    [SerializeField] AudioClip clikClip;        // Ŭ�� ����� Ŭ��

    [SerializeField] AudioClip narration03;     // �����̼� ����� Ŭ��

    [SerializeField] AudioClip teacherGuid04;   // �������� �׹�° ���� Ŭ��

    [SerializeField] AudioClip coughtClip01;    // ��ħ�ϴ� ����� Ŭ��

    [SerializeField] AudioClip coughtClip02;

    [SerializeField] AudioClip coughtClip03;

    [Header("Animator")]

    [SerializeField] Animator[] kidsAni;

    [SerializeField] Animator teacherAni;

    [Header("Transform")]

    [SerializeField] Transform[] kidsTr;    //�ֵ��� ��ġ�� ����Ʈ�� ������

    [SerializeField] Transform[] targetTr;      // �̵��� ��ġ

    [SerializeField] Transform teacherTr;       // �������� ��ġ

    [SerializeField] Transform playerTr;        // �÷��̾��� ��ġ

    [SerializeField] GameObject nextStep;       // ���� �������� �Ѿ�� ���� ������Ʈ
    AudioManager audioManager;                  // ����� �Ŵ����� ����ϱ� ���� ����

    TweenManager tweenManager;                  // Ʈ�� �Ŵ����� ����ϱ� ���� ����


    float startVal = 0.0f;

    float endVal = 1.0f;
    void OnEnable()
    {
        tweenManager = new TweenManager();                  // ��� �����̹��� ��ӹ��� �ʾƵ� �Ǳ� ������ Ŭ�󽺸� ���� ���� �ش�
        tweenManager.PopUpUI(transform);

        audioManager = gameObject.AddComponent<AudioManager>();                        // ���� �����̽��� ���� �� ���� Ŭ����(�������̹����)
        audioManager.NarrationPlay(stepSource, narration03, 1.0f);                    //������ ����� �Ŵ��� Ŭ���� �����̼� �޼��带 ��� �� �� �ִ�

        targetTr = GameObject.Find("TargetPosition").GetComponentsInChildren<Transform>();  
    }
    void OnDisable()
    {
        tweenManager = null;                                // �����ߴ� Ʈ�� �Ŵ����� ����ش�
        audioManager = null;                                // �����ߴ� ����� �Ŵ����� �����ش�
        Destroy(gameObject.GetComponent<AudioManager>());   // �߰��ߴ� ������Ʈ�� ���� �� �ش�
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
        stepSource.PlayOneShot(clikClip, 1.0f);            // �ߺ��� �Ǵ� �ܹ߼� ����� �÷���
        tweenManager.CloseUI(transform);
        Vector3 rot = new Vector3(0, 0, 0);
        Sequence seq = DOTween.Sequence();

        //tweenManager.CloseUI(transform, nextStep, 1.0f);    // â�� �ݰ� ���� ������ ���� Ʈ�� �޼��� ����
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
            Tween floatTween = DOTween.To(() => startVal, x => startVal = x, endVal, 1.0f).SetUpdate(true)  // startVal�� 1�� ����°� DOTween.To ���Ἥ �������� 1�� ������ִ� Ʈ��
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
            Tween floatTween = DOTween.To(() => startVal, x => startVal = x, endVal, 1.0f).SetUpdate(true)  // startVal�� 1�� ����°� DOTween.To ���Ἥ �������� 1�� ������ִ� Ʈ��
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
