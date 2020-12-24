using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateOverTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //transform.DORotate(new Vector3(transform.position.x, transform.position.y, 180f), 1f, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear);
        //transform.DORestart();
        //float rnd = Random.Range(180f, 25f);
        //transform.DORotate(new Vector3(0, 0, 270f), 2f, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear);
        this.transform.DOLocalRotate(new Vector3(0.0f, 0.0f, 180f), 2.0f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        //transform.DORotate(new Vector3(0, 0, 180f),1f).SetLoops(-1, LoopType.Incremental);
        // Sequence sequence = DOTween.Sequence();
        //         sequence
        //         .Append(transform.DORotate(new Vector3(transform.position.x, transform.position.y, 45f), 1f, RotateMode.Fast)).SetEase(Ease.InQuad)
        //         .Append(transform.DORotate(new Vector3(transform.position.x, transform.position.y, -45f), 1f, RotateMode.Fast)).SetEase(Ease.InQuad)
        //         .SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
