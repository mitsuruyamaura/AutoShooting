using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    [Header("移動速度の最小値")]
    public float minMoveSpeed;
    [Header("移動速度の最大値")]
    public float maxMoveSpeed;

    [Header("攻撃力の最小値")]
    public int minAttackPower;
    [Header("攻撃力の最大値")]
    public int maxAttackPower;

    private float moveSpeed;　　// 適用する移動速度
    public int attackPower;　　// 適用する攻撃力

    public enum ENEMY_STATE {
        SET_UP,
        WAIT,
        MOVE,
        ATTACK,
        RANDOM,
        READY
    }
    public ENEMY_STATE enemyState;

    private float actionTime;
    private Rigidbody rb;
    private Animator anim;

    public SearchArea searchArea;
    private Vector3 destinationPos;   // 索敵時の移動の目的地(Playerの位置情報)
    Vector3 direction;                // 移動する際の方向

    void Start() {
        // 移動速度などの値を設定する
        SetUpEnemyParameter();
    }

    /// <summary>
    　　/// 敵のパラメータ(移動速度や攻撃力など)と状態を設定
    　　/// </summary>
    private void SetUpEnemyParameter() {
        enemyState = ENEMY_STATE.SET_UP;
        Debug.Log(enemyState);

        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        attackPower = Random.Range(minAttackPower, maxAttackPower);
        actionTime = Random.Range(3, 6);

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        enemyState = ENEMY_STATE.WAIT;
        
        Debug.Log(enemyState);
        Debug.Log("MoveSpeed : " + moveSpeed);
        Debug.Log("AttakPower : "+ attackPower);
    }

    void Update() {
        // 準備状態。敵のパラメータの準備が終了していない場合にはUpdateを処理しない
        if (enemyState == ENEMY_STATE.SET_UP) {
            return;
        }

        actionTime -= Time.deltaTime;

        // 待機状態
        if (enemyState == ENEMY_STATE.WAIT) {
            //Debug.Log("待機中 あと : " + actionTime + " 秒");
            if (actionTime <= 0) {
                CheckNextAction();
                Debug.Log("待機終了");
                //NextAction(ENEMY_STATE.MOVE);
            }
        }

        // 移動状態
        if (enemyState == ENEMY_STATE.MOVE) {
            Move();
            //Debug.Log("移動中 あと : " + actionTime + " 秒");
            
            if (actionTime <= 0) {
                NextWait();
            }
        }

        // 索敵範囲内で目的地(Playerの位置)がある場合、目的地まで移動する
        if (enemyState == ENEMY_STATE.ATTACK) {
            Debug.Log(Vector3.Distance(transform.position, destinationPos));
            if (Vector3.Distance(transform.position, destinationPos) > 0.8f) {
                Move();
                Debug.Log("移動攻撃中");

            } else {
                // 目的地に着いたら攻撃
                enemyState = ENEMY_STATE.READY;
                StartCoroutine(Attack());
                Debug.Log("攻撃");
                return;
            }
        }
    }

    /// <summary>
    /// 次の行動を確認する
    /// </summary>
    private void CheckNextAction() {
        // 索敵範囲内にPlayerがいる場合
        if (searchArea.isSearching) {
            if (searchArea.player != null) {
                // Playerの位置まで移動して攻撃をする
                MoveAttack(searchArea.player);
                Debug.Log("移動して攻撃する");
            }
        } else {
            // 索敵範囲内にPlayerがいない場合、移動の準備をする
            PraparateMove();
            Debug.Log("移動準備");
        }
    }

    /// <summary>
    /// 索敵範囲内のPlayerの位置を目的地とし、移動する
    /// </summary>
    /// <param name="player"></param>
    private void MoveAttack(GameObject player) {
        // 目的地を設定しておく
        destinationPos = player.transform.position;
        // 移動する方向を設定する
        direction = (transform.position - destinationPos).normalized;
        // 移動する方向を向く
        transform.LookAt(destinationPos);
        // UpdateのAttackの部分で移動させる
        enemyState = ENEMY_STATE.ATTACK;
        anim.SetBool("Run", true);
    }

    /// <summary>
    /// 移動の準備
    /// </summary>
    private void PraparateMove() {
        // ランダムな方向を向く
        transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), new Vector3(0, 1, 0));
        direction = transform.position.normalized;
        enemyState = ENEMY_STATE.MOVE;
        actionTime = Random.Range(3, 5);
        anim.SetBool("Run", true);
    }

    /// <summary>
    /// 次回の行動までの待機
    /// </summary>
    /// <param name="nextState">ENEMY_STATE</param>
    private void NextWait() {
        anim.SetBool("Run", false);
        // 待機
        actionTime = Random.Range(3, 6);
        enemyState = ENEMY_STATE.WAIT;
    }

    /// <summary>
    /// 移動の処理
    /// </summary>
    private void Move() {
        rb.AddForce(-direction * moveSpeed);
        Debug.Log(rb.velocity);
    }

    /// <summary>
    /// 攻撃の処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator Attack() {
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(2.0f);
        NextWait();
    }
}
