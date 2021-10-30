/// <summary>
/// ゲーム中のオブジェクトの設定情報を取得するクラス
/// </summary>
public static class ObjectSettingHelper
{
    /// <summary>
    /// ObjectDestructibleData(HP、クリティカル確率、クリティカル時の攻撃力倍率)を取得して戻す
    /// </summary>
    /// <returns></returns>
    public static GameData.DestructibleObjectData GetDestructibleObjectData(int no) {      
        GameData.DestructibleObjectData objectData = new GameData.DestructibleObjectData();
        //foreach (GameData.ObjectSetting obj in GameData.instance.objectSettingList) {
        //    if (obj.no == no) {
        //        objectSetting = obj;
        //        return objectSetting;
        //    }
        //}

        objectData = GameData.instance.objectSettingList.Find(x => x.no == no);
        return objectData;
    }  

    /// <summary>
    /// DropItemDataを取得して戻す
    /// </summary>
    /// <param name="no"></param>
    /// <returns></returns>
    public static GameData.DropItemData GetDropItemData(int no) {
        GameData.DropItemData itemData = new GameData.DropItemData();
        itemData = GameData.instance.dropItemList.Find(x => x.no == no);
        return itemData;
    }
}
