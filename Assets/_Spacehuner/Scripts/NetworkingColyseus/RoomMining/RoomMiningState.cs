using Colyseus.Schema;
using SH.Networking.PVE;

namespace SH.Networking.Mining
{
	public partial class RoomMiningState : Schema
	{
		[Type(0, "map", typeof(MapSchema<RoomMiningNetworkEntity>))]
		public MapSchema<RoomMiningNetworkEntity> NetworkEntities = new MapSchema<RoomMiningNetworkEntity>();

		[Type(1, "map", typeof(MapSchema<RoomMiningNetworkMineral>))]
		public MapSchema<RoomMiningNetworkMineral> NetworkMinerals = new MapSchema<RoomMiningNetworkMineral>();

		[Type(2, "map", typeof(MapSchema<RoomPVENetworkEnemy>))]
		public MapSchema<RoomPVENetworkEnemy> NetworkEnemies = new MapSchema<RoomPVENetworkEnemy>();
	}
}


