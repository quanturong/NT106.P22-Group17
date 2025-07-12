
using System;
using System.Collections.Generic;
using UnityEngine;

using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Photon.Pun.UtilityScripts
{
    [Serializable]
    public class PhotonTeam
    {
        public string Name;
        public byte Code;

        public override string ToString()
        {
            return string.Format("{0} [{1}]", this.Name, this.Code);
        }
    }
    [DisallowMultipleComponent]
    public class PhotonTeamsManager : MonoBehaviour, IMatchmakingCallbacks, IInRoomCallbacks
    {
        #if UNITY_EDITOR
        #pragma warning disable 0414
        [SerializeField]
        private bool listFoldIsOpen = true;
        #pragma warning restore 0414
        #endif

        [SerializeField]
        private List<PhotonTeam> teamsList = new List<PhotonTeam>
        {
            new PhotonTeam { Name = "Blue", Code = 1 },
            new PhotonTeam { Name = "Red", Code = 2 }
        };

        private Dictionary<byte, PhotonTeam> teamsByCode;
        private Dictionary<string, PhotonTeam> teamsByName;
        private Dictionary<byte, HashSet<Player>> playersPerTeam;
        public const string TeamPlayerProp = "_pt";

        public static event Action<Player, PhotonTeam> PlayerJoinedTeam;
        public static event Action<Player, PhotonTeam> PlayerLeftTeam;

        private static PhotonTeamsManager instance;
        public static PhotonTeamsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    
                    #if UNITY_6000_0_OR_NEWER
                    instance = FindFirstObjectByType<PhotonTeamsManager>();
                    #else
                    instance = FindObjectOfType<PhotonTeamsManager>();
                    #endif
                    if (instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = "PhotonTeamsManager";
                        instance = obj.AddComponent<PhotonTeamsManager>();
                    }
                    instance.Init();
                }

                return instance;
            }
        }

        #region MonoBehaviour

        private void Awake()
        {
            if (instance == null || ReferenceEquals(this, instance))
            {
                this.Init();
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
            this.ClearTeams();
        }

        private void Init()
        {
            teamsByCode = new Dictionary<byte, PhotonTeam>(teamsList.Count);
            teamsByName = new Dictionary<string, PhotonTeam>(teamsList.Count);
            playersPerTeam = new Dictionary<byte, HashSet<Player>>(teamsList.Count);
            for (int i = 0; i < teamsList.Count; i++)
            {
                teamsByCode[teamsList[i].Code] = teamsList[i];
                teamsByName[teamsList[i].Name] = teamsList[i];
                playersPerTeam[teamsList[i].Code] = new HashSet<Player>();
            }
        }

        #endregion

        #region IMatchmakingCallbacks

        void IMatchmakingCallbacks.OnJoinedRoom()
        {
            this.UpdateTeams();
        }

        void IMatchmakingCallbacks.OnLeftRoom()
        {
            this.ClearTeams();
        }

        void IInRoomCallbacks.OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            object temp;
            if (changedProps.TryGetValue(TeamPlayerProp, out temp))
            {
                if (temp == null)
                {
                    foreach (byte code in playersPerTeam.Keys)
                    {
                        if (playersPerTeam[code].Remove(targetPlayer))
                        {
                            if (PlayerLeftTeam != null)
                            {
                                PlayerLeftTeam(targetPlayer, teamsByCode[code]);
                            }
                            break;
                        }
                    }
                } 
                else if (temp is byte)
                {
                    byte teamCode = (byte) temp;
                    foreach (byte code in playersPerTeam.Keys)
                    {
                        if (code == teamCode)
                        {
                            continue;
                        }
                        if (playersPerTeam[code].Remove(targetPlayer))
                        {
                            if (PlayerLeftTeam != null)
                            {
                                PlayerLeftTeam(targetPlayer, teamsByCode[code]);
                            }
                            break;
                        }
                    }
                    PhotonTeam team = teamsByCode[teamCode];
                    if (!playersPerTeam[teamCode].Add(targetPlayer))
                    {
                        Debug.LogWarningFormat("Unexpected situation while setting team {0} for player {1}, updating teams for all", team, targetPlayer);
                        this.UpdateTeams();
                    }
                    if (PlayerJoinedTeam != null)
                    {
                        PlayerJoinedTeam(targetPlayer, team);
                    }
                }
                else
                {
                    Debug.LogErrorFormat("Unexpected: custom property key {0} should have of type byte, instead we got {1} of type {2}. Player: {3}", 
                        TeamPlayerProp, temp, temp.GetType(), targetPlayer);
                }
            }
        }

        void IInRoomCallbacks.OnPlayerLeftRoom(Player otherPlayer)
        {
            if (otherPlayer.IsInactive)
            {
                return;
            }
            PhotonTeam team = otherPlayer.GetPhotonTeam();
            if (team != null && !playersPerTeam[team.Code].Remove(otherPlayer))
            {
                Debug.LogWarningFormat("Unexpected situation while removing player {0} who left from team {1}, updating teams for all", otherPlayer, team);
                this.UpdateTeams();
            }
        }

        void IInRoomCallbacks.OnPlayerEnteredRoom(Player newPlayer)
        {
            PhotonTeam team = newPlayer.GetPhotonTeam();
            if (team == null)
            {
                return;
            }
            if (playersPerTeam[team.Code].Contains(newPlayer))
            {
                return;
            }
            foreach (var key in teamsByCode.Keys)
            {
                if (playersPerTeam[key].Remove(newPlayer))
                {
                    break;
                }
            }
            if (!playersPerTeam[team.Code].Add(newPlayer))
            {
                Debug.LogWarningFormat("Unexpected situation while adding player {0} who joined to team {1}, updating teams for all", newPlayer, team);
                this.UpdateTeams();
            }
        }

        #endregion

        #region Private methods

        private void UpdateTeams()
        {
            this.ClearTeams();
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Player player = PhotonNetwork.PlayerList[i];
                PhotonTeam playerTeam = player.GetPhotonTeam();
                if (playerTeam != null)
                {
                    playersPerTeam[playerTeam.Code].Add(player);
                }
            }
        }

        private void ClearTeams()
        {
            foreach (var key in playersPerTeam.Keys)
            {
                playersPerTeam[key].Clear();
            }
        }

        #endregion

        #region Public API
        public bool TryGetTeamByCode(byte code, out PhotonTeam team)
        {
            return teamsByCode.TryGetValue(code, out team);
        }
        public bool TryGetTeamByName(string teamName, out PhotonTeam team)
        {
            return teamsByName.TryGetValue(teamName, out team);
        }
        public PhotonTeam[] GetAvailableTeams()
        {
            if (teamsList != null)
            {
                return teamsList.ToArray();
            }
            return null;
        }
        public bool TryGetTeamMembers(byte code, out Player[] members)
        {
            members = null;
            HashSet<Player> players;
            if (this.playersPerTeam.TryGetValue(code, out players))
            {
                members = new Player[players.Count];
                int i = 0;
                foreach (var player in players)
                {
                    members[i] = player;
                    i++;
                }
                return true;
            }
            return false;
        }
        public bool TryGetTeamMembers(string teamName, out Player[] members)
        {
            members = null;
            PhotonTeam team;
            if (this.TryGetTeamByName(teamName, out team))
            {
                return this.TryGetTeamMembers(team.Code, out members);
            }
            return false;
        }
        public bool TryGetTeamMembers(PhotonTeam team, out Player[] members)
        {
            members = null;
            if (team != null)
            {
                return this.TryGetTeamMembers(team.Code, out members);
            }
            return false;
        }
        public bool TryGetTeamMatesOfPlayer(Player player, out Player[] teamMates)
        {
            teamMates = null;
            if (player == null)
            {
                return false;
            }
            PhotonTeam team = player.GetPhotonTeam();
            if (team == null)
            {
                return false;
            }
            HashSet<Player> players;
            if (this.playersPerTeam.TryGetValue(team.Code, out players))
            {
                if (!players.Contains(player))
                {
                    Debug.LogWarningFormat("Unexpected situation while getting team mates of player {0} who is joined to team {1}, updating teams for all", player, team);
                    this.UpdateTeams();
                }
                teamMates = new Player[players.Count - 1];
                int i = 0;
                foreach (var p in players)
                {
                    if (p.Equals(player))
                    {
                        continue;
                    }
                    teamMates[i] = p;
                    i++;
                }
                return true;
            }
            return false;
        }
        public int GetTeamMembersCount(byte code)
        {
            PhotonTeam team;
            if (this.TryGetTeamByCode(code, out team))
            {
                return this.GetTeamMembersCount(team);
            }
            return 0;
        }
        public int GetTeamMembersCount(string name)
        {
            PhotonTeam team;
            if (this.TryGetTeamByName(name, out team))
            {
                return this.GetTeamMembersCount(team);
            }
            return 0;
        }
        public int GetTeamMembersCount(PhotonTeam team)
        {
            HashSet<Player> players;
            if (team != null && this.playersPerTeam.TryGetValue(team.Code, out players) && players != null)
            {
                return players.Count;
            }
            return 0;
        }

        #endregion

        #region Unused methods

        void IMatchmakingCallbacks.OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }

        void IMatchmakingCallbacks.OnCreatedRoom()
        {
        }

        void IMatchmakingCallbacks.OnCreateRoomFailed(short returnCode, string message)
        {
        }

        void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message)
        {
        }

        void IMatchmakingCallbacks.OnJoinRandomFailed(short returnCode, string message)
        {
        }

        void IInRoomCallbacks.OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
        }

        void IInRoomCallbacks.OnMasterClientSwitched(Player newMasterClient)
        {
        }

        #endregion
    }
    public static class PhotonTeamExtensions
    {
        public static PhotonTeam GetPhotonTeam(this Player player)
        {
            object teamId;
            PhotonTeam team;
            if (player.CustomProperties.TryGetValue(PhotonTeamsManager.TeamPlayerProp, out teamId) && PhotonTeamsManager.Instance.TryGetTeamByCode((byte)teamId, out team))
            {
                return team;
            }
            return null;
        }
        public static bool JoinTeam(this Player player, PhotonTeam team)
        {
            if (team == null)
            {
                Debug.LogWarning("JoinTeam failed: PhotonTeam provided is null");
                return false;
            }
            PhotonTeam currentTeam = player.GetPhotonTeam();
            if (currentTeam != null)
            {
                Debug.LogWarningFormat("JoinTeam failed: player ({0}) is already joined to a team ({1}), call SwitchTeam instead", player, team);
                return false;
            }
            return player.SetCustomProperties(new Hashtable { { PhotonTeamsManager.TeamPlayerProp, team.Code } });
        }
        public static bool JoinTeam(this Player player, byte teamCode)
        {
            PhotonTeam team;
            return PhotonTeamsManager.Instance.TryGetTeamByCode(teamCode, out team) && player.JoinTeam(team);
        }
        public static bool JoinTeam(this Player player, string teamName)
        {
            PhotonTeam team;
            return PhotonTeamsManager.Instance.TryGetTeamByName(teamName, out team) && player.JoinTeam(team);
        }
        public static bool SwitchTeam(this Player player, PhotonTeam team)
        {
            if (team == null)
            {
                Debug.LogWarning("SwitchTeam failed: PhotonTeam provided is null");
                return false;
            }
            PhotonTeam currentTeam = player.GetPhotonTeam();
            if (currentTeam == null)
            {
                Debug.LogWarningFormat("SwitchTeam failed: player ({0}) was not joined to any team, call JoinTeam instead", player);
                return false;
            }
            if (currentTeam.Code == team.Code)
            {
                Debug.LogWarningFormat("SwitchTeam failed: player ({0}) is already joined to the same team {1}", player, team);
                return false;
            }
            return player.SetCustomProperties(new Hashtable { { PhotonTeamsManager.TeamPlayerProp, team.Code } },
                new Hashtable { { PhotonTeamsManager.TeamPlayerProp, currentTeam.Code }});
        }
        public static bool SwitchTeam(this Player player, byte teamCode)
        {
            PhotonTeam team;
            return PhotonTeamsManager.Instance.TryGetTeamByCode(teamCode, out team) && player.SwitchTeam(team);
        }
        public static bool SwitchTeam(this Player player, string teamName)
        {
            PhotonTeam team;
            return PhotonTeamsManager.Instance.TryGetTeamByName(teamName, out team) && player.SwitchTeam(team);
        }
        public static bool LeaveCurrentTeam(this Player player)
        {
            PhotonTeam currentTeam = player.GetPhotonTeam();
            if (currentTeam == null)
            {
                Debug.LogWarningFormat("LeaveCurrentTeam failed: player ({0}) was not joined to any team", player);
                return false;
            }
            return player.SetCustomProperties(new Hashtable {{PhotonTeamsManager.TeamPlayerProp, null}}, new Hashtable {{PhotonTeamsManager.TeamPlayerProp, currentTeam.Code}});
        }
        public static bool TryGetTeamMates(this Player player, out Player[] teamMates)
        {
            return PhotonTeamsManager.Instance.TryGetTeamMatesOfPlayer(player, out teamMates);
        }
    }
}