using Colyseus.Schema;

namespace SH.Networking.Space
{
    public class RoomSpaceState : Schema
    {
        [Type(0, "map", typeof(MapSchema<RoomSpaceNetworkEntity>))]
        public MapSchema<RoomSpaceNetworkEntity> NetworkEntities = new MapSchema<RoomSpaceNetworkEntity>();

        [Type(1, "map", typeof(MapSchema<RoomSpaceAsteroidEntity>))]
        public MapSchema<RoomSpaceAsteroidEntity> Asteroids = new MapSchema<RoomSpaceAsteroidEntity>();
    }
}
