using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VrFireSim;

public class Step05 : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;   // �����̼��� �÷����� ����� �ҽ�

    [Header("AudioClip")]

    [SerializeField] AudioClip clikClip;        // Ŭ�� ����� Ŭ��

    [SerializeField] AudioClip narration01;     // �����̼� ����� Ŭ��

    [Header("Transform")]
    [SerializeField] Transform[] kidsTr;    //�ֵ��� ��ġ�� ����Ʈ�� ������
    [SerializeField] Transform targetTr;  // �̵��� ��ġ
    [SerializeField] Transform teacherTr;   // �������� ��ġ
    [SerializeField] Transform playerTr;    // �÷��̾��� ��ġ

    [Header("Animator")]
    [SerializeField] Animator[] kidsAni;
    [SerializeField] Animator teacherAni;

    [SerializeField] Image fadeImage;
    [SerializeField] GameObject nextStep;   // ���� �������� �Ѿ�� ���� ������Ʈ
    float startVal = 0.0f;
    float endVal = 1.0f;

    AudioManager audioManager;                  // ����� �Ŵ����� ����ϱ� ���� ����

    TweenManager tweenManager;                  // Ʈ�� �Ŵ����� ����ϱ� ���� ����
    void OnEnable()
    {
        tweenManager = new TweenManager();                  // ��� �����̹��� ��ӹ��� �ʾƵ� �Ǳ� ������ Ŭ�󽺸� ���� ���� �ش�
        tweenManager.PopUpUI(transform);

        audioManager = gameObject.AddComponent<AudioManager>();                        // ���� �����̽��� ���� �� ���� Ŭ����(�������̹����)
        audioManager.NarrationPlay(audioSource, narration01, 1.0f);                    //������ ����� �Ŵ��� Ŭ���� �����̼� �޼��带 ��� �� �� �ִ�

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
        audioSource.PlayOneShot(clikClip, 1.0f);            // �ߺ��� �Ǵ� �ܹ߼� ����� �÷���

        tweenManager.CloseUI(transform);    // â�� �ݰ� ���� ������ ���� Ʈ�� �޼��� ����
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
