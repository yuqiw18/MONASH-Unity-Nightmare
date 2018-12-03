using UnityEngine;
using System.Collections;

public class Script_Cursor : MonoBehaviour {

	public bool showCursor;
	public bool lockCursor;

	// Use this for initialization
	void Start () {
		Cursor.visible = showCursor;
		Screen.lockCursor = lockCursor;
	}
}
