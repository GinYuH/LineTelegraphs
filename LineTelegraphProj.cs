using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace LineTelegraphs
{
    public class ColorCache : ModSystem
    {
        public static Dictionary<int, Color> projColors = new Dictionary<int, Color>();
        public static Dictionary<int, Color> npcColors = new Dictionary<int, Color>();

        public override void PreUpdateWorld()
        {
            if (!LineConfig.Instance.allRedProj)
            if (projColors.Count <= 0)
                for (int i = 0; i < ContentSamples.ProjectilesByType.Count; i++)
                {
                    if (ContentSamples.ProjectilesByType.ContainsKey(i))
                    {
                        Projectile p = ContentSamples.ProjectilesByType[i];
                        if (p.friendly && !LineConfig.Instance.showFriendProj)
                            continue;
                        if (!p.hostile && !LineConfig.Instance.showFriendProj)
                            continue;
                        Main.instance.LoadProjectile(i);

                        Texture2D sprite = TextureAssets.Projectile[i].Value;
                        Color[,] colors = GetColorsFromTexture(sprite);
                        List<Color> colorList = new List<Color>();
                        for (int g = 0; g < colors.GetLength(0); g++)
                        {
                            for (int j = 0; j < colors.GetLength(1); j++)
                            {
                                Color thing = colors[g, j];
                                    if (colorList.Contains(thing))
                                        continue;
                                    if ((thing.R + thing.G + thing.B) > 255f)
                                {
                                    colorList.Add(thing);
                                }
                            }
                        }
                        Color final = Color.Yellow;
                        if (colorList.Count > 0)
                        {
                            colorList.Sort((x, y) => (x.R + x.G + x.B).CompareTo(y.R + y.G + y.B));
                            final = colorList[colorList.Count / 2];
                        }
                        projColors.Add(i, final);
                    }
                    }
            if (!LineConfig.Instance.allRedNPC)
                if (npcColors.Count <= 0)
                for (int i = 0; i < ContentSamples.NpcsByNetId.Count; i++)
                {
                    if (ContentSamples.NpcsByNetId.ContainsKey(i))
                    {
                        NPC p = ContentSamples.NpcsByNetId[i];
                        if (p.friendly || p.CountsAsACritter)
                            continue;
                        Main.instance.LoadNPC(i);

                        Texture2D sprite = TextureAssets.Npc[i].Value;
                        Color[,] colors = GetColorsFromTexture(sprite);
                        List<Color> colorList = new List<Color>();
                        for (int g = 0; g < colors.GetLength(0); g++)
                        {
                            for (int j = 0; j < colors.GetLength(1); j++)
                            {
                                Color thing = colors[g, j];
                                    if (colorList.Contains(thing))
                                        continue;
                                if ((thing.R + thing.G + thing.B) > 255f)
                                {
                                    colorList.Add(thing);
                                }
                            }
                        }
                        Color final = Color.Red;
                        if (colorList.Count > 0)
                        {
                            colorList.Sort((x, y) => (x.R + x.G + x.B).CompareTo(y.R + y.G + y.B));
                            final = colorList[colorList.Count / 2];
                        }
                        npcColors.Add(i, final);
                    }
                }
        }


        public static Color[,] GetColorsFromTexture(Texture2D texture)
        {
            Color[] alignedColors = new Color[texture.Width * texture.Height];
            texture.GetData(alignedColors); // Fills the color array with all the colors in the texture

            Color[,] colors2D = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    colors2D[x, y] = alignedColors[x + y * texture.Width];
                }
            }
            return colors2D;
        }
    }
	public class LineTelegraphProj : GlobalProjectile
	{

        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            if ((projectile.friendly || !projectile.hostile) && !LineConfig.Instance.showFriendProj)
                return true;
            if (LineConfig.Instance.showProj)
            {
                Color final = Color.Yellow;
                if (!LineConfig.Instance.allRedProj)
                    if (ColorCache.projColors.Count > 0)
                        if (ColorCache.projColors.ContainsKey(projectile.type))
                            if (projectile.velocity != Vector2.Zero)
                            {
                                final = ColorCache.projColors[projectile.type];
                            }
                float length = LineConfig.Instance.ProjLineLength;
                Vector2 endPoint = projectile.Center + projectile.velocity.SafeNormalize(Vector2.UnitY) * length;
                Utils.DrawLine(Main.spriteBatch, projectile.Center, endPoint, final, final * 0.4f, LineConfig.Instance.ProjLineWidth);
            }
            return true;
        }
    }

    public class LineTelegraphNPC : GlobalNPC
    {
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if ((npc.friendly || npc.CountsAsACritter) && !LineConfig.Instance.showFriendNPC)
                return true;
            if (npc.velocity != Vector2.Zero && LineConfig.Instance.showEnemy)
            {
                Color final = Color.Red;
                if (!LineConfig.Instance.allRedNPC)
                    if (ColorCache.npcColors.Count > 0)
                        if (ColorCache.npcColors.ContainsKey(npc.type))
                            if ((npc.damage > 0 || LineConfig.Instance.showFriendNPC))
                            {
                                final = ColorCache.npcColors[npc.type];
                            }
                float length = LineConfig.Instance.NPCLineLength;
                Vector2 endPoint = npc.Center + npc.velocity.SafeNormalize(Vector2.UnitY) * length;
                Utils.DrawLine(spriteBatch, npc.Center, endPoint, final, final * 0.4f, LineConfig.Instance.NPCLineWidth);
            }
            return true;
        }
    }
}
