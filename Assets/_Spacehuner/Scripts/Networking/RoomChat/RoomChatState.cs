using Colyseus.Schema;

namespace SH.Networking.Chat
{
    public class RoomChatState : Schema
    {
        [Type(0, "map", typeof(MapSchema<RoomChatNetworkEntity>))]
        public MapSchema<RoomChatNetworkEntity> NetworkEntities = new MapSchema<RoomChatNetworkEntity>();
    }
}
