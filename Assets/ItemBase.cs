using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// アイテム用親クラス
/// </summary>
public class ItemBase : MonoBehaviour
{
    [Header("アイテムの種類")]
    public ITEM_TYPE itemType;
    public Image imgItem;

    public CONDITION_TYPE conditionType;

    [Header("アイテムの効力")]
    public float efficacyPoint;
    [Header("アイテムの効果時間")]
    public float duration;

    //PlayerController playerController;
    protected MoveSample playerController;

    public string itemName;

    /// <summary>
    /// アイテム情報の初期設定用。生成された時に呼び出される
    /// </summary>
    protected virtual void InitItem(ITEM_TYPE itemType, float efficacyPoint, float duration) {
        // アイテムの種類を設定
        this.itemType = itemType;

        // アイテムの効力を設定(回復量や倍率など、アイテムの種類によって設定する)
        this.efficacyPoint = efficacyPoint;

        // アイテムの効果時間を設定(一瞬で終わる効果の場合は0)
        this.duration = duration;

        // アイテムの種類に応じてマテリアルを変更し、アイテムの外見をアイテムに合わせる
        imgItem.material = Resources.Load<Material>("ItemMaterial/" + itemType);
    }

    // 未使用
    //public virtual void InitItem(GameData.DropItemData itemData) {
    //    // アイテムの種類を設定
    //    itemType = itemData.itemType;

    //    // アイテムの効力を設定(回復量や倍率など、アイテムの種類によって設定する)
    //    efficacyPoint = itemData.effectiveTime;

    //    // アイテムの効果時間を設定(一瞬で終わる効果の場合は0)
    //    duration = itemData.option_1;

    //    // アイテムの種類に応じてマテリアルを変更し、アイテムの外見をアイテムに合わせる
    //    imgItem.material = Resources.Load<Material>("ItemMaterial/" + itemType);

    //    // 各効果による内容を設定するためにコンディションを設定
    //    conditionType = (CONDITION_TYPE)Enum.Parse(typeof(CONDITION_TYPE), itemData.name, true);
    //}

    /// <summary>
    /// アイテム取得時の効果発動の準備用
    /// このメソッドを呼び出すと効果発動が自動的に呼ばれる
    /// </summary>
    public virtual void PraparateItemEffect(MoveSample playerController) {
        this.playerController = playerController;
        
        // アイテムの効果発動
        TriggerItemEffect();
    }

    /// <summary>
    /// アイテム取得時の効果発動
    /// </summary>
    protected virtual void TriggerItemEffect() {
        //StartCoroutine(UIManager.instance.CreateItemInfo(itemType, itemName));

        // (TODO) 子クラス内に行わせたい処理を記載する

        // 最後にDestroyItemを呼び出すことでアイテムが破壊される

    }

    /// <summary>
    /// アイテム破壊用
    /// </summary>
    /// <param name="destroyTime"></param>
    protected virtual void DestroyItem(float destroyTime = 0) {
        Destroy(this.gameObject, destroyTime);
    }
}
