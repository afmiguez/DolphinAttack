using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DolphinAttack
{
    public class ScrollingBackground
    {
        // class ScrollingBackground
        private Vector2 screenpos, origin, texturesize;
        private Texture2D mytexture;
        private int screenwidth;
        public void Load(GraphicsDevice device, Texture2D backgroundTexture)
        {
            mytexture = backgroundTexture;
            screenwidth = device.Viewport.Width;
            int screenheight = device.Viewport.Height;
            // Set the origin so that we're drawing from the 
            // center of the top edge.
            origin = new Vector2(0, mytexture.Height / 2);
            // Set the screen position to the center of the screen.
            screenpos = new Vector2(0, screenheight / 2);
            // Offset to draw the second texture, when necessary.
            texturesize = new Vector2(mytexture.Width, 0);
        }
        // ScrollingBackground.Update
        public void Update(float deltaY)
        {
            screenpos.X -= deltaY;
            screenpos.X = screenpos.X % mytexture.Width;
        }
        // ScrollingBackground.Draw
        public void Draw(SpriteBatch batch)
        {
            // Draw the texture, if it is still onscreen.
            if (screenpos.X < screenwidth)
            {
                batch.Draw(mytexture, screenpos, null,
                     Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            }
            // Draw the texture a second time, behind the first,
            // to create the scrolling illusion.
            batch.Draw(mytexture, screenpos + texturesize, null, Color.White, 0, origin, 1, SpriteEffects.None, 0f);
        }
    }
}
