using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCharacterAudio : MonoBehaviour
{
    public List<AudioClip> stepSounds;
    public List<AudioClip> attackSounds;
    public List<AudioClip> hitSounds;
    public AudioClip deathSound;

    private AudioSource _source;
    private ThirdPersonControllerMovement _character;
    private float _timeSinceLastStep = 0;
    private float _timeByStep = 0.2f;
    private int _currentStepSound = 0;
    private bool MustReproduceStepSound { get { return _timeSinceLastStep >= _timeByStep; } }

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _character = ThirdPersonControllerMovement.s;
    }

    // Update is called once per frame
    void Update()
    {
        if (_character.IsMoving)
        {
            _timeSinceLastStep += Time.deltaTime;

            if (MustReproduceStepSound)
            {
                _timeSinceLastStep = 0;
                _source.volume = 0.15f;
                _source.clip = stepSounds[_currentStepSound];
                _source.Play();
                _currentStepSound = (_currentStepSound + 1) % stepSounds.Count;
            }
        }
        else 
        {
            _timeSinceLastStep = 0;
            _currentStepSound = 0;
        }
    }

    public void PlayAttackSound()
    {
        _source.volume = 0.2f;
        _source.clip = attackSounds[Random.Range(0, attackSounds.Count)];
        _source.Play();
    }

    public void PlayHitSound()
    {
        _source.volume = 0.2f;
        _source.clip = hitSounds[Random.Range(0, hitSounds.Count)];
        _source.Play();
    }

    public void PlayDeathSound()
    {
        _source.volume = 0.35f;
        _source.clip = deathSound;
        _source.Play();
    }
}
