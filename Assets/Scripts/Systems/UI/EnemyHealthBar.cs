using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    private Enemy _enemy;
    public Enemy Enemy { get { return _enemy; } }
    
    private Slider _slider;

    public enum TYPE { NORMAL = 0, BOSS }
    public TYPE type = TYPE.NORMAL;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateValue();
        if(type == TYPE.NORMAL) 
            UpdatePosition();
    }

    public void UpdateValue()
    {
        _slider.value = _enemy.Performance.GetCurrentHealthPercentage();
    }

    // Updates the slider position so it is always a bit above the enemy
    private void UpdatePosition() 
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(_enemy.transform.position + 2*Vector3.up);
        transform.position = screenPos;
    }

    public void LinkToEnemy(Enemy e) { _enemy = e; }

    public void Unlink() { _enemy = null; }

}
