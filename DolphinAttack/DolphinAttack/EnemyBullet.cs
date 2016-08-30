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
    public class EnemyBullet : Bullet
    {
        public EnemyBullet(ContentManager contentManager, int x, int y, int windowWidth, int windowHeight, int speed, int width, int height, string sprite)
            : base(contentManager, x, y, windowWidth, windowHeight, speed, width, height)
        {
            LoadContent(contentManager, sprite, width, height);
            this.Velocity = new Vector2(speed, 0);
        }

        public bool Update(Dolphin dolphin, GameTime gameTime)
        {
            if (Active)
            {
                this.updateLocation();
                if (this.drawRectangle.X < 0 || this.drawRectangle.X > this.windowWidth)
                {
                    Active = false;
                }

                if (this.drawRectangle.Intersects(dolphin.getRectangle()) && dolphin.Active && !dolphin.Shield)
                {
                    dolphin.Active = false;
                    Active = false;
                    return true;
                }
                
            }
            return false;
        }
    }
}
