using UnityEngine;
using System.Collections;

public class GoDataType 
{

	public float floatType { get; set; }
	
	public int intType { get; set; }
	
	public Vector2 vector2Type { get; set; }
    
	public Vector3 vector3Type { get; set; }
    
	public Vector4 vector4Type { get; set; }
    
	public Color colorType { get; set; }
    
	public GoDataType( float floatStart )
	{
		floatType = floatStart;
	}
	
	public GoDataType( int intStart )
	{
		intType = intStart;
    }
    
	public GoDataType( Vector2 vector2Start )
	{
		vector2Type = vector2Start;
    }
    
	public GoDataType( Vector3 vector3Start )
	{
		vector3Type = vector3Start;
    }   
    
	public GoDataType( Vector4 vector4Start )
	{
		vector4Type = vector4Start;
    }   
    
	public GoDataType( Color colorStart )
	{
		colorType = colorStart;
    }                                             
    
}
