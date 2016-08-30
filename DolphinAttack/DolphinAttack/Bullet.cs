using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DolphinAttack
{
    public class Bullet : Object
    {
        public Bullet(ContentManager contentManager, int x, int y, int windowWidth, int windowHeight, int speed, int width, int height)
            : base(contentManager, x, y, windowWidth, windowHeight, speed)
        {
            Active = false;
            LoadContent(contentManager, "bullet", width, height);
        }
        protected void LoadContent(ContentManager contentManager, string spriteName, int width, int height)
        {
            // load content and set remainder of draw rectangle
            sprite = contentManager.Load<Texture2D>(spriteName);
            drawRectangle = new Rectangle(0, 0, width, height);
        }
    }
}
