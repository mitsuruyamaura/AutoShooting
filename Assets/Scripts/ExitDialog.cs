using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDialog : MonoBehaviour {

    [SerializeField]
    Canvas canvas;             //  ダイアログのキャンバス

    public bool isInActive;    //  キー操作無効のフラグ。GameOver.csで代入される

    // Use this for initialization
    void Start ()
    {
        //  ダイアログのキャンバスを非表示にする
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInActive)    //  GameOver状態なら無効化
        {
            canvas.enabled = false;

            return;
        }

        //  ESCキーを押したらダイアログを呼び出す
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //  ダイアログが開いていない場合
            if (canvas.enabled == false)
            {
                //  終了確認のダイアログを表示するメソッドの呼び出し
                OnApplicationQuit();
            }
            else //(canvas.enabled == true)  //  ダイアログが開いている場合
            {
                // 終了確認のダイアログを非表示するメソッドの呼び出し
                OnCallCancel();
            }
        }
    }

    void OnApplicationQuit()
    {
        //  ダイアログが開いていなければ終了処理はキャンセル
        if(canvas.enabled == false)
        
            Application.CancelQuit();

            //  ダイアログの表示
            canvas.enabled = true;
    }

    //  終了ボタンのメソッド
    public void OnCallExit()
    {
        //Application.Quit();
        GameEnd();
    }

    public void OnCallCancel()
    {
        canvas.enabled = false;
    }

    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#elif UNITY_WEBGL
        Application.OpenURL("https://unityroom.com/");

#else //UNITY_STANDALONE
        Application.Quit();

#endif
    }
}
