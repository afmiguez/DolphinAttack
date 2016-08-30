using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DolphinAttack
{
    public class SuperBullet : Bullet
    {
        public SuperBullet(ContentManager contentManager, int x, int y, int windowWidth, int windowHeight, int speed, int width, int height)
            : base(contentManager, x, y, windowWidth, windowHeight, speed, width, height)
        {
            Active = false;
            LoadContent(contentManager, "hadouken1", width, height);
            this.Velocity = new Vector2(speed, 0);
        }

        public bool update(List<Enemy> enemies, Boss boss, GameTime gameTime)
        {
            if (this.Active)
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
                        return true;
                    }
                }
                if (this.drawRectangle.Intersects(boss.getRectangle())&&boss.Active)
                {

                    boss.Life -= 2;
                    this.Active = false;
                    return true;

                }

            }
            return false;
        }
        protected void LoadContent(ContentManager contentManager, string spriteName, int width, int height)
        {
            // load content and set remainder of draw rectangle
            sprite = contentManager.Load<Texture2D>(spriteName);
            drawRectangle = new Rectangle(0, 0, width, height);

        }
    }
}
