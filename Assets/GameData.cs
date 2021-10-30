using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    //[Header("デバッグ用(シーン遷移で破棄させない場合にチェック)")]
    public bool isDontDestroy;

    public int totalScore;
    public int score;
    public int treasureCount;

    /// <summary>
    /// 破壊可能なオブジェクトの情報
    /// </summary>
    [System.Serializable]
    public class DestructibleObjectData {
        public int no;
        public string name;
        public int hp;
        [Header("クリティカルの発生確率")]
        public int criticalSuccessRate;
        [Header("クリティカル時の攻撃力への倍率")]
        public float criticalDamageMultiplier;
        [Header("得点")]
        public int score;
        [Header("アイテムの生成確率")]
        public int itemDropRate;
    }
    public List<DestructibleObjectData> objectSettingList = new List<DestructibleObjectData>();

    /// <summary>
    /// オブジェクト破壊時に出現するアイテムの情報
    /// </summary>
    [System.Serializable]
    public class DropItemData {
        public int no;
        public string name;
        public ITEM_TYPE itemType;
        [Header("持続時間")]
        public float effectiveTime;
        [Header("効果内容１")]
        public float option_1;
        [Header("効果内容２")]
        public float option_2;
    }
    public List<DropItemData> dropItemList = new List<DropItemData>();

    void Awake() {
        if (instance == null) {
            instance = this;
            if (isDontDestroy) {
                // デバッグ時以外は破棄させない
                DontDestroyOnLoad(this.gameObject);
            }
        } else {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// スコアの加算
    /// </summary>
    /// <param name="amount"></param>
    public void AddScore(int amount) {
        totalScore += amount;
        score += amount;

        Debug.Log(totalScore);
        Debug.Log(score);
    }
}
