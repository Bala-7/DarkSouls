using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTPCharacter : MonoBehaviour
{
    public enum ANIM_TYPE { FULL_BODY, BODY_LEG_SEPARATED}
    private Animator fullBodyAnimator;

    private void Start()
    {

    }

    public void AssignAnimator() {
        fullBodyAnimator = GetComponent<Animator>();
    }

    public Animator GetFullBodyAnimator() { return fullBodyAnimator; }

    public void SetFullBodyAnimator(Animator anim) { fullBodyAnimator = anim; }

}
