

namespace Photon.Pun
{
    using System;
    using UnityEngine;
    using UnityEngine.Serialization;
    using System.Collections.Generic;
    using Photon.Realtime;

    #if UNITY_EDITOR
    using UnityEditor;
    #endif
    [AddComponentMenu("Photon Networking/Photon View")]
    public class PhotonView : MonoBehaviour
    {
        #if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        private static void SetPhotonViewExecutionOrder()
        {
            int photonViewExecutionOrder = -16000;
            GameObject go = new GameObject();
            PhotonView pv = go.AddComponent<PhotonView>();
            MonoScript monoScript = MonoScript.FromMonoBehaviour(pv);

            if (photonViewExecutionOrder != MonoImporter.GetExecutionOrder(monoScript))
            {
                MonoImporter.SetExecutionOrder(monoScript, photonViewExecutionOrder); // very early but allows other scripts to run even earlier...
            }

            DestroyImmediate(go); 
        }
        #endif

        #if UNITY_EDITOR
        [ContextMenu("Open PUN Wizard")]
        void OpenPunWizard()
        {
            EditorApplication.ExecuteMenuItem("Window/Photon Unity Networking/PUN Wizard");
        }
        #endif

        #if UNITY_EDITOR
        #pragma warning disable 0414
        [SerializeField]
        bool ObservedComponentsFoldoutOpen = true;
        #pragma warning restore 0414
        #endif
        
        #if UNITY_EDITOR
        private void Reset()
        {
            observableSearch = ObservableSearch.AutoFindAll;
        }
        #endif



        [FormerlySerializedAs("group")]
        public byte Group = 0;
        public int Prefix
        {
            get
            {
                if (this.prefixField == -1 && PhotonNetwork.NetworkingClient != null)
                {
                    this.prefixField = PhotonNetwork.currentLevelPrefix;
                }

                return this.prefixField;
            }
            set { this.prefixField = value; }
        }
        [FormerlySerializedAs("prefixBackup")]
        public int prefixField = -1;
        public object[] InstantiationData
        {
            get { return this.instantiationDataField; }
            protected internal set { this.instantiationDataField = value; }
        }

        internal object[] instantiationDataField;
        protected internal List<object> lastOnSerializeDataSent = null;
        protected internal List<object> syncValues;
        protected internal object[] lastOnSerializeDataReceived = null;

        [FormerlySerializedAs("synchronization")]
        public ViewSynchronization Synchronization = ViewSynchronization.UnreliableOnChange;

        protected internal bool mixedModeIsReliable = false;
        [FormerlySerializedAs("ownershipTransfer")]
        public OwnershipOption OwnershipTransfer = OwnershipOption.Fixed;


        public enum ObservableSearch { Manual, AutoFindActive, AutoFindAll }
        public ObservableSearch observableSearch = ObservableSearch.Manual;
        
        public List<Component> ObservedComponents;



        internal MonoBehaviour[] RpcMonoBehaviours;



        [Obsolete("Renamed. Use IsRoomView instead")]
        public bool IsSceneView
        {
            get { return this.IsRoomView; }
        }
        public bool IsRoomView
        {
            get { return this.CreatorActorNr == 0; }
        }

        public bool IsOwnerActive
        {
            get { return this.Owner != null && !this.Owner.IsInactive; }
        }
        public bool IsMine { get; private set; }
        public bool AmController
        {
            get { return this.IsMine; }
        }

        public Player Controller { get; private set; }
        
        public int CreatorActorNr { get; private set; }

        public bool AmOwner { get; private set; }
        public Player Owner { get; private set; }



        [NonSerialized]
        private int ownerActorNr;

        public int OwnerActorNr
        {
            get { return this.ownerActorNr; }
            set
            {
                if (value != 0 && this.ownerActorNr == value)
                {
                    return;
                }

                Player prevOwner = this.Owner;

                this.Owner = PhotonNetwork.CurrentRoom == null ? null : PhotonNetwork.CurrentRoom.GetPlayer(value, true);
                this.ownerActorNr = this.Owner != null ? this.Owner.ActorNumber : value;

                this.AmOwner = PhotonNetwork.LocalPlayer != null && this.ownerActorNr == PhotonNetwork.LocalPlayer.ActorNumber;

                this.UpdateCallbackLists();
                if (!ReferenceEquals(this.OnOwnerChangeCallbacks, null))
                {
                    for (int i = 0, cnt = this.OnOwnerChangeCallbacks.Count; i < cnt; ++i)
                    {
                        this.OnOwnerChangeCallbacks[i].OnOwnerChange(this.Owner, prevOwner);
                    }
                }
            }
        }

        
        [NonSerialized]
        private int controllerActorNr;

