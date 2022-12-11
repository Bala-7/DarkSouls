using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource _musicSource;
    private GameObserver _gameObserver;

    private void Awake()
    {
        if (!instance)
            instance = this;

        _musicSource = transform.Find("MusicSource").GetComponent<AudioSource>();
    }


    // Start is called before the first frame update
    void Start()
    {
        _gameObserver = new GameObserver(new List<GAME_EVENTS>() { GAME_EVENTS.BOSS_DEFEAT },
            new List<GameObserver.OnNotifyDelegate>() { OnBossDead });
        EventsManager.instance.RegisterObserver(_gameObserver);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public AudioClip GetRandomSound(List<AudioClip> soundList)
    {
        return (soundList.Count > 0) ? soundList[Random.Range(0, soundList.Count)] : null;
    }

    public void PlayAudio(AudioClip clip, AudioSource source, float volume)
    {
        source.volume = volume;
        PlayAudio(clip, source);
    }

    public void PlayAudio(AudioClip clip, AudioSource source)
    {
        source.clip = clip;
        source.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }

    private void OnBossDead()
    { 
        StopMusic(); 
    }
}
