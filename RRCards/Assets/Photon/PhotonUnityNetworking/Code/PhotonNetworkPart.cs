

namespace Photon.Pun
{
    using System;
    using System.Linq;
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    using ExitGames.Client.Photon;
    using Photon.Realtime;

    using Hashtable = ExitGames.Client.Photon.Hashtable;
    using SupportClassPun = ExitGames.Client.Photon.SupportClass;

    public static partial class PhotonNetwork
    {
        private static HashSet<byte> allowedReceivingGroups = new HashSet<byte>();

        private static HashSet<byte> blockedSendingGroups = new HashSet<byte>();

        private static HashSet<PhotonView> reusablePVHashset = new HashSet<PhotonView>();
        private static NonAllocDictionary<int, PhotonView> photonViewList = new NonAllocDictionary<int, PhotonView>();
        [System.Obsolete("Use PhotonViewCollection instead for an iterable collection of current photonViews.")]
        public static PhotonView[] PhotonViews
        {
            get
            {
                var views = new PhotonView[photonViewList.Count];
                int idx = 0;
                foreach (var v in photonViewList.Values)
                {
                    views[idx] = v;
                    idx++;
                }
                return views;
            }
        }
        public static NonAllocDictionary<int, PhotonView>.ValueIterator PhotonViewCollection
        {
            get
            {
                return photonViewList.Values;
            }
        }

        public static int ViewCount
        {
            get { return photonViewList.Count; }
        }
        private static event Action<PhotonView, Player> OnOwnershipRequestEv;
        private static event Action<PhotonView, Player> OnOwnershipTransferedEv;
        private static event Action<PhotonView, Player> OnOwnershipTransferFailedEv;
        public static void AddCallbackTarget(object target)
        {
            if (target is PhotonView)
            {
                return;
            }

            IPunOwnershipCallbacks punOwnershipCallback = target as IPunOwnershipCallbacks;
            if (punOwnershipCallback != null)
            {
                OnOwnershipRequestEv += punOwnershipCallback.OnOwnershipRequest;
                OnOwnershipTransferedEv += punOwnershipCallback.OnOwnershipTransfered;
                OnOwnershipTransferFailedEv += punOwnershipCallback.OnOwnershipTransferFailed;
            }

            NetworkingClient.AddCallbackTarget(target);
        }
        public static void RemoveCallbackTarget(object target)
        {
            if (target is PhotonView || NetworkingClient == null)
            {
                return;
            }

            IPunOwnershipCallbacks punOwnershipCallback = target as IPunOwnershipCallbacks;
            if (punOwnershipCallback != null)
            {
                OnOwnershipRequestEv -= punOwnershipCallback.OnOwnershipRequest;
                OnOwnershipTransferedEv -= punOwnershipCallback.OnOwnershipTransfered;
                OnOwnershipTransferFailedEv -= punOwnershipCallback.OnOwnershipTransferFailed;
            }

            NetworkingClient.RemoveCallbackTarget(target);
        }

        internal static string CallbacksToString()
        {
            var x = NetworkingClient.ConnectionCallbackTargets.Select(m => m.ToString()).ToArray();
            return string.Join(", ", x);
        }

        internal static byte currentLevelPrefix = 0;
        internal static bool loadingLevelAndPausedNetwork = false;
        internal const string CurrentSceneProperty = "curScn";
        internal const string CurrentScenePropertyLoadAsync = "curScnLa";
        public static IPunPrefabPool PrefabPool
        {
            get
            {
                return prefabPool;
            }
            set
            {
                if (value == null)
                {
                    Debug.LogWarning("PhotonNetwork.PrefabPool cannot be set to null. It will default back to using the 'DefaultPool' Pool");
                    prefabPool = new DefaultPool();
                }
                else
                {
                    prefabPool = value;
                }
            }
        }

        private static IPunPrefabPool prefabPool;
        public static bool UseRpcMonoBehaviourCache;

        private static readonly Dictionary<Type, List<MethodInfo>> monoRPCMethodsCache = new Dictionary<Type, List<MethodInfo>>();

        private static Dictionary<string, int> rpcShortcuts;  // lookup "table" for the index (shortcut) of an RPC name
        public static bool RunRpcCoroutines = true;
        private static AsyncOperation _AsyncLevelLoadingOperation;

