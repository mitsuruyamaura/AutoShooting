using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour {

    public static int score;             //  現在のスコア。シーン間で引き継ぐ
    public static int highScore;         //  今までのハイスコア。スコアが超えた時に更新。シーン間で引き継ぎ、保存する

    Text scoreText;                      //  スコア表示用
    Text highScoreText;                  //  ハイスコア表示用

    string HIGH_SCORE_KEY = "highScoreKey";   //  保存用のキー

    // Use this for initialization
    void Start () {
        if (SceneManager.GetActiveScene().name == "GameScene1")   //  ゲームスタート時のみ初期化する
        {
            Initialize();
        }

        //  スコアとハイスコア表示用のコンポーネント取得する
        scoreText = transform.GetChild(0).gameObject.GetComponent<Text>();
        highScoreText = transform.GetChild(1).gameObject.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
		//  スコアがハイスコアよりも大きくなったら更新する
        if(highScore < score)
        {
            highScore = score;
        }

        //  スコアとハイスコアを表示する
        scoreText.text = score.ToString();
        highScoreText.text = highScore.ToString();

        //  ハイスコアのリセット。外部からも呼べる
        if(Input.GetKeyDown("left ctrl"))
        {
            HighScoreReset();
        }
	}

    public void Initialize()                        //  ゲーム開始時の場合に呼ばれる
    {
        //  スコアを0に戻す
        score = 0;

        //  ハイスコアを取得する。保存されていなければ0を取得する
        highScore = PlayerPrefs.GetInt("highScoreKey", 0);
    }

    //  敵を倒した場合とステージをクリアした場合に呼ばれる
    public void AddPoint(int scorePoint)
    {
        score += scorePoint;
        Debug.Log(score);
    }

    //  ゲームを終了する場合とクリアした場合に呼ばれる(Clear)
    public void Save()
    {
        //  ハイスコアを保存する
        PlayerPrefs.SetInt("highScoreKey", highScore);
        PlayerPrefs.Save();
        Debug.Log("HighScore Saved");
    }

    [ContextMenu("HighScoreReset")]
    private void HighScoreReset()
    {
        PlayerPrefs.DeleteAll();
        //Debug.Log("HighScore Reset!!");
    }
}
