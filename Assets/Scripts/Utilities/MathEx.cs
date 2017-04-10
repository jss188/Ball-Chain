using UnityEngine;
using System.Collections;

public class MathEx : MonoBehaviour {

	public static Vector3 closestPositionOnLine (Vector3 line, Vector3 point){
		float dot = Vector3.Dot (point, line.normalized);
		Vector3 projection = line * Mathf.Clamp01 (dot);
		return projection;
	}

	public static Vector3 closestPositionOnLine (Vector3 start, Vector3 end, Vector3 point) {
		float t;
		return closestPositionOnLine(start,end,point, out t);
	}

	public static Vector3 closestPositionOnLine (Vector3 start, Vector3 end, Vector3 point, out float t){
		Vector3 offsetFromStart = (point - start);
		Vector3 tangent = (end - start);

		float dot = Vector3.Dot (offsetFromStart, tangent.normalized);

		t = Mathf.Clamp01(dot / tangent.magnitude);

		Debug.DrawLine(start, start + offsetFromStart, Color.white);
		Debug.DrawLine(start, Vector3.Lerp(start,end, 0.8f), Color.blue);
		Debug.DrawLine(Vector3.Lerp(start,end, 0.8f), end, Color.red);
		Debug.DrawLine(start + offsetFromStart, Vector3.Lerp(start,end, t), Color.cyan);

		return Vector3.Lerp(start,end, t);
		//return projectedPoint;
	}

	public static float closestTOnLine (Vector3 start, Vector3 end, Vector3 point){
		Vector3 offsetFromStart = (point - start);
		Vector3 tangent = (end - start);

		float dot = Vector3.Dot (offsetFromStart, tangent.normalized);
		Vector3 projectedPoint = tangent.normalized * dot;

		float closestT = Mathf.Clamp01(projectedPoint.magnitude / tangent.magnitude);

		Debug.DrawLine(start, start + offsetFromStart, Color.white);
		Debug.DrawLine(start, Vector3.Lerp(start,end, 0.8f), Color.blue);
		Debug.DrawLine(Vector3.Lerp(start,end, 0.8f), end, Color.red);
		Debug.DrawLine(start + offsetFromStart, Vector3.Lerp(start,end, closestT), Color.cyan);

		return closestT;
	}
}