        private static float _levelLoadingProgress = 0f;
        public static float LevelLoadingProgress
        {
            get
            {
                if (_AsyncLevelLoadingOperation != null)
                {
                    _levelLoadingProgress = _AsyncLevelLoadingOperation.progress;
                }
                else if (_levelLoadingProgress > 0f)
                {
                    _levelLoadingProgress = 1f;
                }

                return _levelLoadingProgress;
            }
        }
        private static void LeftRoomCleanup()
        {
            if (_AsyncLevelLoadingOperation != null)
            {
                _AsyncLevelLoadingOperation.allowSceneActivation = false;
                _AsyncLevelLoadingOperation = null;
            }
            
            rpcEvent.Clear();   // none of the last RPC parameters are needed anymore

            bool wasInRoom = NetworkingClient.CurrentRoom != null;
            bool autoCleanupSettingOfRoom = wasInRoom && CurrentRoom.AutoCleanUp;

            allowedReceivingGroups = new HashSet<byte>();
            blockedSendingGroups = new HashSet<byte>();
            if (autoCleanupSettingOfRoom || offlineModeRoom != null)
            {
                LocalCleanupAnythingInstantiated(true);
            }
        }
        internal static void LocalCleanupAnythingInstantiated(bool destroyInstantiatedGameObjects)
        {
            if (destroyInstantiatedGameObjects)
            {
                HashSet<GameObject> instantiatedGos = new HashSet<GameObject>();
                foreach (PhotonView view in photonViewList.Values)
                {
                    if (view.isRuntimeInstantiated)
                    {
                        instantiatedGos.Add(view.gameObject); // HashSet keeps each object only once
                    }
                    else
                    {
                        view.ResetPhotonView(true);
                    }
                }

                foreach (GameObject go in instantiatedGos)
                {
                    RemoveInstantiatedGO(go, true);
                }
            }
            PhotonNetwork.lastUsedViewSubId = 0;
            PhotonNetwork.lastUsedViewSubIdStatic = 0;
        }
        private static void ResetPhotonViewsOnSerialize()
        {
            foreach (PhotonView photonView in photonViewList.Values)
            {
                photonView.lastOnSerializeDataSent = null;
            }
        }
#pragma warning disable 0414
        private static readonly Type typePunRPC = typeof(PunRPC);
        private static readonly Type typePhotonMessageInfo = typeof(PhotonMessageInfo);
        private static readonly object keyByteZero = (byte)0;
        private static readonly object keyByteOne = (byte)1;
        private static readonly object keyByteTwo = (byte)2;
        private static readonly object keyByteThree = (byte)3;
        private static readonly object keyByteFour = (byte)4;
        private static readonly object keyByteFive = (byte)5;
        private static readonly object keyByteSix = (byte)6;
        private static readonly object keyByteSeven = (byte)7;
        private static readonly object keyByteEight = (byte)8;
        private static readonly object[] emptyObjectArray = new object[0];
        private static readonly Type[] emptyTypeArray = new Type[0];
#pragma warning restore 0414
        internal static void ExecuteRpc(Hashtable rpcData, Player sender)
        {
            if (rpcData == null || !rpcData.ContainsKey(keyByteZero))
            {
                Debug.LogError("Malformed RPC; this should never occur. Content: " + SupportClassPun.DictionaryToString(rpcData));
                return;
            }
            int netViewID = (int)rpcData[keyByteZero]; // LIMITS PHOTONVIEWS&PLAYERS
            int otherSidePrefix = 0;    // by default, the prefix is 0 (and this is not being sent)
            if (rpcData.ContainsKey(keyByteOne))
            {
                otherSidePrefix = (short)rpcData[keyByteOne];
            }


            string inMethodName;
            if (rpcData.ContainsKey(keyByteFive))
            {
                int rpcIndex = (byte)rpcData[keyByteFive];  // LIMITS RPC COUNT
                if (rpcIndex > PhotonNetwork.PhotonServerSettings.RpcList.Count - 1)
                {
                    Debug.LogError("Could not find RPC with index: " + rpcIndex + ". Going to ignore! Check PhotonServerSettings.RpcList");
                    return;
                }
                else
                {
                    inMethodName = PhotonNetwork.PhotonServerSettings.RpcList[rpcIndex];
                }
            }
            else
            {
                inMethodName = (string)rpcData[keyByteThree];
            }

            object[] arguments = null;
            if (rpcData.ContainsKey(keyByteFour))
            {
                arguments = (object[])rpcData[keyByteFour];
            }

            PhotonView photonNetview = GetPhotonView(netViewID);
            if (photonNetview == null)
            {
                int viewOwnerId = netViewID / PhotonNetwork.MAX_VIEW_IDS;
                bool owningPv = (viewOwnerId == NetworkingClient.LocalPlayer.ActorNumber);
                bool ownerSent = sender != null && viewOwnerId == sender.ActorNumber;

                if (owningPv)
                {
                    Debug.LogWarning("Received RPC \"" + inMethodName + "\" for viewID " + netViewID + " but this PhotonView does not exist! View was/is ours." + (ownerSent ? " Owner called." : " Remote called.") + " By: " + sender);
                }
                else
                {
                    Debug.LogWarning("Received RPC \"" + inMethodName + "\" for viewID " + netViewID + " but this PhotonView does not exist! Was remote PV." + (ownerSent ? " Owner called." : " Remote called.") + " By: " + sender + " Maybe GO was destroyed but RPC not cleaned up.");
                }
                return;
            }

            if (photonNetview.Prefix != otherSidePrefix)
            {
                Debug.LogError("Received RPC \"" + inMethodName + "\" on viewID " + netViewID + " with a prefix of " + otherSidePrefix + ", our prefix is " + photonNetview.Prefix + ". The RPC has been ignored.");
                return;
            }
            if (string.IsNullOrEmpty(inMethodName))
            {
                Debug.LogError("Malformed RPC; this should never occur. Content: " + SupportClassPun.DictionaryToString(rpcData));
                return;
            }

            if (PhotonNetwork.LogLevel >= PunLogLevel.Full)
            {
                Debug.Log("Received RPC: " + inMethodName);
            }
            if (photonNetview.Group != 0 && !allowedReceivingGroups.Contains(photonNetview.Group))
            {
                return; // Ignore group
            }

            Type[] argumentsTypes = null;
            if (arguments != null && arguments.Length > 0)
            {
                argumentsTypes = new Type[arguments.Length];
                int i = 0;
                for (int index = 0; index < arguments.Length; index++)
                {
                    object objX = arguments[index];
                    if (objX == null)
                    {
                        argumentsTypes[i] = null;
                    }
                    else
                    {
                        argumentsTypes[i] = objX.GetType();
                    }

                    i++;
                }
            }


            int receivers = 0;
            int foundMethods = 0;
            if (!PhotonNetwork.UseRpcMonoBehaviourCache || photonNetview.RpcMonoBehaviours == null || photonNetview.RpcMonoBehaviours.Length == 0)
            {
                photonNetview.RefreshRpcMonoBehaviourCache();
            }

            for (int componentsIndex = 0; componentsIndex < photonNetview.RpcMonoBehaviours.Length; componentsIndex++)
            {
                MonoBehaviour monob = photonNetview.RpcMonoBehaviours[componentsIndex];
                if (monob == null)
                {
                    Debug.LogError("ERROR You have missing MonoBehaviours on your gameobjects!");
                    continue;
                }

                Type type = monob.GetType();
                List<MethodInfo> cachedRPCMethods = null;
                bool methodsOfTypeInCache = monoRPCMethodsCache.TryGetValue(type, out cachedRPCMethods);

                if (!methodsOfTypeInCache)
                {
                    List<MethodInfo> entries = SupportClassPun.GetMethods(type, typePunRPC);

                    monoRPCMethodsCache[type] = entries;
                    cachedRPCMethods = entries;
                }

                if (cachedRPCMethods == null)
                {
                    continue;
                }
                for (int index = 0; index < cachedRPCMethods.Count; index++)
                {
                    MethodInfo mInfo = cachedRPCMethods[index];
                    if (!mInfo.Name.Equals(inMethodName))
                    {
                        continue;
                    }

                    ParameterInfo[] parameters = mInfo.GetCachedParemeters();
                    foundMethods++;
                    if (arguments == null)
                    {
                        if (parameters.Length == 0)
                        {
                            receivers++;
                            object o = mInfo.Invoke((object)monob, null);
                            if (PhotonNetwork.RunRpcCoroutines)
                            {
                                IEnumerator ie = null;//o as IEnumerator;
                                if ((ie = o as IEnumerator) != null)
                                {
                                    PhotonHandler.Instance.StartCoroutine(ie);
                                }
                            }
                        }
                        else if (parameters.Length == 1 && parameters[0].ParameterType == typeof(PhotonMessageInfo))
                        {
                            int sendTime = (int)rpcData[keyByteTwo];

                            receivers++;
                            object o = mInfo.Invoke((object)monob, new object[] { new PhotonMessageInfo(sender, sendTime, photonNetview) });
                            if (PhotonNetwork.RunRpcCoroutines)
                            {
                                IEnumerator ie = null;//o as IEnumerator;
                                if ((ie = o as IEnumerator) != null)
                                {
                                    PhotonHandler.Instance.StartCoroutine(ie);
                                }
                            }
                        }
                        continue;
                    }
                    if (parameters.Length == arguments.Length)
                    {
                        if (CheckTypeMatch(parameters, argumentsTypes))
                        {
                            receivers++;
                            object o = mInfo.Invoke((object)monob, arguments);
                            if (PhotonNetwork.RunRpcCoroutines)
                            {
                                IEnumerator ie = null;//o as IEnumerator;
                                if ((ie = o as IEnumerator) != null)
                                {
                                    PhotonHandler.Instance.StartCoroutine(ie);
                                }
                            }
                        }
                        continue;
                    }

                    if (parameters.Length == arguments.Length + 1)
                    {
                        if (parameters[parameters.Length - 1].ParameterType == typeof(PhotonMessageInfo) && CheckTypeMatch(parameters, argumentsTypes))
                        {
                            int sendTime = (int)rpcData[keyByteTwo];
                            object[] argumentsWithInfo = new object[arguments.Length + 1];
                            arguments.CopyTo(argumentsWithInfo, 0);
                            argumentsWithInfo[argumentsWithInfo.Length - 1] = new PhotonMessageInfo(sender, sendTime, photonNetview);

                            receivers++;
                            object o = mInfo.Invoke((object)monob, argumentsWithInfo);
                            if (PhotonNetwork.RunRpcCoroutines)
                            {
                                IEnumerator ie = null;//o as IEnumerator;
                                if ((ie = o as IEnumerator) != null)
                                {
                                    PhotonHandler.Instance.StartCoroutine(ie);
                                }
                            }
                        }
                        continue;
                    }

                    if (parameters.Length == 1 && parameters[0].ParameterType.IsArray)
                    {
                        receivers++;
                        object o = mInfo.Invoke((object)monob, new object[] { arguments });
                        if (PhotonNetwork.RunRpcCoroutines)
                        {
                            IEnumerator ie = null;//o as IEnumerator;
                            if ((ie = o as IEnumerator) != null)
                            {
                                PhotonHandler.Instance.StartCoroutine(ie);
                            }
                        }
                        continue;
                    }
                }
            }
            if (receivers != 1)
            {
                string argsString = string.Empty;
                int argsLength = 0;
                if (argumentsTypes != null)
                {
                    argsLength = argumentsTypes.Length;
                    for (int index = 0; index < argumentsTypes.Length; index++)
                    {
                        Type ty = argumentsTypes[index];
                        if (argsString != string.Empty)
                        {
                            argsString += ", ";
                        }

                        if (ty == null)
                        {
                            argsString += "null";
                        }
                        else
                        {
                            argsString += ty.Name;
                        }
                    }
                }

                GameObject context = photonNetview != null ? photonNetview.gameObject : null;
                if (receivers == 0)
                {
                    if (foundMethods == 0)
                    {
                        Debug.LogErrorFormat(context, "RPC method '{0}({2})' not found on object with PhotonView {1}. Implement as non-static. Apply [PunRPC]. Components on children are not found. " +
                            "Return type must be void or IEnumerator (if you enable RunRpcCoroutines). RPCs are a one-way message.", inMethodName, netViewID, argsString);
                    }
                    else
                    {
                        Debug.LogErrorFormat(context, "RPC method '{0}' found on object with PhotonView {1} but has wrong parameters. Implement as '{0}({2})'. PhotonMessageInfo is optional as final parameter." +
                            "Return type must be void or IEnumerator (if you enable RunRpcCoroutines).", inMethodName, netViewID, argsString);
                    }
                }
                else
                {
                    Debug.LogErrorFormat(context, "RPC method '{0}({2})' found {3}x on object with PhotonView {1}. Only one component should implement it." +
                            "Return type must be void or IEnumerator (if you enable RunRpcCoroutines).", inMethodName, netViewID, argsString, foundMethods);
                }
            }
        }
        private static bool CheckTypeMatch(ParameterInfo[] methodParameters, Type[] callParameterTypes)
        {
            if (methodParameters.Length < callParameterTypes.Length)
            {
                return false;
            }

            for (int index = 0; index < callParameterTypes.Length; index++)
            {
#if NETFX_CORE
                TypeInfo methodParamTI = methodParameters[index].ParameterType.GetTypeInfo();
                TypeInfo callParamTI = callParameterTypes[index].GetTypeInfo();

                if (callParameterTypes[index] != null && !methodParamTI.IsAssignableFrom(callParamTI) && !(callParamTI.IsEnum && System.Enum.GetUnderlyingType(methodParamTI.AsType()).GetTypeInfo().IsAssignableFrom(callParamTI)))
                {
                    return false;
                }
#else
                Type type = methodParameters[index].ParameterType;
                if (callParameterTypes[index] != null && !type.IsAssignableFrom(callParameterTypes[index]) && !(type.IsEnum && System.Enum.GetUnderlyingType(type).IsAssignableFrom(callParameterTypes[index])))
                {
                    return false;
                }
#endif
            }

            return true;
        }
        public static void DestroyPlayerObjects(int playerId, bool localOnly)
        {
            if (playerId <= 0)
            {
                Debug.LogError("Failed to Destroy objects of playerId: " + playerId);
                return;
            }

            if (!localOnly)
            {
                OpRemoveFromServerInstantiationsOfPlayer(playerId);
                OpCleanActorRpcBuffer(playerId);
                SendDestroyOfPlayer(playerId);
            }
            HashSet<GameObject> playersGameObjects = new HashSet<GameObject>();
            foreach (PhotonView view in photonViewList.Values)
            {
                if (view == null)
                {
                    Debug.LogError("Null view");
                    continue;
                }
                if (view.CreatorActorNr == playerId)
                {
                    playersGameObjects.Add(view.gameObject);
                    continue;
                }

                if (view.OwnerActorNr == playerId)
                {
                    var previousOwner = view.Owner;
                    view.OwnerActorNr = view.CreatorActorNr;
                    view.ControllerActorNr = view.CreatorActorNr;
                    if (PhotonNetwork.OnOwnershipTransferedEv != null)
                    {
                        PhotonNetwork.OnOwnershipTransferedEv(view, previousOwner);
                    }
                }
            }
            foreach (GameObject gameObject in playersGameObjects)
            {
                RemoveInstantiatedGO(gameObject, true);
            }
        }

