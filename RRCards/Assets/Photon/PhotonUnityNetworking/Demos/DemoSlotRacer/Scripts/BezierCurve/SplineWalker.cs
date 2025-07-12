
using UnityEngine;

namespace Photon.Pun.Demo.SlotRacer.Utils
{
	[ExecuteInEditMode]
	public class SplineWalker : MonoBehaviour {

		public BezierSpline spline;

		public float Speed = 0f;

		public bool lookForward;

		public bool reverse;

		private float progress;

		public float currentDistance =0f;

		public float currentClampedDistance;

		public void SetPositionOnSpline(float position)
		{
			currentDistance = position;
			ExecutePositioning ();
		}

		void Update()
		{
			currentDistance += Speed * Time.deltaTime;
			ExecutePositioning ();
		}

		public void ExecutePositioning()
		{
			if(spline==null)
			{
				return;
			}
			transform.position = spline.GetPositionAtDistance(currentDistance,this.reverse);
			currentDistance += Speed * Time.deltaTime;


			if (lookForward) {
				transform.LookAt(spline.GetPositionAtDistance(currentDistance+1,this.reverse));
			}
		}
	}	
}