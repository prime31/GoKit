using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// place this script on any GameObject to enable route editing. note that it is not required at runtime! it is
/// only required to be in your scene while editing a path
/// </summary>
public class GoDummyPath : MonoBehaviour
{
	public string pathName = string.Empty;
	public Color pathColor = Color.magenta; // color of the path if visible in the editor
	public List<Vector3> nodes = new List<Vector3>() { Vector3.zero, Vector3.zero };
	public bool useStandardHandles = false;
}
