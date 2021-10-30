using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAttack : MonoBehaviour {

    public GameObject _player;              //  ドラゴンのオブジェクト
    public float flyingDamage;              //  ジャンプ攻撃の攻撃力

    BoxCollider bCol;                       //  各コンポーネント格納用
    Animator anim;
    AnimatorStateInfo stateInfo;
    Enemy enemy;

	// Use this for initialization
	void Start () {
        enemy = transform.parent.GetComponent<Enemy>();                   //  親のスクリプトを取得
        bCol = GetComponent<BoxCollider>();
        anim = _player.GetComponent<Animator>();
        bCol.enabled = false;
	}

    private void Update()
    {
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);       //  アニメの再生状態を取得
        if (stateInfo.IsName("Attack3"))
        {
            if(stateInfo.normalizedTime < 0.5f)
            {
                bCol.enabled = false;
            }
            else if (stateInfo.normalizedTime <= 0.55f)        //  着地のタイミングで当たり判定ありにする
            {
                bCol.enabled = true;
            }
            else                         
            {
                bCol.enabled = false;                         //  その後なしに戻す
            }
        }
    }

    public void Flying()
    {
        anim.SetTrigger("FlyingAttack");                                  //  アニメ再生       
    }

    private void OnTriggerEnter(Collider col)                             
    {
        if(col.gameObject.tag == "Player")
        {
            enemy.PlayerDamage(flyingDamage);                             //  ジャンプ攻撃の攻撃力をメソッドに渡す
            Debug.Log(flyingDamage);
        }
    }
}
