using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharacter : MonoBehaviour {

    [SerializeField]
    private GameObject[] characters;   //  キャラの数と種類。外部で設定可能。

    [SerializeField]
    private GameObject[] cameras;      //  カメラの数。外部でキャラをアサインして連動させる

    [SerializeField]
    private GameObject equipButton;    //  装備変更ボタン。Unityちゃんのみ表示させる

    private int chara;                 //  現在のキャラクターの管理用

    PlayerMove move;

    // Use this for initialization
    void Start () {
        //  初期キャラ設定。すべてfalseにしておく。装備変更ボタンを表示
        chara = 0;

        characters[chara].SetActive(true);
        cameras[chara].SetActive(true);

        equipButton.SetActive(true);

        move = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (move.isEnd)
        {
            return;
        }

        if (!move.isEnd)
        {
            //  キャラ変更ボタンでメソッド呼び出し
            if (Input.GetKeyDown("3"))
            {
                CharaChange();
            }
        }
    }

    //  シーン上のボタンでも呼び出せる
    public void CharaChange()
    {
        chara ++;

        //  配列の最大値を超えたら0に戻す
        if (chara >= characters.Length)
        {
            chara = 0;
        }

        //  キャラを切り替え
        for (var i = 0; i < characters.Length; i++)
        {
            //  for分で回してiと同じになったキャラで止めて取得
            if (i == chara)
            {
                //  選択されたキャラをアクティブにし、コライダーをセットする
                characters[i].SetActive(true);
                cameras[i].SetActive(true);
                characters[i].GetComponent<Collider>();
            }
            else   //  iと同じでないキャラはfalseにする
            {
                characters[i].SetActive(false);
                cameras[i].SetActive(false);
            }

            //  装備変更ボタンの表示・非表示を制御する
            if (chara == 0)
            {
                equipButton.SetActive(true);
            }
            else
            {
                equipButton.SetActive(false);
            }
        }
    }
}
