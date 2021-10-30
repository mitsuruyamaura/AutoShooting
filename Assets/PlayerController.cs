using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour {

    private Rigidbody rigidbody;
    float walkForce = 100.0f;
    float maxWalkSpeed = 5.0f;
    // public float speed = 10.0f;
    // public float rotationSpeed = 100.0f;
    float jumpForce = 250.0f;
    public bool isJump;
    float rotH = 90f;
    float rotV = 180f;
    Vector3 lastPos;
    Vector3 diff;
    Vector3 diffY0;
    Vector3 nlzdiffY0;
    float diffY0Speed;


    void Start() {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && isJump == false) {
            this.rigidbody.AddForce(transform.up * this.jumpForce);
            //空中にいるとき旗揚げ
            isJump = true;
        }
    }

    void FixedUpdate() {
        // 速度制限
        // if(this.rigidbody.velocity.magnitude < this.maxWalkSpeed)
        // {
        //移動
        // if(Input.GetKey(KeyCode.W))
        // {
        //     this.rigidbody.AddForce(transform.forward * this.walkForce);
        // }
        // if(Input.GetKey(KeyCode.S))
        // {
        //     this.rigidbody.AddForce(transform.forward * this.walkForce * -1f);
        // }
        // if(Input.GetKey(KeyCode.D))
        // {
        //     this.rigidbody.AddForce(transform.right * this.walkForce);
        // }
        // if(Input.GetKey(KeyCode.A))
        // {
        //     this.rigidbody.AddForce(transform.right * this.walkForce * -1f);
        // }
        // }
        //VerticalとHorizontalAxisをそれぞれ前進、回転動作につなげる
        // float translation = Input.GetAxisRaw("Vertical") * speed;
        // float rotation = Input.GetAxisRaw("Horizontal") * rotationSpeed;
        // //Z軸へ前進
        // transform.Translate(0, 0, translation);
        // // Y軸を中心に回転
        // // transform.Rotate(0,rotation, 0);
        // //速度、回転を10f/s,100f/sにする
        // translation *= Time.deltaTime;
        // rotation *= Time.deltaTime;



        // 速度制限
        //if (this.rigidbody.velocity.magnitude < this.maxWalkSpeed) {
            // 縦横のAxisの値を取る
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            //Axisの値をベクトルに変換
            Vector3 force = new Vector3(x, 0, z);
            //ベクトルを正規化
            Vector3 nlzForce = force.normalized;
            //正規化されたベクトルと歩く力を計算する
            nlzForce *= this.walkForce;
            // Debug.Log(force);
            // Debug.Log(nlzForce);
            // 物体に力を加える
            this.rigidbody.AddForce(nlzForce);

            Debug.Log(this.rigidbody.velocity.magnitude);
       // }
        // transform.rotation = Quaternion.Euler(0, Input.GetAxis("Horizontal") * rotH, 0);
        //1フレーム前のPlayerの位置との差を計り、どの方向に進んでいるかを計算する。
        Vector3 diff = transform.position - lastPos;
        

        // キャラクターが上を向かないようにするためdiffのy軸方向のベクトルを0にする
        this.diffY0 = this.diff;
        this.diffY0.y = 0;

        //transform.positionの値を代入する動作を含むことによって、ここの値は、1つ前のフレームのものになる。
        lastPos = transform.position;


        Debug.Log(diff.magnitude);

        //少し動いたら
        //if (diff.magnitude > 0.01f) {
        //    //差の方向を向く
        //    transform.rotation = Quaternion.LookRotation(diff);
        //}

        if (diffY0.magnitude > 0.01f) {
            //差の方向を向く
            // transform.rotation = Quaternion.LookRotation(diffY0);
            //差の方向を向き向くためのRotationの値を代入
            Quaternion playerRotation = Quaternion.LookRotation(diffY0);
            //最初に向いていた角度から差の方向へフレームごとに25%づつ回転する
            transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, 0.25f);
        }

        // Debug.Log(transform.position);
        Debug.Log(lastPos);


    }
    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Floor")) {
            //地面に触れたら旗下げ
            isJump = false;
        }
    }
}


