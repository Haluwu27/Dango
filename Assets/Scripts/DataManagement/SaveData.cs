/// セーブデータの実体クラス
/// float型やUnity独自の型は使えず
/// int、double、string、boolだけ使えると覚えておいてください
/// 特にfloatが使えないのは忘れがちなので注意
/// ネストしたオブジェクト型が使えるかは未検証
///
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    //データに含む物が分からないので仮でこうなってます
    //後で色々付け足しといてください

    public int a = 0;
    public string b = "string";
    public double c = 3.14d;
    public bool d = false;
}
