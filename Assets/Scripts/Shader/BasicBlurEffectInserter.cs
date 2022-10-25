using UnityEngine;

// ！Cameraコンポーネントを持つゲームオブジェクトにアタッチしてください！
// ExecuteInEditMode            : プレイしなくても動作させる
// ImageEffectAllowedInSceneView: シーンビューにポストエフェクトを反映させる
[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class BasicBlurEffectInserter : MonoBehaviour
{
    [SerializeField]
    private Material _material;

    private int _resolution;
    private Vector2 _displaySize;

    private void Awake()
    {
        _resolution = Shader.PropertyToID("_Resolution"); //シェーダーのプロパティIDを検索
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        _displaySize.x = Screen.currentResolution.width; //ディスプレイの高さを取得
        _displaySize.y = Screen.currentResolution.height; //ディスプレイの幅を取得
        _material.SetVector(_resolution, _displaySize); //シェーダーのプロパティに流し込み

        //////////横幅を半分にしたレンダーテスクチャを作成（まだ、なにも描かれていない）
        var rth = RenderTexture.GetTemporary(src.width / 2, src.height);
        Graphics.Blit(src, rth, _material); //シェーダー処理を加えて、横半分のレンダーテクスチャにコピー
                                            /////////

        /////////先のテクスチャサイズから、さらに縦半分にしたレンダーテスクチャを作成（まだ、なにも描かれていない）
        var rtv = RenderTexture.GetTemporary(rth.width, rth.height / 2);
        Graphics.Blit(rth, rtv, _material); //シェーダー処理を加えて、縦半分のレンダーテクスチャにコピー
        /////////

        Graphics.Blit(rtv, dest, _material); //元サイズから1/4になったレンダーテクスチャを、元のサイズに戻す

        RenderTexture.ReleaseTemporary(rtv); //テンポラリレンダーテスクチャの開放
        RenderTexture.ReleaseTemporary(rth); //開放しないとメモリリークするので注意
    }
}