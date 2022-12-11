using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockEnemyIcon : MonoBehaviour
{
    private Image _icon;
    private Enemy _lockedEnemy;
    private Camera _camera;

    private void Awake()
    {
        _icon = GetComponentInChildren<Image>();
        _icon.gameObject.SetActive(false);
    }

    private void Start()
    {
        try { _camera = ThirdPersonControllerMovement.s.Camera; } catch (Exception e) { Debug.LogError("CHECK THIS!"); }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocked())
        {
            _icon.rectTransform.position = _camera.WorldToScreenPoint(_lockedEnemy.transform.position + Vector3.up);
        }
    }

    private bool IsLocked()
    {
        return !ReferenceEquals(_lockedEnemy, null);
    }

    public void OnEnemyLocked(Enemy e)
    {
        _lockedEnemy = e;
        _icon.gameObject.SetActive(true);
    }

    public void OnEnemyUnlocked()
    {
        _lockedEnemy = null;
        _icon.gameObject.SetActive(false);
    }
}
