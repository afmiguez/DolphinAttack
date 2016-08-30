// FILE: C:/Users/AF/Documents/Visual Studio 2013/Projects/DolphinAttack/DolphinAttack/DolphinAttack/dolphinAttack/Dolphin.cs

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

// In this section you can add your own using directives
// section -64--88--119-1--6d8e1b93:14c766071a8:-8000:000000000000088E begin
// section -64--88--119-1--6d8e1b93:14c766071a8:-8000:000000000000088E end
namespace DolphinAttack
{

    /// <summary>
    ///  A class that represents ...
    /// 
    ///  @see OtherClasses
    ///  @author your_name_here
    /// </summary>
    public class Dolphin : Object
    {
        KeyboardState KBactual;
        KeyboardState KBprevious;

        GamePadState GPactual;
        GamePadState GPprevious;

        // Attributes
        private int bulletCount = 20;
        private float concentratedShot = 0;

        SoundEffect shotSound;
        public SuperBullet superBullet;
        SoundEffect hadouken;
        bool buildUpFlag = true;
        SoundEffect buildUp;
        float chargeTimer;
        bool chargeFlag = true;
        SoundEffect charge;

        SoundEffect dashSound;
        float dashTimer = 0;
        bool dashFlag = false;
        float dashCooldown = 2;
        int dashSpeed = 4;

        private bool shield;
        private float shieldTimer = 0;
        private float shieldDraw = 0;

        public bool Shield
        {
            get { return shield; }
            set { shield = value; }
        }

        private float shotTime = 0;
        // Associations

        private List<DolphinBullet> bullets = new List<DolphinBullet>();

        // Operations
        public List<DolphinBullet> getBullets()
        {
            return bullets;
        }

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
        /// <returns>
        /// </returns>
        public Dolphin(ContentManager content, int x, int y, int windowWidth, int windowHeight, int speed)
            : base(content, x, y, windowWidth, windowHeight, speed)
        {
            // section -64--88-43-39--10332b78:14cf1b7a08f:-8000:0000000000000AAA begin
            shield = false;
            loadContent(content, "DragonDolphin", x, y);
            this.Velocity = new Vector2(speed, speed);
            for (int i = 0; i < bulletCount; i++)
            {
                bullets.Add(new DolphinBullet(content, 0, 0, windowWidth, windowHeight, 10, 35, 35));
            }
            this.superBullet = new SuperBullet(content, 0, 0, windowWidth, windowHeight, 20, 50, 50);
            shotSound = content.Load<SoundEffect>("shot");
            hadouken = content.Load<SoundEffect>("kame2");
            buildUp = content.Load<SoundEffect>("kame1");
            charge = content.Load<SoundEffect>("kame1");
            dashSound = content.Load<SoundEffect>("dash");
            // section -64--88-43-39--10332b78:14cf1b7a08f:-8000:0000000000000AAA end

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
        private void loadContent(ContentManager content, string spriteName, int x, int y)
        {
            // section -64--88-43-39--10332b78:14cf1b7a08f:-8000:0000000000000AB0 begin
            // load content and set remainder of draw rectangle
            sprite = content.Load<Texture2D>(spriteName);
            drawRectangle = new Rectangle(x, y, 100, 70);
            // section -64--88-43-39--10332b78:14cf1b7a08f:-8000:0000000000000AB0 end

        }

        /// <summary>
        ///  An operation that does...
        /// 
        ///  @param firstParam a description of this parameter
        /// </summary>
        /// <param name="enemies">
        /// </param>
        /// <param name="gameTime">
        /// </param>
        /// <returns>
        /// </returns>
        public void update(List<Enemy> enemies, GameTime gameTime)
        {
            KBactual = Keyboard.GetState();
            GPactual = GamePad.GetState(PlayerIndex.One);
            // section -64--88-43-39--10332b78:14cf1b7a08f:-8000:0000000000000AB6 begin
            if (Active)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (this.shield)
                {
                    shieldTimer += elapsed;
                    if (shieldTimer >= 2)
                    {
                        shieldTimer = 0;
                        shield = false;
                    }


                }
                shotTime += elapsed;
                dashCooldown += elapsed;
                if (KBactual.IsKeyDown(Keys.Right) || GPactual.IsButtonDown(Buttons.LeftThumbstickRight))
                {
                    if (this.drawRectangle.Right < this.windowWidth)
                    {
                        this.drawRectangle.X += (int)this.Velocity.X;
                    }
                }
                if (KBactual.IsKeyDown(Keys.Left) || GPactual.IsButtonDown(Buttons.LeftThumbstickLeft))
                {
                    if (this.drawRectangle.Left > 0)
                    {
                        this.drawRectangle.X -= (int)this.Velocity.X;
                    }

                }
                if (KBactual.IsKeyDown(Keys.Up) || GPactual.IsButtonDown(Buttons.LeftThumbstickUp))
                {
                    if (this.drawRectangle.Top > 0)
                    {
                        this.drawRectangle.Y -= (int)this.Velocity.Y;
                    }

                }
                if (KBactual.IsKeyDown(Keys.Down) || GPactual.IsButtonDown(Buttons.LeftThumbstickDown))
                {
                    if (this.drawRectangle.Bottom < this.windowHeight)
                    {
                        this.drawRectangle.Y += (int)this.Velocity.Y;
                    }
                }
                if ((KBactual.IsKeyDown(Keys.LeftShift) || GPactual.IsButtonDown(Buttons.B)) && dashCooldown >= 2 && !dashFlag)
                {
                    dashCooldown = 0;
                    dashFlag = true;
                    dashSound.Play();
                    this.setVelocity(this.Velocity.X + dashSpeed, this.Velocity.Y + dashSpeed);
                }
                if (dashFlag)
                {
                    dashTimer += elapsed;
                    if (dashTimer >= 1)
                    {
                        dashFlag = false;
                        dashTimer = 0;
                        this.setVelocity(this.Velocity.X - dashSpeed, this.Velocity.Y - dashSpeed);
                    }
                }
                //shot button is pressed right now
                if (KBactual.IsKeyDown(Keys.LeftControl) || GPactual.IsButtonDown(Buttons.A))
                {
                    //shot button was not pressed before, so just shot a regular bullet
                    if (!KBprevious.IsKeyDown(Keys.LeftControl) && !GPprevious.IsButtonDown(Buttons.A))
                    {
                        if (shotTime >= 0.3 && concentratedShot < 2)
                        {
                            shot();
                            shotTime = 0;
                            concentratedShot = 0;
                            buildUpFlag = true;
                        }
                    }
                    //shot button is pressed now and was pressed before, so build up time for super bullet
                    else
                    {
                        if (concentratedShot < 2)
                        {
                            concentratedShot += elapsed;
                            if (buildUpFlag && concentratedShot > 0.3)
                            {
                                buildUp.Play();
                                buildUpFlag = false;
                            }
                        }
                        else
                        {
                            if (chargeTimer < (float)charge.Duration.TotalSeconds - 0.5)
                            {
                                chargeFlag = false;
                                chargeTimer += elapsed;
                            }
                            else
                            {
                                chargeFlag = true;
                                chargeTimer = 0;
                            }
                            if (chargeFlag)
                            {
                                charge.Play();
                                chargeFlag = false;
                            }
                        }

                    }
                }
                //shot button is not pressed right now
                else
                {
                    //shot button was pressed before, so it was being pressed for some time
                    if (KBactual.IsKeyUp(Keys.LeftControl) || GPactual.IsButtonUp(Buttons.A))
                    {
                        //if time it was pressed is higher than 2 seconds, blast super shot and reset the timer
                        if (concentratedShot >= 2)
                        {
                            shotConcentrated();
                            concentratedShot = 0;
                            buildUpFlag = true;
                        }
                    }
                }
                KBprevious = KBactual;
                GPprevious = GPactual;

            }
            else
            {
                concentratedShot = 0;
                shotTime = 0;
                shield = true;
            }
            // section -64--88-43-39--10332b78:14cf1b7a08f:-8000:0000000000000AB6 end

        }

