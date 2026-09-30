using System;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float playerCoolDown = 1f;

    float coolDownTimer = 0f;
    const string stringHit = "Hit";

    void Update()
    {
        coolDownTimer += Time.deltaTime;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (coolDownTimer < playerCoolDown) return;

        animator.SetTrigger(stringHit);
        coolDownTimer = 0f;
        Debug.Log(collision.gameObject.name);
    }
}
