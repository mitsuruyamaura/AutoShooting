using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackForce : MonoBehaviour {

    public GameObject _player;     //  ドラゴンのオブジェクト
    public string earthTag;        //  除外するタグの名前
    public float forceHeight;      //  吹き飛ばす高さの調整値
    public float forcePower;       //　吹き飛ばす強さの調整値
    //CapsuleCollider cCol;        //  当たり判定のコライダー。メソッドで呼ばれるまではfalse;
    public GameObject forceRange;
    AnimatorStateInfo animStateInfo;
    Animator anim;
    AudioSource sound1;

    private void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        sound1 = audioSources[0];
        //cCol = GetComponent<CapsuleCollider>();   //  コンポーネント取得
        //cCol.enabled = false;
        anim = _player.GetComponent<Animator>();
        forceRange.SetActive(false);
    }

    public void Force(GameObject playerObj)
    {
        Debug.Log("ForceAttack!");
        //cCol.enabled = true;                      //  当たり判定を有効にする
        forceRange.SetActive(true);
        anim.SetTrigger("ForceAttack");           //  アニメを再生する
        //this.gameObject.tag = "ForceAttack";
        sound1.Play();                            //  SE再生
    //}

    //private void OnTriggerEnter(Collider col)
    //{
        //anim.Update(0);
        //animStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        //if (earthTag == col.gameObject.tag)       //  除外対象のタグがついたオブジェクトならなにもしない
        //{
            //return;
        //}
        //if (_player && _player == col.gameObject)   //  自分の体は除外
        //{
            //return;
        //}
        Rigidbody playerRb = playerObj.GetComponent<Rigidbody>();    //  ぶつかった相手のRigidBodyを取得
        //if (!otherRigidbody)
        //{
            //return;                                                  //  空なら何もしない
        //}
        Debug.Log("GetRigidBody");
        //Vector3 toVec = GetAngleVec(_player, col.gameObject);        //  吹き飛ばす方向を求める

        //toVec = toVec + new Vector3(0, 0, forceHeight);              //  Y方向を足す

        //otherRigidbody.AddForce(toVec * forcePower, ForceMode.Impulse);   //  吹き飛ばす
        playerRb.AddForce(transform.root.forward * forcePower, ForceMode.Impulse);
        StartCoroutine("ForceTime");              //  有効時間のコルーチン
    }

    //Vector3 GetAngleVec(GameObject from, GameObject to)              //  高さの概念を入れないベクトルを作る
    //{
        //Vector3 fromVec = new Vector3(from.transform.position.x, from.transform.position.y,0);
        //Vector3 toVec = new Vector3(to.transform.position.x, to.transform.position.y,0);

        //return Vector3.Normalize(toVec - fromVec);
    //}

    IEnumerator ForceTime()
    {
        yield return new WaitForSeconds(3.5f);                       //  当たり判定をオフにする
        //cCol.enabled = false;
        //this.gameObject.tag = "Enemy";
        forceRange.SetActive(false);
    }
}
