using UnityEngine;
using System.Collections.Generic;

public class WaveHelper : MonoBehaviour
{
	
	public float StartHeight 
	{
		get {return _startHeight;} 
		set {_startHeight = value;}
	}
	public float MaxHeight 
	{
		get {return _maxHeight;} 
		set{_maxHeight = value;}
	}
	public float SpeedAnimation 
	{
		get {return _speedAnimation;}
		set {if(value < 0) value = 0f; _speedAnimation = value;}
	}
		public float TimeAnimation 
	{
		get {return _timeAnimation;}
		set {if(value < 0) value = 0f; _timeAnimation = value;}
	}
	
    List<Impuls> WaveImpuls = new List<Impuls>();
    List<Vector2> WaveCenters = new List<Vector2>();
    List<float> WaveTime = new List<float>();

	float _startHeight = 0.6f;
	float _maxHeight = 1.2f;
	
    float _height = 0;
	Vector3 _normal = Vector3.zero;
	float _normalVector = 2.0f;
	float _normalAmplifare = 1.2f;
    float _speedAnimation = 5.0f;
    float _timeAnimation = 0.5f;
	GameObject _image;

	float[] _heightArray;
	Vector3[] _normalArray;

	Animator _anim;

    void Start()
    {
		_anim = transform.GetComponent<Animator> ();
    }

	Vector3 a, b;

    void Update()
    {
		if (WaveImpuls.Count == 0)
		{
			if(!_anim.isActiveAndEnabled)
				_anim.enabled = true;
			return;
		}

		_anim.enabled = false;

        _height = StartHeight;
		_normal = Vector3.zero;
		_image = transform.FindChild ("Sprite").gameObject;

        for (int i = 0; i < WaveImpuls.Count; i++)
        {
			_heightArray = new float[WaveImpuls.Count];
			_normalArray = new Vector3[WaveImpuls.Count];

			if (Time.time - WaveTime [i] < TimeAnimation / 2) 
			{
				_heightArray[i] = WaveImpuls[i].GetImpuls(new Vector2(WaveCenters[i].x, WaveCenters[i].y),
					new Vector2(transform.localPosition.x, transform.localPosition.y), Time.time - WaveTime[i]);

				a = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
				b = new Vector3 (WaveCenters [i].x, WaveCenters [i].y, 0);

				_normalArray[i] = _normalAmplifare * Vector3.Normalize (a - b) * Mathf.Exp (-Vector3.Distance (a, b) / _normalVector) / (WaveImpuls.Count);

			} 
			else if (Time.time - WaveTime[i] > TimeAnimation / 2 && Time.time - WaveTime[i] < TimeAnimation)
			{
				_normalArray[i] = Vector3.zero;
				_heightArray[i] = 0.0f;
			}

            else
            {
                WaveImpuls.Remove(WaveImpuls[i]);
                WaveCenters.Remove(WaveCenters[i]);
                WaveTime.Remove(WaveTime[i]);
                _height = StartHeight;
                transform.localScale = new Vector3(_height, _height, 0);
				transform.GetComponent<Item> ().WaitForWave = false;
            }

			_height += _heightArray[i];
			_normal += _normalArray [i];
        }

        if (_height > MaxHeight)
            _height = MaxHeight; 

        transform.localScale = Vector3.Lerp(transform.localScale,
            new Vector3(_height, _height, 0), Time.deltaTime * SpeedAnimation);

		_image.transform.localPosition = Vector3.Lerp(_image.transform.localPosition,
			_normal, Time.deltaTime * SpeedAnimation);
    }

    public void AddCenter(Vector2 center)
    {
		// тут імпульс має 2 реалізації конструктора можна задати свою частоту амплітуду і тд.
		// Impuls(float amplitude, float lenght, float omega) тут omega це частота зміни амплітуди
		// імпульс являє собою функцію затухаючу експоненту  А * Sin (omega * time ) / exp (time * lenght * (pos - center.pos)^2 / (2pi))
		// lenght - чим більше, тим менше буде взаємодія з дальніми обєктами

		if (WaveCenters.Count > 1)
			return;

        WaveImpuls.Add(new Impuls());
		
        WaveCenters.Add(center);
        WaveTime.Add(Time.time);
    }

	public void RemoveAll()
	{
		WaveImpuls = new List<Impuls> ();
		WaveCenters = new List<Vector2>();
		WaveTime = new List<float>();
		_height = StartHeight;
		transform.localScale = new Vector3(_height, _height, 0);
		transform.GetComponent<Item> ().WaitForWave = false;
	}

    public float GiveTimeToEnd()
    {
        if (WaveTime.Count == 0)
            return 0.0f;
        return Time.time - WaveTime[0];
    }
		
}
