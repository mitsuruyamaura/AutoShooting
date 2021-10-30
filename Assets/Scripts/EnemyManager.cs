using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour {

    public Spawner[] spawners;                     //  生成を実行するスポナー
    List<int> numbers = new List<int>();           //  乱数を入れるリスト

    [SerializeField] LevelManager levelManager;    //  コンポーネント格納用
    [SerializeField] Clear clear;
    [SerializeField] Timer timer;
    [SerializeField] HPbar hpBar;
    [SerializeField] MpBar mpBar;

    //float latencyTime1;                          //  各スポナーの生成待機時間
    //float latencyTime2;
    //float latencyTime3;
    //float latencyTime4;

    // Use this for initialization
    void Start() {
        for (int i = 0; i < spawners.Length; i++)        //  スポナーの要素数分のリストを作る
        {
            numbers.Add(i);                              //  要素数を追加する
            Debug.Log(i);
        }
        ChoiseSpawner();                                 //  敵生成をするスポナーを決定メソッド呼び出し
    }

    public void ChoiseSpawner()
    {                            //  レベルを加算する
        if (levelManager.initLevel == levelManager.maxLevel)  //  クリアレベルになっているなら
        {
            levelManager.StageClear();                        //  クリア用メソッド呼び出し
            clear.StageCleard();                              //  クリア用メソッド呼び出し
            return;
        }
        else                                                   //  クリアレベルではないなら
        {
            levelManager.initLevel++;                          //  レベルを加算する
            Debug.Log(levelManager.initLevel);
            hpBar.SetHp();                                     //  最大HPをセットし直す
            mpBar.SetMp();                                     //  最大MPをセットし直す
            levelManager.DisplayLevel();                       //  現在のレベルを画面上に表示するメソッドの呼び出し
            timer.AddTime();                                   //  残り時間を追加するメソッドの呼び出し。
            int index = Random.Range(0, numbers.Count -1);     //  Listの要素数を上限にindexに取得（その時点での最大数）
            int randomValue = numbers[index];                  //  乱数として要素（index）を取り出し、randomValueに入れる
            numbers.RemoveAt(index);                           //  indexの要素をListより除外する（１つずつ詰められる）＝重複しない
            CreateOrder(randomValue);                          //  敵生成の命令を出すメソッド呼び出し
        }
    }

    public void CreateOrder(int randomValue)
    {
        spawners[randomValue].EnemyCreate();            //  乱数をスポナーの要素数とし、敵を生成するメソッド実行
    }
}

    // Update is called once per frame
    //void Update()
    //{
        //if(maxCount < totalEnemyCount)      //  最大数を超えたら生成しない
        //{
            //return;
        //}

        //if (!spawner1.isGenerate)                           //  生成していないなら
        //{
            //var randomValue = Random.Range(1.5f,3.0f);
            //latencyTime1 += Time.deltaTime;                 //  生成カウント開始
            //if (latencyTime1 > randomValue)                 //  ランダム秒超えたら    
            //{
                //spawner1.EnemyCreate();                     //  敵を生成するメソッド実行
                //latencyTime1 = 0;                           //  待機カウントを０に戻す
            //}
        //}

        //if (!spawner2.isGenerate)                           //  生成していないなら
        //{
            //var randomValue = Random.Range(1.5f, 3.0f);
            //latencyTime2 += Time.deltaTime;                 //  生成カウント開始
            //if (latencyTime2 > randomValue)                 //  ランダム秒超えたら    
            //{
                //spawner2.EnemyCreate();                     //  敵を生成するメソッド実行
                //latencyTime2 = 0;                           //  待機カウントを０に戻す
            //}
        //}

        //if (!spawner3.isGenerate)                           //  生成していないなら
        //{
            //var randomValue = Random.Range(1.5f, 3.0f);
            //latencyTime3 += Time.deltaTime;                 //  生成カウント開始
            //if (latencyTime3 > randomValue)                 //  ランダム秒超えたら    
            //{
                //spawner3.EnemyCreate();                     //  敵を生成するメソッド実行
                //latencyTime3 = 0;                           //  待機カウントを０に戻す
            //}
        //}

        //if (!spawner4.isGenerate)                           //  生成していないなら
        //{
            //var randomValue = Random.Range(1.5f, 3.0f);
            //latencyTime4 += Time.deltaTime;                 //  生成カウント開始
            //if (latencyTime4 > randomValue)                 //  ランダム秒超えたら    
            //{
                //spawner4.EnemyCreate();                     //  敵を生成するメソッド実行
                //latencyTime4 = 0;                           //  待機カウントを０に戻す
            //}
        //}
   // }

