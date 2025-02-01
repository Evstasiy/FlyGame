using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using System;
using System.Linq;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    private AudioContainer[] audioContainers;
    private AudioSource audioSource => GetComponent<AudioSource>();
    private bool isSoundPlay;

    public static AudioController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } 
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.isRepositoryChange += CheckSongPlay;
        CheckSongPlay();
    }

    private void CheckSongPlay()
    {
        isSoundPlay = DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.GetPlayerIsMusic();
        if (isSoundPlay)
        {
        }
        else
        {
            audioSource.Pause();
        }
    }

    private void OnDestroy()
    {
        DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.isRepositoryChange -= CheckSongPlay;
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        isSoundPlay = hasFocus && DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.GetPlayerIsMusic();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        isSoundPlay = !pauseStatus && DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.GetPlayerIsMusic();
    }

    public void PlayClip(string audioSongName)
    {
        if (!isSoundPlay || audioContainers == null || String.IsNullOrEmpty(audioSongName))
        {
            return;
        }
        var audioContainer = audioContainers.FirstOrDefault(x => x.AudioSongName == audioSongName);
        if(audioContainer == null)
        {
            //Debug.LogError("AudioSong not found with name: " + audioSongName);
            return;
        }

        audioSource.clip = audioContainer.AudioClips.First();
        audioSource.Play();
    }
}

[Serializable]
public class AudioContainer
{
    public string AudioSongName;
    public AudioClip[] AudioClips;
}
