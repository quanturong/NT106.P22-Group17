

#pragma warning disable 1587
#pragma warning restore 1587


namespace Photon.Pun
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using ExitGames.Client.Photon;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Photon.Realtime;
    using SupportClassPun = ExitGames.Client.Photon.SupportClass;
    public class PunRPC : Attribute
    {
    }
    public class MonoBehaviourPun : MonoBehaviour
    {
        private PhotonView pvCache;
        public PhotonView photonView
        {
            get
            {
                #if UNITY_EDITOR
                if (!Application.isPlaying || this.pvCache == null)
                {
                    this.pvCache = PhotonView.Get(this);
                }
                #else
                if (this.pvCache == null)
                {
                    this.pvCache = PhotonView.Get(this);
                }
                #endif
                return this.pvCache;
            }
        }
    }
    public class MonoBehaviourPunCallbacks : MonoBehaviourPun, IConnectionCallbacks , IMatchmakingCallbacks , IInRoomCallbacks, ILobbyCallbacks, IWebRpcCallback, IErrorInfoCallback
    {
        public virtual void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public virtual void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }
        public virtual void OnConnected()
        {
        }
        public virtual void OnLeftRoom()
        {
        }
        public virtual void OnMasterClientSwitched(Player newMasterClient)
        {
        }
        public virtual void OnCreateRoomFailed(short returnCode, string message)
        {
        }
        public virtual void OnJoinRoomFailed(short returnCode, string message)
        {
        }
        public virtual void OnCreatedRoom()
        {
        }
        public virtual void OnJoinedLobby()
        {
        }
        public virtual void OnLeftLobby()
        {
        }
        public virtual void OnDisconnected(DisconnectCause cause)
        {
        }
        public virtual void OnRegionListReceived(RegionHandler regionHandler)
        {
        }
        public virtual void OnRoomListUpdate(List<RoomInfo> roomList)
        {
        }
        public virtual void OnJoinedRoom()
        {
        }
        public virtual void OnPlayerEnteredRoom(Player newPlayer)
        {
        }
        public virtual void OnPlayerLeftRoom(Player otherPlayer)
        {
        }
        public virtual void OnJoinRandomFailed(short returnCode, string message)
        {
        }
        public virtual void OnConnectedToMaster()
        {
        }
        public virtual void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
        }
        public virtual void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
        }
        public virtual void OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }
        public virtual void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
        }
        public virtual void OnCustomAuthenticationFailed (string debugMessage)
        {
        }
        public virtual void OnWebRpcResponse(OperationResponse response)
        {
        }
        public virtual void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
        }
        public virtual void OnErrorInfo(ErrorInfo errorInfo)
        {
        }
    }
    public struct PhotonMessageInfo
    {
        private readonly int timeInt;
        public readonly Player Sender;
        public readonly PhotonView photonView;

        public PhotonMessageInfo(Player player, int timestamp, PhotonView view)
        {
            this.Sender = player;
            this.timeInt = timestamp;
            this.photonView = view;
        }

        [Obsolete("Use SentServerTime instead.")]
        public double timestamp
        {
            get
            {
                uint u = (uint) this.timeInt;
                double t = u;
                return t / 1000.0d;
            }
        }

        public double SentServerTime
        {
            get
            {
                uint u = (uint)this.timeInt;
                double t = u;
                return t / 1000.0d;
            }
        }

        public int SentServerTimestamp
        {
            get { return this.timeInt; }
        }

        public override string ToString()
        {
            return string.Format("[PhotonMessageInfo: Sender='{1}' Senttime={0}]", this.SentServerTime, this.Sender);
        }
    }
    internal class PunEvent
    {
        public const byte RPC = 200;
        public const byte SendSerialize = 201;
        public const byte Instantiation = 202;
        public const byte CloseConnection = 203;
        public const byte Destroy = 204;
        public const byte RemoveCachedRPCs = 205;
        public const byte SendSerializeReliable = 206; // TS: added this but it's not really needed anymore
        public const byte DestroyPlayer = 207; // TS: added to make others remove all GOs of a player
        public const byte OwnershipRequest = 209;
        public const byte OwnershipTransfer = 210;
        public const byte VacantViewIds = 211;
        public const byte OwnershipUpdate = 212;
    }
    public class PhotonStream
    {
        private List<object> writeData;
        private object[] readData;
        private int currentItem; //Used to track the next item to receive.
        public bool IsWriting { get; private set; }
        public bool IsReading
        {
            get { return !this.IsWriting; }
        }
        public int Count
        {
            get { return this.IsWriting ? this.writeData.Count : this.readData.Length; }
        }
        public PhotonStream(bool write, object[] incomingData)
        {
            this.IsWriting = write;

            if (!write && incomingData != null)
            {
                this.readData = incomingData;
            }
        }

        public void SetReadStream(object[] incomingData, int pos = 0)
        {
            this.readData = incomingData;
            this.currentItem = pos;
            this.IsWriting = false;
        }

        internal void SetWriteStream(List<object> newWriteData, int pos = 0)
        {
            if (pos != newWriteData.Count)
            {
                throw new Exception("SetWriteStream failed, because count does not match position value. pos: "+ pos + " newWriteData.Count:" + newWriteData.Count);
            }
            this.writeData = newWriteData;
            this.currentItem = pos;
            this.IsWriting = true;
        }

        internal List<object> GetWriteStream()
        {
            return this.writeData;
        }


        [Obsolete("Either SET the writeData with an empty List or use Clear().")]
        internal void ResetWriteStream()
        {
            this.writeData.Clear();
        }
        public object ReceiveNext()
        {
            if (this.IsWriting)
            {
                Debug.LogError("Error: you cannot read this stream that you are writing!");
                return null;
            }

            object obj = this.readData[this.currentItem];
            this.currentItem++;
            return obj;
        }
        public object PeekNext()
        {
            if (this.IsWriting)
            {
                Debug.LogError("Error: you cannot read this stream that you are writing!");
                return null;
            }

            object obj = this.readData[this.currentItem];
            return obj;
        }
        public void SendNext(object obj)
        {
            if (!this.IsWriting)
            {
                Debug.LogError("Error: you cannot write/send to this stream that you are reading!");
                return;
            }

            this.writeData.Add(obj);
        }

        [Obsolete("writeData is a list now. Use and re-use it directly.")]
        public bool CopyToListAndClear(List<object> target)
        {
            if (!this.IsWriting) return false;

            target.AddRange(this.writeData);
            this.writeData.Clear();

            return true;
        }
        public object[] ToArray()
        {
            return this.IsWriting ? this.writeData.ToArray() : this.readData;
        }
        public void Serialize(ref bool myBool)
        {
            if (this.IsWriting)
            {
                this.writeData.Add(myBool);
            }
            else
            {
                if (this.readData.Length > this.currentItem)
                {
                    myBool = (bool) this.readData[this.currentItem];
                    this.currentItem++;
                }
            }
        }
        public void Serialize(ref int myInt)
        {
            if (this.IsWriting)
            {
                this.writeData.Add(myInt);
            }
            else
            {
                if (this.readData.Length > this.currentItem)
                {
                    myInt = (int) this.readData[this.currentItem];
                    this.currentItem++;
                }
            }
        }
        public void Serialize(ref string value)
        {
            if (this.IsWriting)
            {
                this.writeData.Add(value);
            }
            else
            {
                if (this.readData.Length > this.currentItem)
                {
                    value = (string) this.readData[this.currentItem];
                    this.currentItem++;
                }
            }
        }
        public void Serialize(ref char value)
        {
            if (this.IsWriting)
            {
                this.writeData.Add((short)value);
            }
            else
            {
                if (this.readData.Length > this.currentItem)
                {
                    value = (char)((short)this.readData[this.currentItem]);
                    this.currentItem++;
                }
            }
        }
        public void Serialize(ref byte value)
        {
            if (this.IsWriting)
            {
                this.writeData.Add(value);
            }
            else
            {
                if (this.readData.Length > this.currentItem)
                {
                    value = (byte)this.readData[this.currentItem];
                    this.currentItem++;
                }
            }
        }
        public void Serialize(ref short value)
        {
            if (this.IsWriting)
            {
                this.writeData.Add(value);
            }
            else
            {
                if (this.readData.Length > this.currentItem)
                {
                    value = (short) this.readData[this.currentItem];
                    this.currentItem++;
                }
            }
        }
        public void Serialize(ref float obj)
        {
            if (this.IsWriting)
            {
                this.writeData.Add(obj);
            }
            else
            {
                if (this.readData.Length > this.currentItem)
                {
                    obj = (float) this.readData[this.currentItem];
                    this.currentItem++;
                }
            }
        }
        public void Serialize(ref Player obj)
        {
            if (this.IsWriting)
            {
                this.writeData.Add(obj);
            }
            else
            {
                if (this.readData.Length > this.currentItem)
                {
                    obj = (Player) this.readData[this.currentItem];
                    this.currentItem++;
                }
            }
        }
        public void Serialize(ref Vector3 obj)
        {
            if (this.IsWriting)
            {
                this.writeData.Add(obj);
            }
            else
            {
                if (this.readData.Length > this.currentItem)
                {
                    obj = (Vector3) this.readData[this.currentItem];
                    this.currentItem++;
                }
            }
        }
        public void Serialize(ref Vector2 obj)
        {
            if (this.IsWriting)
            {
                this.writeData.Add(obj);
            }
            else
            {
                if (this.readData.Length > this.currentItem)
                {
                    obj = (Vector2) this.readData[this.currentItem];
                    this.currentItem++;
                }
            }
        }
        public void Serialize(ref Quaternion obj)
        {
            if (this.IsWriting)
            {
                this.writeData.Add(obj);
            }
            else
            {
                if (this.readData.Length > this.currentItem)
                {
                    obj = (Quaternion) this.readData[this.currentItem];
                    this.currentItem++;
                }
            }
        }
    }


    public class SceneManagerHelper
    {
        public static string ActiveSceneName
        {
            get
            {
                Scene s = SceneManager.GetActiveScene();
                return s.name;
            }
        }

        public static int ActiveSceneBuildIndex
        {
            get { return SceneManager.GetActiveScene().buildIndex; }
        }


        #if UNITY_EDITOR
        public static string EditorActiveSceneName
        {
            get { return SceneManager.GetActiveScene().name; }
        }
        #endif
    }
    public class DefaultPool : IPunPrefabPool
    {
        public readonly Dictionary<string, GameObject> ResourceCache = new Dictionary<string, GameObject>();
        public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
        {
            GameObject res = null;
            bool cached = this.ResourceCache.TryGetValue(prefabId, out res);
            if (!cached)
            {
                res = Resources.Load<GameObject>(prefabId);
                if (res == null)
                {
                    Debug.LogError("DefaultPool failed to load \"" + prefabId + "\". Make sure it's in a \"Resources\" folder. Or use a custom IPunPrefabPool.");
                }
                else
                {
                    this.ResourceCache.Add(prefabId, res);
                }
            }

            bool wasActive = res.activeSelf;
            if (wasActive) res.SetActive(false);

            GameObject instance =GameObject.Instantiate(res, position, rotation) as GameObject;

            if (wasActive) res.SetActive(true);
            return instance;
        }
        public void Destroy(GameObject gameObject)
        {
            GameObject.Destroy(gameObject);
        }
    }
    public static class PunExtensions
    {
        public static Dictionary<MethodInfo, ParameterInfo[]> ParametersOfMethods = new Dictionary<MethodInfo, ParameterInfo[]>();

        public static ParameterInfo[] GetCachedParemeters(this MethodInfo mo)
        {
            ParameterInfo[] result;
            bool cached = ParametersOfMethods.TryGetValue(mo, out result);

            if (!cached)
            {
                result = mo.GetParameters();
                ParametersOfMethods[mo] = result;
            }

            return result;
        }

        public static PhotonView[] GetPhotonViewsInChildren(this UnityEngine.GameObject go)
        {
            return go.GetComponentsInChildren<PhotonView>(true) as PhotonView[];
        }

        public static PhotonView GetPhotonView(this UnityEngine.GameObject go)
        {
            return go.GetComponent<PhotonView>() as PhotonView;
        }
        public static bool AlmostEquals(this Vector3 target, Vector3 second, float sqrMagnitudePrecision)
        {
            return (target - second).sqrMagnitude < sqrMagnitudePrecision; // TODO: inline vector methods to optimize?
        }
        public static bool AlmostEquals(this Vector2 target, Vector2 second, float sqrMagnitudePrecision)
        {
            return (target - second).sqrMagnitude < sqrMagnitudePrecision; // TODO: inline vector methods to optimize?
        }
        public static bool AlmostEquals(this Quaternion target, Quaternion second, float maxAngle)
        {
            return Quaternion.Angle(target, second) < maxAngle;
        }
        public static bool AlmostEquals(this float target, float second, float floatDiff)
        {
            return Mathf.Abs(target - second) < floatDiff;
        }


        public static bool CheckIsAssignableFrom(this Type to, Type from)
        {
            #if !NETFX_CORE
            return to.IsAssignableFrom(from);
            #else
            return to.GetTypeInfo().IsAssignableFrom(from.GetTypeInfo());
            #endif
        }

        public static bool CheckIsInterface(this Type to)
        {
            #if !NETFX_CORE
            return to.IsInterface;
            #else
            return to.GetTypeInfo().IsInterface;
            #endif
        }
    }
}