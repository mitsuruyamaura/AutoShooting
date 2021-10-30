using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    private const string WEAPON = "Weapon";   // タグ指定用

    public int no;
    public string objectName = "";

    [Header("現在のHP")]
    public int maxHp;

    [Header("クリティカルの発生確率")]
    public int criticalSuccessRate;

    [Header("クリティカル時の攻撃力への倍率")]
    public float criticalDamageMultiplier;

    [Header("得点")]
    public int score;

    [Header("アイテムの生成確率")]
    public int itemDropRate;

    public int hp;  // 現在値

    public ItemDetail itemDetailPrefab;

    public ItemBase itemBasePrefab;

    public float offset;

    private HpBarInfo hpBarInfo;

    void Start()
    {
        InitObjectParameter();
    }

    /// <summary>
    /// オブジェクト情報の取得と設定
    /// </summary>
    public void InitObjectParameter() {
        // 情報取得用の変数用意
        GameData.DestructibleObjectData objectData = new GameData.DestructibleObjectData();

        // 情報を取得して代入
        objectData = ObjectSettingHelper.GetDestructibleObjectData(no);

        // 取得した情報を元にオブジェクト情報として設定
        objectName = objectData.name;
        maxHp = objectData.hp;
        criticalSuccessRate = objectData.criticalSuccessRate;
        criticalDamageMultiplier = objectData.criticalDamageMultiplier;
        score = objectData.score;
        itemDropRate = objectData.itemDropRate;

        hp = maxHp;

        // HpBarに初期HPを同期
        hpBarInfo = GetComponentInChildren<HpBarInfo>();
        hpBarInfo.UpdatedHpBarValue(hp, maxHp, true);
    }

    private void OnCollisionEnter(Collision col) {
        // Weaponタグ以外は当たり判定として判定しない
        if (col.gameObject.tag != WEAPON) {
            return;
        }

        // 攻撃力への初期倍率
        float attackPowerRate = 1.0f;

        // クリティカルの判定を戻り値で行う。Trueが戻ってきたらクリティカルした判定
        if (JudgeSuccessOrFailure(criticalSuccessRate)) {
            // クリティカルした判定の場合、攻撃倍率を変更。スコアにも倍率をかける
            attackPowerRate = criticalDamageMultiplier;
            score = Mathf.FloorToInt(score * criticalDamageMultiplier);
            Debug.Log(score);
        }

        int damage = (int)Mathf.Floor(col.gameObject.GetComponent<FireProjectile>().attackPower * attackPowerRate);
        Debug.Log(damage);

        // HPからdamage分だけ減算
        hp -= damage;
        Debug.Log("残りHP : " + hp);
    
        // 残りHPの確認
        if (hp <= 0) {
            Debug.Log("破壊した");

            // オブジェクトの破壊時の処理
            DestroyObject();
        } else {
            // HpbarをHPの値と同期
            hpBarInfo.UpdatedHpBarValue(hp, maxHp);
        }
    }

    /// <summary>
    /// オブジェクトの破壊とそれに関連する処理
    /// </summary>
    private void DestroyObject() {
        // スコアの加算
        GameData.instance.AddScore(score);

        // アイテム生成の判定
        if (JudgeSuccessOrFailure(itemDropRate)) {
            // 判定成功した場合にはアイテムの生成
            CreateDropItem();
        }

        Destroy(this.gameObject);
    }

    /// <summary>
    /// 乱数による成否判定。クリティカルの有無判定とアイテムのドロップ有無判定に用いる
    /// </summary>
    /// <returns></returns>
    private bool JudgeSuccessOrFailure(int judgeRate) {
        // 乱数を１つ取得
        int randomValue = Random.Range(0, 100);

        // 乱数と成功確率を比べ、成功確率よりも乱数の方が低いなら成功とする
        if (randomValue <= judgeRate) {
            Debug.Log("Critical!!");
            return true;
        }
        return false;
    }

    /// <summary>
    /// アイテムの生成
    /// </summary>
    private void CreateDropItem() {
        // TODO コンディション数にする
        int value = Random.Range(0, (int)CONDITION_TYPE.COUNT);

        // ItemDataを初期化して用意
        GameData.DropItemData itemData = new GameData.DropItemData();

        // ランダムで選ばれたIteｍDataを代入
        itemData = GameData.instance.dropItemList.Find(x => x.no == value);

        // 生成の高さを調整
        Vector3 createPos = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);
        // 生成の向きを調整
        Quaternion createRot = new Quaternion(transform.rotation.x, 180, transform.rotation.z, 0);

        // TODO コメントアウト中 アイテム生成
        ItemDetail itemDetail = Instantiate(itemDetailPrefab, createPos, createRot);

        // アイテムの情報を設定
        itemDetail.InitItem(itemData);

        // TODO コメントアウト中 アイテム生成
        //ItemBase itemBase = Instantiate(itemBasePrefab, createPos, createRot);

        //itemBase.InitItem(itemData.name);
    }
}
