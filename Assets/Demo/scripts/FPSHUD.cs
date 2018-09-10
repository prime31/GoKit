using UnityEngine;
using System.Collections;


public class FPSHUD : MonoBehaviour
{
	public  float updateInterval = 0.5f;

	private float accum   = 0; // FPS accumulated over the interval
	private int   frames  = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval
	private float fps;


	void Awake()
	{
		Application.targetFrameRate = 60;
		useGUILayout = false;
	}


	void Start()
	{
	    timeleft = updateInterval;
	}


	void Update()
	{
	    timeleft -= Time.deltaTime;
	    accum += Time.timeScale/Time.deltaTime;
	    ++frames;

	    // Interval ended - update GUI text and start new interval
	    if( timeleft <= 0.0 )
	    {
		    fps = accum/frames;
		    if( fps > 20 )
			{
		        timeleft = updateInterval;
		        accum = 0.0f;
		        frames = 0;
		    }
		}
	}


	void OnGUI()
	{
		GUI.Label( new Rect( 0, 0, 100, 40 ), string.Format( "{0:F2} FPS", fps ) );
	}

}