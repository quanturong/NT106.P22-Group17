
using UnityEngine;

using Photon.Pun.Demo.SlotRacer.Utils;

namespace Photon.Pun.Demo.SlotRacer
{
	public class SlotLanes : MonoBehaviour {
		public static SlotLanes Instance;
		public SplinePosition[] GridPositions;

		void Awake()
		{
			Instance = this;
		}
	}
}