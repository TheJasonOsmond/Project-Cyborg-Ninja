using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTimes : MonoBehaviour
{
    [HideInInspector] public float attackTime;
    [HideInInspector] public float damagedTime;
    [HideInInspector] public float deathTime;
    [HideInInspector] public float idleTime;

    [SerializeField] Animator animator;
    private AnimationClip clip;

    // Use this for initialization
    void Start()
    {
        //animator = gameObject.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.Log("Error: Did not find animator!");
            return;
        }

        UpdateAnimClipTimes();
    }
    public void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                //Cases use name of motion file, not the name in animator
                case "attack":
                    attackTime = clip.length;
                    break;
                case "damaged":
                    damagedTime = clip.length;
                    break;
                case "vanish": 
                    deathTime = clip.length;
                    Debug.Log("Length of death anim: "+ deathTime);
                    break;
                case "dash":
                    idleTime = clip.length;
                    break;
            }
        }
    }

    public float getCurrentClipLength()
    {
        AnimatorClipInfo[] clipInfoArray= animator.GetCurrentAnimatorClipInfo(0);

        if (clipInfoArray.Length > 0)
        {
            float speed = animator.GetCurrentAnimatorStateInfo(0).speed;

            float clipLength = clipInfoArray[0].clip.length * speed;

            Debug.Log("Clip Length = " + clipLength + " | Length = " + clipInfoArray[0].clip.length + 
                " | Speed = " + speed + " | Name = " + clipInfoArray[0].clip.name);

            return clipLength;
        }
        else
            Debug.Log("No Clip");
            return 0; 
    }
}
