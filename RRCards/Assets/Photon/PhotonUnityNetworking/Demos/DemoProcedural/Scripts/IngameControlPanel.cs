using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.Demo.Procedural
{
    public class IngameControlPanel : MonoBehaviourPunCallbacks
    {
        private bool isSeedValid;

        private InputField seedInputField;
        private Dropdown worldSizeDropdown;
        private Dropdown clusterSizeDropdown;
        private Dropdown worldTypeDropdown;
        private Button generateWorldButton;

        #region UNITY
        public void Awake()
        {
            isSeedValid = false;

            seedInputField = GetComponentInChildren<InputField>();
            seedInputField.characterLimit = 10;
            seedInputField.characterValidation = InputField.CharacterValidation.Integer;
            seedInputField.interactable = PhotonNetwork.PhotonServerSettings.StartInOfflineMode;
            seedInputField.onValueChanged.AddListener((string value) =>
            {
                int seed;
                if (int.TryParse(value, out seed))
                {
                    isSeedValid = true;
                    WorldGenerator.Instance.Seed = seed;
                }
                else
                {
                    isSeedValid = false;
                    Debug.LogError("Invalid Seed entered. Only numeric Seeds are allowed.");
                }
            });
            this.seedInputField.text = (Random.Range(0, 256)).ToString();

            worldSizeDropdown = GetComponentsInChildren<Dropdown>()[0];
            worldSizeDropdown.interactable = PhotonNetwork.PhotonServerSettings.StartInOfflineMode;
            worldSizeDropdown.onValueChanged.AddListener((int value) =>
            {
                switch (value)
                {
                    case 0:
                    {
                        WorldGenerator.Instance.WorldSize = WorldSize.Tiny;
                        break;
                    }
                    case 1:
                    {
                        WorldGenerator.Instance.WorldSize = WorldSize.Small;
                        break;
                    }
                    case 2:
                    {
                        WorldGenerator.Instance.WorldSize = WorldSize.Medium;
                        break;
                    }
                    case 3:
                    {
                        WorldGenerator.Instance.WorldSize = WorldSize.Large;
                        break;
                    }
                }
            });

            clusterSizeDropdown = GetComponentsInChildren<Dropdown>()[1];
            clusterSizeDropdown.interactable = PhotonNetwork.PhotonServerSettings.StartInOfflineMode;
            clusterSizeDropdown.onValueChanged.AddListener((int value) =>
            {
                switch (value)
                {
                    case 0:
                    {
                        WorldGenerator.Instance.ClusterSize = ClusterSize.Small;
                        break;
                    }
                    case 1:
                    {
                        WorldGenerator.Instance.ClusterSize = ClusterSize.Medium;
                        break;
                    }
                    case 2:
                    {
                        WorldGenerator.Instance.ClusterSize = ClusterSize.Large;
                        break;
                    }
                }
            });

            worldTypeDropdown = GetComponentsInChildren<Dropdown>()[2];
            worldTypeDropdown.interactable = PhotonNetwork.PhotonServerSettings.StartInOfflineMode;
            worldTypeDropdown.onValueChanged.AddListener((int value) =>
            {
                switch (value)
                {
                    case 0:
                    {
                        WorldGenerator.Instance.WorldType = WorldType.Flat;
                        break;
                    }
                    case 1:
                    {
                        WorldGenerator.Instance.WorldType = WorldType.Standard;
                        break;
                    }
                    case 2:
                    {
                        WorldGenerator.Instance.WorldType = WorldType.Mountain;
                        break;
                    }
                }
            });

            generateWorldButton = GetComponentInChildren<Button>();
            generateWorldButton.interactable = PhotonNetwork.PhotonServerSettings.StartInOfflineMode;
            generateWorldButton.onClick.AddListener(() =>
            {
                if (!PhotonNetwork.InRoom)
                {
                    Debug.LogError("Client is not in a room.");
                    return;
                }

                if (!isSeedValid)
                {
                    Debug.LogError("Invalid Seed entered. Only numeric Seeds are allowed.");
                    return;
                }

                WorldGenerator.Instance.ConfirmAndUpdateProperties();
            });
        }

        #endregion

        #region PUN CALLBACKS
        public override void OnJoinedRoom()
        {
            seedInputField.interactable = PhotonNetwork.IsMasterClient;
            worldSizeDropdown.interactable = PhotonNetwork.IsMasterClient;
            clusterSizeDropdown.interactable = PhotonNetwork.IsMasterClient;
            worldTypeDropdown.interactable = PhotonNetwork.IsMasterClient;
            generateWorldButton.interactable = PhotonNetwork.IsMasterClient;
        }
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            seedInputField.interactable = newMasterClient.IsLocal;
            worldSizeDropdown.interactable = newMasterClient.IsLocal;
            clusterSizeDropdown.interactable = newMasterClient.IsLocal;
            worldTypeDropdown.interactable = newMasterClient.IsLocal;
            generateWorldButton.interactable = newMasterClient.IsLocal;
        }
        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.ContainsKey(WorldGenerator.Instance.SeedPropertiesKey) && propertiesThatChanged.ContainsKey(WorldGenerator.Instance.WorldSizePropertiesKey) && propertiesThatChanged.ContainsKey(WorldGenerator.Instance.ClusterSizePropertiesKey) && propertiesThatChanged.ContainsKey(WorldGenerator.Instance.WorldTypePropertiesKey))
            {
                int seed = (int) propertiesThatChanged[WorldGenerator.Instance.SeedPropertiesKey];
                seedInputField.text = seed.ToString();
                WorldSize worldSize = (WorldSize) propertiesThatChanged[WorldGenerator.Instance.WorldSizePropertiesKey];
                switch (worldSize)
                {
                    case WorldSize.Tiny:
                    {
                        worldSizeDropdown.value = 0;
                        break;
                    }
                    case WorldSize.Small:
                    {
                        worldSizeDropdown.value = 1;
                        break;
                    }
                    case WorldSize.Medium:
                    {
                        worldSizeDropdown.value = 2;
                        break;
                    }
                    case WorldSize.Large:
                    {
                        worldSizeDropdown.value = 3;
                        break;
                    }
                }
                ClusterSize clusterSize = (ClusterSize) propertiesThatChanged[WorldGenerator.Instance.ClusterSizePropertiesKey];
                switch (clusterSize)
                {
                    case ClusterSize.Small:
                    {
                        clusterSizeDropdown.value = 0;
                        break;
                    }
                    case ClusterSize.Medium:
                    {
                        clusterSizeDropdown.value = 1;
                        break;
                    }
                    case ClusterSize.Large:
                    {
                        clusterSizeDropdown.value = 2;
                        break;
                    }
                }
                WorldType worldType = (WorldType) propertiesThatChanged[WorldGenerator.Instance.WorldTypePropertiesKey];
                switch (worldType)
                {
                    case WorldType.Flat:
                    {
                        worldTypeDropdown.value = 0;
                        break;
                    }
                    case WorldType.Standard:
                    {
                        worldTypeDropdown.value = 1;
                        break;
                    }
                    case WorldType.Mountain:
                    {
                        worldTypeDropdown.value = 2;
                        break;
                    }
                }
                WorldGenerator.Instance.CreateWorld();
            }
        }

        #endregion
    }
}