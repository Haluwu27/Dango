using Dango.Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static DangoRole;

public class Role<T>
{
    private T[] m_role;
    private string m_rolename;
    private float m_score;
    private int m_madeCount = 0;

    /// <param name="t">役の配列</param>
    /// <param name="n">役名</param>
    /// <param name="s">スコア</param>
    public Role(T[] t, string n, float s)
    {
        m_role = t;
        m_rolename = n;
        m_score = s;
    }

    public T[] GetData() => m_role;
    public string GetRolename() => m_rolename;
    public float GetScore() => m_score;
    public int GetMadeCount() => m_madeCount;
    public void AddMadeCount() => m_madeCount++;
}

class DangoRole
{
    public enum POSROLE
    {
        MONOCOLOR,
        LINE_SYMMETRY,
        LOOP,
        DIVIDED_INTO_TWO,
        DIVIDED_INTO_THREE,
    }

    //静的な役名
    //注）この処理をインスタンス生成以下に書くと実行順的に役名がnullになります。
    public static readonly string POSROLE_MONOCOLOR = "単色役";
    public static readonly string POSROLE_LINE_SYMMETRY = "線対称";
    public static readonly string POSROLE_LOOP = "ループ";
    public static readonly string POSROLE_DIVIDED_INTO_TWO = "二分割";
    public static readonly string POSROLE_DIVIDED_INTO_THREE = "三分割";

    //インスタンス生成
    //多数生成すると、スタックオーバーフローを起こしたためシングルトンパターンで行います
    public static readonly DangoRole instance = new();

    private DangoRole()
    {
        posRoles = new()
    {
        new Role<int>(new int[]{0,0,0},POSROLE_MONOCOLOR,3),
        new Role<int>(new int[]{0,0,0,0},POSROLE_MONOCOLOR,4),
        new Role<int>(new int[]{0,0,0,0,0},POSROLE_MONOCOLOR,5),
        new Role<int>(new int[]{0,0,0,0,0,0},POSROLE_MONOCOLOR,6),
        new Role<int>(new int[]{0,0,0,0,0,0,0},POSROLE_MONOCOLOR,7),
        new Role<int>(new int[]{0,1,0},POSROLE_LINE_SYMMETRY,3),
        new Role<int>(new int[]{0,1,1,0},POSROLE_LINE_SYMMETRY,4),
        new Role<int>(new int[]{0,0,1,0,0},POSROLE_LINE_SYMMETRY,5),
        new Role<int>(new int[]{0,1,0,1,0},POSROLE_LINE_SYMMETRY,5),
        new Role<int>(new int[]{0,1,1,1,0},POSROLE_LINE_SYMMETRY,5),
        new Role<int>(new int[]{0,1,2,1,0},POSROLE_LINE_SYMMETRY,5),
        new Role<int>(new int[]{0,0,1,1,0,0},POSROLE_LINE_SYMMETRY,6),
        new Role<int>(new int[]{0,1,0,0,1,0},POSROLE_LINE_SYMMETRY,6),
        new Role<int>(new int[]{0,1,1,1,1,0},POSROLE_LINE_SYMMETRY,6),
        new Role<int>(new int[]{0,1,2,2,1,0},POSROLE_LINE_SYMMETRY,6),
        new Role<int>(new int[]{0,0,0,1,0,0,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,0,1,0,1,0,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,0,1,1,1,0,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,0,1,2,1,0,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,1,0,0,0,1,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,1,0,1,0,1,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,1,0,2,0,1,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,1,1,0,1,1,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,1,1,1,1,1,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,1,1,2,1,1,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,1,2,0,2,1,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,1,2,1,2,1,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,1,2,2,2,1,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,1,2,3,2,1,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,1,0,1},POSROLE_LOOP,4),
        new Role<int>(new int[]{0,0,1,0,0,1},POSROLE_LOOP,6),
        new Role<int>(new int[]{0,1,1,0,1,1},POSROLE_LOOP,6),
        new Role<int>(new int[]{0,1,2,0,1,2},POSROLE_LOOP,6),
        new Role<int>(new int[]{0,1,0,1,0,1},POSROLE_LOOP,6),
        new Role<int>(new int[]{0,0,1,1},POSROLE_DIVIDED_INTO_TWO,4),
        new Role<int>(new int[]{0,0,0,1,1,1},POSROLE_DIVIDED_INTO_TWO,6),
        new Role<int>(new int[]{0,0,1,1,2,2},POSROLE_DIVIDED_INTO_THREE,6),
    };
    }

    List<DangoColor> _color = new();
    QuestManager _questManager = new();
    Random rand = new();

    private List<Role<DangoColor>> specialRoles = new()
    {
    };

    private List<Role<DangoColor>> colorRoles = new()
    {
    };

    private List<Role<int>> posRoles;

