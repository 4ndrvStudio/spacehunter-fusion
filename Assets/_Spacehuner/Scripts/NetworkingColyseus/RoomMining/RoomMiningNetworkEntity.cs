using Colyseus.Schema;

namespace SH.Networking.Mining
{
	public partial class RoomMiningNetworkEntity : Schema
	{
		[Type(0, "string")]
		public string Id = default(string);

		[Type(1, "string")]
		public string Name = default(string);

		[Type(2, "number")]
		public float PosX = default(float);

		[Type(3, "number")]
		public float PosY = default(float);

		[Type(4, "number")]
		public float PosZ = default(float);

		[Type(5, "number")]
		public float RotX = default(float);

		[Type(6, "number")]
		public float RotY = default(float);

		[Type(7, "number")]
		public float RotZ = default(float);

		[Type(8, "number")]
		public float RotW = default(float);

		[Type(9, "number")]
		public float Speed = default(float);

		[Type(10, "number")]
		public float VelX = default(float);

		[Type(11, "number")]
		public float VelY = default(float);

		[Type(12, "number")]
		public float VelZ = default(float);

		[Type(13, "boolean")]
		public bool IsHost = default;

		[Type(14, "string")]
		public string IdCharacter = default;

		[Type(15, "map", typeof(MapSchema<string>), "string")]
		public MapSchema<string> Attributes = new MapSchema<string>();

		public RoomMiningNetworkEntity Clone(RoomMiningNetworkEntity entity)
		{
			return new RoomMiningNetworkEntity()
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
