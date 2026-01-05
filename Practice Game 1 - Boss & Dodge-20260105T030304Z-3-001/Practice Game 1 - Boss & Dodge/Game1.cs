using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;
using System.ComponentModel.Design.Serialization;
using System.Collections.Generic;
using System.Xml;
using EnemyClass;

namespace BossDodge1;

public class Game1 : Game
{
    // - Sprite Stuff
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _font, _fontBig;
    private Texture2D _pixel, eyeballs, holes;   // - Sprite Stuff

    // - Gamepad & Keyboad
    private GamePadState pad1;
    private KeyboardState board;
    private MouseState mouse;
    private Point mousePosition;   // - Gamepad & Keyboad
    // - Coordinate Points
    private int xmove=350;
    private int ymove=200;
    private int enemyX = 150;
    private int enemyY = 250;
    private int enemy2X = 650;
    private int enemy2Y = 250;
    private int lifeX;
    private int lifeY;  // - Coordinate Points
    // - Acceleration 
    private double EnemyMultipler1;
    private double EnemyMultiplier2;
    private double XAccel;
    private double YAccel;
    private double XAccelEnemy;
    private double YAccelEnemy;
    private double XAccelEnemy2;
    private double YAccelEnemy2;       // - Acceleration
    private bool collided = false;
    // - Rectangles
    private Rectangle _player;
    private Rectangle _enemy, _enemy2;
    private Rectangle _life;
    private int randomsize1, randomsize2;    // - Rectangles
    // - Colors
    private byte redIntensity = 0;
    private byte greenIntensity = 0;
    private byte blueIntensity = 0;
    private Color backgroundColor;
    private bool colorIncrease = true;  // - Colors
    // - Stuff for Life Code
    private Random rand = new Random();
    private int randomNum;
    private bool setPoint = true;
    private int lives = 3;
    private Texture2D healthSprite;
    private Rectangle healthRect;
    private bool playGame = true;
    private int hit = 1;   // - Stuff for Life Code

    // - Stuff for Invincibility Frames
    private int iframescount = 180;
    private bool iframeson = false;     // - Stuff for Invincibility Frames

    //- Stuff for Title Screen & Death
    private String screen = "Title";
    private Rectangle button1, button2;
    private Texture2D buttonimg, deathscreen, wall;
    private Color buttoncol1, buttoncol2;
    private Color textcol1, textcol2;    // - Stuff for Title Screen

    // - Stuff for Off-Screen Attacks
    private Rectangle offattack;
    private int countoff, warnoff, randomoff,speedoff, sizeoff; //(Random speed and size?)
    private bool startoff, attackon, left, right;
    private Texture2D imageoff,warningoff;// - Stuff for Off-Screen Attacks (Giant hand for attack?)

    /*
    ----To Do List----
    --- Add a title screen(DONE)
    --- Add background(regular and death screen)(DONE)
    --- Make sprites for player and enemy(KINDA DONE)
    --- Design an actual healthbar

    --- *** (50% DONE)Add Invincibility Frames timer           NEEDS FINE TUNING
    --- Add randomly occuring off-screen attacks
    --- *** Add warning system for off-screen attacks
    ----To Do List----
    */
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        EnemyMultipler1 = (rand.NextDouble()*3)+1;
        EnemyMultiplier2 = (rand.NextDouble() * 3) + 1;

