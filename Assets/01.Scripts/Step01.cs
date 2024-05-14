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

    [SerializeField] AudioClip fireWarningClip;     //ȭ������ Ŭ��

    [SerializeField] AudioClip clikClip;            // Ŭ�� ����� Ŭ��

    [SerializeField] GameObject nextStep;           // ���� �������� �Ѿ�� ���� ������Ʈ

    [Header("Animator")]

    [SerializeField] Animator teacherAni;           // �������� �ִϸ�����

    [SerializeField] Animator kidAni01;             // ���� 1�� �ִϸ�����

    [SerializeField] Animator kidAni02;

    [SerializeField] Animator kidAni03;

    [Header("Character")]

    [SerializeField] Transform player;              // �÷��̾ �̵���ų Ʈ������

    [SerializeField] Transform teacher;             // �������� �̵���ų Ʈ������

    [SerializeField] Transform kid01;               //���� 1�� ������ Ʈ������

    [SerializeField] Transform kid02;

    [SerializeField] Transform kid03;

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

            kidAni01.SetTrigger("StandUp");
            kidAni02.SetTrigger("StandUp");
            kidAni03.SetTrigger("StandUp");

            kid01.DOMoveZ(1.11f, 1.0f);
            kid02.DOMoveZ(1.11f, 1.0f);
            kid03.DOMoveZ(1.11f, 1.0f);
        });
        seq.AppendInterval(2.0f);
        seq.AppendCallback(() => FireWarningClipChange());  //ȭ�������� ���ۺκ��� �ٲ� ������
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
        seq.AppendCallback(() => teacherSource.PlayOneShot(teacherGuid03, 1.0f));       //�������� 3��° ���� ����� ����
        seq.AppendInterval(8.0f);
        // ���� �ܰ�� �̵��ϱ� ���� �ݹ� �Լ��� �߰��մϴ�.
        seq.AppendCallback(() => CharacterMove());

        // �������� �����մϴ�.
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

        // ����� Ŭ���� ���ο� ���� (���� �ð����� ������)
        int samplesToCopy = fireWarningClip.samples - (int)(startTime * fireWarningClip.frequency);

        // ���ο� AudioClip�� �����ϰ� ����� �����͸� �����մϴ�.
        AudioClip newClip = AudioClip.Create("NewClip", samplesToCopy, fireWarningClip.channels, fireWarningClip.frequency, false);
        float[] data = new float[samplesToCopy * fireWarningClip.channels];
        fireWarningClip.GetData(data, (int)(startTime * fireWarningClip.frequency));
        newClip.SetData(data, 0);

        // ���ο� AudioClip�� AudioSource�� �Ҵ��Ͽ� ����մϴ�.
        warningAudioSource.clip = newClip; 
        audioManager.FireWarningPlay(warningAudioSource, newClip);
    }
}
