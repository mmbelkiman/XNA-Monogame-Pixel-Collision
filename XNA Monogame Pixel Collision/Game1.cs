using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace XNA_Monogame_Pixel_Collision
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private Vector2 _mousePosition = Vector2.Zero;
        private SpriteBatch _spriteBatch;
        private AnimatedTexture2D _textureEnemyA;
        private AnimatedTexture2D _textureEnemyB;
        private Texture2D _textureMouse;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 300,
                PreferredBackBufferHeight = 200
            };
            Content.RootDirectory = "Content";
        }

        public bool IsIntersectPixels(Rectangle rectangleA, Color[] dataA, Rectangle rectangleB, Color[] dataB)
        {
            // GET BOUNDS
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // GET COLORS FROM CURRENT POINT
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // IF BOTH NOT TRANSPARENT, HAS INTERSECTION
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            {
                _textureEnemyA.Draw(gameTime, _spriteBatch, GraphicsDevice);
                _textureEnemyB.Draw(gameTime, _spriteBatch, GraphicsDevice);
                _spriteBatch.Draw(_textureMouse, _mousePosition, Color.White);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _textureEnemyA = new AnimatedTexture2D(Content, 5, 1, "enemyA")
            {
                Position = new Vector2(10, 10)
            };

            _textureEnemyB = new AnimatedTexture2D(Content, 3, 1, "enemyB")
            {
                Position = new Vector2(200, 100)
            };

            _textureMouse = Content.Load<Texture2D>("hand");
        }

        protected override void Update(GameTime gameTime)
        {
            _mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            _textureEnemyA.Update(gameTime);
            _textureEnemyB.Update(gameTime);

            //MOUSE RECT + DATA

            Color[] mouseData = new Color[_textureMouse.Width * _textureMouse.Height];
            _textureMouse.GetData(mouseData);
            Rectangle mouseRect = new Rectangle(
                (int)_mousePosition.X,
                (int)_mousePosition.Y,
                _textureMouse.Width,
                _textureMouse.Height);

            //ENEMY A RECT + DATA + COLISION
            if (_textureEnemyA.cropTexture != null)
            {
                Color[] enemyAData = new Color[_textureEnemyA.cropTexture.Width * _textureEnemyA.cropTexture.Height];
                _textureEnemyA.cropTexture.GetData(enemyAData);
                Rectangle enemyARect = new Rectangle(
                     (int)_textureEnemyA.Position.X,
                     (int)_textureEnemyA.Position.Y,
                    _textureEnemyA.cropTexture.Width,
                    _textureEnemyA.cropTexture.Height);

                if (IsIntersectPixels(enemyARect, enemyAData, mouseRect, mouseData))
                { _textureEnemyA.Color = Color.DarkRed; }
                else
                { _textureEnemyA.Color = Color.White; }
            }

            //ENEMY B RECT + DATA + COLISION
            if (_textureEnemyB.cropTexture != null)
            {
                Color[] enemyBData = new Color[_textureEnemyB.cropTexture.Width * _textureEnemyB.cropTexture.Height];
                _textureEnemyB.cropTexture.GetData(enemyBData);
                Rectangle enemyBRect = new Rectangle(
                    (int)_textureEnemyB.Position.X,
                    (int)_textureEnemyB.Position.Y,
                    _textureEnemyB.cropTexture.Width,
                    _textureEnemyB.cropTexture.Height);

                if (IsIntersectPixels(enemyBRect, enemyBData, mouseRect, mouseData))
                { _textureEnemyB.Color = Color.DarkRed; }
                else
                { _textureEnemyB.Color = Color.White; }
            }

            base.Update(gameTime);
        }
    }
}