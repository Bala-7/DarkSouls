using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : InteractableObject
{
    private ParticleSystem _smokeFX;
    private Collider _trigger;
    private Collider _environmentCollider;
    private Enemy _enemy;
    private AudioClip _bossMusic;

    protected override void Awake()
    {
        base.Awake();

        _bossMusic = Resources.Load<AudioClip>("Sound/Music/Danger LOOP NO INTRO");
        _smokeFX = GetComponentInChildren<ParticleSystem>();
        _trigger = GetComponent<Collider>();
        _environmentCollider = transform.Find("Collider").GetComponent<Collider>();
        inRangeMessage = "Enter the fog.";
        nMaxInteractions = 1;
    }

    protected override void OnPlayerInput(NewPlayerInput input)
    {
        if ((input.GetButtonState(INPUT_BUTTONS.BUTTON_A) == BUTTON_STATE.RELEASE) && canBeInteracted) 
        {
            AudioManager.instance.PlayMusic(_bossMusic);

            _smokeFX.Stop();
            _trigger.enabled = false;
            _environmentCollider.enabled = false;
            EventsManager.instance.NotifyEvent(BOSS_EVENTS.BOSS_ENTER, _enemy);
            canBeInteracted = false;
            UIManager.s.ButtonMessage.SetShowingMessage(false);
        }
    }

    public void LinkToEnemy(Enemy e)
    {
        _enemy = e;
    }
    
}
