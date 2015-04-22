using UnityEngine;
using System.Collections;

public class GoProxyProp<T>  
{

	public T value { get; set; }
	
	public GoProxyProp( T startValue )
	{
		value = startValue;
	}                                                   
    
}
