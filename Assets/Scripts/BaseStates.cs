using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStates : MonoBehaviour {

    public float offensivePower;
    public float defensePower;

    public static float currentOffensivePower;
    public static float currentDefensePower;

    public static bool isStart;

    // Use this for initialization
    void Start ()
    {
        //　ゲーム開始時のみ代入
        if (!isStart)
        {
            currentOffensivePower = offensivePower;
            currentDefensePower = defensePower;
            isStart = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
