using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.ConfigData
{
    [System.Serializable]
    public class TraitMaterials
    {
        public Material[] Payload;
    }

    [System.Serializable]
    public class Trait
    {
        public TraitName Name;
        public List<Mesh> Meshes;
        public List<TraitMaterials> Materials;
    }

    [System.Serializable]
    public class SkinTraitList
    {
        public Trait Trait1;
        public Trait Trait2;
        public Trait Trait3;
        public Trait Trait4;
        public Trait Trait5;
        public Trait Trait6;
        public Trait Trait7;
        public Trait Trait8;
    }

    public enum CharacterName
    {
        None = 0,
        DisryMale = 1,
        HumesMale = 2,
        MabitMale = 3,
        MutasMale = 4,
        VasinMale = 5,
    }

    public enum TraitName
    {
        None,
        Hair,
        Body,
        Gloves,
        Glass,
        Weapon,
        Skin,
        Item,
        Face,
        Hat,
        Mask,
        Toy,
        Cloth,
        Head,
        Shoes,
        Eyes,
        Limbs,
        Horn,
        Facial
    }
    [CreateAssetMenu(fileName = "Skin Character Config", menuName = "Game Data/Character/Skin")]
    public class SkinCharacterConfig : ScriptableObject
    {
        public CharacterName CharacterName;

        public SkinTraitList TraitList = new SkinTraitList();
    }
}