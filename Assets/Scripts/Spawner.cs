using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public float roteY;                //  生成時のRotation.Yの位置のオフセット用
    public bool isGenerate;            //  生成中かどうかのフラグ。Trueなら生成をしない
    public bool isDestroyed;           //  このスポナーの敵が全滅したかどうかのフラグ
    int applyNum;

    [SerializeField] private RangeInteger[] m_ints;　　　　//　外部の構造体を参照して、最小値と最大値を持つ配列を作る

    GameObject[] enemys;
    public GameObject enemyPrefab;     //  敵のプレファブ元
    //GameObject enemy;                //  インスタンスを入れる

    private int destroyCount;
    private int normaCount;
    
    public void EnemyCreate()          //  敵を生成するメソッド EnemyManager.csより呼ばれる
    {
        applyNum = GameObject.Find("StartCountText").GetComponent<LevelManager>().initLevel - 1;  //  生成するレベルを取得する
        var randomValue = Random.Range(m_ints[applyNum].MinValue, m_ints[applyNum].MaxValue);         //  生成数を最小値と最大値の範囲でランダム設定する
        enemys = new GameObject[randomValue];                 //  配列をランダム数分だけ用意する

        for (int i = 0; i < randomValue; i++)                 //  配列にインスタンス化したオブジェクトを入れる
        {
            enemys[i] = Instantiate(enemyPrefab, transform.position, Quaternion.Euler(0.0f, roteY, 0.0f));  //  インスタンス生成
            enemys[i].transform.parent = transform;                                         //  子にする

        }
        isGenerate = true;                                                          //  生成中のフラグをたてる
        FindObjectOfType<Clear>().SetNorma(randomValue);                            //  Clear.csのSetNormaメソッドを呼ぶ
        normaCount = randomValue;
        Debug.Log(randomValue);
    }

    public void DestroyCheck()                                   //  enemy.csより討伐数をカウントし、フラグを管理呼ばれるメソッド
    {
        destroyCount++;                                         //  討伐数をカウント
        if (destroyCount == normaCount)                           //  現在のノルマを超えたら親であるspwanerのフラグを管理する
        {
            if (isGenerate)
            {
                isDestroyed = true;                                  //  この生成場所での生成は終わり。次を生成する
                //FindObjectOfType<EnemyManager>().ChoiseSpawner();    //  EnemyManagerの生成場所命令メソッドを呼び出す
                Debug.Log("Choise");
                //isGenerate = false;
            }
        }
    }
}
