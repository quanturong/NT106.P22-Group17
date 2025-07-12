
using System;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Photon.Pun.UtilityScripts
{
    [Obsolete("do not use this or add it to the scene. use PhotonTeamsManager instead")]
    public class PunTeams : MonoBehaviourPunCallbacks
    {
        [Obsolete("use custom PhotonTeam instead")]
        public enum Team : byte { none, red, blue };
        [Obsolete("use PhotonTeamsManager.Instance.TryGetTeamMembers instead")]
        public static Dictionary<Team, List<Player>> PlayersPerTeam;
        [Obsolete("do not use this. PhotonTeamsManager.TeamPlayerProp is used internally instead.")]
        public const string TeamPlayerProp = "team";


        #region Events by Unity and Photon

        public void Start()
        {
            PlayersPerTeam = new Dictionary<Team, List<Player>>();
            Array enumVals = Enum.GetValues(typeof(Team));
            foreach (var enumVal in enumVals)
            {
                PlayersPerTeam[(Team)enumVal] = new List<Player>();
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();
            this.Start();
        }
        public override void OnJoinedRoom()
        {

            this.UpdateTeams();
        }

        public override void OnLeftRoom()
        {
            Start();
        }
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            this.UpdateTeams();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            this.UpdateTeams();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            this.UpdateTeams();
        }

        #endregion

        [Obsolete("do not call this.")]
        public void UpdateTeams()
        {
            Array enumVals = Enum.GetValues(typeof(Team));
            foreach (var enumVal in enumVals)
            {
                PlayersPerTeam[(Team)enumVal].Clear();
            }

            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Player player = PhotonNetwork.PlayerList[i];
                Team playerTeam = player.GetTeam();
                PlayersPerTeam[playerTeam].Add(player);
            }
        }
    }
    public static class TeamExtensions
    {
        [Obsolete("Use player.GetPhotonTeam")]
        public static PunTeams.Team GetTeam(this Player player)
        {
            object teamId;
            if (player.CustomProperties.TryGetValue(PunTeams.TeamPlayerProp, out teamId))
            {
                return (PunTeams.Team)teamId;
            }

            return PunTeams.Team.none;
        }
        [Obsolete("Use player.JoinTeam")]
        public static void SetTeam(this Player player, PunTeams.Team team)
        {
            if (!PhotonNetwork.IsConnectedAndReady)
            {
                Debug.LogWarning("JoinTeam was called in state: " + PhotonNetwork.NetworkClientState + ". Not IsConnectedAndReady.");
                return;
            }

            PunTeams.Team currentTeam = player.GetTeam();
            if (currentTeam != team)
            {
                player.SetCustomProperties(new Hashtable() { { PunTeams.TeamPlayerProp, (byte)team } });
            }
        }
    }
}