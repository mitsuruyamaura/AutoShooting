using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public int sceneNum;

    PlayerMove move;

    private void Start()
    {
        //  タイトルでない場合、取得する
        if (sceneNum != 0)
        {
            move = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        }
    }

    private void Update()
    {
        if (sceneNum != 0)
        {
            if (move.isEnd)
            {
                return;
            }

            if (!move.isEnd)
            {
                //  キーボードから呼ぶ処理
                //if (Input.GetKeyDown("2"))
                //{
                    //SceneChange();
                //}
            }
        }
        else   //  タイトル画面からステージ１を呼ぶ
        {
            if (Input.GetKeyDown("space"))
            {
                SceneChange();
            }
        }
    }

    //  シーン上のボタンから呼ばれる
    // Update is called once per frame
    public void SceneChange()
    {
        switch (sceneNum)
        {
            case 0:
                SceneManager.LoadScene("GameScene1");
                break;

            case 1:
                SceneManager.LoadScene("GameScene2");
                break;

            case 2:
                SceneManager.LoadScene("Title");
                break;

            default:
                break;
        }
    }

    //  リトライボタンより呼ばれる
    public void Retry()
    {
        SceneManager.LoadScene("GameScene1");
    }
}
