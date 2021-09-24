using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEffect : MonoBehaviour
{
    private GameObject fire_Image;

    public Vector3 inc_Size;
    public LeanTweenType tweenType;
    public float time;
    private Vector2 size;
    void Start()
    {
        fire_Image = this.gameObject;
        size = fire_Image.transform.localScale;
        Tween();
    }

    void Tween()
    {
        //LeanTween.scaleX(start_Image, valueX * size.x, time).setEase(tweenType).setLoopPingPong(-1).setIgnoreTimeScale(true);
        //LeanTween.scaleY(start_Image, valueY * size.y, time).setEase(tweenType).setLoopPingPong(-1).setIgnoreTimeScale(true);
        LeanTween.scale(fire_Image, inc_Size, time).setEase(tweenType).setLoopPingPong(-1).setIgnoreTimeScale(true);
        LeanTween.delayedCall(time, () => LeanTween.scale(fire_Image, size, time).setEase(tweenType).setLoopPingPong(-1).setIgnoreTimeScale(true));
    }
}
