using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//今回は単一シーンでの実装になるため、シーンのように読み込む必要がありません
//しかし、すべてがステージ上に存在してしまうとメモリをその分食ってしまいます
//というのを自力で解決するのがこのスクリプトです

#if UNITY_EDITOR
[CustomEditor(typeof(SceneSystem))]
public class SceneSystemOnGUI : Editor
{
    private SceneSystem sceneSystem;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        sceneSystem = target as SceneSystem;

        EditorGUILayout.HelpBox("プレハブを追加する際は順番に注意してください。", MessageType.Info);

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Scenes : Element " + (int)sceneSystem.debugScenes);
        EditorGUILayout.Separator();

        base.OnInspectorGUI();

    }
}
#endif


class SceneSystem : MonoBehaviour
{
    public enum Scenes
    {
        Title,
        Tutorial,
        Menu,
        StageSelect,
        Stage1,
        Stage2,
        Stage3,
        InGamePause,
        Success,
        GameOver,
        Option,
        Ex,

        [InspectorName("")]
        Max,
    }

#if UNITY_EDITOR
    /// <summary>デバッグ用です。使用禁止</summary>
    public Scenes debugScenes;
#endif

    public static SceneSystem Instance { get; private set; }

    [SerializeField] GameObject[] _sceneRoots = new GameObject[(int)Scenes.Max];
    GameObject[] _scenes = new GameObject[(int)Scenes.Max];

    private void Awake()
    {
        Instance = this;
        //Load(Scenes.Option);
        //Load(Scenes.Stage1);
        //Load(Scenes.Title);
        Load(Scenes.Menu);
    }

    public bool Load(Scenes scene)
    {
        _scenes[(int)scene] = Instantiate(_sceneRoots[(int)scene]);
        return true;
    }

    public bool UnLoad(Scenes scene)
    {
        Destroy(_scenes[(int)scene]);
        return true;
    }
}
