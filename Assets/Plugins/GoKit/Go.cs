using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Go : MonoBehaviour
{
	// defaults used for all tweens/properties that are not explicitly set
	public static EaseType defaultEaseType = EaseType.Linear;
	public static LoopType defaultLoopType = LoopType.RestartFromBeginning;
	public static UpdateType defaultUpdateType = UpdateType.Update;
	
	// defines what we should do in the event that a TweenProperty is added and an already existing tween has the same
	// property and target
	public static DuplicatePropertyRuleType duplicatePropertyRule = DuplicatePropertyRuleType.None;
	public static GoLogLevel logLevel = GoLogLevel.Warn;
	
	// validates that the target object still exists each tick of the tween. NOTE: it is recommended
	// that you just properly remove your tweens before destroying any objects even though this might destroy them for you
	public static bool validateTargetObjectsEachTick = true;
	
	private static List<AbstractTween> _tweens = new List<AbstractTween>(); // contains Tweens, TweenChains and TweenFlows
	private bool _timeScaleIndependentUpdateIsRunning;
	
	// only one Go can exist
	static Go _instance = null;
	public static Go instance
	{
		get
		{
			if( !_instance )
			{
				// check if there is a GO instance already available in the scene graph
				_instance = FindObjectOfType( typeof( Go ) ) as Go;

				// nope, create a new one
				if( !_instance )
				{
					var obj = new GameObject( "GoKit" );
					_instance = obj.AddComponent<Go>();
					DontDestroyOnLoad( obj );
				}
			}

			return _instance;
		}
	}
	
	
	/// <summary>
	/// loops through all the Tweens and updates any that are of updateType. If any Tweens are complete
	/// (the update call will return true) they are removed.
	/// </summary>
	private void handleUpdateOfType( UpdateType updateType, float deltaTime )
	{
		// loop backwards so we can remove completed tweens
		for( var i = _tweens.Count - 1; i >= 0; --i )
		{
			var t = _tweens[i];
			
			// only process tweens with our update type that are running
			if( t.updateType == updateType && t.state == TweenState.Running && t.update( deltaTime * t.timeScale ) )
			{
				// tween is complete if we get here. if destroyed or set to auto remove kill it
				if( t.state == TweenState.Destroyed || t.autoRemoveOnComplete )
				{
					removeTween( t );
					t.destroy();
				}
			}
		}
	}
	
	
	#region Monobehaviour

	private void Update()
	{
		if( _tweens.Count == 0 )
			return;
		
		handleUpdateOfType( UpdateType.Update, Time.deltaTime );
	}
	
	
	private void LateUpdate()
	{
		if( _tweens.Count == 0 )
			return;
		
		handleUpdateOfType( UpdateType.LateUpdate, Time.deltaTime );
	}

	
	private void FixedUpdate()
	{
		if( _tweens.Count == 0 )
			return;
		
		handleUpdateOfType( UpdateType.FixedUpdate, Time.deltaTime );
	}
	
	#endregion
	
	
    /// <summary>
    /// this only runs as needed and handles time scale independent Tweens
    /// </summary>
    private IEnumerator timeScaleIndependentUpdate()
    {
		_timeScaleIndependentUpdateIsRunning = true;
		var time = 0f;
		
        while( _tweens.Count > 0 )
        {
            var elapsed = Time.realtimeSinceStartup - time;
            time = Time.realtimeSinceStartup;

            // update tweens
            handleUpdateOfType( UpdateType.TimeScaleIndependentUpdate, elapsed );

            yield return null;
        }
		
		_timeScaleIndependentUpdateIsRunning = false;
    }
	
	
	/// <summary>
	/// checks for duplicate properties. if one is found and the DuplicatePropertyRuleType is set to
	/// DontAddCurrentProperty it will return true indicating that the tween should not be added.
	/// this only checks tweens that are not part of an AbstractTweenCollection
	/// </summary>
	private static bool handleDuplicatePropertiesInTween( Tween tween )
	{
		// first fetch all the current tweens with the same target object as this one
		var allTweensWithTarget = tweensWithTarget( tween.target );

		// store a list of all the properties in the tween
		var allProperties = tween.allTweenProperties();
		
		// TODO: perhaps only perform the check on running Tweens?
		
		// loop through all the tweens with the same target
		foreach( var tweenWithTarget in allTweensWithTarget )
		{
			// loop through all the properties in the tween and see if there are any dupes
			foreach( var tweenProp in allProperties )
			{
				warn( "found duplicate TweenProperty {0} in tween {1}", tweenProp, tween );
				
				// check for a matched property
				if( tweenWithTarget.containsTweenProperty( tweenProp ) )
				{
					// handle the different duplicate property rules
					if( duplicatePropertyRule == DuplicatePropertyRuleType.DontAddCurrentProperty )
					{
						return true;
					}
					else if( duplicatePropertyRule == DuplicatePropertyRuleType.RemoveRunningProperty )
					{
						// TODO: perhaps check if the Tween has any properties left and remove it if it doesnt?
						tweenWithTarget.removeTweenProperty( tweenProp );
					}
					
					return false;
				}
			}
		}
		
		return false;
	}
	
	
	#region Logging
	
	/// <summary>
	/// logging should only occur in the editor so we use a conditional
	/// </summary>
	[System.Diagnostics.Conditional( "UNITY_EDITOR" )]
	private static void log( object format, params object[] paramList )
	{
		if( format is string )
			Debug.Log( string.Format( format as string, paramList ) );
		else
			Debug.Log( format );
	}
	
	
	[System.Diagnostics.Conditional( "UNITY_EDITOR" )]
	public static void warn( object format, params object[] paramList )
	{
		if( logLevel == GoLogLevel.None || logLevel == GoLogLevel.Info )
			return;

		if( format is string )
			Debug.LogWarning( string.Format( format as string, paramList ) );
		else
			Debug.LogWarning( format );
	}
	
	
	[System.Diagnostics.Conditional( "UNITY_EDITOR" )]
	public static void error( object format, params object[] paramList )
	{
		if( logLevel == GoLogLevel.None || logLevel == GoLogLevel.Info || logLevel == GoLogLevel.Warn )
			return;

		if( format is string )
			Debug.LogError( string.Format( format as string, paramList ) );
		else
			Debug.LogError( format );
	}
	
	#endregion

	
	#region public API
	
	/// <summary>
	/// helper function that creates a "to" Tween and adds it to the pool
	/// </summary>
	public static Tween to( object target, float duration, TweenConfig config )
	{
		var tween = new Tween( target, duration, config );
		addTween( tween );

		return tween;
	}
	
	
	/// <summary>
	/// helper function that creates a "from" Tween and adds it to the pool
	/// </summary>
	public static Tween from( object target, float duration, TweenConfig config )
	{
		config.setIsFrom();
		var tween = new Tween( target, duration, config );
		addTween( tween );
		
		return tween;
	}
	
	
	/// <summary>
	/// adds an AbstractTween (Tween, TweenChain or TweenFlow) to the current list of running Tweens
	/// </summary>
	public static void addTween( AbstractTween tween )
	{
		// early out for invalid items
		if( !tween.isValid() )
			return;
		
		// dont add the same tween twice
		if( _tweens.Contains( tween ) )
			return;
		
		// check for dupes and handle them before adding the tween. we only need to check for Tweens
		if( duplicatePropertyRule != DuplicatePropertyRuleType.None && tween is Tween )
		{
			// if handleDuplicatePropertiesInTween returns true it indicates we should not add this tween
			if( handleDuplicatePropertiesInTween( tween as Tween ) )
				return;
			
			// if we became invalid after handling dupes dont add the tween
			if( !tween.isValid() )
				return;
		}
		
		_tweens.Add( tween );
		
		// enable ourself if we are not enabled
		if( !instance.enabled ) // purposely using the static instace property just once for initialization
			_instance.enabled = true;
		
		// if the Tween isn't paused and it is a "from" tween jump directly to the start position
		if( tween is Tween && ((Tween)tween).isFrom && tween.state != TweenState.Paused )
			tween.update( 0 );
		
		// should we start up the time scale independent update?
		if( !_instance._timeScaleIndependentUpdateIsRunning && tween.updateType == UpdateType.TimeScaleIndependentUpdate )
			_instance.StartCoroutine( _instance.timeScaleIndependentUpdate() );
	}
	
	
	/// <summary>
	/// removes the Tween returning true if it was removed or false if it was not found
	/// </summary>
	public static bool removeTween( AbstractTween tween )
	{
		if( _tweens.Contains( tween ) )
		{
			_tweens.Remove( tween );
			
			if( _tweens.Count == 0 )
			{
				// disable ourself if we have no more tweens
				instance.enabled = false;
			}
			
			return true;
		}
		
		return false;
	}
	
	
	/// <summary>
	/// returns a list of all Tweens, TweenChains and TweenFlows with the given id
	/// </summary>
	public static List<AbstractTween> tweensWithId( int id )
	{
		List<AbstractTween> list = null;
		
		foreach( var tween in _tweens )
		{
			if( tween.id == id )
			{
				if( list == null )
					list = new List<AbstractTween>();
				list.Add( tween );
			}
		}
		
		return list;
	}
	
	
	/// <summary>
	/// returns a list of all Tweens with the given target. TweenChains and TweenFlows can optionally
	/// be traversed and matching Tweens returned as well.
	/// </summary>
	public static List<Tween> tweensWithTarget( object target, bool traverseCollections = false )
	{
		List<Tween> list = new List<Tween>();
		
		foreach( var item in _tweens )
		{
			// we always check Tweens so handle them first
			var tween = item as Tween;
			if( tween != null && tween.target == target )
				list.Add( tween );
			
			// optionally check TweenChains and TweenFlows. if tween is null we have a collection
			if( traverseCollections && tween == null )
			{
				var tweenCollection = item as AbstractTweenCollection;
				if( tweenCollection != null )
				{
					var tweensInCollection = tweenCollection.tweensWithTarget( target );
					if( tweensInCollection.Count > 0 )
						list.AddRange( tweensInCollection );
				}
			}
		}
		
		return list;
	}
	
	
	/// <summary>
	/// kills all tweens with the given target by calling the destroy method on each one
	/// </summary>
	public static void killAllTweensWithTarget( object target )
	{
		foreach( var tween in tweensWithTarget( target, true ) )
			tween.destroy();
	}
	
	#endregion

}
