using UnityEngine;


public static class GoEaseLinear
{
    public static float EaseNone( float t, float b, float c, float d )
    {
        return c * t / d + b;
    }
	
	
    public static float Punch( float t, float b, float c, float d )
    {
        if( t == 0 )
            return 0;

        if( ( t /= d ) == 1 )
            return 0;
 
        const float p = 0.3f;
        return ( c * Mathf.Pow( 2, -10 * t ) * Mathf.Sin( t * ( 2 * Mathf.PI ) / p ) );
    }
}