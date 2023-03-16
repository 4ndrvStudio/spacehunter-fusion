//using Newtonsoft.Json;
using SH.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.PlayerData
{
    public class PlayerCharacter
    {
        public CharacterData Data { private set; get; }

        public void Setup(CharacterData data)
        {
            if (data != null)
            {
                this.Data = data;
            }
        }
    }

    public class CharacterData
    {
        public List<Character> Characters;
        public Character CharacterInUse;
    }

    public class Character
    {
        public string Id;
        public CharacterType CharacterType;
        public int Level;
        public int Exp;
        public SlotCharacterState State;
        public Properties Properties;
    }

    public class Properties {
    public int trait1;
    public int trait2;
    public int trait3;
    public int trait4;
    public int trait5;
    public int trait6;
    public int trait7;
    public int trait8;
} 

    public enum SlotCharacterState
    {
        Lock,
        Unlock,
    }
}
  
