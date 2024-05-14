using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFireSim;

public class Step00 : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;   // �����̼��� �÷����� ����� �ҽ�

    [SerializeField] AudioClip clikClip;        // Ŭ�� ����� Ŭ��

    [SerializeField] AudioClip narration01;     // �����̼� ����� Ŭ��

    [SerializeField] GameObject nextStep;       // ���� �������� �Ѿ�� ���� ������Ʈ

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

        tweenManager.CloseUI(transform, nextStep, 1.0f);    // â�� �ݰ� ���� ������ ���� Ʈ�� �޼��� ����
    }
}
