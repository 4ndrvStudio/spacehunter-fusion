using System.Collections;
using System.Collections.Generic;

namespace SH.Define
{
    public static class SceneName
    {
        public static string SceneLogin = "scene_login";
        public static string SceneStation = "scene_station";
        public static string SceneMining = "scene_mining";
        public static string ScenePVE = "scene_pve";
        public static string SceneCreateCharacter = "scene_create_character";
        public static string SceneSpace = "scene_space";
    }

    public static class AnimationParam
    {
        public static string Vertical = "vertical";
        public static string Horizontal = "horizontal";
        public static string OnWeapon = "onWeapon";
        public static string CanMove = "canMove";
        public static string Interacting = "interacting";
        public static string LockOn = "lockOn";
        public static string OnGround = "onGround";
        public static string Run = "run";
        public static string Mining = "mining";

    }

    public static class MineralAction
    {
        public const string Action = "act";
        public const string Create = "create";
        public const string Update = "update";
        public const string Destroy = "destroy";
    }

    public enum CharacterType
    {
        // 0 is unlock slot but dont have character
        DisryMale = 1,
        HumesMale = 2,
        MabitMale = 3,
        MutasMale = 4,
        VasinMale = 5,
        DisryFemale = 6,
        HumesFemale = 7,
        MabitFemale = 8,
        MutasFemale = 9,
        VasinFemale = 10,
    }
    public enum SceneDefs : byte
    {
        scene_init = 0,
        scene_login = 1,
        scene_stationFusion = 2,
        scene_miningFusion = 3,
        scene_spaceFusion = 4,
    }

}

