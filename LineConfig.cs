using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace LineTelegraphs
{
    public class LineConfig : ModConfig
    {
        public static LineConfig Instance;
        public override ConfigScope Mode => ConfigScope.ClientSide;

        /// NPCs
        [Header("$Mods.LineTelegraphs.Configs.NPCHeader")]

        [DefaultValue(true)]
        public bool showEnemy { get; set; }


        [DefaultValue(false)]
        public bool showFriendNPC { get; set; }


        [DefaultValue(false)]
        public bool allRedNPC { get; set; }


        [DefaultValue(2600)]
        [Range(100, 10000)]
        public int NPCLineLength { get; set; }


        [DefaultValue(4)]
        [Range(1, 100)]
        public int NPCLineWidth { get; set; }


        /// Projectiles
        [Header("$Mods.LineTelegraphs.Configs.ProjectileHeader")]

        [DefaultValue(true)]
        public bool showProj { get; set; }


        [DefaultValue(false)]
        public bool showFriendProj { get; set; }


        [DefaultValue(false)]
        public bool allRedProj { get; set; }


        [DefaultValue(2600)]
        [Range(100, 10000)]
        public int ProjLineLength { get; set; }


        [DefaultValue(4)]
        [Range(1, 100)]
        public int ProjLineWidth { get; set; }

    }
}