        public static void DestroyAll(bool localOnly)
        {
            if (!localOnly)
            {
                OpRemoveCompleteCache();
                SendDestroyOfAll();
            }

            LocalCleanupAnythingInstantiated(true);
        }

        internal static List<PhotonView> foundPVs = new List<PhotonView>();
        internal static void RemoveInstantiatedGO(GameObject go, bool localOnly)
        {
            if (ConnectionHandler.AppQuits)
                return;

            if (go == null)
            {
                Debug.LogError("Failed to 'network-remove' GameObject because it's null.");
                return;
            }
            go.GetComponentsInChildren<PhotonView>(true, foundPVs);
            if (foundPVs.Count <= 0)
            {
                Debug.LogError("Failed to 'network-remove' GameObject because has no PhotonView components: " + go);
                return;
            }

            PhotonView viewZero = foundPVs[0];
            if (!localOnly)
            {
                if (!viewZero.IsMine)
                {
                    Debug.LogError("Failed to 'network-remove' GameObject. Client is neither owner nor MasterClient taking over for owner who left: " + viewZero);
                    foundPVs.Clear();   // as foundPVs is re-used, clean it to avoid lingering references
                    return;
                }
            }
            if (!localOnly)
            {
                ServerCleanInstantiateAndDestroy(viewZero); // server cleaning
            }

            int creatorActorNr = viewZero.CreatorActorNr;
            for (int j = foundPVs.Count - 1; j >= 0; j--)
            {
                PhotonView view = foundPVs[j];
                if (view == null)
                {
                    continue;
                }
                if (j != 0)
                {
                    if (view.CreatorActorNr != creatorActorNr)
                    {
                        view.transform.SetParent(null, true);
                        continue;
                    }
                }
                view.OnPreNetDestroy(viewZero);
                if (view.InstantiationId >= 1)
                {
                    LocalCleanPhotonView(view);
                }
                if (!localOnly)
                {
                    OpCleanRpcBuffer(view);
                }
            }

            if (PhotonNetwork.LogLevel >= PunLogLevel.Full)
            {
                Debug.Log("Network destroy Instantiated GO: " + go.name);
            }
            
            foundPVs.Clear();           // as foundPVs is re-used, clean it to avoid lingering references

            go.SetActive(false);        // PUN 2 disables objects before the return to the pool
            prefabPool.Destroy(go);     // PUN 2 always uses a PrefabPool (even for the default implementation)
        }


        private static readonly ExitGames.Client.Photon.Hashtable removeFilter = new ExitGames.Client.Photon.Hashtable();
        private static readonly ExitGames.Client.Photon.Hashtable ServerCleanDestroyEvent = new ExitGames.Client.Photon.Hashtable();
        private static readonly RaiseEventOptions ServerCleanOptions = new RaiseEventOptions() { CachingOption = EventCaching.RemoveFromRoomCache };

        internal static RaiseEventOptions SendToAllOptions = new RaiseEventOptions() { Receivers = ReceiverGroup.All };
        internal static RaiseEventOptions SendToOthersOptions = new RaiseEventOptions() { Receivers = ReceiverGroup.Others };
        internal static RaiseEventOptions SendToSingleOptions = new RaiseEventOptions() { TargetActors = new int[1] };
        private static void ServerCleanInstantiateAndDestroy(PhotonView photonView)
        {
            int filterId;
            if (photonView.isRuntimeInstantiated)
            {
                filterId = photonView.InstantiationId; // actual, live InstantiationIds start with 1 and go up
                removeFilter[keyByteSeven] = filterId;
                ServerCleanOptions.CachingOption = EventCaching.RemoveFromRoomCache;
                PhotonNetwork.RaiseEventInternal(PunEvent.Instantiation, removeFilter, ServerCleanOptions, SendOptions.SendReliable);
            }
            else
            {
                filterId = photonView.ViewID;
            }
            ServerCleanDestroyEvent[keyByteZero] = filterId;
            ServerCleanOptions.CachingOption = photonView.isRuntimeInstantiated ? EventCaching.DoNotCache : EventCaching.AddToRoomCacheGlobal;   // if the view got loaded with the scene, cache EvDestroy for anyone (re)joining later

            PhotonNetwork.RaiseEventInternal(PunEvent.Destroy, ServerCleanDestroyEvent, ServerCleanOptions, SendOptions.SendReliable);
        }

        private static void SendDestroyOfPlayer(int actorNr)
        {
            ExitGames.Client.Photon.Hashtable evData = new ExitGames.Client.Photon.Hashtable();
            evData[keyByteZero] = actorNr;

            PhotonNetwork.RaiseEventInternal(PunEvent.DestroyPlayer, evData, null, SendOptions.SendReliable);
        }

        private static void SendDestroyOfAll()
        {
            ExitGames.Client.Photon.Hashtable evData = new ExitGames.Client.Photon.Hashtable();
            evData[keyByteZero] = -1;

            PhotonNetwork.RaiseEventInternal(PunEvent.DestroyPlayer, evData, null, SendOptions.SendReliable);
        }

        private static void OpRemoveFromServerInstantiationsOfPlayer(int actorNr)
        {
            RaiseEventOptions options = new RaiseEventOptions() { CachingOption = EventCaching.RemoveFromRoomCache, TargetActors = new int[] { actorNr } };
            PhotonNetwork.RaiseEventInternal(PunEvent.Instantiation, null, options, SendOptions.SendReliable);
        }

        internal static void RequestOwnership(int viewID, int fromOwner)
        {
            PhotonNetwork.RaiseEventInternal(PunEvent.OwnershipRequest, new int[] { viewID, fromOwner }, SendToAllOptions, SendOptions.SendReliable);
        }

        internal static void TransferOwnership(int viewID, int playerID)
        {
            PhotonNetwork.RaiseEventInternal(PunEvent.OwnershipTransfer, new int[] { viewID, playerID }, SendToAllOptions, SendOptions.SendReliable);
        }
        internal static void OwnershipUpdate(int[] viewOwnerPairs, int targetActor = -1)
        {
            RaiseEventOptions opts;
            if (targetActor == -1)
            {
                opts = SendToOthersOptions;
            }
            else
            {
                SendToSingleOptions.TargetActors[0] = targetActor;
                opts = SendToSingleOptions;
            }
            PhotonNetwork.RaiseEventInternal(PunEvent.OwnershipUpdate, viewOwnerPairs, opts, SendOptions.SendReliable);
        }

        public static bool LocalCleanPhotonView(PhotonView view)
        {
            view.removedFromLocalViewList = true;
            return photonViewList.Remove(view.ViewID);
        }

        public static PhotonView GetPhotonView(int viewID)
        {
            PhotonView result = null;
            photonViewList.TryGetValue(viewID, out result);

            return result;
        }

