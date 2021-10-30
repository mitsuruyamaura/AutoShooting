using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public CONDITION_TYPE statusType;
    public float effectiveTime;
    public float duration;

    private bool isEndCondition;
    protected MoveSample playerController;

    /// <summary>
    /// コンディション効果の準備
    /// </summary>
    /// <param name="type"></param>
    /// <param name="time"></param>
    /// <param name="duration"></param>
    public void InitCondition(CONDITION_TYPE type, float time, float duration, MoveSample player) {
        statusType = type;
        effectiveTime = time;
        this.duration = duration;
        playerController = player;

        StartCondition();
    }

    protected virtual void StartCondition() {

    }


    void Update()
    {
        if (isEndCondition) {
            return;
        }

        effectiveTime -= Time.deltaTime;
        if (effectiveTime <= 0) {
            EndCondition();
        }
    }

    /// <summary>
    /// 効果時間の延長
    /// </summary>
    /// <param name="time"></param>
    public virtual void AddEffectiveTime(float time) {
        effectiveTime += time;
    }

    /// <summary>
    /// 効果終了
    /// </summary>
    protected virtual void EndCondition() {
        Destroy(this);
    }
}
