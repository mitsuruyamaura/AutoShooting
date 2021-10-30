using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

    [SerializeField]
    Canvas gameOverCanvas;                //  GameOverのCanvas;

    PlayerMove move;

    // Use this for initialization
    void Start () {
        gameOverCanvas.enabled = false;   //  ゲームオーバーダイアログを非表示にする
    }
	
	public void CanvasActive() {
        gameOverCanvas.enabled = true;    //  ゲームオーバーダイアログを表示にする

        //  ゲームオーバー処理を呼ぶ
        move = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        move.LoseControl();
    }
}
