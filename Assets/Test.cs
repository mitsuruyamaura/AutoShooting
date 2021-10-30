using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineTest : MonoBehaviour
{
    // Coroutine型の宣言。外部のクラスから呼びたい場合にはPublic修飾子をつける
    private Coroutine actionCoroutine = null;

    void Start()
    {
        // 変数にコルーチンメソッドを代入して実行
        actionCoroutine = StartCoroutine(ActionFire());        
    }

    public void Slash() {
        // 変数に値（メソッド）がないか確認
        if (actionCoroutine != null) {
            // メソッドがある場合にはコルーチン処理が動いているので停止させる
            StopCoroutine(actionCoroutine);
            // 代入されている値をnullにする（メソッドを抜く）
            actionCoroutine = null;
        }

        // 変数にコルーチンメソッドを代入して実行
        actionCoroutine = StartCoroutine(ActionSlash());
    }

    IEnumerator ActionFire() {
        Debug.Log("Action");
        yield return new WaitForSeconds(1.0f);

        // 処理が終了したので、代入されている値をnullにする（メソッドを抜く）
        if (actionCoroutine != null) {
            actionCoroutine = null;
        }
    }

    IEnumerator ActionSlash() {
        Debug.Log("Slash");
        yield return new WaitForSeconds(1.0f);

        // 処理が終了したので、代入されている値をnullにする（メソッドを抜く）
        if (actionCoroutine != null) {
            actionCoroutine = null;
        }
    }
}