    /// <summary>
    /// 食べた団子に役があるか判定して点数を返す関数
    /// </summary>
    /// <param name="dangos">食べた団子</param>
    /// <returns>float:点数</returns>
    public float CheckRole(List<DangoColor> dangos, int currentMaxDango)
    {
        //カラーの初期化
        _color.Clear();
        DangoRoleUI.OnGUIReset();

        //返り値の得点
        float score = 0;

        //特殊役の判定、trueならここで判定終了。
        //if (CheckSpecialRole(dangos, ref score)) return score;

        //特殊役がなかったら・・・
        foreach (DangoColor c in dangos)
        {
            //重複を防いで・・・
            if (!_color.Contains(c))
            {
                //所持カラーのリストに追加
                _color.Add(c);
            }
        }

        //その他役の判定
        if (CheckPosRole(dangos, currentMaxDango, ref score))
        {
            SoundManager.Instance.PlaySE(rand.Next((int)SoundSource.VOISE_PRINCE_CREATEROLE01, (int)SoundSource.VOISE_PRINCE_CREATEROLE02 + 1));

            //役付きで団子を○個食べた系クエスト・その他クエストのチェック
            _questManager.SucceedChecker.CheckQuestEatDangoSucceed(_questManager, dangos, true);

            //何らかの役で○色を含んで役を作れ系のクエストのチェック
            _questManager.SucceedChecker.CheckQuestCreateRoleSucceed(_questManager, dangos, currentMaxDango);

            //役を作らない系クエストの連続の場合のリセットに使用
            _questManager.SucceedChecker.CheckQuestCreateRoleSucceed(_questManager, true);
        }
        else
        {
            SoundManager.Instance.PlaySE(rand.Next((int)SoundSource.VOISE_PRINCE_NOROLE01, (int)SoundSource.VOISE_PRINCE_NOROLE02 + 1));

            //EatDangoに分類される大半のクエストのチェック。役なしを数える数えない問題のために上記のチェッカーと分けています
            _questManager.SucceedChecker.CheckQuestEatDangoSucceed(_questManager, dangos, false);

            //役を作らないで○回食べる系のクエストのチェック
            _questManager.SucceedChecker.CheckQuestCreateRoleSucceed(_questManager, false);
        }

        //CheckColorRole(ref score);//処理内部にソートを含むため、位置役より下に配置。

        //全体的な点数計算（この処理は役の有無に関わらず実行される）
        score += (8 - _color.Count) * dangos.Count();

        return score;
    }

    /// <summary>
    /// 特殊役の判定
    /// </summary>
    /// <param name="value">点数の出力</param>
    /// <returns>
    /// <para>true : あり</para>
    /// <para>false : なし</para>
    /// </returns>
    private bool CheckSpecialRole(List<DangoColor> dangos, ref float score)
    {
        foreach (var specialRole in specialRoles)
        {
            //配列をリストに変換
            List<DangoColor> specialRoleList = specialRole.GetData().ToList();

            //色と位置がロールと一致していたら
            if (dangos.SequenceEqual(specialRoleList))
            {
                // 表示
                DangoRoleUI.OnGUIRoleName(specialRole.GetRolename(), specialRole.GetScore());

                //作った回数を増やし
                specialRole.AddMadeCount();

                //スコアを加算し抜ける
                score += specialRole.GetScore();
                return true;
            }
        }

        //役が何もなかったらfalseを返し、抜ける
        return false;
    }

    /// <summary>
    /// 色役の判定
    /// </summary>
    /// <returns>
    /// <para>true : あり</para>
    /// <para>false : なし</para>
    /// </returns>
    private bool CheckColorRole(ref float score)
    {
        //昇順ソート
        _color.Sort();

        foreach (var colorRole in colorRoles)
        {
            //配列をリストに変換
            List<DangoColor> colorRoleList = colorRole.GetData().ToList();

            //念のためこちらもソート
            colorRoleList.Sort();

            //色がロールと一致していたら
            if (_color.SequenceEqual(colorRoleList))
            {
                // 表示
                DangoRoleUI.OnGUIRoleName(colorRole.GetRolename(), colorRole.GetScore());

                //作った回数を増やし
                colorRole.AddMadeCount();

                //スコアを加算し抜ける
                score += colorRole.GetScore();
                return true;
            }
        }

        //役が何もなかったらfalseを返し、抜ける
        return false;
    }

    /// <summary>
    /// 位置役の判定
    /// </summary>
    /// <returns>
    /// <para>true : あり</para>
    /// <para>false : なし</para>
    /// </returns>
    private bool CheckPosRole(List<DangoColor> dangos, int currentMaxDango, ref float score)
    {
        //色に応じたインデックスを割り振った配列を作成
        var normalizeDangoList = new List<int>();

        //団子の色データを正規化する
        foreach (DangoColor d in dangos)
        {
            normalizeDangoList.Add(_color.IndexOf(d));
        }

        foreach (var posRole in posRoles)
        {
            //配列をリストに変換
            List<int> posRoleList = posRole.GetData().ToList();

            //配置がロールと一致していなかったら次に移行
            if (!normalizeDangoList.SequenceEqual(posRoleList)) continue;

            //表示
            DangoRoleUI.OnGUIRoleName(posRole.GetRolename(), posRole.GetScore());

            //作った回数を増やし・・・
            posRole.AddMadeCount();

            //さらにスコアを加算し抜ける
            score += posRole.GetScore();

            //クエストクリアしているかチェック
            _questManager.SucceedChecker.CheckQuestCreateRoleSucceed(_questManager, posRole, dangos, true, currentMaxDango);

            //[Debug]役名の表示
            //Logger.Log(posRole.GetRolename());
            return true;
        }

        //役が何もなかったらfalseを返し、抜ける
        return false;
    }

    public List<Role<int>> GetPosRoles() => posRoles;
}
