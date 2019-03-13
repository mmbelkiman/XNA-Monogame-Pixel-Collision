using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XNA_Monogame_Pixel_Collision
{
    internal class AnimatedTexture2D
    {
        public Color Color = Color.White;
        public Texture2D cropTexture;
        public Vector2 Position = Vector2.Zero;

        private int _columns = 0;
        private int _currentColumn = 0;
        private int _currentFrame = 0;
        private int _currentLine = 0;
        private float _elapsedTime = 0;
        private int _frameHeight = 0;
        private int _frameWidth = 0;
        private int _lines = 0;
        private Vector2 _origin = Vector2.Zero;
        private Rectangle _source;
        private Texture2D _texture;

        public AnimatedTexture2D(ContentManager content, int columns, int lines, string assetName)
        {
            _texture = content.Load<Texture2D>(assetName);

            _columns = columns;
            _lines = lines;
            _frameWidth = _texture.Width / _columns;
            _frameHeight = _texture.Height / _lines;

            //Set Origin to center
            _origin = new Vector2(
                _frameWidth / 2,
                _frameHeight / 2);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            Rectangle cropSource = new Rectangle(_currentFrame * _frameWidth, 0, _frameWidth, _frameHeight);
            cropTexture = new Texture2D(graphicsDevice, cropSource.Width, cropSource.Height);
            Color[] cropData = new Color[cropSource.Width * cropSource.Height];

            _texture.GetData(0, cropSource, cropData, 0, cropData.Length);
            cropTexture.SetData(cropData);

            spriteBatch.Draw(texture: cropTexture, position: Position, color: Color, scale: new Vector2(1, 1));

            //Original Image animated
            /* spriteBatch.Draw(
                             _texture,
                             Position,
                             _source,
                             Color.White,
                             0,
                             _origin,
                             1,
                             SpriteEffects.None,
                             0f);*/

            Color[] textureData = new Color[_texture.Width * _texture.Height];
            _texture.GetData(textureData);
        }

        public void Update(GameTime gameTime)
        {
            if (gameTime != null)
            {
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            //PLAY NEXT FRAME
            while (_elapsedTime > 0.1f)
            {
                _currentFrame++;
                _currentColumn++;
                _elapsedTime = 0f;
            }

            //RESET VALUES
            if (_currentFrame > _columns - 1)
            {
                _currentFrame = 0;
                _currentColumn = 0;
                _currentLine = 0;
            }

            // Calculate the source rectangle of the current frame.
            _source = new Rectangle(_currentColumn * _frameWidth,
                                    _currentLine * _frameHeight,
                                    _frameWidth,
                                    _frameHeight);
        }
    }
}