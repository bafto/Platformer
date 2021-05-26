using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Platformer.src
{
    public class ContentManager
    {
        /// <summary>
        /// Loads texture from file
        /// </summary>
        /// <param name="path">absolute file path</param>
        /// <returns>the loaded Texture</returns>
        public static Texture2D LoadTexture(string path)
        {
            return Main.instance.Content.Load<Texture2D>(path);
        }

        /// <summary>
        /// Loads a part of a Texture from path defined by a sourceRectangle
        /// </summary>
        /// <param name="path">absolute file path</param>
        /// <param name="srcRect">source Rectangle which defines what Part of the Texture is loaded</param>
        /// <returns>A Texture2D containing the specified part</returns>
        public static Texture2D LoadTexturePart(string path, Rectangle srcRect)
        {
            Texture2D wholeTex = Main.instance.Content.Load<Texture2D>(path);
            Texture2D returnTex = new Texture2D(Main.instance.GraphicsDevice, srcRect.Width, srcRect.Height);
            Color[] data = new Color[srcRect.Width * srcRect.Height];
            wholeTex.GetData(0, srcRect, data, 0, data.Length);
            returnTex.SetData(data);
            return returnTex;
        }

        public static SoundEffect LoadSoundEffect(string file)
        {
            return Main.instance.Content.Load<SoundEffect>(file);
        }

        public static Song LoadSong(string file)
        {
            return Main.instance.Content.Load<Song>(file);
        }
    }
}
