using Colyseus.Schema;

namespace SH.Networking.Station
{
    public class RoomStationState : Schema
    {
        [Type(0, "map", typeof(MapSchema<RoomStationNetworkEntity>))]
        public MapSchema<RoomStationNetworkEntity> NetworkEntities = new MapSchema<RoomStationNetworkEntity>();
    }
}
