using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Photon.Pun.Demo.Procedural
{
    public enum WorldSize
    {
        Tiny = 16,
        Small = 32,
        Medium = 64,
        Large = 128
    }
    public enum WorldType
    {
        Flat = 4,
        Standard = 8,
        Mountain = 16
    }
    public enum ClusterSize
    {
        Small = 4,
        Medium = 16,
        Large = 64
    }
    public class WorldGenerator : MonoBehaviour
    {
        public readonly string SeedPropertiesKey = "Seed";
        public readonly string WorldSizePropertiesKey = "WorldSize";
        public readonly string ClusterSizePropertiesKey = "ClusterSize";
        public readonly string WorldTypePropertiesKey = "WorldType";

        private static WorldGenerator instance;

        public static WorldGenerator Instance
        {
            get
            {
                if (instance == null)
                {
                    #if UNITY_6000_0_OR_NEWER
                    instance = FindFirstObjectByType<WorldGenerator>();
                    #else
                    instance = FindObjectOfType<WorldGenerator>();
                    #endif
                }

                return instance;
            }
        }

        public int Seed { get; set; }

        public WorldSize WorldSize { get; set; }

        public ClusterSize ClusterSize { get; set; }

        public WorldType WorldType { get; set; }

        private Dictionary<int, GameObject> clusterList;

        public Material[] WorldMaterials;

        #region UNITY

        public void Awake()
        {
            clusterList = new Dictionary<int, GameObject>();

            WorldSize = WorldSize.Tiny;
            ClusterSize = ClusterSize.Small;
            WorldType = WorldType.Standard;
        }

        #endregion

        #region CLASS FUNCTIONS
        public void CreateWorld()
        {
            StopAllCoroutines();
            DestroyWorld();
            StartCoroutine(GenerateWorld());
        }
        private void DestroyWorld()
        {
            foreach (GameObject cluster in clusterList.Values)
            {
                Cluster clusterComponent = cluster.GetComponent<Cluster>();
                clusterComponent.DestroyCluster();

                Destroy(cluster);
            }

            clusterList.Clear();
        }
        public void ConfirmAndUpdateProperties()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            Hashtable properties = new Hashtable
            {
                {SeedPropertiesKey, Seed},
                {WorldSizePropertiesKey, (int) WorldSize},
                {ClusterSizePropertiesKey, (int) ClusterSize},
                {WorldTypePropertiesKey, (int) WorldType}
            };

            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }
        public void DecreaseBlockHeight(int clusterId, int blockId)
        {
            Cluster c = clusterList[clusterId].GetComponent<Cluster>();

            if (c != null)
            {
                c.DecreaseBlockHeight(blockId);
            }
        }
        public void IncreaseBlockHeight(int clusterId, int blockId)
        {
            Cluster c = clusterList[clusterId].GetComponent<Cluster>();

            if (c != null)
            {
                c.IncreaseBlockHeight(blockId);
            }
        }

        #endregion

        #region COROUTINES
        private IEnumerator GenerateWorld()
        {
            Debug.Log(string.Format("<b>Procedural Demo</b>: Creating world using Seed: {0}, World Size: {1}, Cluster Size: {2} and World Type: {3}", Seed, WorldSize, ClusterSize, WorldType));

            Simplex.Noise.Seed = Seed;

            int clusterId = 0;
            for (int x = 0; x < (int) WorldSize; x += (int) Mathf.Sqrt((int) ClusterSize))
            {
                for (int z = 0; z < (int) WorldSize; z += (int) Mathf.Sqrt((int) ClusterSize))
                {
                    GameObject cluster = new GameObject();
                    cluster.name = "Cluster " + clusterId;

                    cluster.transform.SetParent(transform);
                    cluster.transform.position = new Vector3(x, 0.0f, z);

                    Cluster clusterComponent = cluster.AddComponent<Cluster>();
                    clusterComponent.ClusterId = clusterId;

                    clusterList.Add(clusterId++, cluster);
                }
            }

            yield return new WaitForEndOfFrame();
            foreach (GameObject cluster in clusterList.Values)
            {
                Vector3 clusterPosition = cluster.transform.position;

                int blockId = 0;

                for (int x = 0; x < (int) Mathf.Sqrt((int) ClusterSize); ++x)
                {
                    for (int z = 0; z < (int) Mathf.Sqrt((int) ClusterSize); ++z)
                    {
                        float noiseValue = Simplex.Noise.CalcPixel2D((int) clusterPosition.x + x, (int) clusterPosition.z + z, 0.02f);

                        int height = (int) noiseValue / (int) (256.0f / (float) WorldType);
                        int materialIndex = (int) noiseValue / (int) (256.0f / WorldMaterials.Length);

                        GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        block.name = "Block " + blockId;

                        block.transform.SetParent(cluster.transform);
                        block.transform.localScale = new Vector3(1.0f, height, 1.0f);
                        block.transform.position = new Vector3(clusterPosition.x + x, height / 2.0f, clusterPosition.z + z);
                        block.GetComponent<MeshRenderer>().material = WorldMaterials[materialIndex];

                        Block blockComponent = block.AddComponent<Block>();
                        blockComponent.BlockId = blockId;
                        blockComponent.ClusterId = cluster.GetComponent<Cluster>().ClusterId;

                        cluster.GetComponent<Cluster>().AddBlock(blockId++, block);
                    }
                }

                yield return new WaitForEndOfFrame();
            }
            foreach (DictionaryEntry entry in PhotonNetwork.CurrentRoom.CustomProperties)
            {
                if (entry.Value == null)
                {
                    continue;
                }

                string key = entry.Key.ToString();

                if ((key == SeedPropertiesKey) || (key == WorldSizePropertiesKey) || (key == ClusterSizePropertiesKey) || (key == WorldTypePropertiesKey))
                {
                    continue;
                }

                int indexOfBlank = key.IndexOf(' ');
                key = key.Substring(indexOfBlank + 1, (key.Length - (indexOfBlank + 1)));

                int.TryParse(key, out clusterId);

                GameObject cluster;
                if (clusterList.TryGetValue(clusterId, out cluster))
                {
                    Cluster c = cluster.GetComponent<Cluster>();

                    if (c != null)
                    {
                        Dictionary<int, float> clusterModifications = (Dictionary<int, float>) entry.Value;

                        foreach (KeyValuePair<int, float> pair in clusterModifications)
                        {
                            c.SetBlockHeightRemote(pair.Key, pair.Value);
                        }
                    }
                }
            }
        }

        #endregion
    }
}