using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTrigger : MonoBehaviour
{
    Enemy enemy;
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    public void CheckIfEnemyFall()
    {
        if (enemy.isThrown && enemy.isGrounded)
        {
            enemy.FallOnGround();
        }
    }
}
