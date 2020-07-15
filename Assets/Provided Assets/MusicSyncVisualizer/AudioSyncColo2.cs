using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioSyncColo2 :AudioSyncer
{
	Color _initialColor;
	void Start() 
	{
		Color _initialColor = mat.GetColor("_EmissionColor");
	}
	private IEnumerator MoveToColor(Color _target)
	{
		Color _curr = mat.color;
		Color _initial = _curr;
		float _timer = 0;
		while (_curr != _target)
		{
		print("ddd");
			//mat.color = Color.Lerp(_initial, _target, _timer / timeToBeat);
			//mat.SetColor("EmissionColor", mat.GetColor("_EmissionColor") * Mathf.Lerp(0, Random.Range(0,1.0f), _timer / timeToBeat));
			mat.SetColor("_EmissionColor",Color.Lerp(mat.GetColor("_EmissionColor") * GetEmissionMultiplier(mat), RandomColor(), Time.deltaTime / timeToBeat));
			_timer += Time.deltaTime;
			yield return null;
		}

		m_isBeat = false;
	}
	static float GetEmissionMultiplier(Material mat)
	{
		var colour = mat.GetColor("_EmissionColor");
		return Mathf.Max(Random.Range(0,colour.r), Random.Range(0,colour.g), Random.Range(0,colour.b));
	}
	private void StartC()
    {
		_initialColor= mat.GetColor("_EmissionColor");

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

		mat.color = Color.Lerp(mat.color, _initialColor, restSmoothTime * Time.deltaTime);
	}

	public override void OnBeat()
	{
		base.OnBeat();
		print("agya");
		Color _c = RandomColor();
		if (this.name == this.name)
		{

			StopCoroutine("MoveToColor");
			StartCoroutine("MoveToColor", _c);
		
		}
	}

	

	public Color[] beatColors;
	public Color restColor;

	private int m_randomIndx;
	[SerializeField]
	private Material mat;
}