        public int ControllerActorNr
        {
            get { return this.controllerActorNr; }
            set
            {
                Player prevController = this.Controller;

                this.Controller = PhotonNetwork.CurrentRoom == null ? null : PhotonNetwork.CurrentRoom.GetPlayer(value, true);
                if (this.Controller != null && this.Controller.IsInactive)
                {
                    this.Controller = PhotonNetwork.MasterClient;
                }
                this.controllerActorNr = this.Controller != null ? this.Controller.ActorNumber : value;

                this.IsMine = PhotonNetwork.LocalPlayer != null && this.controllerActorNr == PhotonNetwork.LocalPlayer.ActorNumber;

                if (!ReferenceEquals(this.Controller, prevController))
                {
                    this.UpdateCallbackLists();
                    if (!ReferenceEquals(this.OnControllerChangeCallbacks, null))
                    {
                        for (int i = 0, cnt = this.OnControllerChangeCallbacks.Count; i < cnt; ++i)
                        {
                            this.OnControllerChangeCallbacks[i].OnControllerChange(this.Controller, prevController);
                        }
                    }
                }
            }
        }
        [SerializeField]
        [FormerlySerializedAs("viewIdField")]
        [HideInInspector]
        public int sceneViewId = 0; // TODO: in best case, this is not public
        [NonSerialized]
        private int viewIdField = 0;
        public int ViewID
        {
            get
            {
                return this.viewIdField;
            }

            set
            {
                if (value != 0 && this.viewIdField != 0)
                {
                    Debug.LogWarning("Changing a ViewID while it's in use is not possible (except setting it to 0 (not being used). Current ViewID: " + this.viewIdField);
                    return;
                }
                
                if (value == 0 && this.viewIdField != 0)
                {
                    PhotonNetwork.LocalCleanPhotonView(this);
                }

                this.viewIdField = value;
                this.CreatorActorNr = value / PhotonNetwork.MAX_VIEW_IDS;   // the creator can be derived from the viewId. this is also the initial owner and creator.
                this.OwnerActorNr = this.CreatorActorNr;
                this.ControllerActorNr = this.CreatorActorNr;
                this.RebuildControllerCache();
                if (value != 0)
                {
                    PhotonNetwork.RegisterPhotonView(this);
                }
            }
        }

        [FormerlySerializedAs("instantiationId")]
        public int InstantiationId; // if the view was instantiated with a GO, this GO has a instantiationID (first view's viewID)

        [SerializeField]
        [HideInInspector]
        public bool isRuntimeInstantiated;


        protected internal bool removedFromLocalViewList;
        protected internal void Awake()
        {
            if (this.ViewID != 0)
            {
                return;
            }
            
            if (this.sceneViewId != 0)
            {
                this.ViewID = this.sceneViewId;
            }

            this.FindObservables();
        }
        internal void ResetPhotonView(bool resetOwner)
        {
            this.lastOnSerializeDataSent = null;
        }
        internal void RebuildControllerCache(bool ownerHasChanged = false)
        {
            if (this.controllerActorNr == 0 || this.OwnerActorNr == 0 || this.Owner == null || this.Owner.IsInactive)
            {
                var masterclient = PhotonNetwork.MasterClient;
                this.ControllerActorNr = masterclient == null ? -1 : masterclient.ActorNumber;
            }
            else
            {
                this.ControllerActorNr = this.OwnerActorNr;
            }
        }


        public void OnPreNetDestroy(PhotonView rootView)
        {
            UpdateCallbackLists();

            if (!ReferenceEquals(OnPreNetDestroyCallbacks, null))
                for (int i = 0, cnt = OnPreNetDestroyCallbacks.Count; i < cnt; ++i)
                {
                    OnPreNetDestroyCallbacks[i].OnPreNetDestroy(rootView);
                }
        }

