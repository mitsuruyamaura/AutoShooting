using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class QuitCheckPouUp : MonoBehaviour
{
    public Button btnQuitGame;        // ゲーム終了ボタン
    public Button btnClosePopup;      // ポップアップを閉じてゲームに戻るボタン

    public CanvasGroup canvasGroup;
    public float fadeTime = 1.0f;     // ポップアップを表示する際のフェイドイン/アウトの時間

    public Ease easeType;             // DoTweenのEasing関数の設定。補間処理によりアニメーションが滑らかになる
    public float startScale = 0.5f;   // ポップアップの初期のサイズ設定値

    IEnumerator Start() 
    { 
        // 各ボタンに処理を登録
        btnQuitGame.onClick.AddListener(GameDirector.QuitGame);
        btnClosePopup.onClick.AddListener(OnClickClosePopUp);

        // CanvasGroupのアルファを0にしてポップアップ全体を透明にする
        canvasGroup.alpha = 0;

        // ポップアップの初期サイズを設定
        transform.localScale = Vector3.one * startScale;

        // DoTweenのSequence(シーケンス)を初期化して利用できるようにする。
        Sequence sequence = DOTween.Sequence();

        // アルファを徐々に1に近づけて、ポップアップを表示する　＋　同時にサイズを徐々に大きくしていく
        sequence.Append(canvasGroup.DOFade(1, fadeTime));
        sequence.Join(transform.DOScale(Vector3.one, fadeTime).SetEase(easeType));

        //yield return new WaitForSeconds(fadeTime);
        yield return new WaitUntil(() => canvasGroup.alpha == 1);

        // ゲーム内時間の流れを停止
        Time.timeScale = 0;
    }

    /// <summary>
    /// ポップアップを閉じてゲームに戻る
    /// </summary>
    private void OnClickClosePopUp() {
        // ゲーム内時間の流れを再開する
        Time.timeScale = 1.0f;

        Sequence sequence = DOTween.Sequence();

        // アルファを徐々に0に近づけて、ポップアップを非表示にする　＋　同時にサイズを徐々に小さくしていく
        canvasGroup.DOFade(0, fadeTime);
        sequence.Join(transform.DOScale(Vector3.zero, fadeTime).SetEase(easeType));

        Destroy(gameObject, fadeTime);       
    }
}
