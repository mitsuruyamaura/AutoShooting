using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField]
    GameObject targetObj;
    
    Vector3 targetPos;
    Camera cam;
    MoveSample playerController;

    float scrollSpeed = 10f;

    void Start() {
        cam = GetComponent<Camera>();

        //  取得したオブジェクトのtransform.positionを取得
        targetPos = targetObj.transform.position;
        playerController = targetObj.GetComponent<MoveSample>();
    }

    void Update() {
        // カメラにPlayerを追従させる
        FollowPlayer();

        // TPSモード以外ではカメラの位置変更は出来ない
        if (!playerController.isTPFCameraMode) {
            return;
        }

        // Playerを中心にカメラを公転回転
        RevolveCamera();
        
        // カメラのズームイン・ズームアウト
        ZoomCamera();        
    }

    /// <summary>
    /// カメラにPlayerを追従させる
    /// </summary>
    private void FollowPlayer() {
        // target(Player)の移動量分、自分（カメラ）も移動する
        transform.position += targetObj.transform.position - targetPos;
        targetPos = targetObj.transform.position;
    }

    /// <summary>
    /// Playerを中心にカメラを公転回転
    /// </summary>
    private void RevolveCamera() {
        //  マウスの左クリックを押している間、X軸公転
        if (Input.GetMouseButton(0)) {
            //  マウスの移動量
            float mouseInputX = Input.GetAxis("Mouse X");

            //  targetの位置のY軸を中心に公転する
            //  第１引数に中心にするオブジェクト、第２引数に回転軸の向き、第３引数に移動量を渡す
            transform.RotateAround(targetPos, Vector3.up, mouseInputX * Time.deltaTime * 100f);
        }

        //  マウスの右クリックを押している間、Y軸公転
        if (Input.GetMouseButton(1)) {
            //  マウスの移動量
            float mouseInputY = Input.GetAxis("Mouse Y");
            //  カメラの垂直移動（角度制限なし）
            transform.RotateAround(targetPos, transform.right, mouseInputY * Time.deltaTime * 50f);
        }
    }

    /// <summary>
    /// カメラのズームイン・ズームアウト
    /// </summary>
    private void ZoomCamera() {
        // マウスホイールを回転させた情報を取得
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // fieldofViewを制御してズームイン・ズームアウト
        float view = cam.fieldOfView - (scroll * scrollSpeed);

        // fieldofViewにてズームできる範囲の制限を書ける
        cam.fieldOfView = Mathf.Clamp(value: view, min: 30.0f, max: 120.0f);

        // カメラを初期位置に戻す
        if (Input.GetKeyDown("o")) {
            cam.fieldOfView = 65.0f;
        }
    }

    /// <summary>
    /// カメラのズーム位置を初期化
    /// </summary>
    public void SetWin()
    {
        cam.fieldOfView = 65.0f;
    }
}
