using UnityEngine;

public class ChangeChoiceUtil
{
    public enum OptionDirection
    {
        Vertical,
        Horizontal,
    }

    static readonly Vector2[,] directionTable = { { Vector2.up, Vector2.down }, { Vector2.left, Vector2.right } };

    /// <summary>
    /// 選択肢の選択状況を変更するメソッド
    /// </summary>
    /// <typeparam name="T">enum</typeparam>
    /// <param name="axis">どのaxisを使用するか</param>
    /// <param name="choice">変更したい選択</param>
    /// <param name="TMax">T.Maxを設定してください</param>
    /// <param name="canMoveTopToBottom">上から下に、右から左などにいくか</param>
    /// <param name="direction">縦軸か横軸か</param>
    /// <returns></returns>
    /// where T : 〇〇で、〇〇型に制限が可能。structやSystem.Enumなど。
    public static bool Choice<T>(Vector2 axis, ref T choice, T TMax, bool canMoveTopToBottom, OptionDirection direction) where T: System.Enum
    {
        int choiceValue = (int)(object)choice;
        int max = (int)(object)TMax;

        if (axis == directionTable[(int)direction, 0])
        {
            choiceValue--;

            if (choiceValue == -1) choiceValue = canMoveTopToBottom ? max - 1 : 0;
        }
        else if (axis == directionTable[(int)direction, 1])
        {
            choiceValue++;

            if (choiceValue == max) choiceValue = canMoveTopToBottom ? 0 : max - 1;
        }
        else return false;

        choice = (T)(object)choiceValue;
        return true;
    }

}