        public static void RegisterPhotonView(PhotonView netView)
        {
            if (!Application.isPlaying)
            {
                photonViewList = new NonAllocDictionary<int, PhotonView>();
                return;
            }

            if (netView.ViewID == 0)
            {
                Debug.Log("PhotonView register is ignored, because viewID is 0. No id assigned yet to: " + netView);
                return;
            }

            PhotonView listedView = null;
            bool isViewListed = photonViewList.TryGetValue(netView.ViewID, out listedView);
            if (isViewListed)
            {
                if (netView != listedView)
                {
                    Debug.LogError(string.Format("PhotonView ID duplicate found: {0}. New: {1} old: {2}. Maybe one wasn't destroyed on scene load?! Check for 'DontDestroyOnLoad'. Destroying old entry, adding new.", netView.ViewID, netView, listedView));
                }
                else
                {
                    return;
                }

                RemoveInstantiatedGO(listedView.gameObject, true);
            }
            photonViewList.Add(netView.ViewID, netView);
            netView.removedFromLocalViewList = false;

            if (PhotonNetwork.LogLevel >= PunLogLevel.Full)
            {
                Debug.Log("Registered PhotonView: " + netView.ViewID);
            }
        }
        public static void OpCleanActorRpcBuffer(int actorNumber)
        {
            RaiseEventOptions options = new RaiseEventOptions() { CachingOption = EventCaching.RemoveFromRoomCache, TargetActors = new int[] { actorNumber } };
            PhotonNetwork.RaiseEventInternal(PunEvent.RPC, null, options, SendOptions.SendReliable);
        }
        public static void OpRemoveCompleteCacheOfPlayer(int actorNumber)
        {
            RaiseEventOptions options = new RaiseEventOptions() { CachingOption = EventCaching.RemoveFromRoomCache, TargetActors = new int[] { actorNumber } };
            PhotonNetwork.RaiseEventInternal(0, null, options, SendOptions.SendReliable);
        }


        public static void OpRemoveCompleteCache()
        {
            RaiseEventOptions options = new RaiseEventOptions() { CachingOption = EventCaching.RemoveFromRoomCache, Receivers = ReceiverGroup.MasterClient };
            PhotonNetwork.RaiseEventInternal(0, null, options, SendOptions.SendReliable);
        }
        private static void RemoveCacheOfLeftPlayers()
        {
            ParameterDictionary opParameters = new ParameterDictionary(2);
            opParameters[ParameterCode.Code] = (byte)0;		// any event
            opParameters[ParameterCode.Cache] = (byte)EventCaching.RemoveFromRoomCacheForActorsLeft;    // option to clear the room cache of all events of players who left

            NetworkingClient.LoadBalancingPeer.SendOperation((byte)OperationCode.RaiseEvent, opParameters, SendOptions.SendReliable);   // TODO: Check if this is the best implementation possible
        }
        public static void CleanRpcBufferIfMine(PhotonView view)
        {
            if (view.OwnerActorNr != NetworkingClient.LocalPlayer.ActorNumber && !NetworkingClient.LocalPlayer.IsMasterClient)
            {
                Debug.LogError("Cannot remove cached RPCs on a PhotonView thats not ours! " + view.Owner + " scene: " + view.IsRoomView);
                return;
            }

            OpCleanRpcBuffer(view);
        }


        private static readonly Hashtable rpcFilterByViewId = new ExitGames.Client.Photon.Hashtable();
        private static readonly RaiseEventOptions OpCleanRpcBufferOptions = new RaiseEventOptions() { CachingOption = EventCaching.RemoveFromRoomCache };
        public static void OpCleanRpcBuffer(PhotonView view)
        {
            rpcFilterByViewId[keyByteZero] = view.ViewID;
            PhotonNetwork.RaiseEventInternal(PunEvent.RPC, rpcFilterByViewId, OpCleanRpcBufferOptions, SendOptions.SendReliable);
        }
        public static void RemoveRPCsInGroup(int group)
        {
            foreach (PhotonView view in photonViewList.Values)
            {
                if (view.Group == group)
                {
                    CleanRpcBufferIfMine(view);
                }
            }
        }
        public static bool RemoveBufferedRPCs(int viewId = 0, string methodName = null, int[] callersActorNumbers = null/*, params object[] parameters*/)
        {
            Hashtable filter = new Hashtable(2);
            if (viewId != 0)
            {
                filter[keyByteZero] = viewId;
            }
            if (!string.IsNullOrEmpty(methodName))
            {
                int shortcut;
                if (rpcShortcuts.TryGetValue(methodName, out shortcut))
                {
                    filter[keyByteFive] = (byte)shortcut; // LIMITS RPC COUNT
                }
                else
                {
                    filter[keyByteThree] = methodName;
                }
            }
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
            raiseEventOptions.CachingOption = EventCaching.RemoveFromRoomCache;
            if (callersActorNumbers != null)
            {
                raiseEventOptions.TargetActors = callersActorNumbers;
            }
            return RaiseEventInternal(PunEvent.RPC, filter, raiseEventOptions, SendOptions.SendReliable);
        }
        public static void SetLevelPrefix(byte prefix)
        {

            currentLevelPrefix = prefix;
        }

        static ExitGames.Client.Photon.Hashtable rpcEvent = new ExitGames.Client.Photon.Hashtable();
        static RaiseEventOptions RpcOptionsToAll = new RaiseEventOptions();


        internal static void RPC(PhotonView view, string methodName, RpcTarget target, Player player, bool encrypt, params object[] parameters)
        {
            if (blockedSendingGroups.Contains(view.Group))
            {
                return; // Block sending on this group
            }

            if (view.ViewID < 1)
            {
                Debug.LogError("Illegal view ID:" + view.ViewID + " method: " + methodName + " GO:" + view.gameObject.name);
            }

            if (PhotonNetwork.LogLevel >= PunLogLevel.Full)
            {
                Debug.Log("Sending RPC \"" + methodName + "\" to target: " + target + " or player:" + player + ".");
            }
            rpcEvent.Clear();

            rpcEvent[keyByteZero] = (int)view.ViewID; // LIMITS NETWORKVIEWS&PLAYERS
            if (view.Prefix > 0)
            {
                rpcEvent[keyByteOne] = (short)view.Prefix;
            }
            rpcEvent[keyByteTwo] = PhotonNetwork.ServerTimestamp;
            int shortcut = 0;
            if (rpcShortcuts.TryGetValue(methodName, out shortcut))
            {
                rpcEvent[keyByteFive] = (byte)shortcut; // LIMITS RPC COUNT
            }
            else
            {
                rpcEvent[keyByteThree] = methodName;
            }

            if (parameters != null && parameters.Length > 0)
            {
                rpcEvent[keyByteFour] = (object[])parameters;
            }

            SendOptions sendOptions = new SendOptions() { Reliability = true, Encrypt = encrypt };
            if (player != null)
            {
                if (NetworkingClient.LocalPlayer.ActorNumber == player.ActorNumber)
                {
                    ExecuteRpc(rpcEvent, player);
                }
                else
                {
                    RaiseEventOptions options = new RaiseEventOptions() { TargetActors = new int[] { player.ActorNumber } };
                    PhotonNetwork.RaiseEventInternal(PunEvent.RPC, rpcEvent, options, sendOptions);
                }

                return;
            }

            switch (target)
            {
                case RpcTarget.All:
                    RpcOptionsToAll.InterestGroup = (byte)view.Group;   // NOTE: Test-wise, this is static and re-used to avoid memory garbage
                    PhotonNetwork.RaiseEventInternal(PunEvent.RPC, rpcEvent, RpcOptionsToAll, sendOptions);
                    ExecuteRpc(rpcEvent, NetworkingClient.LocalPlayer);
                    break;
                case RpcTarget.Others:
                    {
                        RaiseEventOptions options = new RaiseEventOptions() { InterestGroup = (byte)view.Group };
                        PhotonNetwork.RaiseEventInternal(PunEvent.RPC, rpcEvent, options, sendOptions);
                        break;
                    }
                case RpcTarget.AllBuffered:
                    {
                        RaiseEventOptions options = new RaiseEventOptions() { CachingOption = EventCaching.AddToRoomCache };
                        PhotonNetwork.RaiseEventInternal(PunEvent.RPC, rpcEvent, options, sendOptions);
                        ExecuteRpc(rpcEvent, NetworkingClient.LocalPlayer);
                        break;
                    }
                case RpcTarget.OthersBuffered:
                    {
                        RaiseEventOptions options = new RaiseEventOptions() { CachingOption = EventCaching.AddToRoomCache };
                        PhotonNetwork.RaiseEventInternal(PunEvent.RPC, rpcEvent, options, sendOptions);
                        break;
                    }
                case RpcTarget.MasterClient:
                    {
                        if (NetworkingClient.LocalPlayer.IsMasterClient)
                        {
                            ExecuteRpc(rpcEvent, NetworkingClient.LocalPlayer);
                        }
                        else
                        {
                            RaiseEventOptions options = new RaiseEventOptions() { Receivers = ReceiverGroup.MasterClient };
                            PhotonNetwork.RaiseEventInternal(PunEvent.RPC, rpcEvent, options, sendOptions);
                        }

                        break;
                    }
                case RpcTarget.AllViaServer:
                    {
                        RaiseEventOptions options = new RaiseEventOptions() { InterestGroup = (byte)view.Group, Receivers = ReceiverGroup.All };
                        PhotonNetwork.RaiseEventInternal(PunEvent.RPC, rpcEvent, options, sendOptions);
                        if (PhotonNetwork.OfflineMode)
                        {
                            ExecuteRpc(rpcEvent, NetworkingClient.LocalPlayer);
                        }

                        break;
                    }
                case RpcTarget.AllBufferedViaServer:
                    {
                        RaiseEventOptions options = new RaiseEventOptions() { InterestGroup = (byte)view.Group, Receivers = ReceiverGroup.All, CachingOption = EventCaching.AddToRoomCache };
                        PhotonNetwork.RaiseEventInternal(PunEvent.RPC, rpcEvent, options, sendOptions);
                        if (PhotonNetwork.OfflineMode)
                        {
                            ExecuteRpc(rpcEvent, NetworkingClient.LocalPlayer);
                        }

                        break;
                    }
                default:
                    Debug.LogError("Unsupported target enum: " + target);
                    break;
            }
        }
        public static void SetInterestGroups(byte[] disableGroups, byte[] enableGroups)
        {

            if (disableGroups != null)
            {
                if (disableGroups.Length == 0)
                {
                    allowedReceivingGroups.Clear();
                }
                else
                {
                    for (int index = 0; index < disableGroups.Length; index++)
                    {
                        byte g = disableGroups[index];
                        if (g <= 0)
                        {
                            Debug.LogError("Error: PhotonNetwork.SetInterestGroups was called with an illegal group number: " + g + ". The Group number should be at least 1.");
                            continue;
                        }

                        if (allowedReceivingGroups.Contains(g))
                        {
                            allowedReceivingGroups.Remove(g);
                        }
                    }
                }
            }

            if (enableGroups != null)
            {
                if (enableGroups.Length == 0)
                {
                    for (byte index = 0; index < byte.MaxValue; index++)
                    {
                        allowedReceivingGroups.Add(index);
                    }

                    allowedReceivingGroups.Add(byte.MaxValue);
                }
                else
                {
                    for (int index = 0; index < enableGroups.Length; index++)
                    {
                        byte g = enableGroups[index];
                        if (g <= 0)
                        {
                            Debug.LogError("Error: PhotonNetwork.SetInterestGroups was called with an illegal group number: " + g + ". The Group number should be at least 1.");
                            continue;
                        }

                        allowedReceivingGroups.Add(g);
                    }
                }
            }

            if (!PhotonNetwork.offlineMode)
            {
                NetworkingClient.OpChangeGroups(disableGroups, enableGroups);
            }
        }
        public static void SetSendingEnabled(byte group, bool enabled)
        {

            if (!enabled)
            {
                blockedSendingGroups.Add(group); // can be added to HashSet no matter if already in it
            }
            else
            {
                blockedSendingGroups.Remove(group);
            }
        }
        public static void SetSendingEnabled(byte[] disableGroups, byte[] enableGroups)
        {

            if (disableGroups != null)
            {
                for (int index = 0; index < disableGroups.Length; index++)
                {
                    byte g = disableGroups[index];
                    blockedSendingGroups.Add(g);
                }
            }

            if (enableGroups != null)
            {
                for (int index = 0; index < enableGroups.Length; index++)
                {
                    byte g = enableGroups[index];
                    blockedSendingGroups.Remove(g);
                }
            }
        }


