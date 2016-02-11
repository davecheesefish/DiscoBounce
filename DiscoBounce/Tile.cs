using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DiscoBounce
{
    class Tile
    {
        public const int SIDE_LENGTH = 50;

        public Vector2 Position { get; private set; }
        public bool IsActive { get; set; }

        private Texture2D texture;

        public Color Colour;

        private TimeSpan flashTime;
        private TimeSpan flashTimer;

        public Vector2 CentrePoint
        {
            get
            {
                return new Vector2(Position.X + 0.5f * (float)Tile.SIDE_LENGTH, Position.Y + 0.5f * (float)Tile.SIDE_LENGTH);
            }
        }

        public Tile(Vector2 position)
        {
            this.Position = position;
            this.Colour = Color.White;

            this.flashTime = TimeSpan.FromSeconds(0.5);
            this.flashTimer = TimeSpan.Zero;
        }

        public void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("tile");
        }

        public void Update(GameTime gameTime)
        {
            if (flashTimer > TimeSpan.Zero)
            {
                flashTimer -= gameTime.ElapsedGameTime;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color drawCol = this.Colour;
            if (this.flashTimer > TimeSpan.Zero)
            {
                drawCol = Color.Lerp(this.Colour, Color.White, (float)(this.flashTimer.TotalMilliseconds / this.flashTime.TotalMilliseconds));
            }

            spriteBatch.Draw(texture, Level.TwoDToIso(Position), null, drawCol, 0.0f, new Vector2(Tile.SIDE_LENGTH, 0), 1.0f, SpriteEffects.None, 1.0f);
        }

        public void Flash()
        {
            this.flashTimer = this.flashTime;
        }
    }
}
