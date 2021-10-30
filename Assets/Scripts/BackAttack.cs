using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAttack : MonoBehaviour {

    GameObject dragon;                      //  ゲームオブジェクトのDarkDragon取得用
    GameObject playerObj;
    CapsuleCollider cCol;                   //  アタッチされているオブジェクトのコライダー（攻撃範囲）を取得
    CapsuleCollider searchCol;              //  索敵用コライダーを取得
    Enemy enemy;                            //  スクリプト取得用
    [SerializeField] AttackForce attackForce;    　　　　　　//  スクリプト取得用
    [SerializeField] FlyingAttack flyingAttack;              //  スクリプト取得用

    private void Awake()
    {
        dragon = GameObject.Find("DarkDragon");                           //  ドラゴンを取得
        if (!dragon)                                                      //  変数に格納されていないなら
        {
            return;                                                       //  何もしない
        }
        //Debug.Log("GetDragon");       
    }

    // Use this for initialization
    void Start()
    {
        cCol = GetComponent<CapsuleCollider>();                           //  攻撃範囲のコライダーを取得
        searchCol = transform.parent.GetComponent<CapsuleCollider>();     //  親のコライダーを取得
        enemy = transform.parent.GetComponent<Enemy>();                   //  親のスクリプトを取得        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")                               //  攻撃範囲内でプレイヤーと接触したら
        {
            if (!enemy.isAttack)                                            //  攻撃中でないなら
            {
                playerObj = col.gameObject;
                if (dragon)     　　　　　　　　　　　　　　　　　　　　//  このオブジェクトがドラゴンなら
                {
                    int randomValue = Random.Range(0, 8);               //  ランダムに攻撃方法を決定する
                    Debug.Log(randomValue);
                    if (0 <= randomValue && randomValue > 2)
                    {
                        enemy.EnemyAttack();                             //  Atack1の呼び出し
                        StartCoroutine("Wait");
                        Debug.Log("NormalAttack");
                    }
                    if (2 <= randomValue && randomValue < 4)
                    {
                        attackForce.Force(playerObj);                    //  Attack2の呼び出し
                        enemy.isAttack = true;
                        StartCoroutine("ResetAttack");
                    }
                    else
                    {
                        flyingAttack.Flying();                           //  Attack3の呼び出し
                        enemy.isAttack = true;
                        StartCoroutine("ResetAttack");
                    }
                }
                if (!dragon)                                                    //  ドラゴン以外なら
                {
                    enemy.EnemyAttack();                                //  攻撃用メソッドの呼び出し
                    StartCoroutine("Wait");
                }
            }
        }
    }

    IEnumerator Wait()
    {                                                   //  連続ダメージ発生を防ぐため
        yield return new WaitForSeconds(0.5f);          //  5秒だけ索敵範囲と攻撃範囲を非アクティブにする
        cCol.enabled = false;
        searchCol.enabled = false;

        yield return new WaitForSeconds(5.0f);
        cCol.enabled = true;
        searchCol.enabled = true;
    }

    IEnumerator ResetAttack()                           //  ドラゴン用　攻撃判定をおろす
    {
        yield return new WaitForSeconds(3.0f);
        enemy.isAttack = false;
    }
}
