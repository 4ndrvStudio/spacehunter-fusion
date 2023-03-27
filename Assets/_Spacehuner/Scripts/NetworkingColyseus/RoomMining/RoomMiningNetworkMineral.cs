using Colyseus.Schema;

namespace SH.Networking.Mining
{
	public partial class RoomMiningNetworkMineral : Schema
	{
		[Type(0, "string")]
		public string Id = default(string);

		[Type(1, "number")]
		public float PosX = default(float);

		[Type(2, "number")]
		public float PosY = default(float);

		[Type(3, "number")]
		public float PosZ = default(float);

		[Type(4, "number")]
		public float CurrentHealth = default(float);

		[Type(5, "number")]
		public float TotalHealth = default(float);

		[Type(6, "number")]
		public float TimeDeath = default(float);

		[Type(7, "string")]
		public string Type = default(string);

		[Type(8, "boolean")]
		public bool Live = default(bool);
	}
}