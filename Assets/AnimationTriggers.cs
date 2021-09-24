using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggers : MonoBehaviour
{
    Player player;
    private void Awake()
    {
        player = GetComponent<Player>();
    }
    public void CheckIfPlayerFall()
    {
        if (player.isThrown && player.isGrounded)
        {
            player.FallOnGroundAnim();
        }
    }
}
