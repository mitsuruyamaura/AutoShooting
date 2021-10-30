using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {

    public float gameTime;               //  ステージ毎のゲームの残り時間
    public int bonusRate;                //  ステージクリア時の残り時間にかける数数
    float timeCounter;                   //  現在の残り時間のカウント

    [SerializeField] Score score;

    Text timeText;
    LevelManager level;
    GameOver gameOver;

    // Use this for initialization
    void Start () {
        timeText = GetComponent<Text>();
        level = GameObject.Find("StartCountText").GetComponent<LevelManager>();
        if(SceneManager.GetActiveScene().name == "GameScene2")
        {
            timeCounter = gameTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (level.stageState)
        {
            case 0:
                return;

            case 1:
                //  ゲーム時間のカウント。画面にカウントを表示
                timeCounter -= Time.deltaTime;

                //  float型をint型にキャストする
                int intTime = (int)timeCounter;

                //  TimerオブジェクトのTextを取得して表示する
                timeText.text = intTime.ToString();

                //  
                if (timeCounter < 1)
                {
                    //  残り時間表示を非表示する
                    timeText.enabled = false;

                    //  ゲームオーバー選択表示
                    gameOver = GameObject.Find("GameOverCanvas").GetComponent<GameOver>();
                    gameOver.CanvasActive();

                    return;
                }
                break;

            case 2:
                //  残り時間表示を非表示する
                timeText.enabled = false;

                //  カウントダウンを止める
                return;

            default:
                break;
        }
    }

    public void Stop()                     //  カウントダウンを止める。ゲームオーバー(HpBar)、ステージクリア時(Clear)
    {
        //  残り時間表示を非表示する
        timeText.enabled = false;

        //  カウントダウンを止める
        return;
    }

    public void AddTime()                  //  追加の敵の生成時（レベル２と３）の際に呼ばれる
    {
        //Debug.Log(timeCounter);
        timeCounter += (gameTime-1);       //  制限時間を追加する
        //Debug.Log(timeCounter);
    }

    public void AddScore()　　　　　　　　 //　Clearより呼ばれる。残り時間をスコアに換算して加算する
    {
        int intTime = (int)timeCounter;    //  float型をint型にキャストする
        intTime *= bonusRate;              //  残り時間×10ポイントとする
        score.AddPoint(intTime);           //  Scoreに渡す 
    }
}