        internal static void NewSceneLoaded()
        {
            if (loadingLevelAndPausedNetwork)
            {
                _AsyncLevelLoadingOperation = null;
                loadingLevelAndPausedNetwork = false;
                PhotonNetwork.IsMessageQueueRunning = true;
            }
            else
            {
                PhotonNetwork.SetLevelInPropsIfSynced(SceneManagerHelper.ActiveSceneName);
            }

            List<int> removeKeys = new List<int>();
            foreach (KeyValuePair<int, PhotonView> kvp in photonViewList)
            {
                PhotonView view = kvp.Value;
                if (view == null)
                {
                    removeKeys.Add(kvp.Key);
                }
            }

            for (int index = 0; index < removeKeys.Count; index++)
            {

                int key = removeKeys[index];
                photonViewList.Remove(key);
            }

            if (removeKeys.Count > 0)
            {
                if (PhotonNetwork.LogLevel >= PunLogLevel.Informational)
                    Debug.Log("New level loaded. Removed " + removeKeys.Count + " scene view IDs from last level.");
            }
        }
        public static int ObjectsInOneUpdate = 20;


        private static readonly PhotonStream serializeStreamOut = new PhotonStream(true, null);
        private static readonly PhotonStream serializeStreamIn = new PhotonStream(false, null);
        private static RaiseEventOptions serializeRaiseEvOptions = new RaiseEventOptions();

        private struct RaiseEventBatch : IEquatable<RaiseEventBatch>
        {
            public byte Group;
            public bool Reliable;

            public override int GetHashCode()
            {
                return (this.Group << 1) + (this.Reliable ? 1 : 0);
            }

            public bool Equals(RaiseEventBatch other)
            {
                return this.Reliable == other.Reliable && this.Group == other.Group;
            }
        }


        private class SerializeViewBatch : IEquatable<SerializeViewBatch>, IEquatable<RaiseEventBatch>
        {
            public readonly RaiseEventBatch Batch;
            public List<object> ObjectUpdates;
            private int defaultSize = PhotonNetwork.ObjectsInOneUpdate;
            private int offset;
            public SerializeViewBatch(RaiseEventBatch batch, int offset)
            {
                this.Batch = batch;
                this.ObjectUpdates = new List<object>(this.defaultSize);
                this.offset = offset;
                for (int i = 0; i < offset; i++) this.ObjectUpdates.Add(null);
            }

            public override int GetHashCode()
            {
                return (this.Batch.Group << 1) + (this.Batch.Reliable ? 1 : 0);
            }

            public bool Equals(SerializeViewBatch other)
            {
                return this.Equals(other.Batch);
            }

            public bool Equals(RaiseEventBatch other)
            {
                return this.Batch.Reliable == other.Reliable && this.Batch.Group == other.Group;
            }

            public override bool Equals(object obj)
            {
                SerializeViewBatch other = obj as SerializeViewBatch;
                return other != null && this.Batch.Equals(other.Batch);
            }

            public void Clear()
            {
                this.ObjectUpdates.Clear();
                for (int i = 0; i < offset; i++) this.ObjectUpdates.Add(null);
            }

            public void Add(List<object> viewData)
            {
                if (this.ObjectUpdates.Count >= this.ObjectUpdates.Capacity)
                {
                    throw new Exception("Can't add. Size exceeded.");
                }

                this.ObjectUpdates.Add(viewData);
            }
        }


        private static readonly Dictionary<RaiseEventBatch, SerializeViewBatch> serializeViewBatches = new Dictionary<RaiseEventBatch, SerializeViewBatch>();
        internal static void RunViewUpdate()
        {
            if (PhotonNetwork.OfflineMode || CurrentRoom == null || CurrentRoom.Players == null)
            {
                return;
            }
#if !PHOTON_DEVELOP
            if (CurrentRoom.Players.Count <= 1)
            {
                return;
            }
#else
            serializeRaiseEvOptions.Receivers = (CurrentRoom.Players.Count == 1) ? ReceiverGroup.All : ReceiverGroup.Others;
#endif



            /* Format of the event's data object[]:
             *  [0] = PhotonNetwork.ServerTimestamp;
             *  [1] = currentLevelPrefix;  OPTIONAL!
             *  [2] = object[] of PhotonView x
             *  [3] = object[] of PhotonView y or NULL
             *  [...]
             *
             *  We only combine updates for XY objects into one RaiseEvent to avoid fragmentation.
             *  The Reliability and Interest Group are only used for RaiseEvent and not contained in the event/data that reaches the other clients.
             *  This is read in OnEvent().
             */


            var enumerator = photonViewList.GetEnumerator();   // replacing foreach (PhotonView view in this.photonViewList.Values) for memory allocation improvement
            while (enumerator.MoveNext())
            {
                PhotonView view = enumerator.Current.Value;
                if (view.Synchronization == ViewSynchronization.Off || view.IsMine == false || view.isActiveAndEnabled == false)
                {
                    continue;
                }

                if (blockedSendingGroups.Contains(view.Group))
                {
                    continue; // Block sending on this group
                }
                List<object> evData = OnSerializeWrite(view);
                if (evData == null)
                {
                    continue;
                }

                RaiseEventBatch eventBatch = new RaiseEventBatch();
                eventBatch.Reliable = view.Synchronization == ViewSynchronization.ReliableDeltaCompressed || view.mixedModeIsReliable;
                eventBatch.Group = view.Group;

                SerializeViewBatch svBatch = null;
                bool found = serializeViewBatches.TryGetValue(eventBatch, out svBatch);
                if (!found)
                {
                    svBatch = new SerializeViewBatch(eventBatch, 2);    // NOTE: the 2 first entries are kept empty for timestamp and level prefix
                    serializeViewBatches.Add(eventBatch, svBatch);
                }

                svBatch.Add(evData);
                if (svBatch.ObjectUpdates.Count == svBatch.ObjectUpdates.Capacity)
                {
                    SendSerializeViewBatch(svBatch);
                }
            }

            var enumeratorB = serializeViewBatches.GetEnumerator();
            while (enumeratorB.MoveNext())
            {
                SendSerializeViewBatch(enumeratorB.Current.Value);
            }
        }


