// FILE: C:/Users/AF/Documents/Visual Studio 2013/Projects/DolphinAttack/DolphinAttack/DolphinAttack/dolphinAttack/Object.cs

// In this section you can add your own using directives
// section -64--88--119-1--6d8e1b93:14c766071a8:-8000:0000000000000867 begin
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
// section -64--88--119-1--6d8e1b93:14c766071a8:-8000:0000000000000867 end



namespace DolphinAttack
{

    /// <summary>
    ///  A class that represents ...
    /// 
    ///  @see OtherClasses
    ///  @author your_name_here
    /// </summary>
    public class Object
    {
        // Attributes

        private bool active;
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        protected Texture2D sprite;

        protected Rectangle drawRectangle;

        private Vector2 velocity;

        protected Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        protected int windowWidth;
        protected int windowHeight;
        // Operations

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
        /// <param name="windowWidth">
        /// </param>
        /// <param name="windowHeight">
        /// </param>
        /// <param name="speed">
        /// </param>
        /// <returns>
        /// </returns>
        public Object(ContentManager content, int x = 0, int y = 0, int windowWidth = 0, int windowHeight = 0, int speed = 0)
        {
            // section -64--88-43-39--10332b78:14cf1b7a08f:-8000:0000000000000A83 begin
            this.active = true;
            this.velocity.X = speed;
            this.velocity.Y = speed;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            // section -64--88-43-39--10332b78:14cf1b7a08f:-8000:0000000000000A83 end

        }

        /// <summary>
        ///  An operation that does...
        /// 
        ///  @param firstParam a description of this parameter
        /// </summary>
        /// <param name="spriteBatch">
        /// </param>
        /// <param name="color">
        /// </param>
        /// <returns>
        /// </returns>
        public void draw(SpriteBatch spriteBatch, Color color)
        {
            // section -64--88-43-39--10332b78:14cf1b7a08f:-8000:0000000000000A8C begin
            if (active)
            {
                spriteBatch.Draw(sprite, this.drawRectangle, color);
            }
            // section -64--88-43-39--10332b78:14cf1b7a08f:-8000:0000000000000A8C end

        }

        public void updateLocation()
        {
            this.drawRectangle.X += (int)this.velocity.X;
            this.drawRectangle.Y += (int)this.velocity.Y;
        }

        public void invertY()
        {
            this.velocity.Y *= -1;
        }
        public void invertX()
        {
            this.velocity.X *= -1;
        }

        /// <summary>
        ///  An operation that does...
        /// 
        ///  @param firstParam a description of this parameter
        /// </summary>
        /// <returns>
        /// </returns>
        public Rectangle getRectangle()
        {
            return this.drawRectangle;
        }

        /// <summary>
        ///  An operation that does...
        /// 
        ///  @param firstParam a description of this parameter
        /// </summary>
        /// <param name="x">
        /// </param>
        /// <param name="y">
        /// </param>
        /// <returns>
        /// </returns>
        public void setCoordinates(int x, int y)
        {
            // section -64--88-43-39--10332b78:14cf1b7a08f:-8000:0000000000000A94 begin
            this.drawRectangle.X = x;
            this.drawRectangle.Y = y;
            // section -64--88-43-39--10332b78:14cf1b7a08f:-8000:0000000000000A94 end

        }

        public void setVelocity(float x, float y)
        {
            this.velocity.X = x;
            this.velocity.Y = y;
        }
    } /* end class Object */

}