        protected internal void OnDestroy()
        {
            if (!this.removedFromLocalViewList)
            {
                bool wasInList = PhotonNetwork.LocalCleanPhotonView(this);

                if (wasInList && this.InstantiationId > 0 && !PhotonHandler.AppQuits && PhotonNetwork.LogLevel >= PunLogLevel.Informational)
                {
                    Debug.Log("PUN-instantiated '" + this.gameObject.name + "' got destroyed by engine. This is OK when loading levels. Otherwise use: PhotonNetwork.Destroy().");
                }
            }
        }
        public void RequestOwnership()
        {
            if (OwnershipTransfer != OwnershipOption.Fixed)
            {
                PhotonNetwork.RequestOwnership(this.ViewID, this.ownerActorNr);
            }
            else
            {
                if (PhotonNetwork.LogLevel >= PunLogLevel.Informational)
                {
                    Debug.LogWarning("Attempting to RequestOwnership of GameObject '" + name + "' viewId: " + ViewID +
                        ", but PhotonView.OwnershipTransfer is set to Fixed.");
                }
            }
        }
        public void TransferOwnership(Player newOwner)
        {
            if (newOwner != null)
                TransferOwnership(newOwner.ActorNumber);
            else
            {
                if (PhotonNetwork.LogLevel >= PunLogLevel.Informational)
                {
                    Debug.LogWarning("Attempting to TransferOwnership of GameObject '" + name + "' viewId: " + ViewID +
                   ", but provided Player newOwner is null.");
                }
            }
        }
        public void TransferOwnership(int newOwnerId)
        {
            if (OwnershipTransfer == OwnershipOption.Takeover || (OwnershipTransfer == OwnershipOption.Request && this.AmController))
            {
                PhotonNetwork.TransferOwnership(this.ViewID, newOwnerId);
            }
            else
            {
                if (PhotonNetwork.LogLevel >= PunLogLevel.Informational)
                {
                    if (OwnershipTransfer == OwnershipOption.Fixed)
                        Debug.LogWarning("Attempting to TransferOwnership of GameObject '" + name + "' viewId: " + ViewID +
                            " without the authority to do so. TransferOwnership is not allowed if PhotonView.OwnershipTransfer is set to Fixed.");
                    else if (OwnershipTransfer == OwnershipOption.Request)
                        Debug.LogWarning("Attempting to TransferOwnership of GameObject '" + name + "' viewId: " + ViewID +
                           " without the authority to do so. PhotonView.OwnershipTransfer is set to Request, so only the controller of this object can TransferOwnership.");
                }
            }
        }
        public void FindObservables(bool force = false)
        {
            if (!force && this.observableSearch == ObservableSearch.Manual)
            {
                return;
            }

            if (this.ObservedComponents == null)
            {
                this.ObservedComponents = new List<Component>();
            }
            else
            {
                this.ObservedComponents.Clear();
            }

            this.transform.GetNestedComponentsInChildren<Component, IPunObservable, PhotonView>(force || this.observableSearch == ObservableSearch.AutoFindAll, this.ObservedComponents);
        }


        public void SerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (this.ObservedComponents != null && this.ObservedComponents.Count > 0)
            {
                for (int i = 0; i < this.ObservedComponents.Count; ++i)
                {
                    var component = this.ObservedComponents[i];
                    if (component != null)
                        SerializeComponent(this.ObservedComponents[i], stream, info);
                }
            }
        }

