using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConditionJumpPowerUp : PlayerCondition
{
    private float currentJumpPower;

    protected override void StartCondition() {
        currentJumpPower = playerController.jumpPower;
        playerController.jumpPower *= duration;
        Debug.Log(playerController.jumpPower);
    }

    protected override void EndCondition() {
        playerController.jumpPower = currentJumpPower;
        Debug.Log(playerController.jumpPower);
        base.EndCondition();
    }
}
