using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        //Managers.UI.ShowSceneUI<UI_Inven>();

        Dictionary<int,Data.Stat> dict = Managers.Data.StatDict;

        gameObject.GetOrAddComponent<CursorController>();

        GameObject player = Managers.Game.Spawn(Define.WorldObejct.Player, "UnityChan");
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);

        //Managers.Game.Spawn(Define.WorldObejct.Monster, "Knight");
        GameObject go = new GameObject { name = "SpawningPool" };
        go.AddComponent<SpawningPool>();
        SpawningPool pool = go.GetComponent<SpawningPool>();
        //SpawningPool pool = go.GetOrAddComponent<SpawningPool>(); // 이거왜 null 뜨지 ... 
        pool.SetKeepMonsterCount(5);
    }

    public override void Clear()
    {

    }
}
