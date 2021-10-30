using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRecover : ItemBase
{
    protected override void TriggerItemEffect() {
        RecoverLife();
    }

    /// <summary>
    /// HP回復処理
    /// </summary>
    private void RecoverLife() {
        // PlayerのHPを回復。efficacyPointをキャストして回復量として使用する
        playerController.hp += (int)efficacyPoint;

        // HPが最大値を超えないように制御
        playerController.hp = Mathf.Clamp(playerController.hp, 0, playerController.maxHp);

        // HpBarに現在値を反映して同期
        UIManager.instance.playerHpBarInfo.UpdatedHpBarValue(playerController.hp, playerController.maxHp);

        // アイテム破壊
        DestroyItem();
    }
}
