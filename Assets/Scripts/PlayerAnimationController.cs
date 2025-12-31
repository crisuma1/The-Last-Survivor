using UnityEngine;
using System.Collections;

public enum EventAnimType
{
    Hit,
    Stun,
    Knockback,
    Die
}

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] int eventLayerIndex = 2;

    Coroutine eventRoutine;
    bool isDead;



    public void PlayEvent(EventAnimType type)
    {
        if (isDead) return;

        if (eventRoutine != null)
            StopCoroutine(eventRoutine);

        switch (type)
        {
            case EventAnimType.Die:
                PlayDie();
                break;

            default:
                eventRoutine = StartCoroutine(PlayTemporaryEvent(type));
                break;
        }
    }

    IEnumerator PlayTemporaryEvent(EventAnimType type)
    {
        animator.SetLayerWeight(eventLayerIndex, 1f);
        animator.SetTrigger(type.ToString()); // Trigger 이름 = enum 이름

        yield return null;

        AnimatorStateInfo state =
            animator.GetCurrentAnimatorStateInfo(eventLayerIndex);

        yield return new WaitForSeconds(state.length);

        animator.SetLayerWeight(eventLayerIndex, 0f);
        eventRoutine = null;
    }

    void PlayDie()
    {
        isDead = true;
        animator.SetLayerWeight(eventLayerIndex, 1f);



        animator.SetTrigger(EventAnimType.Die.ToString());
       
    }
}
