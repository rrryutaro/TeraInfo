using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TeraInfo
{
    public static class TeraInfoUtils
    {
        public const int tileSize = 16;
        public const int maxTilesX = 8400;
        public const int maxTilesY = 2400;

        public static Vector2 Offset(this Vector2 position, float x, float y)
        {
            position.X += x;
            position.Y += y;
            return position;
        }

        public static Texture2D Resize(this Texture2D texture, int width, int height)
        {
            Texture2D result = texture;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                texture.SaveAsPng(ms, width, height);
                result = Texture2D.FromStream(texture.GraphicsDevice, ms);
            }
            return result;
        }
    }
}
