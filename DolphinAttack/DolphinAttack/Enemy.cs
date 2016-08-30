// FILE: C:/Users/AF/Documents/Visual Studio 2013/Projects/DolphinAttack/DolphinAttack/DolphinAttack/dolphinAttack/Enemy.cs

// In this section you can add your own using directives
// section -64--88-43-39--10332b78:14cf1b7a08f:-8000:0000000000000B34 begin
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
// section -64--88-43-39--10332b78:14cf1b7a08f:-8000:0000000000000B34 end
namespace DolphinAttack
{

    /// <summary>
    ///  A class that represents ...
    /// 
    ///  @see OtherClasses
    ///  @author your_name_here
    /// </summary>
    public class Enemy : Object
    {
        // Associations

        /// <summary> 
        /// </summary>
        public Explosion explosion;

        List<EnemyBullet> bullets = new List<EnemyBullet>();

        public List<EnemyBullet> Bullets
        {
            get { return bullets; }
            set { bullets = value; }
        }
        int bulletCount = 1;
        float bulletSpawn = 0;

        public float BulletSpawn
        {
            get { return bulletSpawn; }
            set { bulletSpawn = value; }
        }

        // Operations

        /// <summary>
        ///  An operation that does...
        /// 
        ///  @param firstParam a description of this parameter
        /// </summary>
        /// <param name="content">
        /// </param>
        /// <param name="x">
        /// </param>
        /// <param name="y">
        /// </param>
        /// <param name="windowHeight">
        /// </param>
        /// <param name="speed">
        /// </param>
        /// <returns>
        /// </returns>
        public Enemy(ContentManager content, int x, int y, int windowWidth, int windowHeight, int speed)
            : base(content, x, y, windowWidth, windowHeight, speed)
        {

            this.Active = false;
            loadContent(content, "enemies", x, y);
            //explosion = new Explosion(content);
        }

        /// <summary>
        ///  An operation that does...
        /// 
        ///  @param firstParam a description of this parameter
        /// </summary>
        /// <param name="content">
        /// </param>
        /// <param name="spriteName">
        /// </param>
        /// <param name="x">
        /// </param>
        /// <param name="y">
        /// </param>
        /// <returns>
        /// </returns>
        public void loadContent(ContentManager content, string spriteName, int x, int y)
        {
            // section -64--88-43-39--10332b78:14cf1b7a08f:-8000:0000000000000B3E begin
            sprite = content.Load<Texture2D>(spriteName);
            drawRectangle = new Rectangle(x, y, 80, 60);

            // section -64--88-43-39--10332b78:14cf1b7a08f:-8000:0000000000000B3E end

        }

        public void createBullet(ContentManager content)
        {
                        for (int i = 0; i < bulletCount; i++)
            {
                bullets.Add(new EnemyBullet(content, 0, 0, windowWidth, windowHeight, -5, 15, 15, "bullet1"));
            }
        }
        /// <summary>
        ///  An operation that does...
        /// 
        ///  @param firstParam a description of this parameter
        /// </summary>
        /// <param name="ship">
        /// </param>
        /// <returns>
        /// </returns>
        public bool update(Dolphin dolphin, GameTime gameTime)
        {
            // section -64--88-43-39--10332b78:14cf1b7a08f:-8000:0000000000000B44 begin
            if (Active)
            {
                this.BulletSpawn += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (this.bulletSpawn >= 0.5)
                {
                    shot();
                    bulletSpawn = 0;
                }
                this.updateLocation();
                if (this.drawRectangle.X < 0)
                {
                    this.drawRectangle.X = windowWidth;
                }
                if (this.drawRectangle.Bottom >= this.windowHeight || this.drawRectangle.Top <= 0)
                {
                    this.invertY();
                }
                if (this.drawRectangle.Intersects(dolphin.getRectangle()) && dolphin.Active && !dolphin.Shield)
                {
                    //Rectangle collisionRectangle = Rectangle.Intersect(this.drawRectangle, dolphin.getRectangle());
                    //explosion.Play(collisionRectangle.Center.X, collisionRectangle.Center.Y);
                    dolphin.Active = false;
                    dolphin.makeBulletsInactive();
                    return true;
                }
            }
            return false;
            // section -64--88-43-39--10332b78:14cf1b7a08f:-8000:0000000000000B44 end

        }

        private EnemyBullet getInactiveBullet()
        {
            foreach (EnemyBullet bullet in bullets)
            {
                if (!bullet.Active)
                {
                    return bullet;
                }
            }
            return null;
        }
        public void shot()
        {
            EnemyBullet shot = getInactiveBullet();
            if (shot != null)
            {
                shot.Active = true;
                shot.setCoordinates(this.drawRectangle.Left, this.drawRectangle.Y);
            }
        }
        public List<EnemyBullet> getBullets()
        {
            return bullets;
        }
    } /* end class Enemy */

}
