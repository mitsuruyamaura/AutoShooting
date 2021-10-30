using UnityEngine;
using DG.Tweening;

public class DoTweenRotationTest : MonoBehaviour
{
    public GameObject obj;
    public Ease easeType;

    private void Start() {
        // 自分を公転回転
        transform.DOLocalRotate(new Vector3(0, 360, 0), 3f, RotateMode.FastBeyond360).SetEase(easeType).SetLoops(-1, LoopType.Restart);

        // 子を反対方向に公転回転(親と反対に回転させることで停止しているように見える)
        obj.transform.DOLocalRotate(new Vector3(0, -360, 0), 3f, RotateMode.FastBeyond360).SetEase(easeType).SetLoops(-1, LoopType.Restart);
    }
}
