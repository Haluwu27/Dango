using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitTextData
{
    public struct PTextData
    {
        public string text;
        public int id;
        public float printTime;

        public PTextData(int id, string text, float printTime)
        {
            this.id = id;
            this.text = text;
            this.printTime = printTime;
        }
    }

    List<PTextData> _texts = new();

    public PortraitTextData(params PTextData[] text)
    {
        _texts.AddRange(text);
    }

    public PTextData GetQTextData(int index) => _texts[index];
    public int TextDataIndex => _texts.Count;
}
