
using UnityEngine;

namespace Photon.Pun.Demo.PunBasics
{
	public class LoaderAnime : MonoBehaviour {

		#region Public Variables

		[Tooltip("Angular Speed in degrees per seconds")]
		public float speed = 180f;

		[Tooltip("Radius os the loader")]
		public float radius = 1f;

		public GameObject particles;

		#endregion
		
		#region Private Variables

		Vector3 _offset;

		Transform _transform;

		Transform _particleTransform;

		bool _isAnimating;

		#endregion
		
		#region MonoBehaviour CallBacks
		void Awake()
		{
			_particleTransform =particles.GetComponent<Transform>();
			_transform = GetComponent<Transform>();
		}
		void Update () {
			if (_isAnimating)
			{
				_transform.Rotate(0f,0f,speed*Time.deltaTime);
				_particleTransform.localPosition = Vector3.MoveTowards(_particleTransform.localPosition, _offset, 0.5f*Time.deltaTime);
			}
		}
		#endregion

		#region Public Methods
		public void StartLoaderAnimation()
		{
			_isAnimating = true;
			_offset = new Vector3(radius,0f,0f);
            if (particles != null)
            {
                particles.SetActive(true);
            }
		}
		public void StopLoaderAnimation()
		{
            if (this.particles != null)
            {
                particles.SetActive(false);
            }
		}

		#endregion
	}
}