        healthRect = new Rectangle(15, 450, 0, 15);
        randomsize1 = rand.Next(15, 25);
        randomsize2 = rand.Next(25, 40);
        button1 = new Rectangle(210, 180, 350, 60);
        button2 = new Rectangle(210, 280, 350, 60);
        buttoncol1 = Color.Black;
        buttoncol2 = Color.Black;
        textcol1 = Color.Red;
        textcol2 = Color.Red;
        startoff = false;
        randomoff = rand.Next(0,10)*60;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new Color[] { Color.White });

        _font = Content.Load<SpriteFont>("File");
        _fontBig = Content.Load<SpriteFont>("BigFile");

        deathscreen = Content.Load<Texture2D>("Despair");
        wall = Content.Load<Texture2D>("Wall");

        healthSprite = new Texture2D(GraphicsDevice, 1, 1);
        healthSprite.SetData(new[] { Color.Yellow });
        eyeballs = Content.Load<Texture2D>("eyeball");
        holes = Content.Load<Texture2D>("hole");

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {

        pad1 = GamePad.GetState(PlayerIndex.One);

        board = Keyboard.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        mouse = Mouse.GetState();
        mousePosition = mouse.Position;


        _player = new Rectangle(xmove, ymove, 15*hit, 15*hit);
        _enemy = new Rectangle(enemyX, enemyY, randomsize1, randomsize1);
        _enemy2 = new Rectangle(enemy2X, enemy2Y,randomsize2,randomsize2);



        //New Enemy Code

        _life = new Rectangle(lifeX, lifeY, 15, 15);

        if (playGame == true && screen == "Play")
        {

            //Movement Acceleration Code
            xmove += (int)XAccel;
            ymove += (int)YAccel;
            enemyX += (int)XAccelEnemy;
            enemyY += (int)YAccelEnemy;
            enemy2X += (int)XAccelEnemy2;
            enemy2Y += (int)YAccelEnemy2;

            //Give _life rectangle points
            if (setPoint == true)
            {
                for (int i = 0; i < 450; i++)
                {
                    randomNum = rand.Next(1, 164);

                }
            }

            if (randomNum == 54 && setPoint == true)
            {
                lifeX = rand.Next(75, 450);
                lifeY = rand.Next(100, 400);
                setPoint = false;
            }


            //Collision code
            if (_player.Intersects(_enemy))
            {
                
                if (iframeson != true)
                {
                   lives--;
                   hit++;
                }
                collided = true;
                setPoint = true;
                if (_player.Right > _enemy.Left && board.IsKeyDown(Keys.Right))
                {
                    xmove -= 10;
                    XAccel = -3;
                }
                else if (_player.Left < _enemy.Right && board.IsKeyDown(Keys.Left))
                {
                    xmove += 10;
                    XAccel = 3;
                }
                if (_player.Bottom > _enemy.Top && board.IsKeyDown(Keys.Down))
                {
                    ymove -= 10;
                    YAccel = -3;
                }
                else if (_player.Top < _enemy.Bottom && board.IsKeyDown(Keys.Up))
                {
                    ymove += 10;
                    YAccel = 3;
                }
            }
            else if (_player.Intersects(_enemy) != true)
            {
                collided = false;
            }
            //Collision code enemy 2
            if (_player.Intersects(_enemy2))
            {
                if (iframeson != true)
                {
                   lives--;
                   hit++;
                }
                collided = true;
                setPoint = true;
                if (_player.Right > _enemy2.Left && board.IsKeyDown(Keys.Right))
                {
                    xmove -= 10;
                    XAccel = -3;
                }
                else if (_player.Left < _enemy2.Right && board.IsKeyDown(Keys.Left))
                {
                    xmove += 10;
                    XAccel = 3;
                }
                if (_player.Bottom > _enemy2.Top && board.IsKeyDown(Keys.Down))
                {
                    ymove -= 10;
                    YAccel = -3;
                }
                else if (_player.Top < _enemy2.Bottom && board.IsKeyDown(Keys.Up))
                {
                    ymove += 10;
                    YAccel = 3;
                }
            }
            else if (_player.Intersects(_enemy2) != true)
            {
                collided = false;
            }



            //Declare Background color
            backgroundColor = new Color(redIntensity, greenIntensity, blueIntensity);

            //Horizontal Movement
            if (board.IsKeyDown(Keys.Left) && XAccel > -5 && collided == false)
                XAccel -= 0.2;
            else if (board.IsKeyDown(Keys.Right) && XAccel < 5 && collided == false)
                XAccel += 0.2;
            if (board.IsKeyUp(Keys.Left) && XAccel <= 0 && collided == false)
                XAccel += 0.1;
            else if (board.IsKeyUp(Keys.Right) && XAccel >= 0 && collided == false)
                XAccel -= 0.1;
            //Vertical Movement
            if (board.IsKeyDown(Keys.Up) && YAccel > -5 && collided == false)
                YAccel -= 0.2;
            else if (board.IsKeyDown(Keys.Down) && YAccel < 5 && collided == false)
                YAccel += 0.2;
            if (board.IsKeyUp(Keys.Up) && YAccel <= 0 && collided == false)
                YAccel += 0.1;
            else if (board.IsKeyUp(Keys.Down) && YAccel >= 0 && collided == false)
                YAccel -= 0.1;


            //Enemy Move Code
            if (xmove < enemyX && XAccelEnemy > -3)
            {
                XAccelEnemy -= 0.1 * EnemyMultipler1;
            }
            else if (xmove > enemyX && XAccelEnemy < 3)
            {
                XAccelEnemy += 0.1 * EnemyMultipler1;
            }

            if (ymove < enemyY && YAccelEnemy > -3)
            {
                YAccelEnemy -= 0.05 * EnemyMultipler1;
            }
            else if (ymove > enemyY && YAccelEnemy < 3)
            {
                YAccelEnemy += 0.05 * EnemyMultipler1;
            }
            //Enemy2 Move Code
            if (xmove < enemy2X && XAccelEnemy2 > -3)
            {
                XAccelEnemy2 -= 0.1 * EnemyMultiplier2;
            }
            else if (xmove > enemy2X && XAccelEnemy2 < 3)
            {
                XAccelEnemy2 += 0.1 * EnemyMultiplier2;
            }

            if (ymove < enemy2Y && YAccelEnemy2 > -3)
            {
                YAccelEnemy2 -= 0.05 * EnemyMultiplier2;
            }
            else if (ymove > enemy2Y && YAccelEnemy2 < 3)
            {
                YAccelEnemy2 += 0.05 * EnemyMultiplier2;
            }
            //Enemy Movement Intersection
            if (_player.Intersects(_enemy))
            {
                enemyX = rand.Next(0, 200);
                enemyY = rand.Next(0, 200);
                XAccelEnemy = 0;
                YAccelEnemy = 0;
                if (_enemy2.X < _player.X)
                {
                    XAccelEnemy2 = -2;
                }
                else if (_enemy2.X >= _player.X)
                {
                    XAccelEnemy2 = 2;
                }
                if (_enemy2.Y < _player.Y)
                {
                    YAccelEnemy2 = -2;
                }
                else if (_enemy2.Y >= _player.Y)
                {
                    XAccelEnemy2 = 2;
                }
                iframeson = true;
            }
            //Enemy2 Movement Intersection
            if (_player.Intersects(_enemy2))
            {
                enemy2X = rand.Next(600, 800);
                enemy2Y = rand.Next(200, 400);
                XAccelEnemy2 = 0;
                YAccelEnemy2 = 0;
                if (_enemy.X < _player.X)
                {
                    XAccelEnemy = -2;
                }
                else if (_enemy.X >= _player.X)
                {
                    XAccelEnemy = 2;
                }
                if (_enemy.Y < _player.Y)
                {
                    YAccelEnemy = -2;
                }
                else if (_enemy.Y >= _player.Y)
                {
                    YAccelEnemy = 2;
                }
                iframeson = true;
            }
            if (_enemy.Intersects(_enemy2))
            {
                enemyX = rand.Next(0, 200);
                enemyY = rand.Next(0, 200);
                enemy2X = rand.Next(600, 800);
                enemy2Y = rand.Next(200, 400);
                XAccelEnemy = -2;
                YAccelEnemy = -2;
                XAccelEnemy2 = -2;
                YAccelEnemy2 = -2;
            }

            //Boundaries code - PLAYER
            if (_player.X < 0)
            {
                _player.X = 0;
                XAccel = 1;
            }
            else if (_player.X + _player.Width > 800)
            {
                _player.X = 785;
                XAccel = -1;
            }
            if (_player.Y < 0)
            {
                _player.Y = 0;
                YAccel = 1;
            }
            else if (_player.Y + _player.Height > 480)
            {
                _player.Y = 465;
                YAccel = -1;
            }
            //Boundaries code - ENEMY 1
            if (_enemy.X < 0)
            {
                _enemy.X = 0;
                XAccelEnemy = 1;
            }
            else if (_enemy.X + _enemy.Width > 800)
            {
                _enemy.X = 785;
                XAccelEnemy = -1;
            }
            if (_enemy.Y < 0)
            {
                _enemy.Y = 0;
                YAccelEnemy = 1;
            }
            else if (_enemy.Y + _enemy.Height > 480)
            {
                _enemy.Y = 465;
                YAccelEnemy = -1;
            }
            //Boundaries code - ENEMY 2
            if (_enemy2.X < 0)
            {
                _enemy2.X = 0;
                XAccelEnemy2 = 1;
            }
            else if (_enemy2.X + _enemy2.Width > 800)
            {
                _enemy2.X = 785;
                XAccelEnemy2 = -1;
            }
            if (_enemy2.Y < 0)
            {
                _enemy2.Y = 0;
                YAccelEnemy2 = 1;
            }
            else if(_enemy2.Y + _enemy2.Height > 480)
            {
                _enemy2.Y =465;
                YAccelEnemy2 = -1;
            }

            //Health Bar Update(?)
            healthRect.Width = lives * (int)256;

            //Player intersects life coin
            if (_player.Intersects(_life) == true)
            {
                hit--;
                randomNum = 0;
                if (lives < 3)
                    lives += 1;
                setPoint = true;
            }
            //Enemy intersects life coin
            if (_enemy.Intersects(_life) == true)
            {
                randomNum = 0;
                setPoint = true;
            }
            //Enemy2 intersects life coin
            if (_enemy2.Intersects(_life) == true)
            {
                randomNum = 0;
                setPoint = true;
            }
            //End game
            if (lives == 0)
            {
                playGame = false;
            }


            //Color Change code
            if (colorIncrease)
                redIntensity++;
            else
                redIntensity--;

            if (redIntensity == 110)
                colorIncrease = false;
            else if (redIntensity == 0)
                colorIncrease = true;
            iframescount--;
            if (iframescount == 0)
            {
                iframeson = false;
                iframescount = 180;
            }
            if (hit < 1)
            {
                hit=1;
            }

            //Off screen attack code :(
            countoff++;
            if(countoff == randomoff)
            {
                startoff = true;
                sizeoff = rand.Next(1,7)+1;
                speedoff = rand.Next(1,15)+1;
            }

            if(startoff == true)
            {
                warnoff++;
            }
            if(warnoff == 300)
            {
                attackon = true;
                if(rand.Next(0,100) > 50)
                {
                    offattack = new Rectangle(-400,rand.Next(0,480),400,100*(int)sizeoff);
                    left = true;
                    right = false;
                }
                else
                {
                    offattack = new Rectangle(800, rand.Next(0,480), 400, 100*(int)sizeoff);
                    left = false;
                    right = true;
                }
            }
            if (attackon == true && left == true && offattack.X !=400)
            {
                offattack.X+=speedoff;
                if(offattack.X >= 400)
                {
                    attackon = false;
                    startoff = false;
                    randomoff = rand.Next(0,10)*60;
                    left = false;
                    right = false;
                }
            }
            else if (attackon == true && right == true)
            {
                offattack.X-=speedoff;
                if(offattack.X <= 400)
                {
                    attackon = false;
                    startoff = false;
                    randomoff = rand.Next(0,10)*60;
                    left = false;
                    right = false;
                }
            }
            
        }

        

        if (button1.Contains(mousePosition))
        {
            buttoncol1 = Color.Red;
            textcol1 = Color.Black;
        }
        else
        {
            buttoncol1 = Color.Black;
            textcol1 = Color.Red;
        }
        if (button2.Contains(mousePosition))
        {
            buttoncol2 = Color.Red;
            textcol2 = Color.Black;
        }
        else
        {
            buttoncol2 = Color.Black;
            textcol2 = Color.Red;
        }

        if (button1.Contains(mousePosition) && mouse.LeftButton == ButtonState.Pressed)
        {
            screen = "Play";
        }
        else if(button2.Contains(mousePosition) && mouse.LeftButton == ButtonState.Pressed)
        {
            Exit();
        }


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // TODO: Add your drawing code here

        _spriteBatch.Begin();
        if(screen == "Title")
        {
            backgroundColor = Color.Black;
            _spriteBatch.Draw(wall, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White * 0.25f);
            _spriteBatch.Draw(deathscreen, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White * 0.1f);
            GraphicsDevice.Clear(backgroundColor);
            _spriteBatch.DrawString(_fontBig, "BEHOLDER", new Vector2(GraphicsDevice.Viewport.Width / 2f - _fontBig.MeasureString("BEHOLDER").X /2f, 50), Color.Red);
            _spriteBatch.Draw(_pixel, button1, buttoncol1);
            _spriteBatch.DrawString(_font, "START", new Vector2(GraphicsDevice.Viewport.Width / 2f - _fontBig.MeasureString("START").X /2f, 200), textcol1);
            _spriteBatch.Draw(_pixel, button2, buttoncol2);
            _spriteBatch.DrawString(_font, "EXIT", new Vector2(GraphicsDevice.Viewport.Width / 2f - _fontBig.MeasureString("EXIT").X /2f, 300), textcol2);
            
        }
        else if (screen == "Play")
        { 
            if (lives > 0)
                backgroundColor = new Color(redIntensity, greenIntensity, blueIntensity);
            else if (lives == 0)
                backgroundColor = Color.Black;
            if (playGame == true)
                GraphicsDevice.Clear(backgroundColor);
            else if (playGame != true)
                GraphicsDevice.Clear(Color.Black);

            if (lives > 0)
            {
                _spriteBatch.Draw(wall, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White * 0.25f);
                //Player square
                _spriteBatch.Draw(holes, _player, Color.DarkOrange);
                //Enemy code
                _spriteBatch.Draw(eyeballs, _enemy, Color.Red*0.5f);
                //Enemy2 Code
                _spriteBatch.Draw(eyeballs, _enemy2, Color.Red*0.5f);

                if (randomNum == 54 && _player.Intersects(_life) != true)
                {
                    _spriteBatch.Draw(_pixel, _life, Color.Yellow);
                }
                if(attackon == true)
                {
                    _spriteBatch.Draw(_pixel,offattack,Color.Red);
                }
            }

            else if (lives == 0)
            {
                _spriteBatch.Draw(deathscreen, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White * 0.25f);
                _spriteBatch.DrawString(_fontBig, "GAME OVER", new Vector2(275, 150), Color.White);
            }
        _spriteBatch.Draw(healthSprite, healthRect, Color.Yellow);
        }


        _spriteBatch.End();

        base.Draw(gameTime);
    }
    // Brought to you by Harrison Revak
}
