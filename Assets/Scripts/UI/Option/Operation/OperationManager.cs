using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class OperationManager : MonoBehaviour
{
    enum OperationChoices
    {
        None,

        CameraSensitivity,
        CameraReversalH,
        CameraReversalV,

        Max,
    }

    //表示・非表示切り替え用に管理するもの
    [SerializeField] Canvas _canvas = default!;
    [SerializeField] Image[] _images;

    [SerializeField] ImageUIData _cameraSensitivityImage;
    [SerializeField] List<Sprite> _scaleSprites;

    [SerializeField] Image _methodOfOperation = default!;
    [SerializeField] Sprite[] _methodOfOperationSprites;

    OperationChoices _choice = OperationChoices.None + 1;

    const int MAX_ROTATIONSPEED = 20;
    const int MIN_ROTATIONSPEED = 1;

    private void Start()
    {
        InputSystemManager.Instance.onNavigatePerformed += OnNavigate;
        InputSystemManager.Instance.onChoicePerformed += OnChoice;

        _cameraSensitivityImage.ImageData.SetSprite(_scaleSprites[DataManager.configData.cameraRotationSpeed / 10 - 1]);
    }

    public void OnChangeScene()
    {
        InputSystemManager.Instance.onNavigatePerformed -= OnNavigate;
        InputSystemManager.Instance.onChoicePerformed -= OnChoice;
    }

    /// <summary>
    /// Canvasの表示・非表示を設定する関数
    /// </summary>
    public void SetCanvasEnable(bool enable)
    {
        _canvas.enabled = enable;

        if (enable)
        {
            _images[(int)_choice - 1].color = new Color32(176, 176, 176, 255);
            _choice = OperationChoices.None + 1;
            _images[(int)_choice - 1].color = Color.red;
        }
    }

    private void OnNavigate()
    {
        if (!_canvas.enabled) return;

        ChangeChoice(InputSystemManager.Instance.NavigateAxis);
        CameraSensitivityChange(ref DataManager.configData.cameraRotationSpeed, InputSystemManager.Instance.NavigateAxis.x);
    }

    private void OnChoice()
    {
        if (!_canvas.enabled) return;

        //boolのみ提示
        switch (_choice)
        {
            case OperationChoices.CameraReversalV:
                CameraReversalV();
                break;
            case OperationChoices.CameraReversalH:
                CameraReversalH();
                break;
        }
    }

    private void ChangeChoice(Vector2 axis)
    {
        if (!ChangeChoiceUtil.Choice(axis, ref _choice, OperationChoices.Max, false, ChangeChoiceUtil.OptionDirection.Vertical)) return;

        SetMethodOfOperation();
        _images[(int)_choice - 1 + (int)axis.y].color = new Color32(176, 176, 176, 255);
        _images[(int)_choice - 1].color = Color.red;
        SoundManager.Instance.PlaySE(SoundSource.SE16_UI_SELECTION);
    }

    private void SetMethodOfOperation()
    {
        if (_methodOfOperationSprites.Length != 3) return;

        _methodOfOperation.sprite = _choice switch
        {
            OperationChoices.CameraSensitivity => _methodOfOperationSprites[2],
            OperationChoices.CameraReversalV => _methodOfOperationSprites[1],
            OperationChoices.CameraReversalH => _methodOfOperationSprites[1],
            _ => throw new System.NotImplementedException(),
        };
    }

    private void CameraReversalV()
    {
        DataManager.configData.cameraVerticalOrientation ^= true;
        Logger.Log(DataManager.configData.cameraVerticalOrientation);
    }

    private void CameraReversalH()
    {
        DataManager.configData.cameraHorizontalOrientation ^= true;
        Logger.Log(DataManager.configData.cameraHorizontalOrientation);
    }

    private void CameraSensitivityChange(ref int rotationSpeed, float axis)
    {
        if (_choice != OperationChoices.CameraSensitivity) return;
        if (axis != 1 && axis != -1) return;

        //10を掛けているのは数値が低いと大差がないための補正
        rotationSpeed = Mathf.Clamp(rotationSpeed / 10 + (int)axis, MIN_ROTATIONSPEED, MAX_ROTATIONSPEED) * 10;

        _cameraSensitivityImage.ImageData.SetSprite(_scaleSprites[(rotationSpeed / 10) - 1]);

        //Logger.Log(rotationSpeed);
    }
}