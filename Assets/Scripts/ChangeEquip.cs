using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEquip : MonoBehaviour {

    [SerializeField]
    private GameObject[] weapons;                    //  武器の種類。外部で数と内容を設定可能
    
    private int equipment;                           //  現在の武器の管理用
    private CapsuleCollider capsuleCol;

    //private ProcessMyAttack processMyAttack;       //  コンポーネント取得用
    PlayerMove move;                                 //  コンポーネント取得用

	// Use this for initialization
	void Start () {
        //  初期装備設定。すべてfalseにしておく。
        equipment = 0;
        weapons[equipment].SetActive(true);

        capsuleCol = weapons[equipment].GetComponent<CapsuleCollider>();
        capsuleCol.enabled = false;

        move = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update() {
        if (move.isEnd) {
            return;
        }

        //  武器変更ボタンを押し、現在のstateがNormalなら武器変更メソッド呼び出し
        if (Input.GetKeyDown("4") && PlayerMove.state == PlayerMove.MyState.Normal) {
            Change();
        }
    }

    //  シーン上のボタンでも呼び出せる（その場合、Stateによる制限を受けない）
    public void Change()
    {
        equipment ++;

        //  配列の最大値を超えたら0に戻す
        if(equipment >= weapons.Length)
        {
            equipment = 0;
        }

        //  武器を切り替え
        for(var i = 0; i < weapons.Length; i++)
        {
            //  for分で回してiと同じになった武器で止めてセットする
            if(i == equipment)
            {
                //  選択された武器をアクティブにし、コライダーをセットする
                weapons[i].SetActive(true);
                capsuleCol = weapons[i].GetComponent<CapsuleCollider>();
                capsuleCol.enabled = false;
                //processMyAttack.SetColloder(weapons[i].GetComponent<Collider>());
            }
            else   //  iと同じでない武器はfalseにする
            {
                weapons[i].SetActive(false);
            }            
        }
    }

    public void WeaponAttack()
    {
        capsuleCol.enabled = true;
    }

    public void Disarm()
    {
        capsuleCol.enabled = false;
    }
}
