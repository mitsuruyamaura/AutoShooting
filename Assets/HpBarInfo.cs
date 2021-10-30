using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HpBarInfo : MonoBehaviour
{
    public Slider sliderHp;
    public CanvasGroup canvasGroup;
    public bool isPlayerHpInfo;

    void Start() {
        // HpBarのゲージ(value)を最大値にする
        sliderHp.value = 1.0f;
        
        // Player以外のHPゲージの場合、非表示にする
        if (!isPlayerHpInfo) {
            canvasGroup.alpha = 0;
        }
    }

    void LateUpdate() {
        // HpBar用Canvasの向きを常にカメラの方向に向ける
        LookHpBarToCamera();      
    }

    /// <summary>
    /// HpBar用Canvasの向きを常にカメラの方向に向ける
    /// </summary>
    private void LookHpBarToCamera() {
        transform.rotation = Camera.main.transform.rotation;
    }

    /// <summary>
    /// 敵のHPの現在値とHpbarのvalueを同期させる
    /// </summary>
    public void UpdatedHpBarValue(int hp, int maxHp, bool isSetup = false) {
        if (!isPlayerHpInfo && !isSetup) {
            StartCoroutine(FadeHpBarInfo());
        }
        sliderHp.value = (float)hp / (float)maxHp;
    }

    /// <summary>
    /// 非表示のHPBarを表示して、再度非表示にする
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeHpBarInfo() {
        canvasGroup.alpha = 1.0f;
        yield return new WaitForSeconds(1.0f);
        canvasGroup.DOFade(0, 0.5f);
    }
}