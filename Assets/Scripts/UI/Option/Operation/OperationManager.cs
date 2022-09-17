using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OperationManager : MonoBehaviour
{
    enum OperationChoices
    {
        None,

        UseController,
        CameraSensitivity,
        CameraReversalH,
        CameraReversalV,

        Max,
    }

    //表示・非表示切り替え用に管理するもの
    [SerializeField] Canvas _canvas = default!;
    [SerializeField] Image[] _images;

    [SerializeField] Image _methodOfOperation = default!;
    [SerializeField] Sprite[] _methodOfOperationSprites;

    OperationChoices _choice = OperationChoices.None + 1;

    const int MAX_ROTATIONSPEED = 200;
    const int MIN_ROTATIONSPEED = 10;

    private void Start()
    {
        InputSystemManager.Instance.onNavigatePerformed += OnNavigate;
        InputSystemManager.Instance.onChoicePerformed += OnChoice;
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
        CameraSensitivityChange(InputSystemManager.Instance.NavigateAxis.x);
    }

    private void OnChoice()
    {
        if (!_canvas.enabled) return;

        //boolのみ提示
        switch (_choice)
        {
            case OperationChoices.UseController:
                UseController();
                break;
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
        if (axis != Vector2.up && axis != Vector2.down) return;

        if (axis == Vector2.up)
        {
            if (_choice <= OperationChoices.None + 1) return;

            _choice--;
        }
        else if (axis == Vector2.down)
        {
            if (_choice >= OperationChoices.Max - 1) return;

            _choice++;
        }

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
            OperationChoices.UseController => _methodOfOperationSprites[1],
            OperationChoices.CameraSensitivity => _methodOfOperationSprites[2],
            OperationChoices.CameraReversalV => _methodOfOperationSprites[1],
            OperationChoices.CameraReversalH => _methodOfOperationSprites[1],
            _ => throw new System.NotImplementedException(),
        };
    }

    private void UseController()
    {
        DataManager.configData.gamepadInputEnabled ^= true;
        Logger.Log(DataManager.configData.gamepadInputEnabled);
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

    private void CameraSensitivityChange(float vecX)
    {
        if (_choice != OperationChoices.CameraSensitivity) return;
        if (vecX != 1 && vecX != -1) return;

        DataManager.configData.cameraRotationSpeed += (int)vecX * MIN_ROTATIONSPEED;
        if (DataManager.configData.cameraRotationSpeed < MIN_ROTATIONSPEED) DataManager.configData.cameraRotationSpeed = MIN_ROTATIONSPEED;
        else if (DataManager.configData.cameraRotationSpeed > MAX_ROTATIONSPEED) DataManager.configData.cameraRotationSpeed = MAX_ROTATIONSPEED;
        Logger.Log(DataManager.configData.cameraRotationSpeed);
    }
}