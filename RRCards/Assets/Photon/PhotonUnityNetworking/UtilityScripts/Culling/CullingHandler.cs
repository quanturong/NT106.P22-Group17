
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;

namespace Photon.Pun.UtilityScripts
{
    using ExitGames.Client.Photon;
    [RequireComponent(typeof(PhotonView))]
    public class CullingHandler : MonoBehaviour, IPunObservable
    {
        #region VARIABLES

        private int orderIndex;

        private CullArea cullArea;

        private List<byte> previousActiveCells, activeCells;

        private PhotonView pView;

        private Vector3 lastPosition, currentPosition;
        private float timeSinceUpdate;
        private float timeBetweenUpdatesMin = 0.33f;


        #endregion

        #region UNITY_FUNCTIONS
        private void OnEnable()
        {
            if (this.pView == null)
            {
                this.pView = GetComponent<PhotonView>();

                if (!this.pView.IsMine)
                {
                    return;
                }
            }

            if (this.cullArea == null)
            {
                #if UNITY_6000_0_OR_NEWER
                this.cullArea = FindFirstObjectByType<CullArea>();
                #else
                this.cullArea = FindObjectOfType<CullArea>();
                #endif
            }

            this.previousActiveCells = new List<byte>(0);
            this.activeCells = new List<byte>(0);

            this.currentPosition = this.lastPosition = transform.position;
        }
        private void Start()
        {
            if (!this.pView.IsMine)
            {
                return;
            }

            if (PhotonNetwork.InRoom)
            {
                if (this.cullArea.NumberOfSubdivisions == 0)
                {
                    this.pView.Group = this.cullArea.FIRST_GROUP_ID;

                    PhotonNetwork.SetInterestGroups(this.cullArea.FIRST_GROUP_ID, true);
                }
                else
                {
                    this.pView.ObservedComponents.Add(this);
                }
            }
        }
        private void Update()
        {
            if (!this.pView.IsMine)
            {
                return;
            }
            this.timeSinceUpdate += Time.deltaTime;
            if (this.timeSinceUpdate < this.timeBetweenUpdatesMin)
            {
                return;
            }

            this.lastPosition = this.currentPosition;
            this.currentPosition = transform.position;
            if (this.currentPosition != this.lastPosition)
            {
                if (this.HaveActiveCellsChanged())
                {
                    this.UpdateInterestGroups();
                    this.timeSinceUpdate = 0;
                }
            }
        }
        private void OnGUI()
        {
            if (!this.pView.IsMine)
            {
                return;
            }

            string subscribedAndActiveCells = "Inside cells:\n";
            string subscribedCells = "Subscribed cells:\n";

            for (int index = 0; index < this.activeCells.Count; ++index)
            {
                if (index <= this.cullArea.NumberOfSubdivisions)
                {
                    subscribedAndActiveCells += this.activeCells[index] + " | ";
                }

                subscribedCells += this.activeCells[index] + " | ";
            }
            GUI.Label(new Rect(20.0f, Screen.height - 120.0f, 200.0f, 40.0f), "<color=white>PhotonView Group: " + this.pView.Group + "</color>", new GUIStyle() { alignment = TextAnchor.UpperLeft, fontSize = 16 });
            GUI.Label(new Rect(20.0f, Screen.height - 100.0f, 200.0f, 40.0f), "<color=white>" + subscribedAndActiveCells + "</color>", new GUIStyle() { alignment = TextAnchor.UpperLeft, fontSize = 16 });
            GUI.Label(new Rect(20.0f, Screen.height - 60.0f, 200.0f, 40.0f), "<color=white>" + subscribedCells + "</color>", new GUIStyle() { alignment = TextAnchor.UpperLeft, fontSize = 16 });
        }

        #endregion
        private bool HaveActiveCellsChanged()
        {
            if (this.cullArea.NumberOfSubdivisions == 0)
            {
                return false;
            }

            this.previousActiveCells = new List<byte>(this.activeCells);
            this.activeCells = this.cullArea.GetActiveCells(transform.position);
            while (this.activeCells.Count <= this.cullArea.NumberOfSubdivisions)
            {
                this.activeCells.Add(this.cullArea.FIRST_GROUP_ID);
            }

            if (this.activeCells.Count != this.previousActiveCells.Count)
            {
                return true;
            }

            if (this.activeCells[this.cullArea.NumberOfSubdivisions] != this.previousActiveCells[this.cullArea.NumberOfSubdivisions])
            {
                return true;
            }

            return false;
        }
        private void UpdateInterestGroups()
        {
            List<byte> disable = new List<byte>(0);

            foreach (byte groupId in this.previousActiveCells)
            {
                if (!this.activeCells.Contains(groupId))
                {
                    disable.Add(groupId);
                }
            }

            PhotonNetwork.SetInterestGroups(disable.ToArray(), this.activeCells.ToArray());
        }

        #region IPunObservable implementation
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            while (this.activeCells.Count <= this.cullArea.NumberOfSubdivisions)
            {
                this.activeCells.Add(this.cullArea.FIRST_GROUP_ID);
            }

            if (this.cullArea.NumberOfSubdivisions == 1)
            {
                this.orderIndex = (++this.orderIndex % this.cullArea.SUBDIVISION_FIRST_LEVEL_ORDER.Length);
                this.pView.Group = this.activeCells[this.cullArea.SUBDIVISION_FIRST_LEVEL_ORDER[this.orderIndex]];
            }
            else if (this.cullArea.NumberOfSubdivisions == 2)
            {
                this.orderIndex = (++this.orderIndex % this.cullArea.SUBDIVISION_SECOND_LEVEL_ORDER.Length);
                this.pView.Group = this.activeCells[this.cullArea.SUBDIVISION_SECOND_LEVEL_ORDER[this.orderIndex]];
            }
            else if (this.cullArea.NumberOfSubdivisions == 3)
            {
                this.orderIndex = (++this.orderIndex % this.cullArea.SUBDIVISION_THIRD_LEVEL_ORDER.Length);
                this.pView.Group = this.activeCells[this.cullArea.SUBDIVISION_THIRD_LEVEL_ORDER[this.orderIndex]];
            }
        }

        #endregion
    }
}