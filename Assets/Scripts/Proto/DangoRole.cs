using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Role<T>
{
    private T[] m_role;
    private string m_rolename;
    private float m_score;

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
}

public class DangoRole
{
    static List<DangoColor> color = new();

    private static List<Role<DangoColor>> specialRoles = new()
    {
        new Role<DangoColor>(new DangoColor[]{DangoColor.Red,DangoColor.Yellow,DangoColor.Blue},"temp",30),

    };

    private static List<Role<DangoColor>> colorRoles = new()
    {
        new Role<DangoColor>(new DangoColor[]{DangoColor.Red,DangoColor.Orange},"temp",3),

    };

    private static List<Role<int>> posRoles = new()
    {
        new Role<int>(new int[]{0,0,0},"0,0,0",3),
        new Role<int>(new int[]{0,0,0,0},"0,0,0,0",4),
        new Role<int>(new int[]{0,0,1,1},"0,0,1,1",4),
        new Role<int>(new int[]{0,1,0,1},"0,1,0,1",4),
        new Role<int>(new int[]{0,0,0,0,0},"0,0,0,0,0",5),
        new Role<int>(new int[]{0,1,0,1,0},"0,1,0,1,0",5),
        new Role<int>(new int[]{0,0,1,0,0},"0,0,1,0,0",5),
        new Role<int>(new int[]{0,1,2,3,2,1,0},"0,1,2,3,2,1,0",7),
        new Role<int>(new int[]{0,1,0,1,0,1,0},"0,1,0,1,0,1,0",7),
        new Role<int>(new int[]{0,0,0,0,0,0,0},"0,0,0,0,0,0,0",7),
        new Role<int>(new int[]{0,1,2,3,4,5,6},"0,1,2,3,4,5,6",7),
    };

    /// <summary>
    /// 食べた団子に役があるか判定して点数を返す関数
    /// </summary>
    /// <param name="dangos">食べた団子</param>
    /// <returns>float:点数</returns>
    public static float CheckRole(List<DangoColor> dangos)
    {
        //返り値の得点
        float score = 0;

        //特殊役の判定、trueならここで判定終了。
        if (CheckSpecialRole(dangos, ref score)) return score;

        //特殊役がなかったら・・・
        foreach (DangoColor c in dangos)
        {
            //重複を防いで・・・
            if (!color.Contains(c))
            {
                //所持カラーのリストに追加
                color.Add(c);
            }
        }

        //その他役の判定
        CheckColorRole(ref score);
        CheckPosRole(dangos, ref score);

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
    private static bool CheckSpecialRole(List<DangoColor> dangos, ref float score)
    {
        foreach (var specialRole in specialRoles)
        {
            //配列をリストに変換
            List<DangoColor> specialRoleList = specialRole.GetData().ToList();

            //色と位置がロールと一致していたら
            if (dangos.SequenceEqual(specialRoleList))
            {
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
    private static bool CheckColorRole(ref float score)
    {
        //昇順ソート
        color.Sort();

        foreach (var colorRole in colorRoles)
        {
            //配列をリストに変換
            List<DangoColor> colorRoleList = colorRole.GetData().ToList();

            //念のためこちらもソート
            colorRoleList.Sort();

            //色がロールと一致していたら
            if (color.SequenceEqual(colorRoleList))
            {
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
    private static bool CheckPosRole(List<DangoColor> dangos, ref float score)
    {
        //色に応じたインデックスを割り振った配列を作成
        var normalizeDangoList = new List<int>();

        //団子の色データを正規化する
        foreach (DangoColor d in dangos)
        {
            normalizeDangoList.Add(color.IndexOf(d));
        }

        foreach (var posRole in posRoles)
        {
            //配列をリストに変換
            List<int> posRoleList = posRole.GetData().ToList();

            //配置がロールと一致していたら
            if (normalizeDangoList.SequenceEqual(posRoleList))
            {
                //スコアを加算し抜ける
                score += posRole.GetScore();

                //[Debug]役名の表示
                Logger.Log(posRole.GetRolename());
                return true;
            }
        }

        //役が何もなかったらfalseを返し、抜ける
        return false;
    }
}
