using Cysharp.Threading.Tasks;
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
    Scenes _prebScene;
    Scenes _currentScene;
    Scenes _currentIngameScene;

    [SerializeField] Scenes startScene;

    private void Awake()
    {
        Instance = this;

        _currentScene = startScene;
        _currentIngameScene = startScene;
        Load(startScene);
    }

    public bool Load(Scenes scene)
    {
        int index = (int)scene;

        //多重ロードを防ぐ
        if (_scenes[index] != null && _scenes[index].activeSelf) return false;

        _prebScene = _currentScene;
        _currentScene = scene;

        if (_scenes[index] == null) _scenes[index] = Instantiate(_sceneRoots[index]);
        else _scenes[index].SetActive(true);

        return true;
    }

    public bool UnLoad(Scenes scene, bool destroy)
    {
        //ロードされていないなら弾く
        if (_scenes[(int)scene] == null) return false;

        if (destroy) Destroy(_scenes[(int)scene]);
        else _scenes[(int)scene].SetActive(false);

        return true;
    }

    public async void ReLoad(Scenes scene)
    {
        UnLoad(scene, true);

        await UniTask.Yield();
        await UniTask.Yield();

        Load(scene);
    }

    public Scenes PrebScene => _prebScene;
    public Scenes CurrentIngameScene => _currentIngameScene;
    public void SetIngameScene(Scenes scene) => _currentIngameScene = scene;
}
