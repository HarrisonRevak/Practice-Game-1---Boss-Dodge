using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;
using System.ComponentModel.Design.Serialization;

namespace BossDodge1;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    Texture2D _pixel;

    GamePadState pad1;
    KeyboardState board;

    int xmove;
    int ymove;


    double XAccel;
    double YAccel;

    bool colorIncrease = true;

    bool collided = false;

    Rectangle _player;

    Rectangle _enemy;

    byte redIntensity = 0;
    byte greenIntensity = 0;
    byte blueIntensity = 0;


    bool gameOn = true;

    int lives = 3;


    bool playGame = true;

    Color backgroundColor;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new Color[] { Color.White });

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {

        pad1 = GamePad.GetState(PlayerIndex.One);

        board = Keyboard.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _player = new Rectangle(xmove, ymove, 50, 50);

        _enemy = new Rectangle(150, 200, 50, 50);

        //Movement Acceleration Code
        xmove += (int)XAccel;
        ymove += (int)YAccel;

        //Collision code
        if (_player.Intersects(_enemy))
        {
            lives--;
            collided = true;
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
        //Color Change code
        if (colorIncrease)
            redIntensity++;
        else
            redIntensity--;

        if (redIntensity == 145)
            colorIncrease = false;
        else if (redIntensity == 0)
            colorIncrease = true;

        //Game Over
        if (lives == 0)
            Exit();


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {


        backgroundColor = new Color(redIntensity, greenIntensity, blueIntensity);
        if (playGame == true)
            GraphicsDevice.Clear(backgroundColor);
        else if (playGame != true)
            GraphicsDevice.Clear(Color.Black);
            



        // TODO: Add your drawing code here
        //redIntensity = 0;
        //greenIntensity = 0;
        //blueIntensity = 0;

        _spriteBatch.Begin();

        //Player square
        _spriteBatch.Draw(_pixel, _player, Color.White);
        //Enemy code
        _spriteBatch.Draw(_pixel, _enemy, Color.Red);

        _spriteBatch.End();

            base.Draw(gameTime);
    }
    // Brought to you by Alexander the Great
}
