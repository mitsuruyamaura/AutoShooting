using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour {

    [SerializeField] RotateCamera rCamera;
    [SerializeField] GameObject trailEffect;

    public float moveSpeed;          //  移動速度

    //  private
    float inputHorizontal;           //  X軸移動の入力制御用
    float inputVertical;             //  Z軸移動の入力制御用

    int nowAttack;                   //  攻撃アニメのトリガー設定用

    Rigidbody rb;                    //  各コンポーネント格納用
    Animator anim;
    Action action;
    LevelManager level;

    public enum MyState              //  ステートを設定
    {
        Normal,
        Damage,
        Attack
    };

    public static MyState state;    //  現在のステートの設定

    public bool isEnd;              //  キー操作無効用のフラグ。trueで無効
    bool isDamage;                  //  ダメージを受けているかどうかのフラグ


    // Use this for initialization
    void Start()
    {
        //  各コンポーネント取得
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        action = GetComponent<Action>();
        level = GameObject.Find("StartCountText").GetComponent<LevelManager>();

        //  現在のステートをNormalに設定
        state = MyState.Normal;
        trailEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //  ゲームスタートするまではキー操作無効
        if(level.stageState == 0)
        {
            return;
        }

        //  移動のキー入力受付用
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
    }

    //  キャラクターの移動
    private void FixedUpdate() {
        //  ゲームオーバーなら移動操作無効
        if (isEnd) {
            return;
        }

        //  Unityちゃんがゲームオーバーになっていなければキー入力有効
        //  現在のStateがNormalなら移動できるようにする
        //if(state == MyState.Normal) { 

        //  カメラの方向から、x-z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        //  方向キーの入力値とカメラの向きから移動方向を決定
        Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;

        //  移動方向にスピードをかける。ジャンプや落下がある場合は別途Y軸方向の速度ベクトルを足す
        rb.velocity = moveForward * moveSpeed + new Vector3(0, rb.velocity.y, 0);

        // キャラクターの向きを進行方向にする
        if (moveForward != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(moveForward);

            //  歩くアニメを再生する
            anim.SetFloat("Speed", 0.8f);
        } else {
            anim.SetFloat("Speed", 0f);
        }
    }

    //  Action.csより攻撃ボタンで呼ばれるメソッド
    public void SetState(MyState myState, int attack)
    {
        //  戻り値myStateがNormalなら攻撃アニメを停止し、stateをNormalに戻す
        if(myState == MyState.Normal)
        {
            anim.SetBool("Attack",false);
            state = MyState.Normal;
        }
        //  戻り値myStateがAttackなら攻撃アニメを再生しSEメソッドを呼び、stateをAttackにする
        else if (myState == MyState.Attack)
        {
            state = MyState.Attack;
            nowAttack = attack;
            if (nowAttack == 1)
            {
                anim.SetTrigger("Attack");
                trailEffect.SetActive(true);
            }
            if (nowAttack == 2)
            {
                anim.SetTrigger("Attack_B");
                trailEffect.SetActive(true);
            }
            this.gameObject.tag = "Weapon";
            action.SePlay();
            StartCoroutine("TrailEnd");
        }
        //Debug.Log(state);
        //Debug.Log(attack);
    }

    IEnumerator TrailEnd()
    {
        yield return new WaitForSeconds(0.5f);
        trailEffect.SetActive(false);
    }

    private void OnTriggerEnter(Collider col)
    {
        //  EnemyAttackタグに接触して、かつダメージを受けた（無敵状態）ではない場合
        if (!isDamage && col.gameObject.tag == "EnemyAttack")
        {
            //  無敵時間とノックバックの実行メソッドの呼び出し
            OnDamageEffect(200.0f);
        }
        //  ForceAttackタグに接触して、かつダメージを受けた（無敵状態）ではない場合
        if (!isDamage && col.gameObject.tag == "ForceAttack")
        {
            //  無敵時間とノックバックの実行メソッドの呼び出し
            OnDamageEffect(150.0f);
            anim.SetBool("Lose", true);
            EventSystem eventSystem = FindObjectOfType<EventSystem>();
            eventSystem.enabled = false;
        }
        if (!isDamage && col.gameObject.tag == "FlyingAttack")
        {
            //  無敵時間とノックバックの実行メソッドの呼び出し
            OnDamageEffect(500.0f);
        }
    }

    void OnDamageEffect(float knockPower)
    {
        //  ダメージ中状態のフラグを立てる
        isDamage = true;

        //  ノックバックさせる力
        float s = knockPower * Time.deltaTime;

        //  ノックバック実行
        this.gameObject.transform.Translate(Vector3.forward * -s);

        //コルーチンで無敵時間の管理をする
        StartCoroutine(WaitForIt());
    }

    //  isDamageは使用していない
    IEnumerator WaitForIt()
    {
        //  Layerを変更して敵と接触しないようにする
        this.gameObject.layer = LayerMask.NameToLayer("PlayerDamage");

        //  0.2秒間後にタグを戻す(敵に触れない状態にする)
        yield return new WaitForSeconds(0.5f);

        //  ダメージ中状態を解除
        isDamage = false;

        //  レイヤーをUnitychanに変更
        gameObject.layer = LayerMask.NameToLayer("Player");

        yield return new WaitForSeconds(2.0f);
        anim.SetBool("Lose", false);
        EventSystem eventSystem = FindObjectOfType<EventSystem>();
        eventSystem.enabled = true;
    }

    //  GameOver.csより呼ばれるゲームオーバー処理
    public void LoseControl()
    {
        // ゲームオーバーのアニメを再生する
        anim.SetBool("Lose", true);

        //　ゲーム終了のフラグを立てて移動のキー操作を無効にする
        isEnd = true;
    }

    //  Clear.csより呼ばれる
    public void Win()
    {
        //  キャラクターを正面向ける
        Vector3 temp = this.transform.localEulerAngles;
        temp.y = 180.0f;
        this.transform.localEulerAngles = temp;

        // ゲームクリアのアニメを再生する
        anim.SetBool("Win", true);

        //　ゲーム終了のフラグを立てて移動のキー操作を無効にする
        isEnd = true;

        rCamera.SetWin();
    }
}
