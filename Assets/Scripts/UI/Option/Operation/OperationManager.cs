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
        CameraReversalV,
        CameraReversalH,

        Max,
    }

    //表示・非表示切り替え用に管理するもの
    [SerializeField] Canvas _canvas = default!;
    [SerializeField] Image[] _images;

    OperationChoices _choice = OperationChoices.None + 1;

    private void Start()
    {
        InputSystemManager.Instance.onNavigatePerformed += OnNavigate;
        InputSystemManager.Instance.onChoicePerformed += OnChoice;
        InputSystemManager.Instance.onBackPerformed += OnBack;
    }

    public void OnChangeScene()
    {
        InputSystemManager.Instance.onNavigatePerformed -= OnNavigate;
        InputSystemManager.Instance.onChoicePerformed -= OnChoice;
        InputSystemManager.Instance.onBackPerformed -= OnBack;
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

        if (_choice == OperationChoices.CameraSensitivity)
        {
            CameraSensitivityChange(InputSystemManager.Instance.NavigateAxis.x);
        }
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

    private void OnBack()
    {
        if (!_canvas.enabled) return;
    }

    private void ChangeChoice(Vector2 axis)
    {
        if (axis != Vector2.up && axis != Vector2.down) return;

        if (axis == Vector2.up)
        {
            _choice--;
            if (_choice <= OperationChoices.None)
            {
                _choice = OperationChoices.None + 1;
                return;
            }
        }
        else if (axis == Vector2.down)
        {
            _choice++;
            if (_choice >= OperationChoices.Max)
            {
                _choice = OperationChoices.Max - 1;
                return;
            }
        }

        _images[(int)_choice - 1 + (int)axis.y].color = new Color32(176, 176, 176, 255);
        _images[(int)_choice - 1].color = Color.red;
        SoundManager.Instance.PlaySE(SoundSource.SE16_UI_SELECTION);
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
        if (vecX != 1 && vecX != -1) return;

        DataManager.configData.cameraRotationSpeed += (int)vecX * 10;
        if (DataManager.configData.cameraRotationSpeed < 10) DataManager.configData.cameraRotationSpeed = 10;
        else if (DataManager.configData.cameraRotationSpeed > 200) DataManager.configData.cameraRotationSpeed = 200;
        Logger.Log(DataManager.configData.cameraRotationSpeed);
    }    
}