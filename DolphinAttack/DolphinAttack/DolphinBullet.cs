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
    public class DolphinBullet : Bullet
    {
        public DolphinBullet(ContentManager contentManager, int x, int y, int windowWidth, int windowHeight, int speed, int width, int height)
            : base(contentManager, x, y, windowWidth, windowHeight, speed, 0, 0)
        {

            Active = false;
            LoadContent(contentManager, "bulletbeam", width, height);
            //this.Speed = speed;
            this.Velocity = new Vector2(speed, 0);
        }

        public bool update(List<Enemy> enemies, GameTime gameTime, Boss boss)
        {
            if (Active)
            {
                if (this.drawRectangle.X < 0 || this.drawRectangle.X > this.windowWidth)
                {
                    Active = false;
                }
                this.updateLocation();
                foreach (Enemy enemy in enemies)
                {

                    if (this.drawRectangle.Intersects(enemy.getRectangle()) && enemy.Active)
                    {
                        enemy.Active = false;
                        Active = false;
                        return true;
                    }
                }
                if (this.drawRectangle.Intersects(boss.getRectangle())&&boss.Active)
                {

                    boss.Life--;
                    this.Active = false;
                    Rectangle collisionRectangle = Rectangle.Intersect(this.drawRectangle, boss.getRectangle());
                    return true;

                }

            }
            return false;
        }
    }
}
