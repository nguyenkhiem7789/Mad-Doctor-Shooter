using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageArea : MonoBehaviour
{
    [SerializeField]
    private float deactivateWaitTime = 0.1f;
    private float deactivateTimer;

    [SerializeField]
    private LayerMask playerLayer;

    private bool canDealDamage;

    [SerializeField]
    private float damageAmout = 5f;

    // player health
    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = GameObject.FindWithTag(TagManager.PLAYER_TAG).GetComponent<PlayerHealth>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Physics2D.OverlapCircle(transform.position, 1f, playerLayer))
        {
            if(canDealDamage)
            {
                canDealDamage = false;
                Debug.Log("Damaged Player");
                playerHealth.TakeDamage(damageAmount: damageAmout);
            }
        }
        DeactivateDamageArea();
    }

    void DeactivateDamageArea()
    {
        if (Time.time > deactivateTimer)
            gameObject.SetActive(false);
    }

    public void ResetDeactivateTimer()
    {
        canDealDamage = true;
        deactivateTimer = Time.time + deactivateWaitTime;
    }
}