        private static void SendSerializeViewBatch(SerializeViewBatch batch)
        {
            if (batch == null || batch.ObjectUpdates.Count <= 2)
            {
                return;
            }

            serializeRaiseEvOptions.InterestGroup = batch.Batch.Group;
            batch.ObjectUpdates[0] = PhotonNetwork.ServerTimestamp;
            batch.ObjectUpdates[1] = (currentLevelPrefix != 0) ? (object)currentLevelPrefix : null;
            byte code = batch.Batch.Reliable ? PunEvent.SendSerializeReliable : PunEvent.SendSerialize;

            PhotonNetwork.RaiseEventInternal(code, batch.ObjectUpdates, serializeRaiseEvOptions, batch.Batch.Reliable ? SendOptions.SendReliable : SendOptions.SendUnreliable);
            batch.Clear();
        }
        private static List<object> OnSerializeWrite(PhotonView view)
        {
            if (view.Synchronization == ViewSynchronization.Off)
            {
                return null;
            }
            PhotonMessageInfo info = new PhotonMessageInfo(NetworkingClient.LocalPlayer, PhotonNetwork.ServerTimestamp, view);

            if (view.syncValues == null) view.syncValues = new List<object>();
            view.syncValues.Clear();
            serializeStreamOut.SetWriteStream(view.syncValues);
            serializeStreamOut.SendNext(null);  //to become: viewID,
            serializeStreamOut.SendNext(null);  //to become: is compressed
            serializeStreamOut.SendNext(null);  //to become: null-values (for compression) followed by: values for this object's update


            view.SerializeView(serializeStreamOut, info);
            if (serializeStreamOut.Count <= SyncFirstValue)
            {
                return null;
            }


            List<object> currentValues = serializeStreamOut.GetWriteStream();
            currentValues[SyncViewId] = view.ViewID;
            currentValues[SyncCompressed] = false;      // (bool) compression was used.
            currentValues[SyncNullValues] = null;       // if reliable compressed, this is non-null.

            if (view.Synchronization == ViewSynchronization.Unreliable)
            {
                return currentValues;
            }
            if (view.Synchronization == ViewSynchronization.UnreliableOnChange)
            {
                if (AlmostEquals(currentValues, view.lastOnSerializeDataSent))
                {
                    if (view.mixedModeIsReliable)
                    {
                        return null;
                    }

                    view.mixedModeIsReliable = true;
                    List<object> temp = view.lastOnSerializeDataSent;   // TODO: extract "exchange" into method in PV
                    view.lastOnSerializeDataSent = currentValues;
                    view.syncValues = temp;
                }
                else
                {
                    view.mixedModeIsReliable = false;
                    List<object> temp = view.lastOnSerializeDataSent;   // TODO: extract "exchange" into method in PV
                    view.lastOnSerializeDataSent = currentValues;
                    view.syncValues = temp;
                }


                return currentValues;
            }

            if (view.Synchronization == ViewSynchronization.ReliableDeltaCompressed)
            {
                List<object> dataToSend = DeltaCompressionWrite(view.lastOnSerializeDataSent, currentValues);
                List<object> temp = view.lastOnSerializeDataSent;   // TODO: extract "exchange" into method in PV
                view.lastOnSerializeDataSent = currentValues;
                view.syncValues = temp;

                return dataToSend;
            }

            return null;
        }
        private static void OnSerializeRead(object[] data, Player sender, int networkTime, short correctPrefix)
        {
            int viewID = (int)data[SyncViewId];

            PhotonView view = GetPhotonView(viewID);
            if (view == null)
            {
                if (PhotonNetwork.LogLevel >= PunLogLevel.Informational)
                {
                    Debug.LogWarning("Received OnSerialization for view ID " + viewID + ". We have no such PhotonView! Ignore this if you're joining or leaving a room. State: " + NetworkingClient.State);
                }
                return;
            }

            if (view.Prefix > 0 && correctPrefix != view.Prefix)
            {
                Debug.LogError("Received OnSerialization for view ID " + viewID + " with prefix " + correctPrefix + ". Our prefix is " + view.Prefix);
                return;
            }
            if (view.Group != 0 && !allowedReceivingGroups.Contains(view.Group))
            {
                return; // Ignore group
            }




            if (view.Synchronization == ViewSynchronization.ReliableDeltaCompressed)
            {
                object[] uncompressed = DeltaCompressionRead(view.lastOnSerializeDataReceived, data);
                if (uncompressed == null)
                {
                    if (PhotonNetwork.LogLevel >= PunLogLevel.Informational)
                    {
                        Debug.Log("Skipping packet for " + view.name + " [" + view.ViewID +
                                  "] as we haven't received a full packet for delta compression yet. This is OK if it happens for the first few frames after joining a game.");
                    }
                    return;
                }
                view.lastOnSerializeDataReceived = uncompressed;
                data = uncompressed;
            }

            serializeStreamIn.SetReadStream(data, 3);
            PhotonMessageInfo info = new PhotonMessageInfo(sender, networkTime, view);

            view.DeserializeView(serializeStreamIn, info);
        }
        public const int SyncViewId = 0;
        public const int SyncCompressed = 1;
        public const int SyncNullValues = 2;
        public const int SyncFirstValue = 3;

        private static List<object> DeltaCompressionWrite(List<object> previousContent, List<object> currentContent)
        {
            if (currentContent == null || previousContent == null || previousContent.Count != currentContent.Count)
            {
                return currentContent; // the current data needs to be sent (which might be null)
            }

            if (currentContent.Count <= SyncFirstValue)
            {
                return null; // this send doesn't contain values (except the "headers"), so it's not being sent
            }

            List<object> compressedContent = previousContent; // the previous content is no longer needed, once we compared the values!
            compressedContent[SyncCompressed] = false;
            int compressedValues = 0;

            Queue<int> valuesThatAreChangedToNull = null;
            for (int index = SyncFirstValue; index < currentContent.Count; index++)
            {
                object newObj = currentContent[index];
                object oldObj = previousContent[index];
                if (AlmostEquals(newObj, oldObj))
                {
                    compressedValues++;
                    compressedContent[index] = null;
                }
                else
                {
                    compressedContent[index] = newObj;
                    if (newObj == null)
                    {
                        if (valuesThatAreChangedToNull == null)
                        {
                            valuesThatAreChangedToNull = new Queue<int>(currentContent.Count);
                        }
                        valuesThatAreChangedToNull.Enqueue(index);
                    }
                }
            }
            if (compressedValues > 0)
            {
                if (compressedValues == currentContent.Count - SyncFirstValue)
                {
                    return null;
                }

                compressedContent[SyncCompressed] = true;
                if (valuesThatAreChangedToNull != null)
                {
                    compressedContent[SyncNullValues] = valuesThatAreChangedToNull.ToArray(); // data that is actually null (not just cause we didn't want to send it)
                }
            }

            compressedContent[SyncViewId] = currentContent[SyncViewId];
            return compressedContent; // some data was compressed but we need to send something
        }


        private static object[] DeltaCompressionRead(object[] lastOnSerializeDataReceived, object[] incomingData)
        {
            if ((bool)incomingData[SyncCompressed] == false)
            {
                return incomingData;
            }
            if (lastOnSerializeDataReceived == null)
            {
                return null;
            }


            int[] indexesThatAreChangedToNull = incomingData[2] as int[];
            for (int index = SyncFirstValue; index < incomingData.Length; index++)
            {
                if (indexesThatAreChangedToNull != null && indexesThatAreChangedToNull.Contains(index))
                {
                    continue; // if a value was set to null in this update, we don't need to fetch it from an earlier update
                }
                if (incomingData[index] == null)
                {
                    object lastValue = lastOnSerializeDataReceived[index];
                    incomingData[index] = lastValue;
                }
            }

            return incomingData;
        }


