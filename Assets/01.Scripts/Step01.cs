using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFireSim;

public class Step01 : MonoBehaviour
{
    [Header("AudioSource")]
    [SerializeField] AudioSource teacherSource;     // �������� ���ø� �÷����� ����� �ҽ�

    [SerializeField] AudioSource stepAudioSource;   // �����̼� ����� �ҽ�

    [SerializeField] AudioSource warningAudioSource;    //ȭ�� ������� �︱ ����� �ҽ�

    [Header("AudioClip")]

    [SerializeField] AudioClip narration;           // �����̼� ����� Ŭ��

    [SerializeField] AudioClip teacherGuid01;       // �������� ���� ����� Ŭ�� 01

    [SerializeField] AudioClip teacherGuid02;

    [SerializeField] AudioClip teacherGuid03;

    [SerializeField] AudioClip fireWarningClip;         // ȭ������ Ŭ��

    [SerializeField] AudioClip fireWarningClipChange;   // ȭ�� ������� �ݺ���Ű�� ���� Ŭ�� 

    [SerializeField] AudioClip clikClip;            // Ŭ�� ����� Ŭ��

    [SerializeField] GameObject nextStep;           // ���� �������� �Ѿ�� ���� ������Ʈ

    [Header("Animator")]

    [SerializeField] Animator teacherAni;           // �������� �ִϸ�����

    [SerializeField] List<Animator> kidAnimations = new List<Animator>();

    [Header("Character")]

    [SerializeField] Transform playerTr;              // �÷��̾ �̵���ų Ʈ������

    [SerializeField] Transform teacherTr;             // �������� �̵���ų Ʈ������

    [SerializeField] List<Transform> kidsTr = new List<Transform>();

    AudioManager audioManager;                  // ����� �Ŵ����� ����ϱ� ���� ����

    TweenManager tweenManager;                  // Ʈ�� �Ŵ����� ����ϱ� ���� ����
    void OnEnable()
    {
        tweenManager = new TweenManager();                      // ��� �����̹��� ��ӹ��� �ʾƵ� �Ǳ� ������ Ŭ�󽺸� ���� ���� �ش�
        tweenManager.PopUpUI(transform);

        audioManager = gameObject.AddComponent<AudioManager>(); // ���� �����̽��� ���� �� ���� Ŭ����(�������̹����)
        audioManager.NarrationPlay(stepAudioSource, narration, 1.0f);      //������ ����� �Ŵ��� Ŭ���� �����̼� �޼��带 ��� �� �� �ִ�


        teacherAni.SetTrigger("TalikTrigger");
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
        teacherSource.PlayOneShot(clikClip, 1.0f);            // �ߺ��� �Ǵ� �ܹ߼� ����� �÷���
        Sequence seq = DOTween.Sequence();
        // â�� �ݴ� Ʈ�� �޼��带 �߰��մϴ�.
        seq.AppendCallback(() => tweenManager.CloseUI(transform));
        seq.AppendInterval(1.0f);
        seq.AppendCallback(() =>
        {
            Debug.Log("asd");
            // ȭ�� ����� ����� ������ �� �л� �ִϸ��̼� ����
            audioManager.FireWarningPlay(warningAudioSource, fireWarningClip);
            teacherAni.SetTrigger("IsSurprise");

            for (int i = 0; i < kidAnimations.Count; i++)
            {
                kidAnimations[i].SetTrigger("StandUp");
                kidsTr[i].DOMoveZ(1.11f, 1.0f);
            }
        });
        seq.AppendInterval(2.0f);
        seq.AppendCallback(() => warningAudioSource.clip = fireWarningClipChange);  //ȭ�������� ���ۺκ��� �ٲ� ������
        seq.AppendInterval(4.0f);
        seq.AppendCallback(() =>
        {
            // ��ȭ �ִϸ��̼� ���� �� �������� ���� ����� ���
            teacherAni.SetTrigger("TalkTrigger");
            // �������� ���� ������� ����մϴ�.
            teacherSource.PlayOneShot(teacherGuid01, 1.0f);
        });
        seq.AppendInterval(7.0f);
        seq.AppendCallback(() => teacherSource.PlayOneShot(teacherGuid02, 1.0f));       //�������� 2��° ���� ����� ����
        seq.AppendInterval(7.0f);
        // ���� �ܰ�� �̵��ϱ� ���� �ݹ� �Լ��� �߰��մϴ�.
        seq.AppendCallback(() => CharacterMove());

        // �������� �����մϴ�.
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

            seq01.AppendCallback(() =>tweenManager.CloseUI(transform, nextStep, 1.0f));    // â�� �ݰ� ���� ������ ���� Ʈ�� �޼��� ����
        });
    }
}
