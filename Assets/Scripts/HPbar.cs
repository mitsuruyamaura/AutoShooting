using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour {

    [SerializeField] SceneLoader sceneLoader;      //  現在のシーン取得用
    [SerializeField] LevelManager levelManager;    //  現在のレベルを取得
    [SerializeField] Timer timer;                  //  ゲームオーバー処理用
    [SerializeField] Score score;

    //public float maxHp;                    //  シーンをまたいでHPを利用する場合に使用する
    //public static float currentMaxHp;
    //public static bool isHpStart;

    public float initHp;                     //  HPの設定値
    public float setupHp;                    //　ゲーム内でのHPの最大値
    public float currentHp;                  //  HPの現在値
    public float HealCcoefficient;           //  回復係数

    public float[] decreaseHp;               //  レベルに応じた減少幅
    private float hpDamage;                  //  HPへのダメージ値

    Slider hpSlider;                         //  スライダーとHP現在値との連動制御用
    GameOver gameOver;                       //  ゲームオーバー処理制御用

    // Use this for initialization
    void Start()
    {
        hpSlider = GetComponent<Slider>();   //  HP表示用のバー
        SetHp();                             //  最大HPをセットする
    }

    public void SetHp()
    { 
        if (sceneLoader.sceneNum == 1)       //  ステージ１なら
        {
            switch (levelManager.initLevel)        //  レベルに合わせて最大HPを代入する
            {
                case 1:
                    setupHp = initHp;
                    setupHp *= decreaseHp[0];//1.0f;
                    break;

                case 2:
                    setupHp = initHp;
                    setupHp *= decreaseHp[1];// 0.90f;
                    break;

                case 3:
                    setupHp = initHp;
                    setupHp *= decreaseHp[2];//0.75f;
                    break;
            }
        }
        else                                 //  ステージ２なら
        {
            setupHp = initHp;
            setupHp *= decreaseHp[3];// 0.70f;
        }
        currentHp = setupHp;                 //  HPの現在値を現在の最大値にする
        //Debug.Log(currentHp);

        //　ゲーム開始時のみ代入             //  ステージを超えてHPを継続する場合に使用する
        //if (!isHpStart)
        //{
        //currentMaxHp = maxHp;
        //isHpStart = true;
        //}
        //currentHp = currentMaxHp;
        //hpSlider.maxValue = currentMaxHp;

        hpSlider.maxValue = setupHp;         //  HPバーの最大値をHPの最大値にする
        hpSlider.value = currentHp;          //  HPバーの現在値をHPの現在値にする
    }
	
    //  Enemy.csより呼ばれる
    public void Damage(float playerDamage)
    {
        hpDamage = playerDamage;             //  渡ってきた数値を代入する
        //Debug.Log(playerDamage);
        //Debug.Log(hpDamage);

        if (hpDamage > 0)                    //  ダメージが0以上なら
        {
            if (currentHp > 0)               //  Hpが0以上あるなら
            {
                currentHp -= hpDamage;       //  ダメージ処理
                Debug.Log("プレイヤーは " + hpDamage + " のダメージを受けた。残りhpは " + currentHp);

                hpSlider.value = currentHp;     //  スライダーの値を現在値にする

                if (currentHp <= 0)    //  Hpが0以下になったら
                {
                    currentHp = 0;

                    //  ゲームオーバー選択表示。残り時間のカウントを止めて、ハイスコアをセーブする
                    gameOver = GameObject.Find("GameOverCanvas").GetComponent<GameOver>();
                    gameOver.CanvasActive();
                    timer.Stop();
                    score.Save();
                }
            }
        }
    }

    public void AddLife()   //  MPBar.csより呼ばれる
    {
        if (currentHp > 0)              //  Hpが1以上残っているなら
        {
            float recovery = setupHp * HealCcoefficient;   //  回復量を決める。現在の最大HP値×係数
            currentHp += recovery;                         //  回復させる
            if(currentHp > setupHp)                        //  HP現在値が最大値を超えたら
            {
                currentHp = setupHp;     //  現在値を最大値にする
            }
            hpSlider.value = currentHp; //  スライダーの値を現在値にする
            Debug.Log("プレイヤーのHP " + recovery + " 回復。残りhpは " + currentHp);
        }
    }
}
