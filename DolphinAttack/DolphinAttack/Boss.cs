using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DolphinAttack
{
    public class Boss : Enemy
    {
        private const int maxLife = 100;
        private int life = maxLife;

        Texture2D healthTexture;
        Rectangle healthRectangle;

        public int Life
        {
            get { return life; }
            set { life = value; }
        }

        int bulletCount = 3;

        private Vector2 originalVelocity = new Vector2();
        private float chargeDistance = 0;

        private bool chargeAttackFlag = false;
        float chargeAttackTimer = 0;

        private bool returnFlag = false;

        protected SpriteFont debug;
        float angleDebug;
        Vector2 deltaDebug;

        SoundEffect laugh;
        bool laughFlag = false;

        int originalX;
        int originalY;


        //Boss constructor

        public Boss(ContentManager content, int x, int y, int windowWidth, int windowHeight, int speed)
            : base(content, x, y, windowWidth, windowHeight, speed)
        {

            this.Active = false;
            this.setVelocity(0, speed);
            bossLoadContent(content, "drake", x, y);
            explosion = new Explosion(content);
        }



        public void bossLoadContent(ContentManager content, string spriteName, int x, int y)
        {
            sprite = content.Load<Texture2D>(spriteName);
            drawRectangle = new Rectangle(x, y, 300, 250);
            for (int i = 0; i < bulletCount; i++)
            {
                Bullets.Add(new EnemyBullet(content, 0, 0, windowWidth, windowHeight, -7, 40, 35, "ballbullet"));
            }

            healthTexture = content.Load<Texture2D>("Health");
            this.debug = content.Load<SpriteFont>("myFont");
            this.laugh = content.Load<SoundEffect>("laugh");

        }

        public int remainingLife()
        {
            return maxLife - this.Life;
        }

        public Vector2 delta(int x, int y)
        {
            return new Vector2((this.drawRectangle.Center.X - x), (this.drawRectangle.Center.Y - y));
        }

        private float calcDistance(Vector2 delta)
        {
            return (float)Math.Sqrt((Math.Pow(delta.X, 2)) + (Math.Pow(delta.Y, 2)));
        }
        private float getAngle(Vector2 delta)
        {

            if (delta.X != 0)
            {
                float theta = (float)Math.Abs(Math.Atan(delta.Y / delta.X));
                if (delta.X > 0 && delta.Y < 0)
                {

                    return (float)Math.PI - theta;
                }
                if (delta.X > 0 && delta.Y > 0)
                {
                    return (float)Math.PI + theta;
                }
                if (delta.X < 0 && delta.Y < 0)
                {

                    return (float)(2 * Math.PI - theta);
                }
                return theta;
            }
            if (delta.Y > 0)
            {
                return (float)(Math.PI / 2);
            }
            if (delta.Y < 0)
            {
                return (float)(3 * Math.PI / 2);
            }

            return 0;
        }

        private void swapVelocity()
        {
            Vector2 aux = this.Velocity;
            this.Velocity = this.originalVelocity;
            this.originalVelocity = aux;
        }

        private void setupCharge(int x, int y)
        {
            Vector2 deltaDistance = delta(x, y);
            this.deltaDebug = deltaDistance;
            this.chargeDistance = calcDistance(deltaDistance);
            float theta = getAngle(deltaDistance);
            this.angleDebug = theta;
            this.originalVelocity.X = (float)Math.Cos(theta) * 10;
            this.originalVelocity.Y = (float)Math.Sin(theta) * 10;
            swapVelocity();
        }

        public bool Update(Dolphin dolphin, GameTime gameTime)
        {

            if (this.Active)
            {
                
                healthRectangle = new Rectangle(this.windowWidth / 4, 0, (500 * this.Life) / maxLife, 25);

                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (!dolphin.Active&&!this.laughFlag)
                {
                    laughFlag = true;
                    laugh.Play();
                }
                if (dolphin.Active)
                {
                    laughFlag = false;
                }

                this.BulletSpawn += elapsed;
                chargeAttackTimer += elapsed;
                if (this.BulletSpawn >= 0.5)
                {
                    shot();
                    BulletSpawn = 0;
                }

                if (chargeAttackTimer >= 5)
                {
                    if (!this.chargeAttackFlag && !this.returnFlag)
                    {
                        this.originalX = this.drawRectangle.Center.X;
                        this.originalY = this.drawRectangle.Center.Y;
                        setupCharge(dolphin.getRectangle().Left, dolphin.getRectangle().Center.Y);
                        this.chargeAttackFlag = true;

                    }
                    if (this.chargeAttackFlag && !this.returnFlag)
                    {
                        this.chargeDistance -= calcDistance(this.Velocity);
                        if (this.chargeDistance <= 0 && !returnFlag)
                        {
                            this.chargeAttackFlag = false;
                            returnFlag = true;
                            swapVelocity();
                            setupCharge(this.originalX, this.originalY);
                        }
                    }
                    if (this.returnFlag)
                    {
                        if (this.drawRectangle.Right >= this.windowWidth)
                        {
                            swapVelocity();
                            this.returnFlag = false;
                            this.chargeAttackTimer = 0;
                        }
                    }
                }
                isdead();
                this.updateLocation();
                if (this.drawRectangle.Top <= 0 || this.drawRectangle.Bottom > windowHeight)
                {
                    this.invertY();
                }

                if (this.drawRectangle.Left <= 0 || this.drawRectangle.Right > windowWidth)
                {
                    this.invertX();
                }
                if (this.drawRectangle.Intersects(dolphin.getRectangle())&&dolphin.Active&&!dolphin.Shield)
                {
                    Rectangle collisionRectangle = Rectangle.Intersect(this.drawRectangle, dolphin.getRectangle());
                    explosion.Play(collisionRectangle.Center.X, collisionRectangle.Center.Y);
                    dolphin.Active = false;
                    return true;
                }
            }
            return false;
        }


        public void DrawBoss(SpriteBatch sprite, Color color)
        {
            //debugger
            //sprite.DrawString(debug, "{" + this.deltaVec.X + " " + this.deltaVec.Y + "}angle=" + this.angle * (180.0 / Math.PI) + " speed{" + this.Velocity.X + " " + this.Velocity.Y + "}", new Vector2(this.windowWidth / 2 - 200, this.windowHeight / 2), Color.Black);
            this.draw(sprite, color);
            sprite.Draw(this.healthTexture, this.healthRectangle, color);
        }


        public void isdead()
        {

            if (this.life <= 0)
            {
                this.Active = false;
            }

        }

        private EnemyBullet getInactiveBullet()
        {
            foreach (EnemyBullet bullet in Bullets)
            {
                if (!bullet.Active)
                {
                    return bullet;
                }
            }
            return null;
        }
    }
}
