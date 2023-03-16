using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SH
{
    [CreateAssetMenu(fileName = "ConfigMineral", menuName = "Game Data/Mineral/Config")]
    public class MineralConfig : ScriptableObject
    {
        [SerializeField] private List<MineralRecord> _records;

        public MineralRecord GetRecordById(string id)
        {
            return _records.FirstOrDefault((record) => record.Id == id);
        }

        public MineralRecord GetRecordByType(MineralType type)
        {
            return _records.FirstOrDefault(record => record.Type == type);
        }
    }

    [Serializable]
    public class MineralRecord
    {
        public string Id;
        public MineralType Type;
        public int Health;
        public string Description;
    }

    public enum MineralType
    {
        Lv1 = 1,
        Lv2 = 2,
        Lv3 = 3,
    }
}
