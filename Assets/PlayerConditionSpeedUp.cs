using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConditionSpeedUp : PlayerCondition
{
    private float currentMoveSpeed;

    protected override void StartCondition() {
        currentMoveSpeed = playerController.moveSpeed;
        playerController.moveSpeed *= duration;
        Debug.Log(playerController.moveSpeed);
    }

    protected override void EndCondition() {
        playerController.moveSpeed = currentMoveSpeed;
        Debug.Log(playerController.moveSpeed);
        base.EndCondition();
    }
}
