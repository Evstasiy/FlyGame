using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSupportController : MonoBehaviour, IPauseHandler
{
    private AudioSource audioSource;
    private Coroutine audioCoroutine;
    private bool isSoundPlay;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ProjectContext.instance.PauseManager.Register(this);
        CheckSongPlay();
        DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.isRepositoryChange += CheckSongPlay;
        if (ProjectContext.instance.DataBaseRepository.PlayerFeaturesRepos.GetPlayerIsMusic())
        {
            audioCoroutine = StartCoroutine(PlayAudio());
        }
    }
    public void SetPause(bool isPause)
    {
        if (isPause)
        {
            audioSource.Pause();
            if (audioCoroutine != null)
            {
                StopCoroutine(audioCoroutine);
            }
        } 
        else if (isSoundPlay)
        {
            audioSource.UnPause();
            audioCoroutine = StartCoroutine(PlayAudio());
        }
    }

    private IEnumerator PlayAudio()
    {
        while (true)
        {
            // ¬оспроизводим звук
            audioSource.Play();
            // ∆дем, пока звук закончитс€
            yield return new WaitForSeconds(audioSource.clip.length);
            // ќстанавливаем звук и устанавливаем его позицию в конец
            audioSource.Stop();
            audioSource.time = audioSource.clip.length - 0.1f;
            // ¬оспроизводим звук снова с конца
            audioSource.Play();
            // ∆дем, пока звук закончитс€
            yield return new WaitForSeconds(audioSource.clip.length);
            // ќстанавливаем звук и устанавливаем его позицию в начало
            audioSource.Stop();
            audioSource.time = 0;
        }
    }
    private void CheckSongPlay()
    {
        isSoundPlay = DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.GetPlayerIsMusic();
        if (isSoundPlay && !ProjectContext.instance.PauseManager.IsPause)
        {
            audioSource.Play();
        } 
        else
        {
            audioSource.Pause();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        isSoundPlay = hasFocus && DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.GetPlayerIsMusic();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        isSoundPlay = !pauseStatus && DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.GetPlayerIsMusic();
    }

    private void OnDestroy()
    {
        if (audioCoroutine != null)
        {
            StopCoroutine(audioCoroutine);
        }
        ProjectContext.instance.PauseManager.UnRegister(this);
        DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.isRepositoryChange -= CheckSongPlay;
    }
}