        private static bool AlmostEquals(IList<object> lastData, IList<object> currentContent)
        {
            if (lastData == null && currentContent == null)
            {
                return true;
            }

            if (lastData == null || currentContent == null || (lastData.Count != currentContent.Count))
            {
                return false;
            }

            for (int index = 0; index < currentContent.Count; index++)
            {
                object newObj = currentContent[index];
                object oldObj = lastData[index];
                if (!AlmostEquals(newObj, oldObj))
                {
                    return false;
                }
            }

            return true;
        }
        static bool AlmostEquals(object one, object two)
        {
            if (one == null || two == null)
            {
                return one == null && two == null;
            }

            if (!one.Equals(two))
            {
                if (one is Vector3)
                {
                    Vector3 a = (Vector3)one;
                    Vector3 b = (Vector3)two;
                    if (a.AlmostEquals(b, PhotonNetwork.PrecisionForVectorSynchronization))
                    {
                        return true;
                    }
                }
                else if (one is Vector2)
                {
                    Vector2 a = (Vector2)one;
                    Vector2 b = (Vector2)two;
                    if (a.AlmostEquals(b, PhotonNetwork.PrecisionForVectorSynchronization))
                    {
                        return true;
                    }
                }
                else if (one is Quaternion)
                {
                    Quaternion a = (Quaternion)one;
                    Quaternion b = (Quaternion)two;
                    if (a.AlmostEquals(b, PhotonNetwork.PrecisionForQuaternionSynchronization))
                    {
                        return true;
                    }
                }
                else if (one is float)
                {
                    float a = (float)one;
                    float b = (float)two;
                    if (a.AlmostEquals(b, PhotonNetwork.PrecisionForFloatSynchronization))
                    {
                        return true;
                    }
                }
                return false;
            }

            return true;
        }
        internal static bool GetMethod(MonoBehaviour monob, string methodType, out MethodInfo mi)
        {
            mi = null;

            if (monob == null || string.IsNullOrEmpty(methodType))
            {
                return false;
            }

            List<MethodInfo> methods = SupportClassPun.GetMethods(monob.GetType(), null);
            for (int index = 0; index < methods.Count; index++)
            {
                MethodInfo methodInfo = methods[index];
                if (methodInfo.Name.Equals(methodType))
                {
                    mi = methodInfo;
                    return true;
                }
            }

            return false;
        }
        internal static void LoadLevelIfSynced()
        {
            if (!PhotonNetwork.AutomaticallySyncScene || PhotonNetwork.IsMasterClient || PhotonNetwork.CurrentRoom == null)
            {
                return;
            }
            if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(CurrentSceneProperty))
            {
                return;
            }
            object sceneId = PhotonNetwork.CurrentRoom.CustomProperties[CurrentSceneProperty];
            if (sceneId is int)
            {
                if (SceneManagerHelper.ActiveSceneBuildIndex != (int)sceneId)
                {
                    PhotonNetwork.LoadLevel((int)sceneId);
                }
            }
            else if (sceneId is string)
            {
                if (SceneManagerHelper.ActiveSceneName != (string)sceneId)
                {
                    PhotonNetwork.LoadLevel((string)sceneId);
                }
            }
        }


        internal static void SetLevelInPropsIfSynced(object levelId)
        {
            if (!PhotonNetwork.AutomaticallySyncScene || !PhotonNetwork.IsMasterClient || PhotonNetwork.CurrentRoom == null)
            {
                return;
            }
            if (levelId == null)
            {
                Debug.LogError("Parameter levelId can't be null!");
                return;
            }
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(CurrentSceneProperty))
            {
                object levelIdInProps = PhotonNetwork.CurrentRoom.CustomProperties[CurrentSceneProperty];

                if (levelId.Equals(levelIdInProps))
                {
                    return;
                }
                else
                {
                    int scnIndex = SceneManagerHelper.ActiveSceneBuildIndex;
                    string scnName = SceneManagerHelper.ActiveSceneName;

                    if ((levelId.Equals(scnIndex) && levelIdInProps.Equals(scnName)) || (levelId.Equals(scnName) && levelIdInProps.Equals(scnIndex)))
                    {
                        return;
                    }
                }
            }
            if (_AsyncLevelLoadingOperation != null)
            {
                if (!_AsyncLevelLoadingOperation.isDone)
                {
                    Debug.LogWarning("PUN cancels an ongoing async level load, as another scene should be loaded. Next scene to load: " + levelId);
                }

                _AsyncLevelLoadingOperation.allowSceneActivation = false;
                _AsyncLevelLoadingOperation = null;
            }
            Hashtable setScene = new Hashtable();
            if (levelId is int) setScene[CurrentSceneProperty] = (int)levelId;
            else if (levelId is string) setScene[CurrentSceneProperty] = (string)levelId;
            else Debug.LogError("Parameter levelId must be int or string!");

