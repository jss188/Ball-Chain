using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class saveMesh : MonoBehaviour {

	// Use this for initialization
	void Start(){
		#if UNITY_EDITOR
		MeshFilter mfilter = GetComponent<MeshFilter>();
		// Create a simple material asset
		AssetDatabase.CreateAsset(mfilter.mesh, "Assets/raceTrack.asset");
		#endif
	}

}
