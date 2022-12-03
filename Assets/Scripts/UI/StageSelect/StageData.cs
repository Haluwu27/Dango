using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stage
{
    Stage1,
    Stage2,
    Stage3,
    Stage4,

    Tutorial,
}

public class StageData : MonoBehaviour
{
    [SerializeField] bool _isRelease;
    [SerializeField] Stage _stage;
    [SerializeField] PortraitScript _portraitScript;
    [SerializeField] FusumaManager _fusumaManager;

    public bool IsRelease => _isRelease;

    public Stage Stage => _stage;

    public List<QuestData> QuestData = new();

    protected virtual void Awake()
    {
        InputSystemManager.Instance.Input.SwitchCurrentActionMap("Player");

        if (_fusumaManager != null) _fusumaManager.Open();
    }

    protected virtual void Start()
    {
        _portraitScript.ChangePortraitText(StartPortraitText()).Forget();
        AddQuest();
    }

    protected virtual void AddQuest()
    {
        throw new System.NullReferenceException();
    }

    protected virtual PortraitTextData StartPortraitText()
    {
        return null;
    }

    public virtual List<DangoColor> FloorDangoColors()
    {
        return null;
    }

    public void Release()
    {
        _isRelease = true;
    }

    public void Lock()
    {
        _isRelease = false;
    }
}
