/// Base64によるAES暗号化を使ったセーブデータの管理です
/// 
/// 参考サイト
/// AES暗号 https://qiita.com/kz-rv04/items/62a56bd4cd149e36ca70
/// Base64  https://docs.oracle.com/javase/jp/8/docs/api/java/util/Base64.html　Javaですがやってることは同じです
/// 
/// C#のライブラリとしてLitJsonを使ってます
///
/// 設定データの方は弄っても大丈夫なように暗号化してありません

using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using LitJson;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class DataManager : MonoBehaviour
{
    //暗号鍵はゆくゆくは外部へ逃しておきたいところ
    private static readonly string EncryptKey = "qpyky6kdn3yuvd9975w8ar6ackpdg7jj";
    private static readonly int EncryptPasswordCount = 16;
    private static readonly string PasswordChars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static readonly int PasswordCharsLength = PasswordChars.Length;

    public static ConfigData configData;
    public static SaveData saveData;
    public static PreservationKeyConfigData keyConfigData;

    [SerializeField] InputActionReference Jump;
    [SerializeField] InputActionReference Attack;
    [SerializeField] InputActionReference EatDango;
    [SerializeField] InputActionReference Fire;
    [SerializeField] InputActionReference ExpansionUI;

    public static bool _nowLoadConfigData = false;

    public static bool NowLoadConfigData
    {
        get { return _nowLoadConfigData; }
    }

    private static Dictionary<Language, LanguageData> _languageDataList = new();


    void Awake()
    {
        LoadConfigData();
        if (!NowLoadConfigData)
        {
            SaveConfigData();
        }

        LoadSaveData();
        LoadInputData();

        //言語データは使用しないので不要
        //LoadLanguageData();
    }

    void OnApplicationQuit()
    {
        SaveConfigData();
        SaveSaveData();
        SaveInputData();
    }

    /// <summary>
    /// 設定データをセーブします
    /// </summary>
    public static void SaveConfigData()
    {
        //ロードしていないなら
        if (!_nowLoadConfigData)
        {
            Logger.Warn("データがロードされていないため、初期データで保存します");
            configData = new ConfigData();
        }

        //シリアライズ用設定データ

        //パターンA
        JsonSerializerSettings settings = new()
        {
            Formatting = Formatting.Indented,
        };

        //パターンB
        //LitJson.JsonWriter jwriter = new LitJson.JsonWriter();
        //jwriter.PrettyPrint = true;
        //jwriter.IndentValue = 4;

        //シリアライズ実行

        //パターンA
        string dataString = JsonConvert.SerializeObject(configData, settings);

        //パターンB
        //JsonMapper.ToJson(configData,jwriter);
        //string dataString = jwriter.ToString();

        //ファイルパスを決定
#if UNITY_EDITOR
        string path = Directory.GetCurrentDirectory() + "\\Assets\\Resources";
#else
        string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
#endif

        //ファイルがあるかチェック
        if (!File.Exists(path + "/config.txt"))
        {
            Logger.Warn("設定ファイルが存在しないため、生成します");
            File.Create(path + "/config.txt").Close();
        }

        //保存
        using StreamWriter writer = new(path + "/config.txt", false);
        writer.WriteLine(dataString);
        writer.Flush();
    }

    /// <summary>
    /// 設定ファイルをロードします
    /// </summary>
    public static void LoadConfigData()
    {
        //データ初期化
        configData = new ConfigData();

        //ファイルパスを決定
#if UNITY_EDITOR
        string path = Directory.GetCurrentDirectory() + "\\Assets\\Resources";
#else
        string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
#endif

        //ファイルがあるかチェック
        if (!File.Exists(path + "/config.txt"))
        {
            Logger.Warn("設定ファイルが存在しないため、生成します");
            File.Create(path + "/config.txt").Close();
            _nowLoadConfigData = false;
            configData.IsReset = true;
            return;
        }

        string dataString;

        using (StreamReader reader = new(path + "/config.txt", false))
        {
            dataString = reader.ReadToEnd();
        }

        //Jsonから読み込み

        //パターンA
        //configData = JsonUtility.FromJson<ConfigData>(dataString);

        //パターンB(LitJson)
        configData = JsonMapper.ToObject<ConfigData>(dataString);

        //文字列をenumに変換(例外処理)
        try
        {
            configData.language = (Language)Enum.Parse(typeof(Language), configData.languageString);
            _nowLoadConfigData = true;
        }
        catch (Exception)
        {
            Logger.Error("設定ファイルのロードに失敗しました");
            configData.language = Language.Unknown;
        }
    }

    /// <summary>
    /// 設定データのファイルが存在するかを返します
    /// </summary>
    public static bool HasConfigDataFile()
    {

        //ファイルパスを決定
#if UNITY_EDITOR
        string path = Directory.GetCurrentDirectory() + "\\Assets\\Resources";
#else
        string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
#endif

        //ファイルがあるかチェック
        if (File.Exists(path + "/config.txt"))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 設定データがロードされているかを返します
    /// </summary>
    public static bool HasLoadingConfigData()
    {
        if (configData != null)
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// セーブデータを暗号化してセーブします
    /// </summary>
    public static void SaveSaveData()
    {

        //ロードしていないなら
        if (saveData == null)
        {
            Logger.Warn("データがロードされていないため、初期データで保存します");
            saveData = new SaveData();
        }

        //ファイルパスを取得
#if UNITY_EDITOR
        string path = Directory.GetCurrentDirectory() + "\\Assets\\Resources";
#else
        string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
#endif

        if (!configData.dataFilePath.Equals("default"))
        {
            path = configData.dataFilePath;
        }

        //ファイルがあるかチェック
        if (!File.Exists(path + "/save.dat"))
        {
            Logger.Warn("セーブデータが存在しないため、生成します");
            File.Create(path + "/save.dat").Close();
        }

        string json = JsonMapper.ToJson(saveData);

        //Base64で暗号化
        EncryptAesBase64(json, out string iv, out string base64);

        // 保存
        byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
        byte[] base64Bytes = Encoding.UTF8.GetBytes(base64);
        using FileStream file = new(path + "/save.dat", FileMode.Create, FileAccess.Write);
        using BinaryWriter binary = new(file);
        //4バイトずつ書き込み
        binary.Write(ivBytes.Length);
        binary.Write(ivBytes);

        binary.Write(base64Bytes.Length);
        binary.Write(base64Bytes);
    }

    /// <summary>
    /// セーブデータを復号してロードします
    /// </summary>
    public static void LoadSaveData()
    {
        saveData = new SaveData();

        //ファイルパスを取得
#if UNITY_EDITOR
        string path = Directory.GetCurrentDirectory() + "\\Assets\\Resources";
#else
        string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
#endif

        if (!configData.dataFilePath.Equals("default"))
        {
            path = configData.dataFilePath;
        }

        //ファイルがあるかチェック
        if (!File.Exists(path + "/save.dat"))
        {
            Logger.Warn("セーブデータが存在しないため、生成します");
            File.Create(path + "/save.dat").Close();
            return;
        }

        //暗号化用変数
        byte[] ivBytes = null;
        byte[] base64Bytes = null;

        //ファイルを古典的な方法で開く(Resources.Loadはセーブデータ管理には不向きなので)
        using (FileStream file = new(path + "/save.dat", FileMode.Open, FileAccess.Read))
        using (BinaryReader binary = new(file))
        {
            //4バイトずつ読み込む
            int length = binary.ReadInt32();
            ivBytes = binary.ReadBytes(length);

            length = binary.ReadInt32();
            base64Bytes = binary.ReadBytes(length);
        }

        string iv = Encoding.UTF8.GetString(ivBytes);
        string base64 = Encoding.UTF8.GetString(base64Bytes);
        //Base64で復号化
        DecryptAesBase64(base64, iv, out string json);

        // セーブデータ復元
        saveData = JsonMapper.ToObject<SaveData>(json);

    }

    /// <summary>
    /// セーブデータのファイルが存在するかを返します
    /// </summary>
    public static bool HasSaveDataFile()
    {

        //ファイルパスを決定
#if UNITY_EDITOR
        string path = Directory.GetCurrentDirectory() + "\\Assets\\Resources";
#else
        string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
#endif

        //ファイルがあるかチェック
        if (File.Exists(path + "/save.dat"))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// セーブデータがロードされているかを返します
    /// </summary>
    public static bool HasLoadingSaveData()
    {
        if (saveData != null)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 全ての言語ファイルを読み込みます
    /// </summary>
    public static void LoadLanguageData()
    {

        //インスタンス生成
        _languageDataList = new Dictionary<Language, LanguageData>();

        foreach (Language language in Enum.GetValues(typeof(Language)))
        {

            // Unknownはスキップ
            if (language.Equals(Language.Unknown)) continue;

            // Enumの文字列を取得する
            string lanName = Enum.GetName(typeof(Language), language);

            //ファイルパスを取得
#if UNITY_EDITOR
            string path = Directory.GetCurrentDirectory() + "\\Assets\\Resources";
#else
            string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
#endif
            if (!configData.dataFilePath.Equals("default"))
            {
                path = configData.dataFilePath;
            }

            //ファイルがあるかチェック
            if (!File.Exists(path + "/lang." + lanName))
            {
                Logger.Warn("言語ファイルが存在しないため、生成します");
                File.Create(path + "/lang." + lanName).Close();
                continue;
            }

            //暗号化用変数
            byte[] ivBytes = null;
            byte[] base64Bytes = null;

            //ファイルを古典的な方法で開く(Resources.Loadはセーブデータ管理には不向きなので)
            using (FileStream file = new(path + "/lang." + lanName, FileMode.Open, FileAccess.Read))
            using (BinaryReader binary = new(file))
            {
                //4バイトずつ読み込む
                int length = binary.ReadInt32();
                ivBytes = binary.ReadBytes(length);

                length = binary.ReadInt32();
                base64Bytes = binary.ReadBytes(length);
            }

            string iv = Encoding.UTF8.GetString(ivBytes);
            string base64 = Encoding.UTF8.GetString(base64Bytes);
            //Base64で復号化
            DecryptAesBase64(base64, iv, out string json);

            // Jsonから言語データを生成
            _languageDataList.Add(language, JsonMapper.ToObject<LanguageData>(json));

        }


    }

    /// <summary>
    /// 言語ファイルを作成します
    /// </summary>
    /// <param name="language">言語の種類</param>
    /// <param name="data">言語データ</param>
    public static void SaveLanguageData(Language language, LanguageData data)
    {

        // Unknownはスキップ
        if (language.Equals(Language.Unknown))
        {
            Logger.Error("Unknownの言語データは生成できません");
            return;
        }

        // Enumの文字列を取得する
        string lanName = Enum.GetName(typeof(Language), language);

        //ファイルパスを取得
#if UNITY_EDITOR
        string path = Directory.GetCurrentDirectory() + "\\Assets\\Resources";
#else
            string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
#endif
        if (!configData.dataFilePath.Equals("default"))
        {
            path = configData.dataFilePath;
        }

        //ファイルがあるかチェック
        if (!File.Exists(path + "/lang." + lanName))
        {
            Logger.Warn("言語ファイルが存在しないため、生成します");
            File.Create(path + "/lang." + lanName).Close();
        }

        string json = JsonMapper.ToJson(data);

        //Base64で暗号化
        EncryptAesBase64(json, out string iv, out string base64);

        // 保存
        byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
        byte[] base64Bytes = Encoding.UTF8.GetBytes(base64);
        using FileStream file = new(path + "/lang." + lanName, FileMode.Create, FileAccess.Write);
        using BinaryWriter binary = new(file);
        //4バイトずつ書き込み
        binary.Write(ivBytes.Length);
        binary.Write(ivBytes);

        binary.Write(base64Bytes.Length);
        binary.Write(base64Bytes);
    }

    /// <summary>
    /// 現在の設定で扱っている言語ファイルを取得する
    /// </summary>
    public static LanguageData LanguageData
    {
        get
        {
            if (_languageDataList == null)
            {
                Logger.Error("言語ファイルがロードされる前に、言語データが取得されました");
                return null;
            }
            if (configData.language == Language.Unknown)
            {
                Logger.Error("設定ファイルが初期化されているため、言語を日本語に設定します");
                configData.language = Language.JP;
            }
            return _languageDataList[configData.language];
        }
    }

    /// <summary>
    /// 入力データを保存する
    /// </summary>
    public void SaveInputData()
    {

        keyConfigData = new PreservationKeyConfigData();

        //アクションデータをシリアライズして保存
        keyConfigData.Jump = new string[Jump.action.bindings.Count]; 
        for (int i = 0; i < Jump.action.bindings.Count; i++) 
        {
            keyConfigData.Jump[i] = Jump.action.bindings[i].path;
        }
        keyConfigData.Attack = new string[Attack.action.bindings.Count];
        for (int i = 0; i < Attack.action.bindings.Count; i++)
        {
            keyConfigData.Attack[i] = Attack.action.bindings[i].path;
        }
        keyConfigData.EatDango = new string[EatDango.action.bindings.Count];
        for (int i = 0; i < EatDango.action.bindings.Count; i++)
        {
            keyConfigData.EatDango[i] = EatDango.action.bindings[i].path;
        }
        keyConfigData.Fire = new string[Fire.action.bindings.Count];
        for (int i = 0; i < Fire.action.bindings.Count; i++)
        {
            keyConfigData.Fire[i] = Fire.action.bindings[i].path;
        }
        keyConfigData.ExpansionUI = new string[ExpansionUI.action.bindings.Count];
        for (int i = 0; i < ExpansionUI.action.bindings.Count; i++)
        {
            keyConfigData.ExpansionUI[i] = ExpansionUI.action.bindings[i].path;
        }

        //シリアライズ用設定データ

        //パターンA
        JsonSerializerSettings settings = new()
        {
            Formatting = Formatting.Indented,
        };

        //パターンB
        //LitJson.JsonWriter jwriter = new LitJson.JsonWriter();
        //jwriter.PrettyPrint = true;
        //jwriter.IndentValue = 4;

        //シリアライズ実行

        //パターンA
        string dataString = JsonConvert.SerializeObject(keyConfigData, settings);

        //パターンB
        //JsonMapper.ToJson(configData,jwriter);
        //string dataString = jwriter.ToString();

        //ファイルパスを決定
#if UNITY_EDITOR
        string path = Directory.GetCurrentDirectory() + "\\Assets\\Resources";
#else
        string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
#endif

        //ファイルがあるかチェック
        if (!File.Exists(path + "/keyConfig.txt"))
        {
            Logger.Warn("キーコンフィグファイルが存在しないため、生成します");
            File.Create(path + "/keyConfig.txt").Close();
        }

        //保存
        using StreamWriter writer = new(path + "/keyConfig.txt", false);
        writer.WriteLine(dataString);
        writer.Flush();
    }

    /// <summary>
    /// 入力データをロードする
    /// </summary>
    public void LoadInputData()
    {

        //データ初期化
        keyConfigData = new PreservationKeyConfigData();

        //ファイルパスを決定
#if UNITY_EDITOR
        string path = Directory.GetCurrentDirectory() + "\\Assets\\Resources";
#else
        string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
#endif

        //ファイルがあるかチェック
        if (!File.Exists(path + "/keyConfig.txt"))
        {
            Logger.Warn("キーコンフィグファイルが存在しないため、生成します");
            File.Create(path + "/keyConfig.txt").Close();
            return;
        }

        string dataString;

        using (StreamReader reader = new(path + "/keyConfig.txt", false))
        {
            dataString = reader.ReadToEnd();
        }

        //Jsonから読み込み

        //パターンA
        //configData = JsonUtility.FromJson<ConfigData>(dataString);

        //パターンB(LitJson)
        keyConfigData = JsonMapper.ToObject<PreservationKeyConfigData>(dataString);

        //アクションデータをロード
        for (int i = 0; i < keyConfigData.Jump.Length; i++)
        {
            if (CheckBinding(Jump, keyConfigData.Jump[i]))
            {
                Jump.action.ChangeBinding(new InputBinding { path = keyConfigData.Jump[i] }).Erase();
            }
            Jump.action.AddBinding(new InputBinding { path = keyConfigData.Jump[i] });
        }
        for (int i = 0; i < keyConfigData.Attack.Length; i++)
        {
            if (CheckBinding(Attack, keyConfigData.Attack[i]))
            {
                Attack.action.ChangeBinding(new InputBinding { path = keyConfigData.Attack[i] }).Erase();
            }
            Attack.action.AddBinding(new InputBinding { path = keyConfigData.Attack[i] });
        }
        for (int i = 0; i < keyConfigData.EatDango.Length; i++)
        {
            if (CheckBinding(EatDango, keyConfigData.EatDango[i]))
            {
                EatDango.action.ChangeBinding(new InputBinding { path = keyConfigData.EatDango[i] }).Erase();
            }
            EatDango.action.AddBinding(new InputBinding { path = keyConfigData.EatDango[i] });
        }
        for (int i = 0; i < keyConfigData.Fire.Length; i++)
        {
            if (CheckBinding(Fire, keyConfigData.Fire[i]))
            {
                Fire.action.ChangeBinding(new InputBinding { path = keyConfigData.Fire[i] }).Erase();
            }
            Fire.action.AddBinding(new InputBinding { path = keyConfigData.Fire[i] });
        }
        for (int i = 0; i < keyConfigData.ExpansionUI.Length; i++)
        {
            if (CheckBinding(ExpansionUI, keyConfigData.ExpansionUI[i]))
            {
                ExpansionUI.action.ChangeBinding(new InputBinding { path = keyConfigData.ExpansionUI[i] }).Erase();
            }
            ExpansionUI.action.AddBinding(new InputBinding { path = keyConfigData.ExpansionUI[i] });
        }

    }

    //重複チェック関数
    private bool CheckBinding(InputActionReference inputActionReference,string path)
    {
        foreach(var bind in inputActionReference.action.bindings)
        {
            if (path.Equals(bind.path))
                return true;
        }
        return false;
    }

    /*以下編集は非推奨*/

    /// <summary>
    /// AES暗号化(Base64形式)
    /// </summary>
    public static void EncryptAesBase64(string json, out string iv, out string base64)
    {
        byte[] src = Encoding.UTF8.GetBytes(json);
        EncryptAes(src, out iv, out byte[] dst);
        base64 = Convert.ToBase64String(dst);
    }

    /// <summary>
    /// AES複合化(Base64形式)
    /// </summary>
    public static void DecryptAesBase64(string base64, string iv, out string json)
    {
        byte[] src = Convert.FromBase64String(base64);
        DecryptAes(src, iv, out byte[] dst);
        json = Encoding.UTF8.GetString(dst).Trim('\0');
    }

    /// <summary>
    /// AES暗号化
    /// </summary>
    public static void EncryptAes(byte[] src, out string iv, out byte[] dst)
    {
        iv = CreatePassword(EncryptPasswordCount);
        dst = null;
        using RijndaelManaged rijndael = new();
        rijndael.Padding = PaddingMode.PKCS7;
        rijndael.Mode = CipherMode.CBC;
        rijndael.KeySize = 256;
        rijndael.BlockSize = 128;

        byte[] key = Encoding.UTF8.GetBytes(EncryptKey);
        byte[] vec = Encoding.UTF8.GetBytes(iv);

        using ICryptoTransform encryptor = rijndael.CreateEncryptor(key, vec);
        using MemoryStream ms = new();
        using CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write);
        cs.Write(src, 0, src.Length);
        cs.FlushFinalBlock();
        dst = ms.ToArray();
    }

    /// <summary>
    /// AES複合化
    /// </summary>
    public static void DecryptAes(byte[] src, string iv, out byte[] dst)
    {
        dst = new byte[src.Length];
        using RijndaelManaged rijndael = new();
        rijndael.Padding = PaddingMode.PKCS7;
        rijndael.Mode = CipherMode.CBC;
        rijndael.KeySize = 256;
        rijndael.BlockSize = 128;

        byte[] key = Encoding.UTF8.GetBytes(EncryptKey);
        byte[] vec = Encoding.UTF8.GetBytes(iv);

        using ICryptoTransform decryptor = rijndael.CreateDecryptor(key, vec);
        using MemoryStream ms = new(src);
        using CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read);
        cs.Read(dst, 0, dst.Length);
    }

    /// <summary>
    /// パスワード生成
    /// </summary>
    /// <param name="count">文字列数</param>
    /// <returns>パスワード</returns>
    public static string CreatePassword(int count)
    {
        StringBuilder sb = new(count);
        for (int i = count - 1; i >= 0; i--)
        {
            char c = PasswordChars[UnityEngine.Random.Range(0, PasswordCharsLength)];
            sb.Append(c);
        }
        return sb.ToString();
    }
}
