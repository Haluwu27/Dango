using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventTextUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textUGUI = default!;

    public TextData TextData { get; private set; }

    private void Awake()
    {
        TextData = new(textUGUI);
    }
}
