using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DolphinAttack
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        enum GameState
        {
            MainMenu,
            Controls,
            Playing,
            Credits,
            Quit,

        }

        GameState CurrentGameState = GameState.MainMenu;

        cButton btnPlay;

        bool paused = false;

        Texture2D pausedTexture;

        Rectangle pausedRectangle;

        cButton btnP, btnQ;

        cButton btnC, btnQuit;

        cButton btnCredits;

        cButton btnBack;


        Boss boss;
        Dolphin dolphin;
        List<Enemy> enemies = new List<Enemy>();

        private int numEnemies = 10;
        private const int maxEnemyRow = 13;
        private float spawn = 0;
        Random random = new Random((int)DateTime.Now.Ticks);

        protected const int WINDOW_WIDTH = 1000;
        protected const int WINDOW_HEIGHT = 500;

        protected const int lives = 10;
        protected int fails = 0;
        protected int BossCounter = 1;


        protected float resetShip = 0;
        protected float resetTime = 2;

        Explosion explosion;
        private ScrollingBackground background;
        int actualBG = 0;
        float bgTimer = 10;
        const float TIMER = 10;

        //message
        protected SpriteFont scoreText;
        protected SpriteFont live;
        protected SpriteFont EndGame;

        protected int destroyed = 0;

        protected Song musica;
        protected Song song;
        protected Song song1;
        protected Song song2;
        protected SpriteFont font;

        float elapsed = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);


            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;

            graphics.ApplyChanges();

            IsMouseVisible = true;

            pausedTexture = Content.Load<Texture2D>("fundopausa");
            pausedRectangle = new Rectangle(0, 0, pausedTexture.Width, pausedTexture.Height);

            btnPlay = new cButton(Content.Load<Texture2D>("PLAY"), graphics.GraphicsDevice);
            btnPlay.setPosition(new Vector2(450, 300));

            btnC = new cButton(Content.Load<Texture2D>("controls"), graphics.GraphicsDevice);
            btnC.setPosition(new Vector2(450, 350));

            btnQuit = new cButton(Content.Load<Texture2D>("QUIT2"), graphics.GraphicsDevice);
            btnQuit.setPosition(new Vector2(450, 400));

            btnP = new cButton(Content.Load<Texture2D>("RESUME"), graphics.GraphicsDevice);
            btnP.setPosition(new Vector2(450, 300));


            btnQ = new cButton(Content.Load<Texture2D>("QUIT"), graphics.GraphicsDevice);
            btnQ.setPosition(new Vector2(450, 350));

            btnCredits = new cButton(Content.Load<Texture2D>("credits"), graphics.GraphicsDevice);
            btnCredits.setPosition(new Vector2(850, 450));


            btnBack = new cButton(Content.Load<Texture2D>("home"), graphics.GraphicsDevice);
            btnBack.setPosition(new Vector2(0, 400));

            explosion = new Explosion(Content);

            background = new ScrollingBackground();
            background.Load(GraphicsDevice, Content.Load<Texture2D>("ceu"));
            /*background = new List<ScrollingBackground>(bgList.Length);
            for (int i = 0; i < bgList.Length; i++)
            {
                background.Add(new ScrollingBackground());
                background.ElementAt(i).Load(GraphicsDevice, Content.Load<Texture2D>(bgList[i]));
            }*/

            dolphin = new Dolphin(Content, 0, WINDOW_HEIGHT / 2 - 60, WINDOW_WIDTH, WINDOW_HEIGHT, 3);
            for (int i = 0; i < numEnemies; i++)
            {
                enemies.Add(new Enemy(Content, 0, 0, WINDOW_WIDTH, WINDOW_HEIGHT, -2));
                enemies.ElementAt(i).createBullet(Content);
            }

            boss = new Boss(Content, 0, 0, WINDOW_WIDTH, WINDOW_HEIGHT, -2);

            loadMedia();

            // Put the name of your song in instead of "song_title"

            // TODO: use this.Content to load your game content here
        }

        private void loadMedia()
        {
            this.scoreText = Content.Load<SpriteFont>("myFont");
            this.EndGame = Content.Load<SpriteFont>("EndGame");
            this.live = Content.Load<SpriteFont>("myFont");
            this.song = Content.Load<Song>("musicmenu");
            this.song1 = Content.Load<Song>("bossSong");
            //MediaPlayer.Play(song);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            MouseState mouse = Mouse.GetState();


            switch (CurrentGameState)
            {

                case GameState.MainMenu:


                    if (btnPlay.isClicked == true) CurrentGameState = GameState.Playing;
                    btnPlay.Update(mouse);
                    if (btnQuit.isClicked == true) CurrentGameState = GameState.Quit;
                    btnQuit.Update(mouse);

                    if (btnC.isClicked == true) CurrentGameState = GameState.Controls;
                    btnC.Update(mouse);

                    if (btnCredits.isClicked == true) CurrentGameState = GameState.Credits;
                    btnCredits.Update(mouse);


                    break;
                case GameState.Playing:

                    if (!paused)
                    {
                        explosion.Update(gameTime);

                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        {
                            paused = true;
                            btnP.isClicked = false;
                        }
                        elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                        spawn += elapsed;
                        if (!win() && !lose())
                        {
                            if (!dolphin.Active)
                            {
                                resetShip += elapsed;

                                if (resetShip >= resetTime)
                                {

                                    fails++;
                                    dolphin.Active = true;
                                    resetShip = 0;
                                    dolphin.setCoordinates(0, WINDOW_HEIGHT / 2 - 60);

                                }
                            }
                            dolphin.update(enemies, gameTime);
                            if (spawn > 0.3 && destroyed <= 5)
                            {
                                Enemy spawned = this.getFirstInactiveEnemy();
                                if (spawned != null)
                                {
                                    spawned.Active = true;
                                    spawn = 0;

                                    int randomY = random.Next(100, WINDOW_HEIGHT - 100);
                                    if (randomY % 2 == 0)
                                    {
                                        spawned.invertY();
                                    }
                                    random = new Random((int)DateTime.Now.Ticks);
                                    spawned.setCoordinates(WINDOW_WIDTH, randomY);
                                }
                            }
                            foreach (DolphinBullet bullet in dolphin.getBullets())
                            {
                                bool collisionBullet = bullet.update(enemies, gameTime, boss);
                                if (collisionBullet)
                                {
                                    destroyed++;
                                    explosion.Play(bullet.getRectangle().X, bullet.getRectangle().Y);
                                }
                            }
                            bool collisionSuperBullet = dolphin.superBullet.update(enemies, boss, gameTime);
                            if (collisionSuperBullet)
                            {
                                destroyed++;
                                explosion.Play(dolphin.superBullet.getRectangle().X, dolphin.superBullet.getRectangle().Y);
                            }
                            foreach (Enemy enemy in enemies)
                            {
                                foreach (EnemyBullet b in enemy.getBullets())
                                {
                                    bool collisionEnemyBullet = b.Update(dolphin, gameTime);
                                    if (collisionEnemyBullet)
                                    {
                                        explosion.Play(b.getRectangle().X, b.getRectangle().Y);
                                    }
                                }
                                if (enemy.Active)
                                {
                                    bool collisionEnemy = enemy.update(dolphin, gameTime);
                                    if (collisionEnemy)
                                    {
                                        Rectangle collisionRectangle = Rectangle.Intersect(enemy.getRectangle(), dolphin.getRectangle());
                                        explosion.Play(collisionRectangle.Center.X, collisionRectangle.Center.Y);
                                    }
                                }
                            }
                            background.Update(elapsed * 100);
                            if (destroyed >= 10)
                            {
                                if (!boss.Active)
                                {
                                    MediaPlayer.Play(song);
                                    boss.Active = true;
                                    boss.setCoordinates(WINDOW_WIDTH - 300, 100);
                                    if (boss.Life <= 0)
                                    {
                                        MediaPlayer.Stop();
                                        BossCounter--;
                                        MediaPlayer.Play(song1);
                                        win();
                                    }
                                }
                                bool collisionBoss = boss.Update(dolphin, gameTime);
                                if (collisionBoss)
                                {
                                    Rectangle collisionRectangle = Rectangle.Intersect(boss.getRectangle(), dolphin.getRectangle());
                                    explosion.Play(collisionRectangle.Center.X, collisionRectangle.Center.Y);
                                }
                                foreach (EnemyBullet b in boss.Bullets)
                                {
                                    bool collisionEnemyBullet = b.Update(dolphin, gameTime);
                                    if (collisionEnemyBullet)
                                    {
                                        explosion.Play(b.getRectangle().X, b.getRectangle().Y);
                                    }
                                }
                            }
                        }
                    }
                    else if (paused)
                    {
                        if (btnP.isClicked)
                            paused = false;
                        if (btnQ.isClicked)
                            Exit();
                        btnP.Update(mouse);
                        btnQ.Update(mouse);
                    }
                    break;
                case GameState.Quit:
                    Exit();
                    break;
                case GameState.Controls:
                    if (btnBack.isClicked == true) CurrentGameState = GameState.MainMenu;
                    btnBack.Update(mouse);
                    break;
                case GameState.Credits:
                    if (btnBack.isClicked == true) CurrentGameState = GameState.MainMenu;
                    btnBack.Update(mouse);
                    break;
            }
            base.Update(gameTime);

        }

        private Enemy getFirstInactiveEnemy()
        {
            foreach (Enemy enemy in this.enemies)
            {
                if (!enemy.Active)
                {
                    return enemy;
                }
            }
            return null;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            TimeSpan time = MediaPlayer.PlayPosition;
            TimeSpan songTime = song.Duration;

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            // TODO: Add your drawing code here
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(Content.Load<Texture2D>("fundo"), new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT), Color.White);
                    btnPlay.Draw(spriteBatch);
                    btnQuit.Draw(spriteBatch);
                    btnC.Draw(spriteBatch);
                    btnCredits.Draw(spriteBatch);

                    break;

                case GameState.Controls:
                    spriteBatch.Draw(Content.Load<Texture2D>("controls1"), new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT), Color.White);
                    btnBack.Draw(spriteBatch);


                    break;

                case GameState.Credits:
                    spriteBatch.Draw(Content.Load<Texture2D>("credits1"), new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT), Color.White);
                    btnBack.Draw(spriteBatch);

                    break;


                case GameState.Playing:


                    if (!paused)
                    {
                        background.Draw(spriteBatch);
                        explosion.Draw(spriteBatch, Color.White);
                        dolphin.drawDolphin(spriteBatch, gameTime);
                        foreach (DolphinBullet bullet in dolphin.getBullets())
                        {
                            bullet.draw(spriteBatch, Color.White);
                        }
                        dolphin.superBullet.draw(spriteBatch, Color.White);
                        foreach (Enemy enemy in enemies)
                        {
                            foreach (EnemyBullet bullet in enemy.getBullets())
                            {
                                bullet.draw(spriteBatch, Color.White);
                            }
                            enemy.draw(spriteBatch, Color.White);
                        }
                        boss.DrawBoss(spriteBatch, Color.White);

                        foreach (EnemyBullet bullet in boss.getBullets())
                        {
                            bullet.draw(spriteBatch, Color.White);
                        }

                        if (lose())
                        {
                            spriteBatch.DrawString(EndGame, "YOU LOSE", new Vector2(300, 400), Color.Red);
                        }

                        if (win())
                        {
                            spriteBatch.DrawString(EndGame, "YOU WIN!", new Vector2(300, 400), Color.Blue);
                        }

                        spriteBatch.DrawString(live, "" + (lives - fails), new Vector2(20, 10), Color.Goldenrod);
                        spriteBatch.DrawString(scoreText, "" + destroyed, new Vector2(750, 10), Color.NavajoWhite);
                    }
                    else if (paused)
                    {
                        spriteBatch.Draw(pausedTexture, pausedRectangle, Color.White);
                        btnP.Draw(spriteBatch);
                        btnQ.Draw(spriteBatch);
                    }
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }


        private bool win()
        {
            return BossCounter <= 0;
        }

        private bool lose()
        {
            return fails >= lives;
        }
    }
}
