using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveSample : MonoBehaviour {

	[Header("移動速度")]
	public float moveSpeed;
	[Header("回転速度")]
	public float rotateSpeed;

	[Header("ジャンプ力")]
	public float jumpPower;
	[Header("地面判定用レイヤー")]
	public LayerMask groundLayer;

	public int attackPower;
	public float bulletSpeed;


	public bool isGround;

	public WeaponController weaponController;

	private bool isAttack;

	private Rigidbody rb;
	private Animator anim;

	public int maxHp;
	public int hp;

	public MazzleShot mazzleShot;

	public float waitShotTime;
	public int shotActionSpeedLevel;
	float timer;

	public int rapidCount;

	public float bulletDuration;

	public bool isTPFCameraMode;

	public Button btnJump;

	public Joystick joystick;
	float x;
	float z;

	private enum AnimatorState {
	    Attack,
		Jump,
		Speed,
		AttackSpeed,
	}

	private const string HORIZONTAL = "Horizontal";
	private const string VERTICAL = "Vertical";
	private const string JUMP = "Jump";
	private const string ATTACK = "Action";

	//Vector3 lastPos;

	void Start() {
		InitPlayer();
	}

	/// <summary>
	/// Player情報の初期設定
	/// </summary>
	private void InitPlayer() {
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		btnJump.onClick.AddListener(OnClickJump);

		hp = maxHp;
	}

	void Update() {
		// 地面の判定
		CheckGround();

		// ジャンプの判定
		JudgeJump();

		// 攻撃
		Attack();
	}

	void FixedUpdate() {
		//if (anim.GetCurrentAnimatorStateInfo(0).IsName(AnimatorState.Attack.ToString())) {
		//	return;
		//}

		// キー入力
		x = Input.GetAxis(HORIZONTAL);
		z = Input.GetAxis(VERTICAL);

		// JoyStick入力
		x = joystick.Horizontal;
		z = joystick.Vertical;


		// 移動
		if (isTPFCameraMode) {
			TPSMove(x, z);		
		} else {
			Move(x, z);
		}
	}

	/// <summary>
	/// 地面の判定
	/// </summary>
	private void CheckGround() {
		//  Linecastでキャラの足元に地面があるか判定  地面があるときはTrueを返す
		isGround = Physics.Linecast(
						transform.position + transform.up * 1,
						transform.position - transform.up * 0.3f,
						groundLayer);
	}

	/// <summary>
	/// 移動する
	/// </summary>
	/// <param name="x">X軸の移動値</param>
	/// <param name="z">Z軸の移動値</param>
	private void Move(float x, float z) {
		isAttack = true;

		//float horizontal = joystick.Horizontal;
		//float vertical = joystick.Vertical;

        // 入力値を正規化
        //Vector3 moveDir = new Vector3(horizontal, 0, vertical).normalized;
		//transform.position += new Vector3(moveDir.x, 0, moveDir.z);

		// 入力値を正規化
		Vector3 moveDir = new Vector3(x, 0, z).normalized;


        // RigidbodyのAddforceメソッドを利用して移動
        //rb.AddForce(moveDir * moveSpeed);

        rb.velocity = new Vector3(moveDir.x * moveSpeed, rb.velocity.y, moveDir.z * moveSpeed);

        if (moveDir != Vector3.zero) {
            anim.SetFloat(AnimatorState.Speed.ToString(), 0.8f);
        } else {
            //Debug.Log(rb.velocity.magnitude);
            //anim.SetFloat("Speed", rb.velocity.magnitude);
            anim.SetFloat(AnimatorState.Speed.ToString(), 0);
        }

        // 移動に合わせて向きを変える
        LookDirection(moveDir);
	}

	private void TPSMove(float x, float z) {
		isAttack = true;

		// カメラの方向から、X-Z平面の単位ベクトルを取得
		Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

		// 方向キーの入力値とカメラの向きから、移動方向を決定
		Vector3 moveForward = cameraForward * z + Camera.main.transform.right * x;

		// 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
		rb.velocity = moveForward * moveSpeed + new Vector3(0, rb.velocity.y, 0);

		// キャラクターの向きを進行方向に
		if (moveForward != Vector3.zero) {
			transform.rotation = Quaternion.LookRotation(moveForward);
		}

		if (moveForward != Vector3.zero) {
			anim.SetFloat("Speed", 0.8f);
		} else {
			anim.SetFloat("Speed", 0);
		}
	}

	/// <summary>
	/// 向きを変える
	/// </summary>
	/// <param name="dir"></param>
	private void LookDirection(Vector3 dir) {
		// ベクトル(向きと大きさ)の2乗の長さをfloatで戻す = 動いているかどうかの確認
		if (dir.sqrMagnitude <= 0f) {
			return;
		}

		//Vector3 lastPos = transform.position;
		//移動先といた場所を求める なぜ(0, 0, 0)にならないんですか。
		//Vector3 diff = transform.position - lastPos;
		////前にいた場所  なぜ前の場所が代入されて、今の場所が代入されないんですか？
		//lastPos = transform.position;
		////差を判定
		//if (diff.magnitude > 0.01f) {
		//	//差の方向を向く
		//	transform.rotation = Quaternion.LookRotation(diff);
		//}


		// 補間関数Slerp（始まりの位置, 終わりの位置, 時間）　Leapでも動くが途中の挙動が変
		Vector3 forward = Vector3.Slerp(transform.forward, dir, rotateSpeed * Time.deltaTime);

		// 引数はVector3　オブジェクトの向きを変える
		transform.LookAt(transform.position + forward);

		isAttack = false;
	}

	/// <summary>
	/// ジャンプできるか判定
	/// </summary>
	private void JudgeJump() {
		//  キー入力のJumpで反応（GetButton）スペースキー(GetKey)
		if (Input.GetButtonDown(JUMP) && isGround) {
			//  着地していたとき
			Jump();
		}
	}

	/// <summary>
	/// ジャンプボタンでジャンプ
	/// </summary>
	private void OnClickJump() {
        if (isGround) {
			Jump();
        }
    }

	/// <summary>
	/// ジャンプ
	/// </summary>
	private void Jump() {
		//  着地判定をfalse
		isGround = false;

		//  Jumpステートへ遷移してジャンプアニメを再生
		anim.Play(AnimatorState.Jump.ToString());

		//  AddForceにて上方向へ力を加える
		rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
	}

	/// <summary>
	/// 通常攻撃
	/// </summary>
	private void Attack() {
        //if (Input.GetButtonDown("Action")) {
        if (!isAttack) {
			return;
        }

		timer += Time.deltaTime;

        if (timer > waitShotTime) {
			// AnimationState用のParameterをFloat型で用意し、Stateで指定するとAnimationのスピードを変更できる
			anim.SetFloat(AnimatorState.AttackSpeed.ToString(), 1f + (shotActionSpeedLevel / 10));
			anim.Play(AnimatorState.Attack.ToString());
		}

		if (timer > (waitShotTime + 0.5f)) {
			timer = 0;
			StartCoroutine(ActionFireProjectile());
		}
			
			//weaponController.ActivateWeaponCollider(true);
			//Debug.Log("Attack");
		//}
	}

	private void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "EnemyWeapon") {
			// 敵の武器に接触したら
			//hp -= col.gameObject.GetComponent<WeaponController>().enemyController.attackPower;

            if (hp <= 0) {
				hp = 0;
				Debug.Log("Game Over");
            }
		}
        if (col.gameObject.tag == "Item") {
			col.gameObject.GetComponent<ItemRecover>().PraparateItemEffect(this);
		}
	}

	/// <summary>
	/// 弾の生成
	/// </summary>
	private IEnumerator ActionFireProjectile() {
		for (int i = 0; i < rapidCount; i++) {
			mazzleShot.CreateFireProjectile(attackPower, bulletSpeed, bulletDuration);
			yield return new WaitForSeconds(0.2f);
		}
	}

    private void Hit() {

    }
}
