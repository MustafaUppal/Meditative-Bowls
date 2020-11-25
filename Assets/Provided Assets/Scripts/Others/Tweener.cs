using UnityEngine;
using System.Collections;

public class Tweener : MonoBehaviour
{

    // Use this for initialization
    // public GameObject target;
    // public Method method;
    // public MethodType type;
    // public iTween.EaseType easetype;
    // public iTween.LoopType loopType;
    // //	public Axis axis1;
    // //	public float AxisValue;
    // //	public Axis axis2;
    // //	public Axis axis3;
    // public float XValue;
    // public float YValue;
    // public float ZValue;

    // public float duration;
    // public float Delay;
    // public float startDelay = 0f;
    // public bool ignoreTimeScale = false;
    // public bool stopAfterPlay = false;
    // public bool deactiveAfterPlay = false;
    // public enum Method
    // {
    //     add,
    //     by,
    //     from,
    //     to,
    //     update
    // }
    // public enum MethodType
    // {
    //     move,
    //     rotate,
    //     scale
    // }

    // public enum Axis
    // {
    //     x,
    //     y,
    //     z
    // }
    // public bool PlayOnStart = true;
    // void Start()
    // {
    //     //	if(PlayOnStart)
    //     //	Invoke("startAnim",startDelay);

    //     //	if(stopAfterPlay)
    //     //	{
    //     //		Invoke("stopScript",startDelay+duration);
    //     //	}
    // }
    // void OnEnable()
    // {
    //     if (PlayOnStart)
    //         Invoke("startAnim", startDelay);

    //     if (stopAfterPlay)
    //     {
    //         Invoke("stopScript", startDelay + duration);
    //     }
    //     if (deactiveAfterPlay)
    //     {
    //         Invoke("DisableIt", startDelay + duration + 0.5f);
    //     }
    // }
    // // Update is called once per frame
    // void Update()
    // {

    // }

    // public void startAnim()
    // {
    //     iTween.Launch(target, iTween.Hash("x", XValue, "y", YValue, "z", ZValue, "easeType", easetype.ToString(), "loopType", loopType.ToString(), "time", duration, "delay", Delay, "type", type.ToString(), "method", method.ToString(), "islocal", true, "ignoretimescale", ignoreTimeScale));
    // }
    // public void pauseAnim()
    // {
    //     iTween.Pause(target);
    // }
    // public void resumeAnim()
    // {
    //     iTween.Resume(target);
    // }
    // public void stopAnim()
    // {
    //     iTween.Stop(target);
    // }
    // void stopScript()
    // {
    //     stopAnim();
    //     this.enabled = false;
    // }
    // void DisableIt()
    // {
    //     this.gameObject.SetActive(false);
    // }
    // //	public void stopOnCompleteSingleIteration(){
    // //		StartCoroutine(stopAfterDuration());
    // //	}
    // //
    // //	IEnumerator stopAfterDuration(){
    // //		yield return new WaitForSeconds(duration + Delay);
    // //		iTween.Pause(target);
    // //	}

}
