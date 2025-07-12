
using UnityEngine;

namespace Photon.Pun.Demo.SlotRacer.Utils
{
	[ExecuteInEditMode]
	public class SplinePosition : MonoBehaviour {

		public BezierSpline Spline;
		public bool reverse;
		public bool lookForward;
		public float currentDistance = 0f;

		public float currentClampedDistance;

		float LastDistance;

		public void SetPositionOnSpline(float position)
		{
			this.currentDistance = position;
			ExecutePositioning ();
		}

		void Update()
		{
			ExecutePositioning ();
		}

		void ExecutePositioning()
		{
			if(this.Spline==null || this.LastDistance == this.currentDistance )
			{
				return;
			}
			LastDistance = this.currentDistance;
			this.transform.position = this.Spline.GetPositionAtDistance(currentDistance,this.reverse);

			if (this.lookForward) {
				this.transform.LookAt(this.Spline.GetPositionAtDistance(currentDistance+1,this.reverse));
			}
		}


	}
}