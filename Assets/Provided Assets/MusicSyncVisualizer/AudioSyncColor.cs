using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSyncColor : AudioSyncer
{

    public Material mat;
    //public bool emit;
    public void StartC()
    {
        
        // StartCoroutine("MoveToColor");
    }
    public IEnumerator MoveToColor()
    {
            Color _curr = mat.color;
            Color _initial = _curr;
            _curr=_curr* (Random.Range(0,10) + 10) / 20;        
        float _timer = 0;
        //float _curr1 = mat.GetColor("_EmissionColor");

        //_curr = Color.Lerp(_initial, _target, _timer / timeToBeat);
        _timer += Time.deltaTime;
        Debug.Log(_curr);
        
      
        yield return null;
        m_isBeat = false;
        //mat.SetColor("_EmissionColor",_curr*Random.Range(0f,1.0f)) ;
        //FOAT _initial = _curr;
    }

    private Color RandomColor()
    {
        if (beatColors == null || beatColors.Length == 0) return Color.white;
        m_randomIndx = Random.Range(0, beatColors.Length);
        return beatColors[m_randomIndx];
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_isBeat) return;

        //mat.color = Color.Lerp(mat.GetColor("_EmissionColor"), Color.white, restSmoothTime * Time.deltaTime);
    }

    public override void OnBeat()
    {
        base.OnBeat();

        /* Color _c = RandomColor()*/

        if (emit)
        {
            //StopCoroutine("MoveToColor");
            StartCoroutine("MoveToColor");
        }
    }


    public Color[] beatColors;
    public Color restColor;
    private int m_randomIndx;
}
