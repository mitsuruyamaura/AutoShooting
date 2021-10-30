using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] CapsuleCollider capsuleCol;              //  攻撃の当たり判定用
    [SerializeField] ParticleSystem hitEffect1;               //  ヒットエフェクト#1-#2
    [SerializeField] ParticleSystem hitEffect2;

    public float enemyHp;                                     //  敵のHP
    public float enemyOffensive;                              //  敵の攻撃力
    public float enemyDefense;                                //  敵の防御力
    public float enemyMoveSpeed;                              //  敵の移動速度
    public float enemyDiscoveryRate;                          //  索敵範囲に入ったプレイヤーを発見する確率
    public int enemyScore;                                    //  倒した際に加算されるスコア
    public bool isGetStatus;                                  //  ランダムスタータスを取得確認用
    public bool isAttack;                                     //  攻撃中かどうかの確認フラグ
    public float knockBackPower;                              //  攻撃を受けた際にノックバックする力

    private float count = 0;                                  //  待機時間のタイマー
    private float randomCount;                                //  待機時間の目標カウント

    Rigidbody rb;                                             //  このオブジェクトにアタッチしてある各コンポーネント格納用
    Animator anim;
    AudioSource sound1;

    Clear clear;                                              //  外部コンポーネント格納
    HPbar hpBar;
    Score score;
    Spawner spawner;
    Rader rader;

    public enum EnemyState                                    //  敵の状態の遷移用
    {
        wait,
        attack,
        chase
    };

    public EnemyState state;                                  //  現在の敵の状態の管理用

    // Use this for initialization
    void Start()
    {        
        rb = GetComponent<Rigidbody>();                              //  各コンポーネント取得 
        anim = GetComponent<Animator>();
        rader = GetComponent<Rader>();
        spawner = transform.parent.GetComponent<Spawner>();          //  親のスクリプトを取得
        if (!spawner)
        {
            return;
        }
        AudioSource[] audioSources = GetComponents<AudioSource>();
        sound1 = audioSources[0];
        if (!isGetStatus)                                            //  ランダムなステータスを取得するかどうか
        {
            SetStatus();                                             //  獲得する場合にはメソッド呼び出し
        }
        state = EnemyState.wait;                                     //  現在の状態を待機にする
        randomCount = Random.Range(4, 6);                            //  初回の待機時間をランダムで設定する
    }

    void SetStatus()                                               //  生成された敵のステータスをランダムで設定する   
    {
        float randomAttackValue = Random.Range(210, 270);          //  float型はmaxが引数に含まれる
        enemyOffensive = randomAttackValue;                        //  攻撃力

        float randomDefenseValue = Random.Range(210, 270);
        enemyDefense = randomDefenseValue;                         //  防御力

        float randomMoveValue = Random.Range(2.0f, 4.0f);
        enemyMoveSpeed = randomMoveValue;                          //  移動速度

        int randomRateValue = Random.Range(0, 3);                  //  int型はmaxが含まれない
        if (randomRateValue == 0)                                  //  発見率
        {
            enemyDiscoveryRate = 50;
        }
        else if (randomRateValue == 1)
        {
            enemyDiscoveryRate = 33;
        }
        else
        {
            enemyDiscoveryRate = 25;
        }

        int randomScoreValue = Random.Range(0, 4);
        if (randomScoreValue == 0)                                   //  スコア
        {
            enemyScore = 75;
        }
        else if (randomScoreValue == 1)
        {
            enemyScore = 100;
        }
        else if (randomScoreValue == 2)
        {
            enemyScore = 125;
        }
        else
        {
            enemyScore = 150;
        }
        isGetStatus = true;                                          //  ステータスのセットが終わったフラグを立てる
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGetStatus)                         // 生成時、ランダムステータスを取得するまでUpdateは実行しない
        {
            return;
        }

        if (isGetStatus)
        {
            if (state == EnemyState.wait)         //  待機中なら
            {
                count += Time.deltaTime;          //  待機用のカウントを開始
               
                //  4-6秒ごとに待機か移動する
                if (count > randomCount)
                {
                    var randomValue = Random.Range(0, 2);     //  ランダムな行動をとらせる
                    if (randomValue == 0)
                    {
                        anim.SetTrigger("Loiter");            //  見張る
                    }
                    else
                    {
                        float roteY = Random.Range(0,360); //  ランダムな方向に向きを変える
                        transform.rotation = Quaternion.Euler(0.0f, roteY, 0.0f);
                        //float randomMoveValue = Random.Range(2.0f, 4.0f);
                        //enemyMoveSpeed = randomMoveValue;                          //  移動速度
                        //rb.velocity = new Vector3(0.0f, 0.0f,enemyMoveSpeed + transform.position.z);
                        //anim.SetFloat("Speed", 0.4f);
                        //StartCoroutine("Move",randomCount);
                    }                   
                    randomCount = Random.Range(4, 6);         //  再度待機の準備を行う
                    count = 0;                                //  タイマーを0に戻す
                }
            }
        }
    }

    public void EnemyAttack()                    //  AttackRange.csより呼ばれる
    {
        state = EnemyState.attack;               //  敵のstateを攻撃に変える 
        Debug.Log(state);                    
        anim.SetTrigger("Attack");               //  攻撃アニメを再生する
        isAttack = true;                         //  攻撃中のフラグを立てて、続けて呼ばないようにする
        this.gameObject.tag = "EnemyAttack";     //  敵のタグを変える

        StartCoroutine("OnAttack");
    }

    public void PlayerDamage(float damage)　　　 //  Playerにダメージをあたえるメソッド
    {
        sound1.Play();            //  SE再生

        //  プレイヤーの防御力を取得し、敵の攻撃力より減算する
        float playerDamage = enemyOffensive - BaseStates.currentDefensePower;

        //  減算した値playerDamageをHpBarへ渡す
        hpBar = GameObject.Find("HpSlider").GetComponent<HPbar>();
        hpBar.Damage(playerDamage);
        Debug.Log(playerDamage);
    }

    private void OnCollisionEnter(Collision col) //  当たり判定
    {
        if (col.gameObject.tag == "Player")　　　//  プレイヤーと接触したら
        {
            PlayerDamage(enemyOffensive);        //  攻撃力を渡してメソッド呼び出し
        }

        if (col.gameObject.tag == "Weapon")      //  攻撃されたか判定
        {
            if (enemyHp > 0)                     //  Hpが0以上あるなら
            {
                //  ダメージの基本値を取得する
                float damage = BaseStates.currentOffensivePower;
                //Debug.Log(damage);

                //  ダメージ基本値より防御力を減算し、Hpより減算する
                damage -= enemyDefense;
                //Debug.Log(damage);

                if (damage > 0)    //  ダメージが0より大きければHPより減算する(マイナスだとHPが増えてしまうため)
                {
                    enemyHp -= damage;
                    Debug.Log("プレイヤーは " + damage + " のダメージを与えた。敵の残りhpは " + enemyHp);

                    OnDamage();    //  被ダメージ時のメソッド呼び出し
                }
            }
        }
    }
    
    void OnDamage()                //  被ダメージのメソッド。ノックバック、破壊、スコア加算を行う
    {        
        sound1.Play();             //  SE再生と被ダメージのエフェクト再生
        hitEffect1.Play();
        hitEffect2.Play();

        //  ノックバックさせる力を決定
        float s = knockBackPower * Time.deltaTime;

        //  ノックバック実行
        this.gameObject.transform.Translate(Vector3.forward * -s);

        if (enemyHp <= 0)     //  Hpが0以下なら
        {
            anim.SetTrigger("Death");        //  破壊アニメ再生し、破壊する
            Destroy(this.gameObject,2.0f);

            //  敵を倒した数をカウントさせる
            clear = GameObject.Find("NormaText").GetComponent<Clear>();
            clear.GetAchievement();

            //  スコアを加算する
            score = GameObject.Find("ScoreManager").GetComponent<Score>();
            score.AddPoint(enemyScore);
        }
        else   //  HPが残っているならダウンアニメ再生してコルーチン呼び出し
        {
            anim.SetTrigger("Down");
            StartCoroutine("WaitTime");
        }
    }

    private void OnDestroy()
    {
        spawner.DestroyCheck();                    //  討伐数をカウントし、フラグを降ろす
    }

    IEnumerator WaitTime()                         //  ヒットエフェクト解除用のコルーチン
    {
        yield return new WaitForSeconds(0.1f);
        hitEffect1.Stop();                         //  ヒットエフェクトを停止する  
        hitEffect2.Stop();
    }

    IEnumerator OnAttack()
    {
        yield return new WaitForSeconds(1.0f);
        isAttack = false;                           //  攻撃中のフラグをおろす
        state = EnemyState.wait;                    //  状態を待機にする
        this.gameObject.tag = "Enemy";              //  タグを戻す
    }

    IEnumerator Move(float value)
    {
        yield return new WaitForSeconds(value);
        enemyMoveSpeed = 0.0f;                      //  移動をやめさせる
    }
}
