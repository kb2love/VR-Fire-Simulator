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

    [SerializeField] AudioSource fireSource;    // ���� �������� �ٲ� �����Ŭ���� �÷����� ����� �ҽ�

    [Header("AudioClip")]

    [SerializeField] AudioClip teacherGuid;        // Ŭ�� ����� Ŭ��

    [SerializeField] AudioClip narration04;     // �����̼� ����� Ŭ��

    [SerializeField] AudioClip fireClip;     // �߰����� �����̼� ����� Ŭ��

    [SerializeField] AudioClip littleFireClip;  // ���� �������� �ٲ� ����� Ŭ��

    [Header("Transform")]

    [SerializeField] Transform playerTr;

    [SerializeField] Transform arrowImage;      // ��ȭ�Ⱑ �ִ� ��ġ�� �˷��� ȭ��ǥ Image

    [SerializeField] Transform playerFirePos;   // �÷��̾ ��ȭ�⸦ �� ��ġ

    [Header("GameObject")]

    [SerializeField] GameObject playerFire;     // �÷��̾��� �տ��ִ� ��ȭ��

    [SerializeField] GameObject fireextinguisher;   // �ٴڿ� ��ġ�Ǿ��ִ� ��ȭ��

    [SerializeField] GameObject fire;           // ���� ������ ���� �� ������Ʈ

    [SerializeField] GameObject nextStep;       // ���� �������� �Ѿ�� ���� ������Ʈ

    [SerializeField] GameObject digestion;      // ��ȭ�Ⱑ ������ ��ȭ�� ��ƼŬ ������Ʈ

    [SerializeField] ParticleSystemRenderer fireParticle;   // ���� ��ƼŬ �������� ���׸����� ������ ��ƼŬ������

    [SerializeField] Animator fireAni;          // ��ȭ�⸦ �����̰� ���� �ִϸ�����


    AudioManager audioManager;                  // ����� �Ŵ����� ����ϱ� ���� ����

    TweenManager tweenManager;                  // Ʈ�� �Ŵ����� ����ϱ� ���� ����

    Material fireMat;          // ���� ������ ���� ���� ���׸���
    void OnEnable()
    {
        tweenManager = new TweenManager();                  // ��� �����̹��� ��ӹ��� �ʾƵ� �Ǳ� ������ Ŭ�󽺸� ���� ���� �ش�
        tweenManager.PopUpUI(transform);

        audioManager = gameObject.AddComponent<AudioManager>();                        // ���� �����̽��� ���� �� ���� Ŭ����(�������̹����)
        audioManager.NarrationPlay(audioSource, narration04, 1.0f);                    //������ ����� �Ŵ��� Ŭ���� �����̼� �޼��带 ��� �� �� �ִ�
        float originPos = arrowImage.position.y;                    // ȭ��ǥ�� ���� ��ġ�� ������ ���� ��
        float downPos = originPos - 0.5f;                           // ȭ��ǥ�� �Ʒ��� ������ ���� ��
        fireMat = fireParticle.material;                            // fireMat �� ��ƼŬ�������� ���׸��� ���� ����
        Sequence seq = DOTween.Sequence();                          // ���ο� ������ ����
        seq.Append(arrowImage.DOMoveY(downPos, 1.0f));              // ȭ��ǥ�� �Ʒ��� �ٿ�
        seq.Append(arrowImage.DOMoveY(originPos, 1.0f));            // ȭ��ǥ�� ������ġ�� ��
        seq.SetLoops(-1);       // �ݺ�
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
        playerFire.SetActive(true);         // �÷��̾ ����ִ� ��ȭ�⸦ ���ش�
        fireextinguisher.SetActive(false);  // �ٴڿ��ִ� ��ȭ�⸦ ���ش�
        Sequence seq = DOTween.Sequence();  // ���ο� ������ ����
        seq.AppendCallback(() =>
        {
            teacherSource.PlayOneShot(teacherGuid, 1.0f);   // �������� ���� ����� Ŭ���� ���ش�
            tweenManager.CloseUI(transform);                // Uiâ�� ���ش�
        });
        seq.AppendInterval(5.0f);
        seq.AppendCallback(() =>
        {
            playerTr.DOMove(playerFirePos.position, 1.5f);  // �÷��̾��� ��ġ�� ���� �ִ°����� �ű��
            playerTr.DORotate(Vector3.zero, 1.0f);          // �÷��̾��� ������ ���� �ִ°����� ���Ѵ�
        });
        seq.AppendCallback(() => fireAni.SetTrigger("OpenTrigger"));    // ��ȭ�⸦ ����ϴ� �ִϸ��̼��� Ȱ��ȭ�Ѵ�
        seq.AppendInterval(2.0f);
        seq.AppendCallback(() =>
        {
            digestion.SetActive(true);          // ��ȭ�� ��ƼŬ�� Ų��
            audioSource.PlayOneShot(fireClip, 1.0f);    //��ȭ�� �Ѹ��� �Ҹ��� Ų��
            Color color = fireMat.GetColor("_Color");   // fireMat�� 
            DOTween.To(() => color.a, x => {
                color.a = x;
                fireMat.SetColor("_Color", color);
            }, 0, 6.0f);
        });
        seq.AppendInterval(6.0f);
        seq.AppendCallback(() =>
        {
            fireAni.SetTrigger("CloseTrigger");
            digestion.SetActive(false);
            fire.SetActive(false);
            audioManager.LoopAudioPlay(fireSource, littleFireClip);
        });
        seq.AppendInterval(1.0f);
        seq.AppendCallback(() => tweenManager.CloseUI(transform, nextStep, 1.0f));

    }
}