            PhotonNetwork.CurrentRoom.SetCustomProperties(setScene);
            SendAllOutgoingCommands(); // send immediately! because: in most cases the client will begin to load and pause sending anything for a while
        }


        private static void OnEvent(EventData photonEvent)
        {
            int actorNr = photonEvent.Sender;
            Player originatingPlayer = null;
            if (actorNr > 0 && NetworkingClient.CurrentRoom != null)
            {
                originatingPlayer = NetworkingClient.CurrentRoom.GetPlayer(actorNr);
            }

            switch (photonEvent.Code)
            {
                case EventCode.Join:
                    ResetPhotonViewsOnSerialize();
                    break;

                case PunEvent.RPC:
                    ExecuteRpc(photonEvent.CustomData as Hashtable, originatingPlayer);
                    break;

                case PunEvent.SendSerialize:
                case PunEvent.SendSerializeReliable:

                    /* This case must match definition in RunViewUpdate() and OnSerializeWrite().
                     * Format of the event's data object[]:
                     *  [0] = PhotonNetwork.ServerTimestamp;
                     *  [1] = currentLevelPrefix;  OPTIONAL!
                     *  [2] = object[] of PhotonView x
                     *  [3] = object[] of PhotonView y or NULL
                     *  [...]
                     *
                     *  We only combine updates for XY objects into one RaiseEvent to avoid fragmentation.
                     *  The Reliability and Interest Group are only used for RaiseEvent and not contained in the event/data that reaches the other clients.
                     *  This is read in OnEvent().
                     */

                    object[] pvUpdates = (object[])photonEvent[ParameterCode.Data];
                    int remoteUpdateServerTimestamp = (int)pvUpdates[0];
                    short remoteLevelPrefix = (pvUpdates[1] != null) ? (byte)pvUpdates[1] : (short)0;

                    object[] viewUpdate = null;
                    for (int i = 2; i < pvUpdates.Length; i++)
                    {
                        viewUpdate = pvUpdates[i] as object[];
                        if (viewUpdate == null)
                        {
                            break;
                        }
                        OnSerializeRead(viewUpdate, originatingPlayer, remoteUpdateServerTimestamp, remoteLevelPrefix);
                    }
                    break;

                case PunEvent.Instantiation:
                    NetworkInstantiate((Hashtable)photonEvent.CustomData, originatingPlayer);
                    break;

                case PunEvent.CloseConnection:
                    if (PhotonNetwork.EnableCloseConnection == false)
                    {
                        Debug.LogWarning("CloseConnection received from " + originatingPlayer + ". PhotonNetwork.EnableCloseConnection is false. Ignoring the request (this client stays in the room).");
                    }
                    else if (originatingPlayer == null || !originatingPlayer.IsMasterClient)
                    {
                        Debug.LogWarning("CloseConnection received from " + originatingPlayer + ". That player is not the Master Client. " + PhotonNetwork.MasterClient + " is.");
                    }
                    else if (PhotonNetwork.EnableCloseConnection)
                    {
                        PhotonNetwork.LeaveRoom(false);
                    }

                    break;

                case PunEvent.DestroyPlayer:
                    Hashtable evData = photonEvent.CustomData as Hashtable;
                    if (evData == null)
                    {
                        break;
                    }
                    int targetPlayerId = (int)evData[keyByteZero];
                    if (targetPlayerId >= 0)
                    {
                        DestroyPlayerObjects(targetPlayerId, true);
                    }
                    else
                    {
                        DestroyAll(true);
                    }
                    break;

                case EventCode.Leave:
                    if (CurrentRoom != null && CurrentRoom.AutoCleanUp && (originatingPlayer == null || !originatingPlayer.IsInactive))
                    {
                        DestroyPlayerObjects(actorNr, true);
                    }
                    break;

                case PunEvent.Destroy:
                    evData = (Hashtable)photonEvent.CustomData;
                    int instantiationId = (int)evData[keyByteZero];


                    PhotonView pvToDestroy = null;
                    if (photonViewList.TryGetValue(instantiationId, out pvToDestroy))
                    {
                        RemoveInstantiatedGO(pvToDestroy.gameObject, true);
                    }
                    else
                    {
                        Debug.LogError("Ev Destroy Failed. Could not find PhotonView with instantiationId " + instantiationId + ". Sent by actorNr: " + actorNr);
                    }

                    break;

                case PunEvent.OwnershipRequest:
                    {
                        int[] requestValues = (int[])photonEvent.CustomData;
                        int requestedViewId = requestValues[0];
                        int requestedFromOwnerId = requestValues[1];


                        PhotonView requestedView = GetPhotonView(requestedViewId);
                        if (requestedView == null)
                        {
                            Debug.LogWarning("Can't find PhotonView of incoming OwnershipRequest. ViewId not found: " + requestedViewId);
                            break;
                        }

                        if (PhotonNetwork.LogLevel == PunLogLevel.Informational)
                        {
                            Debug.Log(string.Format("OwnershipRequest. actorNr {0} requests view {1} from {2}. current pv owner: {3} is {4}. isMine: {6} master client: {5}", actorNr, requestedViewId, requestedFromOwnerId, requestedView.OwnerActorNr, requestedView.IsOwnerActive ? "active" : "inactive", MasterClient.ActorNumber, requestedView.IsMine));
                        }

                        switch (requestedView.OwnershipTransfer)
                        {
                            case OwnershipOption.Takeover:
                                int currentPvOwnerId = requestedView.OwnerActorNr;
                                if (requestedFromOwnerId == currentPvOwnerId || (requestedFromOwnerId == 0 && currentPvOwnerId == MasterClient.ActorNumber) || currentPvOwnerId == 0)
                                {
                                    Player prevOwner = requestedView.Owner;

                                    requestedView.OwnerActorNr = actorNr;
                                    requestedView.ControllerActorNr = actorNr;

                                    if (PhotonNetwork.OnOwnershipTransferedEv != null)
                                    {
                                        PhotonNetwork.OnOwnershipTransferedEv(requestedView, prevOwner);
                                    }
                                }
                                else
                                {

                                    if (PhotonNetwork.OnOwnershipTransferFailedEv != null)
                                    {
                                        PhotonNetwork.OnOwnershipTransferFailedEv(requestedView, originatingPlayer);
                                    }
                                }
                                break;

                            case OwnershipOption.Request:
                                if (PhotonNetwork.OnOwnershipRequestEv != null)
                                {
                                    PhotonNetwork.OnOwnershipRequestEv(requestedView, originatingPlayer);
                                }
                                break;

                            default:
                                Debug.LogWarning("Ownership mode == " + (requestedView.OwnershipTransfer) + ". Ignoring request.");
                                break;
                        }
                    }
                    break;

                case PunEvent.OwnershipTransfer:
                    {
                        int[] transferViewToUserID = (int[])photonEvent.CustomData;
                        int requestedViewId = transferViewToUserID[0];
                        int newOwnerId = transferViewToUserID[1];

                        if (PhotonNetwork.LogLevel >= PunLogLevel.Informational)
                        {
                            Debug.Log("Ev OwnershipTransfer. ViewID " + requestedViewId + " to: " + newOwnerId + " Time: " + Environment.TickCount % 1000);
                        }

                        PhotonView requestedView = GetPhotonView(requestedViewId);
                        if (requestedView != null)
                        {
                            if (requestedView.OwnershipTransfer == OwnershipOption.Takeover ||
                                (requestedView.OwnershipTransfer == OwnershipOption.Request && (originatingPlayer == requestedView.Controller || originatingPlayer == requestedView.Owner)))
                            {
                                Player prevOwner = requestedView.Owner;

                                requestedView.OwnerActorNr= newOwnerId;
                                requestedView.ControllerActorNr = newOwnerId;

                                if (PhotonNetwork.OnOwnershipTransferedEv != null)
                                {
                                    PhotonNetwork.OnOwnershipTransferedEv(requestedView, prevOwner);
                                }
                            }
                            else if (PhotonNetwork.LogLevel >= PunLogLevel.Informational)
                            {
                                if (requestedView.OwnershipTransfer == OwnershipOption.Request)
                                    Debug.Log("Failed incoming OwnershipTransfer attempt for '" + requestedView.name + "; " + requestedViewId +
                                              " - photonView has OwnershipTransfer set to OwnershipOption.Request, but Player attempting to change owner is not the current owner/controller.");
                                else
                                    Debug.Log("Failed incoming OwnershipTransfer attempt for '" + requestedView.name + "; " + requestedViewId +
                                              " - photonView has OwnershipTransfer set to OwnershipOption.Fixed.");
                            }
                        }
                        else if (PhotonNetwork.LogLevel >= PunLogLevel.ErrorsOnly)
                        {
                            Debug.LogErrorFormat("Failed to find a PhotonView with ID={0} for incoming OwnershipTransfer event (newOwnerActorNumber={1}), sender={2}",
                                                 requestedViewId, newOwnerId, actorNr);
                        }

                        break;
                    }

                case PunEvent.OwnershipUpdate:
                    {
                        reusablePVHashset.Clear();
                        int[] viewOwnerPair = (int[])photonEvent.CustomData;

                        for (int i = 0, cnt = viewOwnerPair.Length; i < cnt; i++)
                        {
                            int viewId = viewOwnerPair[i];
                            i++;
                            int newOwnerId = viewOwnerPair[i];

                            PhotonView view = GetPhotonView(viewId);
                            if (view == null)
                            {
                                if (PhotonNetwork.LogLevel >= PunLogLevel.ErrorsOnly)
                                {
                                    Debug.LogErrorFormat("Failed to find a PhotonView with ID={0} for incoming OwnershipUpdate event (newOwnerActorNumber={1}), sender={2}. If you load scenes, make sure to pause the message queue.", viewId, newOwnerId, actorNr);
                                }

                                continue;
                            }

                            Player prevOwner = view.Owner;
                            Player newOwner = CurrentRoom.GetPlayer(newOwnerId, true);

                            view.OwnerActorNr= newOwnerId;
                            view.ControllerActorNr = newOwnerId;

                            reusablePVHashset.Add(view);
                            if (PhotonNetwork.OnOwnershipTransferedEv != null && newOwner != prevOwner)
                            {
                                PhotonNetwork.OnOwnershipTransferedEv(view, prevOwner);
                            }
                        }
                        foreach (var view in PhotonViewCollection)
                        {
                            if (!reusablePVHashset.Contains(view))
                                view.RebuildControllerCache();
                        }

                        break;
                    }


            }
        }

        private static void OnOperation(OperationResponse opResponse)
        {
            switch (opResponse.OperationCode)
            {
                case OperationCode.GetRegions:
                    if (opResponse.ReturnCode != 0)
                    {
                        if (PhotonNetwork.LogLevel >= PunLogLevel.Full)
                        {
                            Debug.Log("OpGetRegions failed. Will not ping any. ReturnCode: " + opResponse.ReturnCode);
                        }
                        return;
                    }
                    if (ConnectMethod == ConnectMethod.ConnectToBest)
                    {
                        string previousBestRegionSummary = PhotonNetwork.BestRegionSummaryInPreferences;

                        if (PhotonNetwork.LogLevel >= PunLogLevel.Informational)
                        {
                            Debug.Log("PUN got region list. Going to ping minimum regions, based on this previous result summary: " + previousBestRegionSummary);
                        }
                        NetworkingClient.RegionHandler.PingMinimumOfRegions(OnRegionsPinged, previousBestRegionSummary);
                    }
                    break;
                case OperationCode.JoinGame:
                    if (Server == ServerConnection.GameServer)
                    {
                        PhotonNetwork.LoadLevelIfSynced();
                    }
                    break;
            }
        }

        private static void OnClientStateChanged(ClientState previousState, ClientState state)
        {
            if (
                (previousState == ClientState.Joined && state == ClientState.Disconnected) ||
                (Server == ServerConnection.GameServer && (state == ClientState.Disconnecting || state == ClientState.DisconnectingFromGameServer))
            )
            {
                LeftRoomCleanup();
            }

            if (state == ClientState.ConnectedToMasterServer && _cachedRegionHandler != null)
            {
                BestRegionSummaryInPreferences = _cachedRegionHandler.SummaryToCache;
                _cachedRegionHandler = null;
            }
        }
        private static RegionHandler _cachedRegionHandler;

        private static void OnRegionsPinged(RegionHandler regionHandler)
        {
            if (PhotonNetwork.LogLevel >= PunLogLevel.Informational)
            {
                Debug.Log(regionHandler.GetResults());
            }

            _cachedRegionHandler = regionHandler;
            
            #if UNITY_EDITOR
            if (!PhotonServerSettings.DevRegionSetOnce)
            {
                PhotonServerSettings.DevRegionSetOnce = true;
                PhotonServerSettings.DevRegion = _cachedRegionHandler.BestRegion.Code;
            }
            #endif

            #if DEVELOPMENT_BUILD || UNITY_EDITOR
            if (!string.IsNullOrEmpty(PhotonServerSettings.DevRegion) && ConnectMethod == ConnectMethod.ConnectToBest)
            {
                Debug.LogWarning("PUN is in development mode (development build). As the 'dev region' is not empty (" + PhotonServerSettings.DevRegion + ") it overrides the found best region. See PhotonServerSettings.");

                string _finalDevRegion = PhotonServerSettings.DevRegion;
                if (!_cachedRegionHandler.EnabledRegions.Any(p => p.Code == PhotonServerSettings.DevRegion))
                {
                    _finalDevRegion = _cachedRegionHandler.EnabledRegions[0].Code;

                    Debug.LogWarning("The 'dev region' (" + PhotonServerSettings.DevRegion + ") was not found in the enabled regions, the first enabled region is picked (" + _finalDevRegion + ")");
                }

                bool connects = PhotonNetwork.NetworkingClient.ConnectToRegionMaster(_finalDevRegion);
                if (!connects)
                {
                    Debug.LogError("PUN could not ConnectToRegionMaster successfully. Please check error messages.");
                }
                return;
            }
            #endif

            if (NetworkClientState == ClientState.ConnectedToNameServer)
            {
                bool connects = PhotonNetwork.NetworkingClient.ConnectToRegionMaster(regionHandler.BestRegion.Code);
                if (!connects)
                {
                    Debug.LogError("PUN could not ConnectToRegionMaster successfully. Please check error messages.");
                }
            }
        }
    }
}
