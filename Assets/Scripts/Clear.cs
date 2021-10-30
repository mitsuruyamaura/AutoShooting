using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Clear : MonoBehaviour {

    public static int clearLevel;      //  クリア回数＝レベル

    public int norma;                  //  敵の目標数
    public int addNorma;               //  レベルが上がるごとに増える敵の数
    int achievement;                   //  倒した敵の数
    public static bool isPenalty;      //  ペナルティ計算済かどうかのフラグ
    public bool SetPenalty;            //  ペナルティ要素を利用するかどうかのフラグ

    Text achieveText;                  //  倒した数のテキスト表示
    Text clearText;                    //  クリアした時のテキスト表示

    PlayerMove move;                   //  コンポーネント取得用
    [SerializeField] Timer timer;
    [SerializeField] LevelManager levelManager;
    [SerializeField] Score score;

    DownParameter dPara;

    // Use this for initialization
    void Start()
    {
        achieveText = GetComponent<Text>();   //  討伐数を表示するテキストを取得

        if (SetPenalty)                //  ペナルティ利用のフラグが入っているなら
        {            
            if (clearLevel > 0)        //  クリアレベルが1以上なら
            {
                //  ペナルティが確定していないならペナルティ発生と敵の数を増やす
                if (!isPenalty)
                {
                    //  パラメータ減少のメソッドを呼ぶ
                    dPara = GetComponent<DownParameter>();
                    dPara.ChangeStatus(clearLevel);

                    //  敵の数を増やす
                    norma += addNorma;

                    isPenalty = true;
                }
            }
        }        
	}
	
	// Update is called once per frame
	void Update () {
        //  画面に倒した敵の数とノルマを表示
        achieveText.text = achievement.ToString() + " / " + norma.ToString();
    }

    public void SetNorma(int value)            //  Spawner.csより呼ばれる。ノルマをセットする
    {
        norma = value;
    }

    public void GetAchievement()               //  EnemyManager.csより呼ばれる
    {
        achievement++;                         //  撃破数を加算
        if (achievement >= norma)              //  ノルマを超えたら
        {
            if (SceneManager.GetActiveScene().name == "GameScene2")   //  ボス戦なら
            {
                StageCleard();                                        //  クリア処理の呼び出し
                levelManager.StageClear();
            }
            else
            {
                FindObjectOfType<EnemyManager>().ChoiseSpawner();      //  EnemyManagerの生成場所命令メソッドを呼び出す
                achievement = 0;                                       //  初期化する
            }
        }
    }

    public void StageCleard()                                    //  EnemyManager.csより呼ばれる
    {    
        //  敵のカウントを非表示にする
        achieveText.enabled = false;

        //  クリア回数をカウント
        //clearLevel++;
        //Debug.Log(clearLevel);

        //  ペナルティを一旦解除
        isPenalty = false;

        //  クリア表示
        clearText = GameObject.Find("ClearText").GetComponent<Text>();
        clearText.text = "Stage Clear!!";

        //  ステージクリアアニメを再生するメソッドを呼び出す
        move = GameObject.FindGameObjectWithTag("Weapon").GetComponent<PlayerMove>();
        move.Win();

        //  ゲームの進行状態を更新してクリアした状態にする。制限時間を止める。残り時間を加算してハイスコアをセーブする
        timer.Stop();
        timer.AddScore();
        StartCoroutine("SaveTime");        
    }

    IEnumerator SaveTime()
    {
        yield return new WaitForSeconds(1.0f);
        score.Save();
    }
}
