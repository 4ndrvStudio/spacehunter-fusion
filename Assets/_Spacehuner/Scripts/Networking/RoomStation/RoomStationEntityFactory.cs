using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Networking.Station
{
    public class RoomStationEntityFactory
    {
        private Dictionary<string, RoomStationNetworkEntity> _entities;
        private Dictionary<string, RoomStationNetworkEntityView> _entityViews;

        public RoomStationEntityFactory(Dictionary<string, RoomStationNetworkEntity> entities, Dictionary<string, RoomStationNetworkEntityView> entityViews)
        {
            this._entities = entities;
            this._entityViews = entityViews;
        }

        public void RegisterNetworkEntityView(RoomStationNetworkEntity entity, RoomStationNetworkEntityView view, bool isMine)
        {
            if (entity == null || view == null)
            {
                Debug.LogError("entity null");
                return;
            }
            if(!_entities.ContainsKey(entity.Id))
            {
                Debug.LogError("Can not find entity in room");
                return;
            }

            view.InitView(entity, isMine);
            _entityViews.Add(entity.Id, view);

        }
    }
}
