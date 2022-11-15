using UnityEngine;

/// <summary>
/// フェイスアニメーション管理クラス.
/// </summary>
public class FaceAnimationController : MonoBehaviour
{
    public enum FaceTypes
    {
        Default,    /// デフォルト. 
        OpenMouth,  /// 口開け
        Chewing,    /// 咀嚼
        Smile,      /// ニコッ

        ForTitle,   /// タイトル用

        NoFace,     /// のっぺらぼう
    }

    /// <summary>
    /// アニメーション情報
    /// </summary>
    private struct AnimationInfo
    {
        public Vector2 TextureSize; /// テクスチャのサイズ. 
        public Rect Atlas;          /// アトラス情婦. 
        public int Type;            /// 種類. 
        public Material Mat;        /// マテリアル. 
        public int VNum;            /// 縦方向のアトラス数
        public int HNum;            /// 横方向のアトラス数
    }

    private AnimationInfo _faceInfo;
    public Material faceMaterial;
    public Vector2 faceAtlasSize;

    void Awake()
    {
        //顔のテクスチャ情報
        _faceInfo.Mat = faceMaterial;
        Texture texture = _faceInfo.Mat.mainTexture;
        _faceInfo.TextureSize.x = texture.width;
        _faceInfo.TextureSize.y = texture.height;
        _faceInfo.Atlas.width = faceAtlasSize.x;
        _faceInfo.Atlas.height = faceAtlasSize.y;
        _faceInfo.VNum = (int)(_faceInfo.TextureSize.y / _faceInfo.Atlas.height);
        _faceInfo.HNum = (int)(_faceInfo.TextureSize.x / _faceInfo.Atlas.width);
    }

    /// <summary>
    /// 顔の種類を変更.
    /// </summary>
    /// <param name="type">Type.</param>
    public void ChangeFaceType(FaceTypes type)
    {
        _faceInfo.Type = (int)type;

        // 座標を求める
        _faceInfo.Atlas.x = ((int)type / _faceInfo.VNum);
        _faceInfo.Atlas.y = ((int)type - (_faceInfo.Atlas.x * _faceInfo.VNum));
        _faceInfo.Atlas.x *= _faceInfo.Atlas.width;
        _faceInfo.Atlas.y *= _faceInfo.Atlas.height;

        // UV座標計算
        Vector2 offset;
        offset = new Vector2(_faceInfo.Atlas.x / _faceInfo.TextureSize.x, 1.0f - (_faceInfo.Atlas.y / _faceInfo.TextureSize.y));

        // 適用
        _faceInfo.Mat.SetTextureOffset("_MainTex", offset);
    }
}
