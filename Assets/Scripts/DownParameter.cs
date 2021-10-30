using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DownParameter : MonoBehaviour {

    public float paramererCoefficient;    //  パラメータの減少係数
    float originOffensivePower;           //  基本パラメータの取得
    float originDifensePower;
    float originMaxHp;
    Text parameterText;                   //  表示用テキスト

    private void Start()
    {
        parameterText = GameObject.Find("DebugText").GetComponent<Text>();
    }

    private void Update()
    {
        parameterText.text = "攻撃力 : " + BaseStates.currentOffensivePower.ToString() + "\n" +
                             "防御力 : " + BaseStates.currentDefensePower.ToString() + "\n"; //+
                             //"最大HP : " + HPbar.currentMaxHp.ToString();
    }

    //  Clear.csより呼ばれるパラメータ減少のメソッド
    public void ChangeStatus(int clearLevel)
    {
        int penalty = clearLevel;

        originOffensivePower = BaseStates.currentOffensivePower;
        originDifensePower = BaseStates.currentDefensePower;
        //originMaxHp = HPbar.currentMaxHp;

        //  penaltyの回数だけ減算計算する
        //for (int i = 0; i < penalty; i++)
        //{
            var randomValue = Random.Range(0, 3);

            if (randomValue == 0)
            {
                originOffensivePower *= paramererCoefficient;
            }
            else if (randomValue == 1)
            {
                originDifensePower *= paramererCoefficient;
            }
            else
            {
                originMaxHp *= paramererCoefficient;
            }
        //}
        //  戻す
        BaseStates.currentOffensivePower = originOffensivePower;
        BaseStates.currentDefensePower = originDifensePower;
        //HPbar.currentMaxHp = originMaxHp;
    }
}
