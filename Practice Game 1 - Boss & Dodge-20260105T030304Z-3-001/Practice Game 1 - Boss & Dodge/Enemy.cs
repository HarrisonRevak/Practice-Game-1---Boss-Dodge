using System.Drawing;
using System.Numerics;
using Microsoft.Xna.Framework.Graphics;

namespace EnemyClass;

public class Enemy
{
    //Variables here blah blah blah\
    private int x;
    private int y;
    private Texture2D enemyTexture;
    private Microsoft.Xna.Framework.Color color = Microsoft.Xna.Framework.Color.Red;
    public Enemy(int a, int b, Texture2D texture)
    {
        x = a;
        y = b;
        enemyTexture = texture;
    }

    public void changePoint(int a, int b)
    {
        x += a;
        y += b;
    }

    public void draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(enemyTexture, new Vector2(x, y), color);
    }
}