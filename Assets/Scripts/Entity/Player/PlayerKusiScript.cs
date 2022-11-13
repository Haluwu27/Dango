using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKusiScript : MonoBehaviour
{
    [SerializeField] GameObject[] dangoData;
    List<GameObject> dangoList =new List<GameObject>();
    const float distance = -0.2f;//’cq“¯m‚Ì‹——£
    const float adjustment = 180f;//’cq‚ÌŠp“x’²®
    const float size = 0.2f;//’cq‚ÌƒTƒCƒY

    public void SetDango(List<DangoColor> dangos)
    {
        ResetDango();
        for(int i = 0; i < dangos.Count; i++)
        {
            GameObject obj = Instantiate(dangoData[(int)dangos[i]], gameObject.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localPosition += new Vector3(0, i * distance, 0);
            obj.transform.localRotation = new Quaternion(0, 0, adjustment,0);
            obj.transform.localScale *= size;
            dangoList.Add(obj);
        }
    }

    private void ResetDango()
    {
        for (int i = 0; i < dangoList.Count; i++)
            Destroy(dangoList[i]);

        dangoList.Clear();
    }
}
