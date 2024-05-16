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


    [Header("Animator")]

    [SerializeField] Animator teacherAni;           // �������� �ִϸ�����

    [SerializeField] Animator[] kidAnimations;

    [Header("Transform")]

    [SerializeField] Transform playerTr;              // �÷��̾ �̵���ų Ʈ������

    [SerializeField] Transform teacherTr;             // �������� �̵���ų Ʈ������

    [SerializeField] Transform[] kidsTr;

    [SerializeField] Transform[] targetTr;      // �̵��� ��ġ

    [SerializeField] GameObject nextStep;           // ���� �������� �Ѿ�� ���� ������Ʈ

    [SerializeField] GameObject fire;

    AudioManager audioManager;                  // ����� �Ŵ����� ����ϱ� ���� ����

    TweenManager tweenManager;                  // Ʈ�� �Ŵ����� ����ϱ� ���� ����
    void OnEnable()
    {
        tweenManager = new TweenManager();                      // ��� �����̹��� ��ӹ��� �ʾƵ� �Ǳ� ������ Ŭ�󽺸� ���� ���� �ش�
        tweenManager.PopUpUI(transform);

        audioManager = gameObject.AddComponent<AudioManager>(); // ���� �����̽��� ���� �� ���� Ŭ����(�������̹����)
        audioManager.NarrationPlay(stepAudioSource, narration, 1.0f);      //������ ����� �Ŵ��� Ŭ���� �����̼� �޼��带 ��� �� �� �ִ�

        targetTr = GameObject.Find("TargetPosition").GetComponentsInChildren<Transform>();

        teacherAni.SetTrigger("TalkTrigger");
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
        seq.AppendCallback(() => tweenManager.CloseUI(transform));  //UI�� �ݴ´�
        seq.AppendInterval(1.0f);
        seq.AppendCallback(() =>
        {
            Debug.Log("asd");
            audioManager.LoopAudioPlay(warningAudioSource, fireWarningClip);  // ȭ�� ����� ���
            teacherAni.SetTrigger("LookAround");            // �������� ���� ���� �˾�æ��
            fire.SetActive(true);                           // ���� ���ش�
            for (int i = 0; i < kidAnimations.Length; i++)
            {
                kidAnimations[i].SetTrigger("StandUp");     // ���̵��� �Ͼ��
                kidsTr[i].DOMoveZ(1.11f, 1.0f);             // �Ͼ���� ���ڿ� ���������� �ణ �������ش�
            }
        });
        seq.AppendInterval(2.0f);
        seq.AppendCallback(() => audioManager.LoopAudioPlay(warningAudioSource, fireWarningClipChange));  //ȭ�������� ���ۺκ��� �ٲ� ������
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
        seq.AppendCallback(() => CharacterMove());              //ĳ���͵��� �������� �����Ѵ�

    }

    public void CharacterMove()
    {
        Sequence seq = DOTween.Sequence();
        float startVal = 0.0f;
        float endVal = 1.0f;

        seq.AppendCallback(() =>
        {   
            teacherAni.SetTrigger("moveTrigger");   // �������� ������带 ���ش�
            Tween floatTween = DOTween.To(() => startVal, x => startVal = x, endVal, 1.0f).SetUpdate(true)  // startVal�� 1�� ����°� DOTween.To ���Ἥ �������� 1�� ������ִ� Ʈ��
            .OnUpdate(() => teacherAni.SetFloat("moveSpeed", startVal));

            Sequence seq01 = DOTween.Sequence();    // ���ο� �������� �����
            seq01.Append(floatTween);               // ������ �־��ش�
            seq01.Join(teacherTr.DORotate(new Vector3(0f, 90f, 0f), 0.5f));     // ���ÿ� �����δ�
            seq01.Join(teacherTr.DOMove(targetTr[4].position, 5.0f));  // ���ÿ� ȸ���Ѵ�
        });
        seq.AppendInterval(4.0f);
        seq.AppendCallback(() =>
        {
            endVal = 0.0f;  //endVal ���ٽ� 0���� �ʱ�ȭ���� �������� �ȴ°� ���߰��Ѵ�
            Tween floatTween = DOTween.To(() => startVal, x => startVal = x, endVal, 1.0f).SetUpdate(true)
            .OnUpdate(() => teacherAni.SetFloat("moveSpeed", startVal));
        });
        seq.AppendInterval(1.0f);
        seq.Append(teacherTr.DORotate(new Vector3(0f, -90f, 0f), 1.5f));
        seq.AppendCallback(() =>
        {
            teacherAni.SetTrigger("TalkTrigger");           // ���� ����� �������� ���� �ִϸ��̼� ����
            teacherSource.PlayOneShot(teacherGuid03, 1.0f); // ���� ����� �������� ���� ����� ����
        });
        seq.AppendInterval(7.0f);
        seq.AppendCallback(() =>
        {
            teacherAni.SetTrigger("moveTrigger");           // ���̳����� �ٽ� Idle ���·� �ٲ��ش�
            endVal = 1.0f;                                  // ���̵��� �������� ���� endVal = 1.0���� �ʱ�ȭ �Ѵ�
            Sequence seq01 = DOTween.Sequence(); 

            Tween lerpTween = DOTween.To(() => startVal, x => startVal = x, endVal, 1.0f)
                .SetUpdate(true)
                .OnUpdate(() =>
                {
                    kidAnimations[0].SetFloat("moveSpeed", startVal);
                    kidAnimations[1].SetFloat("moveSpeed", startVal);
                    kidAnimations[2].SetFloat("moveSpeed", startVal);

                });

            seq01.Append(lerpTween);
            seq01.Join(kidsTr[0].DORotate(new Vector3(0f, 90f, 0f), 5.0f));
            seq01.Join(kidsTr[1].DORotate(new Vector3(0f, 90f, 0f), 5.0f));
            seq01.Join(kidsTr[2].DORotate(new Vector3(0f, 90f, 0f), 5.0f));

            seq01.Join(kidsTr[0].DOMove(targetTr[3].position, 5.0f));
            seq01.Join(kidsTr[1].DOMove(targetTr[2].position, 5.0f));
            seq01.Join(kidsTr[2].DOMove(targetTr[1].position, 5.0f));
            seq01.AppendCallback(() =>
            {
                endVal = 0.0f;
                Tween lerpTween = DOTween.To(() => startVal, x => startVal = x, endVal, 1.0f)
                .SetUpdate(true)
                .OnUpdate(() =>
                {
                    kidAnimations[0].SetFloat("moveSpeed", startVal);
                    kidAnimations[1].SetFloat("moveSpeed", startVal);
                    kidAnimations[2].SetFloat("moveSpeed", startVal);

                });
            });
            seq01.AppendCallback(() => tweenManager.CloseUI(transform, nextStep, 1.0f));    // â�� �ݰ� ���� ������ ���� Ʈ�� �޼��� ����
        });
    }
}
