using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TextMessageViewer : MonoBehaviour
{
    public List<string> messageList = new List<string>();　　// 表示するメッセージのリスト
    public float wordSpeed;                                  // 1文字当たりの表示速度
    
    public Text txtMessage;                                  // メッセージ表示用
    public GameObject iconNextTap;             　            // タップを促す画像表示

    private int messageListIndex = 0;                        // 表示するメッセージの配列番号
    private int wordCount;                                   // １メッセージ当たりの文字の総数
    private bool isTapped = false;                      　   // 全文表示後にタップを待つフラグ
    private bool isDisplayedAllMessage = false;              // 全メッセージ表示完了のフラグ

    private IEnumerator waitCoroutine;                       // 全文表示までの待機時間メソッド代入用。Stopできるようにしておく
    private Tween tween;                                     // DoTween再生用。Killできるように代入して使用する

    void Start() {
        // １文字ずつ文字を表示する処理をスタート
        StartCoroutine(DisplayMessage());
    }
 
    void Update() {
        if (isDisplayedAllMessage) {
            // 全てのメッセージ表示が終了していたら処理を行わない
            return;
        }

        if (Input.GetMouseButtonDown(0) && tween != null) {
            // 文字送り中にタップした場合、文字送りを停止
            tween.Kill();
            tween = null;
            // 文字送りのための待機時間も停止
            if (waitCoroutine != null) {
                StopCoroutine(waitCoroutine);
                waitCoroutine = null;
            }
            // 全文をまとめて表示
            txtMessage.text = messageList[messageListIndex];
            
            // タップするまで全文を表示したまま待機
            StartCoroutine(NextTouch());
        }

        if (Input.GetMouseButtonDown(0) && wordCount == messageList[messageListIndex].Length) {
            // 全文表示中にタップしたら全文表示を終了
            isTapped = true;
        }
    }

    /// <summary>
    /// １文字ずつ文字を表示
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisplayMessage() {
        isTapped = false;
        
        // 表示テキストとTweenをリセット
        txtMessage.text = "";
        tween = null;
        
        // 文字送りの待機時間を初期化
        if (waitCoroutine != null) {
            StopCoroutine(waitCoroutine);
            waitCoroutine = null;
        }

        // 1文字ずつの文字送り表示が終了するまでループ
        while (messageList[messageListIndex].Length > wordCount) {
            // wordSpeed秒ごとに、文字を1文字ずつ表示。SetEase(Ease.Linear)をセットすることで一定の表示間隔で表示
            tween = txtMessage.DOText(messageList[messageListIndex], messageList[messageListIndex].Length * wordSpeed).
                SetEase(Ease.Linear).OnComplete(() => {
                    Debug.Log("全文表示 完了");
                    // (TODO) 他にも処理があれば追加する

                });
            // 文字送り表示が終了するまでの待機時間を設定して待機を実行
            waitCoroutine = WaitTime();
            yield return StartCoroutine(waitCoroutine);
        }
        
        // タップするまで全文を表示したまま待機
        StartCoroutine(NextTouch());
    }

    /// <summary>
    /// 全文表示までの待機時間(文字数×1文字当たりの表示時間)
    /// タップして全文をまとめて表示した場合にはこの待機時間を停止
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitTime() {
        yield return new WaitForSeconds(messageList[messageListIndex].Length * wordSpeed);
    }

    /// <summary>
    /// タップするまで全文を表示したまま待機
    /// </summary>
    /// <returns></returns>
    private IEnumerator NextTouch() {
        yield return null;
        // 表示した文字の総数を更新
        wordCount = messageList[messageListIndex].Length;

        // タップを促すイメージ表示
        iconNextTap.SetActive(true);

        // タップを待つ
        yield return new WaitUntil(() => isTapped);

        iconNextTap.SetActive(false);

        // 次のメッセージ準備
        messageListIndex++;
        wordCount = 0;

        // リストに未表示のメッセージが残っている場合
        if (messageListIndex < messageList.Count) {
            // １文字ずつ文字を表示する処理をスタート
            StartCoroutine(DisplayMessage());
        } else {
            // 全メッセージ表示終了
            isDisplayedAllMessage = true;
                        
            // 次の処理へ
        }
    }
}
