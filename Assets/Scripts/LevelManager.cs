using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public int initLevel;                   //  現在のレベル
    public int maxLevel;                    //  レベルの最大数

    public int stageState;                  //  ゲームの進行状態。0=ゲーム開始前、1=プレイ中、2=クリア状態     
    public float startCount;                //  ゲーム開始前の事前カウント
    Text startText;                         //  ゲームスタートまでのカウントダウン表示用

    Score score;                            //  スコア管理用
    public SceneLoader sceneLoader;                //  シーン管理用

    private void Awake()
    {
        startText = GetComponent<Text>();   //  コンポーネント格納
    }

    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (stageState == 0)                       //  ゲーム開始前の状態なら
        {
            startCount -= Time.deltaTime;          //  画面にゲーム開始までの事前カウントをカウントダウンして表示
        
            int intTime = (int)startCount;         //  float型をint型にキャストする
            startText.text = intTime.ToString();   //  スタートまでのカウントダウンを表示

            if (startCount < 1)                    //  カウントが1以下になったら
            {
                startCount = 0;
                DisplayLevel();                    //  LEVEL表示するメソッド
                stageState = 1;                    //  ゲーム進行状態をプレイ中に変更する
            }
        }
    }

    public void DisplayLevel()             //  Levelを画面に表示するメソッド。EnemyManagerより呼ばれる
    {
        if (sceneLoader.sceneNum == 1)     //  ステージとレベルによって表示内容を切り替える
        {
            startText.text = "LEVEL " + initLevel.ToString() + " Start!!";
            StartCoroutine("DisplayTime");
        }
        else
        {
            startText.text = "Boss Battle Start!!";
            StartCoroutine("DisplayTime");
        }
    }

    //  Clear.csより呼ばれる
    public void StageClear()
    {
        //  クリア状態にする
        stageState = 2;
        //Debug.Log(stageState);

        //  ハイスコアを保存する
        score = GameObject.Find("ScoreManager").GetComponent<Score>();
        score.Save();

        if (SceneManager.GetActiveScene().name == "GameScene1")
        {
            //  シーン呼び出しのコルーチン
            StartCoroutine("BossStage");
        }
        else
        {
            StartCoroutine("TitleReturn");
        }

    }

    IEnumerator BossStage()                              //  ボス戦への遷移処理
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("GameScene2");
    }

    IEnumerator DisplayTime()                            //  LEVELの表示時間の制御
    {
        yield return new WaitForSeconds(3.5f);
        startText.text = " ";                            //  Level表示を消す
    }
     
    IEnumerator TitleReturn()                            //  タイトル画面への遷移処理  
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("Title");
    }
}
