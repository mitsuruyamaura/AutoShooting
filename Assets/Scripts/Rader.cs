using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;                    //  NavMeshAgentを使うため

public class Rader : MonoBehaviour {

    private float discovery;             //  敵がプレイヤーを発見率する確率。生成時に取得したランダムな数値を格納
    public  bool isFind;                 //  発見しているか、どうかのフラグ
    private Vector3 startPos;            //  生成された位置の保存用。スタート地点に戻るための数値
    private Vector3 targetPos;           //  プレイヤーの位置の取得用。NavMeshを使うので、追尾する
    private Vector3 target;              //  追尾する目標、あるいはスタート地点を目標をいれる
    private float walkSpeed;             //  プレイヤーを追尾する移動速度。生成時に取得したランダムな数値を格納
    private CapsuleCollider cCol;        //  索敵範囲
    private float nonFindTime;           //  発見できない時間
    private bool isNotFind;              //  発見できないフラグ

    NavMeshAgent agent;                  //  NavMeshAgent用の変数
    Animator anim;                       //  アニメの制御用
    Enemy enemy;

    private void Start()
    {
        enemy = GetComponent<Enemy>();                           //  state変更用
        anim = GetComponent<Animator>();                         //  アニメ制御用のコンポーネント格納
        discovery = GetComponent<Enemy>().enemyDiscoveryRate;    //  ランダムな発見率を入れる
        walkSpeed = GetComponent<Enemy>().enemyMoveSpeed;        //  ランダムな移動速度を入れる
        cCol = GetComponent<CapsuleCollider>();                  //  コンポーネント格納
        startPos = transform.position;                           //  生成された位置を入れる
        agent = GetComponent<NavMeshAgent>();                    //  コンポーネント格納
        agent.speed = walkSpeed;                                 //  取得した移動速度をNavMeshのSpeedにいれる
        target = startPos;                                       //  初期の目標を入れる。スタート地点にして移動させずにおく
     }

    private void Update()
    {        
        agent.destination = target;                              //  targetをNavMeshの目標にセット。これにより追尾する
        
        if (isNotFind)                                           //  発見していないフラグが立ったら
        { 
            nonFindTime += Time.deltaTime;                       //  待機時間を加算する
            if(nonFindTime > 5.0f)　　                           //  5秒待機したら
            {
                isNotFind = false;                               //  発見していないフラグを降ろす
                cCol.enabled = true;                             //  索敵範囲をアクティブにする
            }
        }

        if(agent.destination.x == startPos.x)                    //  スタート地点に戻ったら
        {
            anim.SetFloat("Speed", 0.0f);                        //  移動アニメを停止
            enemy.state = Enemy.EnemyState.wait;                 //  敵のStateを待機に変更
        }
    }

    private void OnTriggerStay(Collider col)                         //  索敵範囲内にプレイヤーが入ったかどうか見る
    {
        if (!isFind)                                                 //  プレイヤーを発見していないなら
        {
            if (col.gameObject.tag == "Player")                      //　索敵範囲内にプレイヤーがいるか判定
            {
                int randomValue = Random.Range(0, 100);              //  発見したかどうかの判定値をランダムで取る
                if (randomValue <= discovery)                        //  発見率よりも判定値が低いなら発見
                {
                    isFind = true;                                   //  発見したフラグを立てる
                    targetPos = col.gameObject.transform.position;   //  targetPosにプレイヤーの位置を入れる
                    target = targetPos;                              //  追尾用のtargetにtargetPosを入れて追尾させる
                    enemy.state = Enemy.EnemyState.chase;            //  敵のStateを追尾に変更
                    anim.SetFloat("Speed", 0.8f);                    //  走るアニメの再生
                    Debug.Log(isFind);      
                    Debug.Log(randomValue);
                }
                else                                                 //  発見しなかった場合
                {
                    isNotFind = true;                                //  発見しなかったフラグをたてる
                    cCol.enabled = false;                            //  索敵範囲を非アクティブにする
                }
            }
        }
    }

    private void OnTriggerExit(Collider col)         //  索敵範囲外にプレイヤーが出たかどうかを見る
    {
        if (col.gameObject.tag == "Player")
        {
            isFind = false;                          //  発見フラグをおろす
            target = startPos;                       //  追尾用のtargetにstartPosを入れてスタート地点に戻させる
            anim.SetFloat("Speed", 0.4f);            //  歩くアニメの再生
            Debug.Log(isFind);
        }
    }
}
