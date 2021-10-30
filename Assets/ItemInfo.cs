using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ItemInfo : MonoBehaviour
{
    public Text txtItemName;
    public CanvasGroup canvasGroup;
    public Image imgItem;

    /// <summary>
    /// ItemInfoの設定
    /// </summary>
    /// <param name="conditionType"></param>
    /// <param name="itemName">表示するアイテムの名前とイメージ設定用</param>
    public void InitItemInfo(string itemName) {
        // TODO コメントアウト中 表示するアイテム名を設定
        //if (itemType == ITEM_TYPE.TREASURE) {
        //    txtItemName.text = "? Treasure";           
        //} else {
        //    txtItemName.text = conditionType.ToString();
        //}
        canvasGroup.alpha = 0;

        txtItemName.text = itemName;
        // 表示するアイテムのイメージを設定
        imgItem.sprite = Resources.Load<Sprite>("Textures/Items/" + itemName);
        // ItemInfoを移動させながら表示
        StartCoroutine(MovingDisplayInfo());
    }

    /// <summary>
    /// 画面端から画面内にItemInfoを移動させて表示する
    /// </summary>
    /// <returns></returns>
    private IEnumerator MovingDisplayInfo() {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMoveX(-600, 0.35f).SetEase(Ease.Linear).SetRelative());
        seq.Join(canvasGroup.DOFade(1.0f, 0.75f));
        yield return new WaitForSeconds(1.0f);
        seq.Append(transform.DOMoveY(-100, 0.5f).SetEase(Ease.Linear));
        seq.Join(canvasGroup.DOFade(0, 0.25f));
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