        public void DeserializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (this.ObservedComponents != null && this.ObservedComponents.Count > 0)
            {
                for (int i = 0; i < this.ObservedComponents.Count; ++i)
                {
                    var component = this.ObservedComponents[i];
                    if (component != null)
                        DeserializeComponent(component, stream, info);
                }
            }
        }

        protected internal void DeserializeComponent(Component component, PhotonStream stream, PhotonMessageInfo info)
        {
            IPunObservable observable = component as IPunObservable;
            if (observable != null)
            {
                observable.OnPhotonSerializeView(stream, info);
            }
            else
            {
                Debug.LogError("Observed scripts have to implement IPunObservable. " + component + " does not. It is Type: " + component.GetType(), component.gameObject);
            }
        }

        protected internal void SerializeComponent(Component component, PhotonStream stream, PhotonMessageInfo info)
        {
            IPunObservable observable = component as IPunObservable;
            if (observable != null)
            {
                observable.OnPhotonSerializeView(stream, info);
            }
            else
            {
                Debug.LogError("Observed scripts have to implement IPunObservable. " + component + " does not. It is Type: " + component.GetType(), component.gameObject);
            }
        }
        public void RefreshRpcMonoBehaviourCache()
        {
            this.RpcMonoBehaviours = this.GetComponents<MonoBehaviour>();
        }
        public void RPC(string methodName, RpcTarget target, params object[] parameters)
        {
            PhotonNetwork.RPC(this, methodName, target, false, parameters);
        }
        public void RpcSecure(string methodName, RpcTarget target, bool encrypt, params object[] parameters)
        {
            PhotonNetwork.RPC(this, methodName, target, encrypt, parameters);
        }
        public void RPC(string methodName, Player targetPlayer, params object[] parameters)
        {
            PhotonNetwork.RPC(this, methodName, targetPlayer, false, parameters);
        }
        public void RpcSecure(string methodName, Player targetPlayer, bool encrypt, params object[] parameters)
        {
            PhotonNetwork.RPC(this, methodName, targetPlayer, encrypt, parameters);
        }

        public static PhotonView Get(Component component)
        {
            return component.transform.GetParentComponent<PhotonView>();
        }

        public static PhotonView Get(GameObject gameObj)
        {
            return gameObj.transform.GetParentComponent<PhotonView>();
        }
        public static PhotonView Find(int viewID)
        {
            return PhotonNetwork.GetPhotonView(viewID);
        }

        
        #region Callback Interfaces


        private struct CallbackTargetChange
        {
            public IPhotonViewCallback obj;
            public Type type;
            public bool add;

            public CallbackTargetChange(IPhotonViewCallback obj, Type type, bool add)
            {
                this.obj = obj;
                this.type = type;
                this.add = add;
            }
        }

        private Queue<CallbackTargetChange> CallbackChangeQueue = new Queue<CallbackTargetChange>();

        private List<IOnPhotonViewPreNetDestroy> OnPreNetDestroyCallbacks;
        private List<IOnPhotonViewOwnerChange> OnOwnerChangeCallbacks;
        private List<IOnPhotonViewControllerChange> OnControllerChangeCallbacks;
        public void AddCallbackTarget(IPhotonViewCallback obj)
        {
            CallbackChangeQueue.Enqueue(new CallbackTargetChange(obj, null, true));
        }
        public void RemoveCallbackTarget(IPhotonViewCallback obj)
        {
            CallbackChangeQueue.Enqueue(new CallbackTargetChange(obj, null, false));
        }
        public void AddCallback<T>(IPhotonViewCallback obj) where T : class, IPhotonViewCallback
        {
            CallbackChangeQueue.Enqueue(new CallbackTargetChange(obj, typeof(T), true));
        }
        public void RemoveCallback<T>(IPhotonViewCallback obj) where T : class, IPhotonViewCallback
        {
            CallbackChangeQueue.Enqueue(new CallbackTargetChange(obj, typeof(T), false));
        }
        private void UpdateCallbackLists()
        {
            while (CallbackChangeQueue.Count > 0)
            {
                var item = CallbackChangeQueue.Dequeue();
                var obj = item.obj;
                var type = item.type;
                var add = item.add;

                if (type == null)
                {
                    TryRegisterCallback(obj, ref OnPreNetDestroyCallbacks, add);
                    TryRegisterCallback(obj, ref OnOwnerChangeCallbacks, add);
                    TryRegisterCallback(obj, ref OnControllerChangeCallbacks, add);
                }
                else if (type == typeof(IOnPhotonViewPreNetDestroy))
                    RegisterCallback(obj as IOnPhotonViewPreNetDestroy, ref OnPreNetDestroyCallbacks, add);

                else if (type == typeof(IOnPhotonViewOwnerChange))
                    RegisterCallback(obj as IOnPhotonViewOwnerChange, ref OnOwnerChangeCallbacks, add);

                else if (type == typeof(IOnPhotonViewControllerChange))
                    RegisterCallback(obj as IOnPhotonViewControllerChange, ref OnControllerChangeCallbacks, add);
            }
        }

        private void TryRegisterCallback<T>(IPhotonViewCallback obj, ref List<T> list, bool add) where T : class, IPhotonViewCallback
        {
            T iobj = obj as T;
            if (iobj != null)
            {
                RegisterCallback(iobj, ref list, add);
            }
        }

        private void RegisterCallback<T>(T obj, ref List<T> list, bool add) where T : class, IPhotonViewCallback
        {
            if (ReferenceEquals(list, null))
                list = new List<T>();

            if (add)
            {
                if (!list.Contains(obj))
                    list.Add(obj);
            }
            else
            {
                if (list.Contains(obj))
                    list.Remove(obj);
            }
        }


        #endregion Callback Interfaces


        public override string ToString()
        {
            return string.Format("View {0}{3} on {1} {2}", this.ViewID, (this.gameObject != null) ? this.gameObject.name : "GO==null", (this.IsRoomView) ? "(scene)" : string.Empty, this.Prefix > 0 ? "lvl" + this.Prefix : "");
        }
    }
}