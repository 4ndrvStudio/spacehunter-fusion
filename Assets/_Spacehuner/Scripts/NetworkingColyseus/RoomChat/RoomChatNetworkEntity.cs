using Colyseus.Schema;

namespace SH.Networking.Chat
{
	public class RoomChatNetworkEntity : Schema
	{
		[Type(0, "string")]
		public string Id = default;
	}
}
