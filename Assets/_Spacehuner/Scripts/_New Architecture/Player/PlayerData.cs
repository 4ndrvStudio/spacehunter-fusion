using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Services
{
    public interface IPlayer
	{
		string           UserID          { get; }
		string           Nickname        { get; }
		string			 UnityID         { get; }
	}

    public class PlayerData : IPlayer
    {
        public string           UserID          => _userID;
		public string           UnityID         { get => _unityID; set => _unityID = value; }
		public string           Nickname        { get { return _nickname; } set { _nickname = value; IsDirty = true; } }
		public string           AgentID         { get { return _agentID; } set { _agentID = value; IsDirty = true; } }
		public bool             IsDirty         { get; private set; }

		// PRIVATE MEMBERS

		[SerializeField]
		private string _userID;
		[SerializeField]
		private string _unityID;
		[SerializeField]
		private string _nickname;
		[SerializeField]
		private string _agentID;

		[SerializeField]
		private bool _isLocked;
		[SerializeField]
		private int _lastProcessID;
    }

}
