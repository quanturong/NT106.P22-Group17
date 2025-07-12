
using System;
using System.Collections.Generic;

using UnityEngine;

using Photon.Realtime;

using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Photon.Pun.UtilityScripts
{
	public class PunTurnManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        Player sender;
        public int Turn
        {
            get { return PhotonNetwork.CurrentRoom.GetTurn(); }
            private set
            {

                _isOverCallProcessed = false;

                PhotonNetwork.CurrentRoom.SetTurn(value, true);
            }
        }
        public float TurnDuration = 20f;
        public float ElapsedTimeInTurn
        {
            get { return ((float) (PhotonNetwork.ServerTimestamp - PhotonNetwork.CurrentRoom.GetTurnStart())) / 1000.0f; }
        }
        public float RemainingSecondsInTurn
        {
            get { return Mathf.Max(0f, this.TurnDuration - this.ElapsedTimeInTurn); }
        }
        public bool IsCompletedByAll
        {
            get { return PhotonNetwork.CurrentRoom != null && Turn > 0 && this.finishedPlayers.Count == PhotonNetwork.CurrentRoom.PlayerCount; }
        }
        public bool IsFinishedByMe
        {
            get { return this.finishedPlayers.Contains(PhotonNetwork.LocalPlayer); }
        }
        public bool IsOver
        {
            get { return this.RemainingSecondsInTurn <= 0f; }
        }
        public IPunTurnManagerCallbacks TurnManagerListener;
        private readonly HashSet<Player> finishedPlayers = new HashSet<Player>();
        public const byte TurnManagerEventOffset = 0;
        public const byte EvMove = 1 + TurnManagerEventOffset;
        public const byte EvFinalMove = 2 + TurnManagerEventOffset;
        private bool _isOverCallProcessed = false;

        #region MonoBehaviour CallBack


        void Start(){}

        void Update()
        {
            if (Turn > 0 && this.IsOver && !_isOverCallProcessed)
            {
                _isOverCallProcessed = true;
                this.TurnManagerListener.OnTurnTimeEnds(this.Turn);
            }

        }

        #endregion
        public void BeginTurn()
        {
            Turn = this.Turn + 1; // note: this will set a property in the room, which is available to the other players.
        }
        public void SendMove(object move, bool finished)
        {
            if (IsFinishedByMe)
            {
                UnityEngine.Debug.LogWarning("Can't SendMove. Turn is finished by this player.");
                return;
            }
            Hashtable moveHt = new Hashtable();
            moveHt.Add("turn", Turn);
            moveHt.Add("move", move);

            byte evCode = (finished) ? EvFinalMove : EvMove;
            PhotonNetwork.RaiseEvent(evCode, moveHt, new RaiseEventOptions() {CachingOption = EventCaching.AddToRoomCache}, SendOptions.SendReliable);
            if (finished)
            {
                PhotonNetwork.LocalPlayer.SetFinishedTurn(Turn);
            }
			ProcessOnEvent(evCode, moveHt, PhotonNetwork.LocalPlayer.ActorNumber);
        }
        public bool GetPlayerFinishedTurn(Player player)
        {
            if (player != null && this.finishedPlayers != null && this.finishedPlayers.Contains(player))
            {
                return true;
            }

            return false;
        }

        #region Callbacks
		void ProcessOnEvent(byte eventCode, object content, int senderId)
		{
            if (senderId == -1)
            {
                return;
            }
            
            sender = PhotonNetwork.CurrentRoom.GetPlayer(senderId);
            
            switch (eventCode)
			{
			case EvMove:
				{
					Hashtable evTable = content as Hashtable;
					int turn = (int)evTable["turn"];
					object move = evTable["move"];
					this.TurnManagerListener.OnPlayerMove(sender, turn, move);

					break;
				}
			case EvFinalMove:
				{
					Hashtable evTable = content as Hashtable;
					int turn = (int)evTable["turn"];
					object move = evTable["move"];

					if (turn == this.Turn)
					{
						this.finishedPlayers.Add(sender);

						this.TurnManagerListener.OnPlayerFinished(sender, turn, move);

					}

					if (IsCompletedByAll)
					{
						this.TurnManagerListener.OnTurnCompleted(this.Turn);
					}
					break;
				}
			}
		}
		public void OnEvent(EventData photonEvent)
        {
			this.ProcessOnEvent(photonEvent.Code, photonEvent.CustomData, photonEvent.Sender);
        }
        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {

            if (propertiesThatChanged.ContainsKey("Turn"))
            {
                _isOverCallProcessed = false;
                this.finishedPlayers.Clear();
                this.TurnManagerListener.OnTurnBegins(this.Turn);
            }
        }

        #endregion
    }


    public interface IPunTurnManagerCallbacks
    {
        void OnTurnBegins(int turn);
        void OnTurnCompleted(int turn);
        void OnPlayerMove(Player player, int turn, object move);
        void OnPlayerFinished(Player player, int turn, object move);
        void OnTurnTimeEnds(int turn);
    }


    public static class TurnExtensions
    {
        public static readonly string TurnPropKey = "Turn";
        public static readonly string TurnStartPropKey = "TStart";
        public static readonly string FinishedTurnPropKey = "FToA";
        public static void SetTurn(this Room room, int turn, bool setStartTime = false)
        {
            if (room == null || room.CustomProperties == null)
            {
                return;
            }

            Hashtable turnProps = new Hashtable();
            turnProps[TurnPropKey] = turn;
            if (setStartTime)
            {
                turnProps[TurnStartPropKey] = PhotonNetwork.ServerTimestamp;
            }

            room.SetCustomProperties(turnProps);
        }
        public static int GetTurn(this RoomInfo room)
        {
            if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(TurnPropKey))
            {
                return 0;
            }

            return (int) room.CustomProperties[TurnPropKey];
        }
        public static int GetTurnStart(this RoomInfo room)
        {
            if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(TurnStartPropKey))
            {
                return 0;
            }

            return (int) room.CustomProperties[TurnStartPropKey];
        }
        public static int GetFinishedTurn(this Player player)
        {
            Room room = PhotonNetwork.CurrentRoom;
            if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(TurnPropKey))
            {
                return 0;
            }

            string propKey = FinishedTurnPropKey + player.ActorNumber;
            return (int) room.CustomProperties[propKey];
        }
        public static void SetFinishedTurn(this Player player, int turn)
        {
            Room room = PhotonNetwork.CurrentRoom;
            if (room == null || room.CustomProperties == null)
            {
                return;
            }

            string propKey = FinishedTurnPropKey + player.ActorNumber;
            Hashtable finishedTurnProp = new Hashtable();
            finishedTurnProp[propKey] = turn;

            room.SetCustomProperties(finishedTurnProp);
        }
    }
}