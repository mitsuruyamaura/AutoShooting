using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager instance; 

    public ItemInfo itemInfoPrefab;
    public Transform itemInfoTran;

    public RectTransform rectBattleTime;
    public Text txtBattleTime;

    public int battleTime;

    private float timer;
    private bool isBattleStart;

    private ItemInfo itemInfo;
    public HpBarInfo playerHpBarInfo;

    void Awake() {
        if (instance == null) {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        // TODO あとで変える
        StartCoroutine(InitBattleUI());
    }

    /// <summary>
    /// Battle用UIの初期設定
    /// </summary>
    /// <returns></returns>
    public IEnumerator InitBattleUI() {
        // バトル時間表示
        rectBattleTime.DOSizeDelta(new Vector2(400, 150), 0.75f).SetEase(Ease.InCirc);
        yield return new WaitForSeconds(0.75f);
        DisplayBattleTime(battleTime);
        isBattleStart = true;
    }

    /// <summary>
    /// 獲得アイテム名の表示
    /// </summary>
    /// <param name="conditionType"></param>
    /// <param name="itemType"></param>
    /// <returns></returns>
    public IEnumerator CreateItemInfo(string itemName) {
        if (itemInfo == null) {
            // アイテム表示がない場合のみ生成
            itemInfo = Instantiate(itemInfoPrefab, itemInfoTran, false);
            itemInfo.InitItemInfo(itemName);
            itemInfo = null;
            yield return null;
        }
    }

    void Update() {
        if (!isBattleStart) {
            return;
        }

        timer += Time.deltaTime;
        if (timer >= 1) {
            timer = 0;
            battleTime--;
            DisplayBattleTime(battleTime);

            if (battleTime <= 0) {
                battleTime = 0;
                isBattleStart = false;
                Debug.Log("Game Up");
            }
        }
    }
    
    /// <summary>
    /// 残り時間を更新して[分:秒]で表示する
    /// </summary>
    private void DisplayBattleTime(int limitTime) {
        // 引数で受け取った値を[分:秒]に変換して表示する
        // ToString("00")でゼロプレースフォルダーして、１桁のときは頭に0をつける
        txtBattleTime.text = (limitTime / 60).ToString() + ":" + (limitTime % 60).ToString("00");
    }
}
