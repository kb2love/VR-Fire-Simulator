using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFireSim;
using DG.Tweening;
public class Step04 : MonoBehaviour
{
    [Header("AudioSource")]
    
    [SerializeField] AudioSource audioSource;   // �����̼��� �÷����� ����� �ҽ�

    [SerializeField] AudioSource teacherSource; // �������� ���ø� �÷����� ����� �ҽ�

    [Header("AudioClip")]

    [SerializeField] AudioClip teacherGuid;        // Ŭ�� ����� Ŭ��

    [SerializeField] AudioClip narration04;     // �����̼� ����� Ŭ��

    [SerializeField] AudioClip fireClip;     // �߰����� �����̼� ����� Ŭ��
    

    [Header("Transform")]

    [SerializeField] Transform playerTr;

    [SerializeField] Transform arrowImage;      // ��ȭ�Ⱑ �ִ� ��ġ�� �˷��� ȭ��ǥ Image

    [SerializeField] Transform playerFirePos;   // �÷��̾ ��ȭ�⸦ �� ��ġ

    [Header("GameObject")]

    [SerializeField] GameObject playerFire;     // �÷��̾��� �տ��ִ� ��ȭ��

    [SerializeField] GameObject fireextinguisher;   // �ٴڿ� ��ġ�Ǿ��ִ� ��ȭ��

    [SerializeField] GameObject fire;           // ��

    [SerializeField] GameObject nextStep;       // ���� �������� �Ѿ�� ���� ������Ʈ

    [SerializeField] GameObject digestion;      // ��ȭ�Ⱑ ������ ��ȭ�� ��ƼŬ

    [SerializeField] Animator fireAni;          // ��ȭ�⸦ �����̰� ���� �ִϸ�����


    AudioManager audioManager;                  // ����� �Ŵ����� ����ϱ� ���� ����

    TweenManager tweenManager;                  // Ʈ�� �Ŵ����� ����ϱ� ���� ����
    void OnEnable()
    {
        tweenManager = new TweenManager();                  // ��� �����̹��� ��ӹ��� �ʾƵ� �Ǳ� ������ Ŭ�󽺸� ���� ���� �ش�
        tweenManager.PopUpUI(transform);

        audioManager = gameObject.AddComponent<AudioManager>();                        // ���� �����̽��� ���� �� ���� Ŭ����(�������̹����)
        audioManager.NarrationPlay(audioSource, narration04, 1.0f);                    //������ ����� �Ŵ��� Ŭ���� �����̼� �޼��带 ��� �� �� �ִ�
        float originPos = arrowImage.position.y;
        float downPos = originPos - 0.5f;
        Sequence seq = DOTween.Sequence();
        seq.Append(arrowImage.DOMoveY(downPos, 1.0f));
        seq.Append(arrowImage.DOMoveY(originPos, 1.0f));
        seq.SetLoops(-1);
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
