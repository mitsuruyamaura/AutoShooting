using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConditionAttackPowerUp : PlayerCondition
{
    private int currentAttackPower;

    protected override void StartCondition() {
        currentAttackPower = playerController.attackPower;
        playerController.attackPower *= (int)duration;
        Debug.Log(playerController.attackPower);
    }

    protected override void EndCondition() {
        playerController.attackPower = currentAttackPower;
        Debug.Log(playerController.attackPower);
        base.EndCondition();
    }
}
