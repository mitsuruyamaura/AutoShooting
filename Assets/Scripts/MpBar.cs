using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MpBar : MonoBehaviour {

    [SerializeField] SceneLoader sceneLoader;      //  現在のシーン取得用
    [SerializeField] LevelManager levelManager;    //  現在のレベルを取得
    [SerializeField] HPbar hpBar;                  //  HP回復メソッド呼び出し用 
    [SerializeField] PlayerMove move;              //  キー操作無効の制御用

    public float waitTime;            //  MP1回復までの準備時間
    public float initMp;              //  MPの設定値
    private float setupMp;            //　ゲーム内でのMPの最大値
    private float currentMp;          //  MPの現在値
    private float healtimer;          //  MP回復までのカウント
    public float costMp;              //  HP回復のためのMP消費量
    public float recoveryMp;          //  MP回復量
    public float[] decreaseMp;        //  レベルに応じた減少幅

    Slider mpSlider;                  //  スライダーとMP現在値との連動制御用

    // Use this for initialization
    void Start()
    {
        mpSlider = GetComponent<Slider>();   //  MP表示用のスライダーを取得
        SetMp();                             //  最大MPをセットする
    }

    public void SetMp()
    {
        if (sceneLoader.sceneNum == 1)       //  ステージ１なら
        {
            switch (levelManager.initLevel)        //  レベルに合わせて最大HPを代入する
            {
                case 1:
                    setupMp = initMp;
                    setupMp *= decreaseMp[0];//1.0f;
                    break;

                case 2:
                    setupMp = initMp;
                    setupMp *= decreaseMp[1]; //0.8f;
                    break;

                case 3:
                    setupMp = initMp;
                    setupMp *= decreaseMp[2]; //0.6f;
                    break;
            }
        }
        else
        {
            setupMp = initMp;
            setupMp *= decreaseMp[3]; //0.6f;　　              //  ステージ２なら
        }
        currentMp = setupMp;                  //  MPの現在値を現在の最大値にする
        //Debug.Log(currentMp);
        
        mpSlider.maxValue = setupMp;          //  スライダーの最大値をsetupMpにする
        mpSlider.value = currentMp;           //  スライダーの現在値をcurrentMpにする 
    }

    // Update is called once per frame
    void Update()
    {
        if (move.isEnd)                      //  PlayerMoveよりフラグを取得。ゲームオーバーならカウント、キー入力無効
        {
            return;
        }
        if (currentMp != setupMp)              //  MPが最大値でないなら
        {
            healtimer += Time.deltaTime;       //  回復準備タイマーをカウントアップ
            //Debug.Log(healtimer);
            if (healtimer > waitTime)          //  待機時間を超えたら
            {
                currentMp += recoveryMp;       //  recoveryMp分のMP回復
                mpSlider.value = currentMp;    //  スライダーの現在値をcurrentMpにする
                healtimer = 0;
                Debug.Log(currentMp);
            }
        }

        if (Input.GetButtonDown("Heal"))                  //  Healボタンが押されたら
        {
            if (hpBar.currentHp != hpBar.setupHp)         //  現在HPが現在の最大値でないなら
            {
                if (currentMp >= costMp)                  //  MPの現在値がコスト以上なら
                {
                    currentMp -= costMp;                  //  MPの現在値を減少させる
                    mpSlider.value = currentMp;      　　 //  スライダーの現在値をcurrentMpにする
                    hpBar.AddLife();                      //  HP回復メソッドの呼び出し
                    //Debug.Log(currentMp);
                }
            }
        }
    }
}
