using UnityEngine;

/// <summary>
/// ����̏����Ǘ��E���䂷�邽�߂̃N���X
/// </summary>
public class EquipManager : MonoBehaviour
{
    [SerializeField]
    private WeaponController[] weapons;

    /// <summary>
    /// ����̕ύX�B�����ݒ�̍ۂɂ����p����
    /// </summary>
    /// <param name="weaponIndex"></param>
    public void ChangeEquipWeapon(int weaponIndex) {

        // �o�^����Ă��镐������ׂĊm�F
        for (int i = 0; i < weapons.Length; i++) {

            // �Ώۂ̕���̂ݕ\��������

            // �O�����Z�q�̏ꍇ
            weapons[i].gameObject.SetActive(i == weaponIndex ? true : false);

            // if ���̏ꍇ
            //if (i == weaponIndex) {
            //    // �����\��
            //    weapons[i].gameObject.SetActive(true);            
            //} else {
            //    // ������\��
            //    weapons[i].gameObject.SetActive(false);
            //}
        }
    }


    ///* Player ���Ɉڊǂ��Ă����������@*///

    private int currentEquipWeaponNo;

    /// <summary>
    /// �~���ŕ����ύX���鏀��
    /// </summary>
    public void PreparateChangeEquipWeapon() {
        currentEquipWeaponNo++;

        // ����̍ő�l�𒴂��Ȃ��悤�ɐ���
        currentEquipWeaponNo = currentEquipWeaponNo % weapons.Length;

        // ����̕ύX
        ChangeEquipWeapon(currentEquipWeaponNo);
    }

    /// <summary>
    /// �f�o�b�O�p
    /// </summary>
    void Start() {
        ChangeEquipWeapon(0);
    }

    /// <summary>
    /// �f�o�b�O�p
    /// </summary>
    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            PreparateChangeEquipWeapon();
        }    
    }
}
