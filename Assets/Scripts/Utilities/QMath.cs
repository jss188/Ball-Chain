using UnityEngine;
using System.Collections;

public class QMath : MonoBehaviour {

	public static Quaternion Normalize (Quaternion q){
		float magnitude = Magnitude(q);
		return new Quaternion(q.x / magnitude,
		                      q.y / magnitude,
		                      q.z / magnitude,
		                      q.w / magnitude);
	}

	public static Quaternion Squad (Quaternion q0, Quaternion q1, Quaternion q2, Quaternion q3, float t){
		Quaternion Q0 = ShortestPath(q0, q1);
		Quaternion Q1 = q1;
		Quaternion Q2 = ShortestPathReverse(q1, q2);
		Quaternion Q3 = ShortestPathReverse(q2, q3);

		Quaternion s1 = getInnerQuad(Q0, Q1, Q2);
		Quaternion s2 = getInnerQuad(Q1, Q2, Q3);

		return SquadInterpolate(Q1, s1, s2, Q2, t);
	}

	public static Quaternion SquadInterpolate(Quaternion q1, Quaternion a, Quaternion b, Quaternion q2, float t){
		float h = 2*t * (1-t);
		Quaternion slerpCQ = SlerpNoInvert(q1, q2, t);
		Quaternion slerpIQ = SlerpNoInvert(a, b, t);
		Quaternion slerpBoth = SlerpNoInvert(slerpCQ, slerpIQ, h);
		return slerpBoth;
	}

	public static Quaternion getInnerQuad(Quaternion q0, Quaternion q1, Quaternion q2){
		Quaternion q1Inv = Inverse(q1);
		Quaternion p0 = q1Inv * q0;
		Quaternion p2 = q1Inv * q2;

		Quaternion inner = Multiply( Add( Log(p0),
		                                  Log(p2)),
		                           -0.25f);

		return Normalize(q1 * Exp (inner));
	}

	public static Quaternion ShortestPath(Quaternion q0, Quaternion q1){
		Quaternion added = Add (q0, q1);
		Quaternion subtracted = Subtract (q0, q1);
		return ( Magnitude(added) < Magnitude(subtracted) ) ? Negate(q0) : q0;
	}

	public static Quaternion ShortestPathReverse(Quaternion q0, Quaternion q1){
		Quaternion added = Add (q0, q1);
		Quaternion subtracted = Subtract (q0, q1);
		return ( Magnitude(added) < Magnitude(subtracted) ) ? Negate(q1) : q1;
	}

	public static Quaternion SlerpNoInvert(Quaternion p, Quaternion q, float t){
		t = Mathf.Clamp01(t);
		Quaternion qInv = Inverse(q);
		Quaternion pqInv = (p * qInv);
		Quaternion pqIPow = Pow( pqInv, 1f - t );
		Quaternion result = pqIPow * q;
		return Normalize( result );
	}

	public static Quaternion SlerpNoInvertAgain(Quaternion p, Quaternion q, float t){
		float cosTheta = Quaternion.Dot(p, q);
		float theta = Mathf.Acos(cosTheta);
		
		float pCoeff;
		float qCoeff;
		
		pCoeff = Mathf.Cos(theta * t);
		Quaternion pX = Multiply(p, pCoeff);
		
		Quaternion qX = Subtract(q, Multiply(p, cosTheta) );
		qCoeff = Mathf.Sin(theta * t) / Mathf.Sin(theta);
		qX = Multiply(qX, qCoeff);
		
		return Normalize( Add(pX, qX) );
	}

	public static Quaternion Slerp(Quaternion p, Quaternion q, float t){
		float cosTheta = Quaternion.Dot(p, q);
		float theta = Mathf.Acos(cosTheta);

		float pCoeff;
		float qCoeff;

		Quaternion pX = p;
		Quaternion qX = q;
		float sinTheta = Mathf.Sin(theta);

		if( cosTheta < 0 ){
			qX = Negate(q);
			cosTheta = -cosTheta;
			theta = Mathf.Acos(cosTheta);
		}

		if(cosTheta > 1 - Mathf.Epsilon){
			pCoeff = 1 - t;
			qCoeff = t;
		}
		else{
			pCoeff = Mathf.Sin( (1f - t) * theta )/ Mathf.Sin(theta);
			qCoeff = Mathf.Sin( t 		 * theta )/ Mathf.Sin(theta);
		}

		return Normalize( Add( Multiply(pX, pCoeff),
		                       Multiply(qX, qCoeff) ) );

	}

	public static Quaternion Negate(Quaternion q){
		return new Quaternion( -q.x,
		                       -q.y,
		                       -q.z,
		                       -q.w );
	}

	public static Quaternion Inverse (Quaternion q){
		Quaternion qConjugate = Conjugate(q);
		float qLength = Magnitude(q);
		return Multiply( qConjugate,
		                 1f / (qLength * qLength));
	}

	public static Quaternion Conjugate(Quaternion q){
		return new Quaternion(-q.x, -q.y, -q.z, q.w);
	}

	public static Quaternion Pow(Quaternion q, float pow){
		Quaternion log = Log(q);
		Quaternion multiLog = Multiply(log, pow);
		Quaternion expMultiLog = Exp( multiLog );
		return expMultiLog;
	}

	public static Quaternion Add(Quaternion q0, Quaternion q1){
		return new Quaternion(q0.x + q1.x,
		                      q0.y + q1.y,
		                      q0.z + q1.z,
		                      q0.w + q1.w);
	}

	public static Quaternion Subtract(Quaternion q0, Quaternion q1){
		return new Quaternion(q0.x - q1.x,
		                      q0.y - q1.y,
		                      q0.z - q1.z,
		                      q0.w - q1.w);
	}

	public static Quaternion Multiply (Quaternion q, float x){
		return new Quaternion(q.x * x,
		                      q.y * x,
		                      q.z * x,
		                      q.w * x);
	}

	public static Quaternion Log(Quaternion q){
		float theta = Mathf.Acos(q.w);
		float vCoeff = 0;

		if(theta > 0.00001f){
			vCoeff = theta / Mathf.Sin(theta);
		}

		return new Quaternion(q.x * vCoeff,
		                      q.y * vCoeff,
		                      q.z * vCoeff,
		                      0);
	}

	public static Quaternion Exp(Quaternion q){
		float theta = VectorPartMagnitude(q);
		float vCoeff = 0;
		if(theta > 0.00001f){
			vCoeff = Mathf.Sin(theta) / theta;
		}
		return new Quaternion(q.x * vCoeff,
		                      q.y * vCoeff,
		                      q.z * vCoeff,
		                      Mathf.Cos(theta));
	}

	public static float VectorPartMagnitude(Quaternion q){
		return Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z);
	}

	public static float Magnitude(Quaternion q){
		return Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
	}
}
