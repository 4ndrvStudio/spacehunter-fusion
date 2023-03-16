using Colyseus.Schema;

namespace SH.Networking.Space
{
	public class RoomSpaceNetworkEntity : Schema
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
		public float Speed = default;

		[Type(10, "number")]
		public float VelX = default;

		[Type(11, "number")]
		public float VelY = default;

		[Type(12, "number")]
		public float VelZ = default;

		[Type(13, "boolean")]
		public bool IsHost = default;

		public RoomSpaceNetworkEntity Clone(RoomSpaceNetworkEntity entity)
		{
			return new RoomSpaceNetworkEntity()
			{
				Id = entity.Id,
				Name = entity.Name,

				PosX = entity.PosX,
				PosY = entity.PosY,
				PosZ = entity.PosZ,

				RotX = entity.RotX,
				RotY = entity.RotY,
				RotZ = entity.RotZ,
				RotW = entity.RotW,

				Speed = entity.Speed,

				VelX = entity.VelX,
				VelY = entity.VelY,
				VelZ = entity.VelZ,
			};
		}
	}
}
