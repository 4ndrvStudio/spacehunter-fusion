using Colyseus.Schema;

namespace SH.Networking.PVE
{
    public class RoomPVENetworkEnemy : Schema
    {
		[Type(0, "string")]
		public string Id = default;

		[Type(1, "string")]
		public string Name = default;

		[Type(2, "number")]
		public float PosX = default;

		[Type(3, "number")]
		public float PosY = default;

		[Type(4, "number")]
		public float PosZ = default;

		[Type(5, "number")]
		public float RotX = default;

		[Type(6, "number")]
		public float RotY = default;

		[Type(7, "number")]
		public float RotZ = default;

		[Type(8, "number")]
		public float RotW = default;

		[Type(9, "number")]
		public float Health = default;

		[Type(10, "number")]
		public float TimeDeath = default;

		[Type(11, "boolean")]
		public bool Live = default;

        [Type(12, "string")]
        public string Type = default;

        public RoomPVENetworkEnemy Clone(RoomPVENetworkEnemy a)
        {
			return new RoomPVENetworkEnemy()
			{
				Id = a.Id,
				Name = a.Name,
				PosX = a.PosX,
				PosY = a.PosY,
				PosZ = a.PosZ,
				RotX = a.RotX,
				RotY = a.RotY,
				RotZ = a.RotZ,
				RotW = a.RotW,
				Health = a.Health,
				TimeDeath = a.TimeDeath,
				Live = a.Live
				
			};
        }
	}
}
