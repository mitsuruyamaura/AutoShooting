using UnityEngine;

/// <summary>
/// 各武器のゲームオブジェクトにアタッチして制御するためのクラス
/// </summary>
public class WeaponController : MonoBehaviour
{
    public CapsuleCollider capsuleCollider;

    /// <summary>
    /// コライダーのオンオフ切り替え。AnimationEvent から呼び出す前提
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchCapsuleColliderActivate(bool isSwitch) {
        capsuleCollider.enabled = isSwitch;
    }


    // 以下は昔の内容

    //public MoveSample playerController;
    //public EnemyController enemyController;

    //public float attackAnimeTime;

    //void Start() {
    //    // 構えている武器も当たり判定をしてしまうため、攻撃するまでは武器と敵とが当たらないようにColliderをオフにする
    //    capsuleCollider.enabled = false;
    //}

    ///// <summary>
    ///// 武器のColliderのオン/オフ切り替え
    ///// </summary>
    ///// <param name="isSwitch"></param>
    //public void ActivateWeaponCollider(bool isSwitch) {
    //    capsuleCollider.enabled = isSwitch;
    //    StartCoroutine(AttackTime());
    //}

    ///// <summary>
    ///// 攻撃のアニメーションの時間に合わせてCollierを有効化
    ///// </summary>
    ///// <returns></returns>
    //private IEnumerator AttackTime() {
    //    yield return new WaitForSeconds(attackAnimeTime);
    //    ActivateWeaponCollider(false);
    //}
}
