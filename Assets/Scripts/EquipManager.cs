using UnityEngine;

/// <summary>
/// 武器の情報を管理・制御するためのクラス
/// </summary>
public class EquipManager : MonoBehaviour
{
    [SerializeField]
    private WeaponController[] weapons;

    /// <summary>
    /// 武器の変更。初期設定の際にも利用する
    /// </summary>
    /// <param name="weaponIndex"></param>
    public void ChangeEquipWeapon(int weaponIndex) {

        // 登録されている武器をすべて確認
        for (int i = 0; i < weapons.Length; i++) {

            // 対象の武器のみ表示させる

            // 三項演算子の場合
            weapons[i].gameObject.SetActive(i == weaponIndex ? true : false);

            // if 文の場合
            //if (i == weaponIndex) {
            //    // 武器を表示
            //    weapons[i].gameObject.SetActive(true);            
            //} else {
            //    // 武器を非表示
            //    weapons[i].gameObject.SetActive(false);
            //}
        }
    }


    ///* Player 側に移管してもいい処理　*///

    private int currentEquipWeaponNo;

    /// <summary>
    /// 降順で武器を変更する準備
    /// </summary>
    public void PreparateChangeEquipWeapon() {
        currentEquipWeaponNo++;

        // 武器の最大値を超えないように制御
        currentEquipWeaponNo = currentEquipWeaponNo % weapons.Length;

        // 武器の変更
        ChangeEquipWeapon(currentEquipWeaponNo);
    }

    /// <summary>
    /// デバッグ用
    /// </summary>
    void Start() {
        ChangeEquipWeapon(0);
    }

    /// <summary>
    /// デバッグ用
    /// </summary>
    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            PreparateChangeEquipWeapon();
        }    
    }
}
