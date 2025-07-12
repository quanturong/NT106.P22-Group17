using System.Collections.Generic;

using UnityEngine;

using ExitGames.Client.Photon;

namespace Photon.Pun.Demo.Procedural
{
    public class Cluster : MonoBehaviourPunCallbacks
    {
        private string propertiesKey;

        private Dictionary<int, float> propertiesValue;

        public int ClusterId { get; set; }

        public Dictionary<int, GameObject> Blocks { get; private set; }

        #region UNITY

        public void Awake()
        {
            Blocks = new Dictionary<int, GameObject>();

            propertiesValue = new Dictionary<int, float>();
        }
        private void Start()
        {
            propertiesKey = "Cluster " + ClusterId;
        }

        #endregion

        #region CLASS FUNCTIONS
        public void AddBlock(int blockId, GameObject block)
        {
            Blocks.Add(blockId, block);
        }
        public void DestroyCluster()
        {
            foreach (GameObject block in Blocks.Values)
            {
                Destroy(block);
            }

            Blocks.Clear();

            if (PhotonNetwork.IsMasterClient)
            {
                RemoveClusterFromRoomProperties();
            }
        }
        public void DecreaseBlockHeight(int blockId)
        {
            float height = Blocks[blockId].transform.localScale.y;
            height = Mathf.Max((height - 1.0f), 0.0f);

            SetBlockHeightLocal(blockId, height);
        }
        public void IncreaseBlockHeight(int blockId)
        {
            float height = Blocks[blockId].transform.localScale.y;
            height = Mathf.Min((height + 1.0f), 8.0f);

            SetBlockHeightLocal(blockId, height);
        }
        public void SetBlockHeightRemote(int blockId, float height)
        {
            GameObject block = Blocks[blockId];

            Vector3 scale = block.transform.localScale;
            Vector3 position = block.transform.localPosition;

            block.transform.localScale = new Vector3(scale.x, height, scale.z);
            block.transform.localPosition = new Vector3(position.x, height / 2.0f, position.z);
        }
        private void SetBlockHeightLocal(int blockId, float height)
        {
            GameObject block = Blocks[blockId];

            Vector3 scale = block.transform.localScale;
            Vector3 position = block.transform.localPosition;

            block.transform.localScale = new Vector3(scale.x, height, scale.z);
            block.transform.localPosition = new Vector3(position.x, height / 2.0f, position.z);

            UpdateRoomProperties(blockId, height);
        }
        private void UpdateRoomProperties(int blockId, float height)
        {
            propertiesValue[blockId] = height;

            Hashtable properties = new Hashtable {{propertiesKey, propertiesValue}};
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }
        private void RemoveClusterFromRoomProperties()
        {
            Hashtable properties = new Hashtable {{propertiesKey, null}};
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }

        #endregion

        #region PUN CALLBACKS
        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.ContainsKey(propertiesKey))
            {
                if (propertiesThatChanged[propertiesKey] == null)
                {
                    propertiesValue = new Dictionary<int, float>();
                    return;
                }

                propertiesValue = (Dictionary<int, float>) propertiesThatChanged[propertiesKey];

                foreach (KeyValuePair<int, float> pair in propertiesValue)
                {
                    SetBlockHeightRemote(pair.Key, pair.Value);
                }
            }
        }

        #endregion
    }
}