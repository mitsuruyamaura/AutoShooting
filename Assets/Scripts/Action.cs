using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    [SerializeField]
    ChangeEquip equip;

    Vector3 velocity;                   //  コンポーネント格納用
    Rigidbody rb;
    Animator anim;

    PlayerMove move;
    LevelManager level;

    AudioSource sound1;                 //  SE
    AudioSource sound2;
    AudioSource sound3;

    public float jumppower = 300f;      //  ジャンプ力
    public LayerMask groundLayer;       //  Linecastで判定を行うLayerの指定

    bool isSE;                          //  攻撃時とジャンプ時のSE判定
    bool isAttack;                      //  攻撃判定
    bool isGrounded;                    //  着地判定
    int attack;                         //  攻撃の種類。アニメのトリガー用にPlayerMoveへ渡す


    // Use this for initialization
    void Start()
    {
        //  各コンポーネントの取得
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        move = GetComponent<PlayerMove>();

        level = GameObject.Find("StartCountText").GetComponent<LevelManager>();

        AudioSource[] audioSources = GetComponents<AudioSource>();
        sound1 = audioSources[0];
        sound2 = audioSources[1];
        sound3 = audioSources[2];        
    }

    // Update is called once per frame
    void Update() {
        //  ゲームスタートするまではキー操作無効
        if (level.stageState == 0) {
            return;
        }

        //  PlayerMoveよりフラグを取得
        //  ゲームオーバーならキー入力無効
        if (move.isEnd) {
            return;
        }
     
        //  Unityちゃんがゲームオーバーになっていなければキー入力有効
        if (PlayerMove.state == PlayerMove.MyState.Normal) {
            //  ジャンプ処理
            //  Animatorのjumpステートの確認をし、Jumpしていない状態ならJumpをfalseにセット
            if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Jump")) {
                this.anim.SetBool("Jump", false);
            }

            //  Linecastでキャラの足元に地面があるか判定
            isGrounded = Physics.Linecast(
            transform.position + transform.up * 1,
            transform.position - transform.up * 0.3f,
            groundLayer);

            //  キー入力のJumpで反応（GetButton）スペースキー(GetKey)
            if (Input.GetButtonDown("Jump")) {
                //  着地していたとき
                if (isGrounded) {
                    //  着地判定をfalse
                    isGrounded = false;

                    //  Jumpステートへ遷移してジャンプアニメを再生
                    anim.SetTrigger("Jump");

                    //  AddForceにて上方向へ力を加える
                    rb.AddForce(Vector3.up * jumppower);

                    SePlay();
                }
            }
        }

        //  Locomotion状態になったらWeaponタグをPlayerに戻す
        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) {
            move.SetState(PlayerMove.MyState.Normal, 0);
            this.gameObject.tag = "Player";
            equip.Disarm();
        }

        //  Zボタンを押したら攻撃
        if (Input.GetButtonDown("Action")) {
            attack = 1;
            move.SetState(PlayerMove.MyState.Attack, attack);
        }

        //  Xボタンを押したら攻撃
        if (Input.GetButtonDown("Action2")) {
            attack = 2;
            move.SetState(PlayerMove.MyState.Attack, attack);
        }
    }
   
    // ヒット時のアニメーションイベント（今のところからっぽ。ないとエラーが出る）
    public void Hit()
    {

    }

    public void FootR()
    {

    }

    public void FootL()
    {

    }

    //  ジャンプ時と攻撃時のSE再生メソッド
    public void SePlay()
    {
        equip.WeaponAttack();       //　攻撃メソッド実行

        if (!isSE)                  //  SEがなっていないなら
        {
            //  SEで再生する掛け声をランダムで選ぶ
            int randomValue = Random.Range(0, 3);

            if (randomValue == 0)
            {
                sound1.Play();
            }
            else if (randomValue == 1)
            {
                sound2.Play();
            }
            else
            {
                sound3.Play();
            }
            isSE = true;            //  フラグをたてる
            StartCoroutine("SeStop");
        }
    }

    IEnumerator SeStop()
    {
        yield return new WaitForSeconds(0.5f);
        isSE = false;
    }
}
