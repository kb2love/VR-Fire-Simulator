using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFireSim;
using DG.Tweening;
public class Step02 : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;   // �����̼��� �÷����� ����� �ҽ�

    [SerializeField] AudioClip clikClip;        // Ŭ�� ����� Ŭ��

    [SerializeField] AudioClip narration;     // �����̼� ����� Ŭ��

    [SerializeField] GameObject nextStep;       // ���� �������� �Ѿ�� ���� ������Ʈ

    [SerializeField] Transform playerTr;

    AudioManager audioManager;                  // ����� �Ŵ����� ����ϱ� ���� ����

    TweenManager tweenManager;                  // Ʈ�� �Ŵ����� ����ϱ� ���� ����
    void OnEnable()
    {
        tweenManager = new TweenManager();                  // ��� �����̹��� ��ӹ��� �ʾƵ� �Ǳ� ������ Ŭ�󽺸� ���� ���� �ش�
        tweenManager.PopUpUI(transform);

        audioManager = gameObject.AddComponent<AudioManager>();                        // ���� �����̽��� ���� �� ���� Ŭ����(�������̹����)
        audioManager.NarrationPlay(audioSource, narration, 1.0f);                    //������ ����� �Ŵ��� Ŭ���� �����̼� �޼��带 ��� �� �� �ִ�

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
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => tweenManager.CloseUI(transform));     // â�� �ݰ� ���� ������ ���� Ʈ�� �޼��� ����
        seq.AppendCallback(() => playerTr.DOMove(new Vector3(-1.0f, 0.35f, -1.5f), 5.0f));  //�÷��̾ �̵���Ŵ

    }
}
