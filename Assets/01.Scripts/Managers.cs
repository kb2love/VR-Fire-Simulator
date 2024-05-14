using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace VrFireSim
{
    public class AudioManager : MonoBehaviour
    {
        public void NarrationPlay(AudioSource audioSource, AudioClip audioClip, float delayTime)
        {
            StartCoroutine(DelayRoutine(audioSource, audioClip, delayTime));
        }
        public void FireWarningPlay(AudioSource audioSource, AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.loop = true;
            audioSource.Play();
        }
        IEnumerator DelayRoutine(AudioSource audioSource, AudioClip audioClip, float delayTime)
        {
            audioSource.Stop();
            yield return new WaitForSeconds(delayTime);
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
    public class TweenManager
    {
        public void PopUpUI(Transform step)                                  
        {
            step.GetChild(0).DOPunchScale(Vector3.one * 0.0002f, 1.0f);      
        }

        public void PopUpUI(Transform step, Tween action)                   
        {
            step.GetChild(0).DOPunchScale(Vector3.one * 0.0002f, 1.0f).OnComplete(() => action.Play());  
        }

        public void CloseUI(Transform step)                               
        {
            step.GetChild(0).DOScaleY(0.0f, 0.5f);
        }

        public void CloseUI(Transform step, Tween action)                 
        {
            step.GetChild(0).DOScaleY(0.0f, 0.5f).OnComplete(() => action.Play());                       
        }

        public void CloseUI(Transform step, GameObject nextStep, float delayTime)
        {
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(delayTime); 
            seq.OnComplete(() => { step.gameObject.SetActive(false); nextStep.SetActive(true); });
            seq.Pause();

            step.GetChild(0).DOScaleY(0.0f, 0.5f).OnComplete(() => seq.Play());
        }
    }
}