        /// <summary>
        ///  An operation that does...
        /// 
        ///  @param firstParam a description of this parameter
        /// </summary>
        /// <returns>
        /// </returns>
        private Bullet getInactiveBullet()
        {
            // section -64--88-43-39--10332b78:14cf1b7a08f:-8000:0000000000000ABC begin
            foreach (Bullet bullet in bullets)
            {
                if (!bullet.Active)
                {
                    return bullet;
                }
            }
            return null;
            // section -64--88-43-39--10332b78:14cf1b7a08f:-8000:0000000000000ABC end

        }

        /// <summary>
        ///  An operation that does...
        /// 
        ///  @param firstParam a description of this parameter
        /// </summary>
        /// <returns>
        /// </returns>
        private void shot()
        {
            // section -64--88-43-39--10332b78:14cf1b7a08f:-8000:0000000000000ABF begin
            Bullet shot = getInactiveBullet();
            if (shot != null)
            {
                shotSound.Play();
                shot.Active = true;
                shot.setCoordinates(this.drawRectangle.Right, this.drawRectangle.Y);
            }
            // section -64--88-43-39--10332b78:14cf1b7a08f:-8000:0000000000000ABF end
        }

        private void shotConcentrated()
        {
            hadouken.Play();
            superBullet.Active = true;
            superBullet.setCoordinates(this.drawRectangle.Right, this.drawRectangle.Y);
        }

        public void vitoria()
        {
            this.drawRectangle.Y += (int)this.Velocity.Y;
            this.drawRectangle.X += (int)this.Velocity.X;

        }


        public void makeBulletsInactive()
        {
            foreach (Bullet b in bullets)
            {
                b.Active = false;
            }
        }

        public void drawDolphin(SpriteBatch sprite, GameTime gameTime)
        {
            if (shield)
            {
                shieldDraw += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (shieldDraw >= 0.1)
                {
                    this.draw(sprite, Color.White);
                    shieldDraw = 0;
                }
            }
            else
            {
                this.draw(sprite, Color.White);
            }
        }
    } /* end class Dolphin */

}
