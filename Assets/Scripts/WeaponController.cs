using UnityEngine;

/// <summary>
/// �e����̃Q�[���I�u�W�F�N�g�ɃA�^�b�`���Đ��䂷�邽�߂̃N���X
/// </summary>
public class WeaponController : MonoBehaviour
{
    public CapsuleCollider capsuleCollider;

    /// <summary>
    /// �R���C�_�[�̃I���I�t�؂�ւ��BAnimationEvent ����Ăяo���O��
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchCapsuleColliderActivate(bool isSwitch) {
        capsuleCollider.enabled = isSwitch;
    }


    // �ȉ��͐̂̓��e

    //public MoveSample playerController;
    //public EnemyController enemyController;

    //public float attackAnimeTime;

    //void Start() {
    //    // �\���Ă��镐��������蔻������Ă��܂����߁A�U������܂ł͕���ƓG�Ƃ�������Ȃ��悤��Collider���I�t�ɂ���
    //    capsuleCollider.enabled = false;
    //}

    ///// <summary>
    ///// �����Collider�̃I��/�I�t�؂�ւ�
    ///// </summary>
    ///// <param name="isSwitch"></param>
    //public void ActivateWeaponCollider(bool isSwitch) {
    //    capsuleCollider.enabled = isSwitch;
    //    StartCoroutine(AttackTime());
    //}

    ///// <summary>
    ///// �U���̃A�j���[�V�����̎��Ԃɍ��킹��Collier��L����
    ///// </summary>
    ///// <returns></returns>
    //private IEnumerator AttackTime() {
    //    yield return new WaitForSeconds(attackAnimeTime);
    //    ActivateWeaponCollider(false);
    //}
}
