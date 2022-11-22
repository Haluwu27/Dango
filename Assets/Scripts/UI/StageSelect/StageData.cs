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
}

public class StageData : MonoBehaviour
{
    public interface IAddQuest
    {
        public void AddQuest();
    }

    public interface IPortrait
    {
        public PortraitTextData GetPortraitQuest();
    }

    [SerializeField] bool _isRelease;
    public bool IsRelease => _isRelease;

    [SerializeField] Stage _stage;
    public Stage Stage => _stage;

    [SerializeField] PortraitScript _portraitScript;

    public IAddQuest CurrentStage { get; private set; }
    public IPortrait CurrentPortrait { get; private set; }

    private void Awake()
    {
        InputSystemManager.Instance.Input.SwitchCurrentActionMap("Player");

        if (Stage == Stage.Stage3)
        {
            CurrentStage = Stage001Data.Instance;
            CurrentPortrait = Stage001Data.Instance;
        }
    }

    private void Start()
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM(SoundSource.BGM1A_STAGE1 + (int)_stage);
        _portraitScript.ChangePortraitText(CurrentPortrait.GetPortraitQuest()).Forget();
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
