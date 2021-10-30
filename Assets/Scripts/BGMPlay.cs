using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlay : MonoBehaviour {

    public AudioSource [] sound;   
    //public static AudioSource nowSound;       //  現在のBGM取得用。シーン間で引き継ぐ
 
    //public static bool isBGMPlaying;          //  BGMが再生中かどうかのフラグ。シーン間で引き継ぐ

    //public bool DontDestroyEnabled = true;    //  シーンをまたいでもオブジェクトを破壊しないようにする 

    //PlayerMove move;
    
    // Use this for initialization
    void Start ()
    {
        //if (DontDestroyEnabled)
        //{
            // Sceneを遷移してもオブジェクトが消えないようにする
            //DontDestroyOnLoad(this);
        //}

        //move = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();

        AudioSource[] audioSources = GetComponents<AudioSource>();
        for(int i =0; i < sound.Length; i++)
        {
            sound[i] = audioSources[i];
            //Debug.Log(sound[i]);
        }

        //  BGMが流れていないときだけ再生するメソッド呼び出し
        //if (!isBGMPlaying)
        //{
            PlayMusic();
            //Debug.Log("Play");
        //}
    }

    private void Update()
    {
        //if (move.isEnd)
        //{
            //return;
        //}

        //if (!move.isEnd)
        //{
            //  キーボードから呼ぶ処理
            //if (Input.GetKeyDown("1"))
            //{
                //PlayMusic();
            //}
        //}
    }

    //  シーン上、あるいはキーボードから呼ばれるメソッド
    public void PlayMusic()
    {
        //  BGMが再生中なら止める
        //if (isBGMPlaying)
        //{
            //nowSound.Stop();
        //}

        //  ランダムでBGMを再生し、nowSoundに入れる
        var randomValue = Random.Range(0, sound.Length);
        sound[randomValue].Play();
        //nowSound = sound[randomValue];
        
        //Debug.Log(randomValue);
        //  再生中のフラグをたてる
        //isBGMPlaying = true;
    }
}
