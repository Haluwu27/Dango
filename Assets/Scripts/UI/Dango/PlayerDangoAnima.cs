using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDangoAnima : MonoBehaviour
{
    [SerializeField] Vector3 stertPos;
    [SerializeField] Vector3 endPos;
    [SerializeField] float addSpeed;
    [SerializeField] float removeSpeed;
    RectTransform rectTransform ;
    Animator animator;
    private bool _isRemove;
    int isSetHash = Animator.StringToHash("IsSet");
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        animator =GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isRemove)
            Remove();
        else
            Add();
    }

    public void AddDango()
    {
        rectTransform.localPosition = stertPos;
        animator.SetBool(isSetHash, false);
        _isRemove = false;
    }
    public void RemoveDango()
    {
        rectTransform.localPosition = endPos;
        animator.SetBool(isSetHash, false);
        _isRemove = true;
    }


    private void Remove()
    {
        if (rectTransform.localPosition.y < stertPos.y)
        {
            rectTransform.localPosition += new Vector3(0, removeSpeed * Time.deltaTime, 0);
        }
        else
        {
            animator.SetBool(isSetHash, false);
            rectTransform.localPosition = endPos;
            gameObject.SetActive(false);
        }
    }
    private void Add()
    {
        if (rectTransform.localPosition.y > endPos.y)
        {
            rectTransform.localPosition -= new Vector3(0, addSpeed * Time.deltaTime, 0);
        }
        else
        {
            animator.SetBool(isSetHash, true);
            rectTransform.localPosition = endPos;
        }
    }
}
