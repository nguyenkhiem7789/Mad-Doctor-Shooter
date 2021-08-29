using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3.5f;

    [SerializeField]
    private float minBound_X = -71f, maxBound_X = 71f, minBound_Y = -3.3f, maxBound_Y = 0f;

    private Vector3 temPos;

    private float xAxis, yAxis;

    private PlayerAnimation playerAnimation;

    [SerializeField]
    private float shootWaitTime = 0.5f;

    private float waitBeforeShooting;

    [SerializeField]
    private float moveWaitTime = 0.3f;

    private float waitBeforeMoving;

    private bool canMove;

    private PlayerShootingManager playerShootingManager;

    private bool playerDied;

    private void Awake()
    {
        playerAnimation = GetComponent<PlayerAnimation>();

        playerShootingManager = GetComponent<PlayerShootingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDied)
            return;
        HandleMovement();
        HandleAnimation();
        HandleFacingDireaction();
        HandleShooting();
        CheckIfCanMove();
    }

    void HandleMovement()
    {
        xAxis = Input.GetAxisRaw(TagManager.HORIZONTAL_AXIS);
        yAxis = Input.GetAxisRaw(TagManager.VERTICAL_AXIS);

        if (!canMove)
            return;

        temPos = transform.position;

        temPos.x += xAxis * moveSpeed * Time.deltaTime;
        temPos.y += yAxis * moveSpeed * Time.deltaTime;

        if (temPos.x < minBound_X) temPos.x = minBound_X;
        if (temPos.x > maxBound_X) temPos.x = maxBound_X;
        if (temPos.y < minBound_Y) temPos.y = minBound_Y;
        if (temPos.y > maxBound_Y) temPos.y = maxBound_Y;

        transform.position = temPos;
    }

    void HandleAnimation()
    {
        if (!canMove)
            return;

        if (Mathf.Abs(xAxis) > 0 || Mathf.Abs(yAxis) > 0)
            playerAnimation.PlayAnimation(TagManager.WALK_ANIMATION_NAME);
        else
            playerAnimation.PlayAnimation(TagManager.IDLE_ANIMATION_NAME);
    }

    void HandleFacingDireaction()
    {
        if (xAxis > 0)
            playerAnimation.SetFacingDirection(true);
        else if (xAxis < 0)
            playerAnimation.SetFacingDirection(false);
    }

    void StopMovement()
    {
        canMove = false;
        waitBeforeMoving = Time.time + moveWaitTime;
    }

    void Shoot()
    {
        waitBeforeShooting = Time.time + shootWaitTime;
        StopMovement();
        playerAnimation.PlayAnimation(TagManager.SHOOT_ANIMATION_NAME);

        playerShootingManager.Shoot(transform.localScale.x);
    }

    void CheckIfCanMove()
    {
        if (Time.time > waitBeforeMoving)
            canMove = true;
    }

    void HandleShooting()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            if (Time.time > waitBeforeShooting)
                Shoot();
        }
    }

    public void PlayerDied()
    {
        playerDied = true;
        playerAnimation.PlayAnimation(TagManager.DEATH_ANIMATION_NAME);
        Invoke("DestroyPlayerAfterDelay", 2f);
    }

    void DestroyPlayerAfterDelay()
    {
        Destroy(gameObject);
    }
}