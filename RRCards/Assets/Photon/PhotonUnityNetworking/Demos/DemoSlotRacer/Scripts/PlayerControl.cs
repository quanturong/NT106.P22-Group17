
using UnityEngine;

using System.Collections;

using Photon.Pun.Demo.SlotRacer.Utils;
using Photon.Pun.UtilityScripts;

namespace Photon.Pun.Demo.SlotRacer
{
    [RequireComponent(typeof(SplineWalker))]
    public class PlayerControl : MonoBehaviourPun, IPunObservable
    {
        public GameObject[] CarPrefabs;
        public float MaximumSpeed = 20;
        public float Drag = 5;
        private float CurrentSpeed;
        private float CurrentDistance;
        private GameObject CarInstance;
        private SplineWalker SplineWalker;
        private bool m_firstTake = true;


        private float m_input;


        #region IPunObservable implementation
        void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(this.CurrentDistance);
                stream.SendNext(this.CurrentSpeed);
                stream.SendNext(this.m_input);
            }
            else
            {
                if (this.m_firstTake)
                {
                    this.m_firstTake = false;
                }

                this.CurrentDistance = (float) stream.ReceiveNext();
                this.CurrentSpeed = (float) stream.ReceiveNext();
                this.m_input = (float) stream.ReceiveNext();
            }
        }

        #endregion IPunObservable implementation


        #region private
        private void SetupCarOnTrack(int gridStartIndex)
        {
            this.SplineWalker.spline = SlotLanes.Instance.GridPositions[gridStartIndex].Spline;
            this.SplineWalker.currentDistance = SlotLanes.Instance.GridPositions[gridStartIndex].currentDistance;
            this.SplineWalker.ExecutePositioning();
            this.CarInstance = (GameObject) Instantiate(this.CarPrefabs[gridStartIndex], this.transform.position, this.transform.rotation);
            if (!this.photonView.IsMine)
            {
                this.CarInstance.SetActive(false);
            }
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                this.m_firstTake = false;
            }

            this.CarInstance.transform.SetParent(this.transform);
        }

        #endregion private


        #region Monobehaviour
        private void Awake()
        {
            this.SplineWalker = this.GetComponent<SplineWalker>();
            this.m_firstTake = true;
        }
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => this.photonView.Owner.GetPlayerNumber() >= 0);
            this.SetupCarOnTrack(this.photonView.Owner.GetPlayerNumber());
        }
        private void OnDestroy()
        {
            Destroy(this.CarInstance);
        }
        private void Update()
        {
            if (this.SplineWalker == null || this.CarInstance == null)
            {
                return;
            }

            if (this.photonView.IsMine)
            {
                this.m_input = Input.GetAxis("Vertical");
                if (this.m_input == 0f)
                {
                    this.CurrentSpeed -= Time.deltaTime * this.Drag;
                }
                else
                {
                    this.CurrentSpeed += this.m_input;
                }
                this.CurrentSpeed = Mathf.Clamp(this.CurrentSpeed, 0f, this.MaximumSpeed);
                this.SplineWalker.Speed = this.CurrentSpeed;

                this.CurrentDistance = this.SplineWalker.currentDistance;
            }
            else
            {
				if (this.m_input == 0f)
				{
					this.CurrentSpeed -= Time.deltaTime * this.Drag;
				}

				this.CurrentSpeed = Mathf.Clamp (this.CurrentSpeed, 0f, this.MaximumSpeed);
				this.SplineWalker.Speed = this.CurrentSpeed;



				if (this.CurrentDistance != 0 && this.SplineWalker.currentDistance != this.CurrentDistance)
				{
					this.SplineWalker.Speed += (this.CurrentDistance - this.SplineWalker.currentDistance) * Time.deltaTime * 50f;
				}

            }
            if (!this.m_firstTake && !this.CarInstance.activeSelf)
            {
                this.CarInstance.SetActive(true);
				this.SplineWalker.Speed = this.CurrentSpeed;
				this.SplineWalker.SetPositionOnSpline (this.CurrentDistance);

            }
        }

        #endregion Monobehaviour
    }
}