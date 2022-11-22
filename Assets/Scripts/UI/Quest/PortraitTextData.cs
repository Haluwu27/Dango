using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitTextData
{
    public enum FacePatturn
    {
        Normal,


    }

    public struct PTextData
    {
        public string text;
        public int id;
        public float printTime;
        public FacePatturn face;

        public PTextData(int id, string text, float printTime, FacePatturn face)
        {
            this.id = id;
            this.text = text;
            this.printTime = printTime;
            this.face = face;
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
