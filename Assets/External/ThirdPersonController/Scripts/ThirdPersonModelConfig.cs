using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonModelConfig : MonoBehaviour
{
    public GameObject model;
    public RuntimeAnimatorController animatorController;
    private GameObject instance;
    //public  thirdPersonAnimator;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        //ThirdPersonControllerMovement.s.InitializeModelInfo();
    }



    public void SetNewModel() {
        if (model && animatorController)
        {
            Transform previousBody = transform.Find("Body");
            if (previousBody) DestroyImmediate(previousBody.gameObject);

            instance = Instantiate(model, transform);
            instance.AddComponent<MyTPCharacter>();
            Animator anim = instance.GetComponent<Animator>();
            if (!anim) anim = instance.AddComponent<Animator>();
            anim.runtimeAnimatorController = animatorController;
            instance.name = "Body";

            ThirdPersonCameraMovement cm = GetComponent<ThirdPersonCameraMovement>();
            for (int i = 0; i < instance.transform.childCount; ++i)
            {
                if (!instance.transform.GetChild(i).GetComponent<MeshRenderer>())
                    cm.lookAt = instance.transform.GetChild(i);
            };
            Debug.Log("Setting new model!");
        }
        else Debug.LogError("You need to assign a new model and AnimatorController before trying to set it!");

        
    }
}
