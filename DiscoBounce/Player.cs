using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DiscoBounce
{
    class Player
    {
        private Level level;
        public Vector3 Position;

        private float verticalVelocity;

        private Texture2D texture;
        private Texture2D crossHairTexture;

        public Color Colour;

        public int NumberOfBounces { get; private set; }

        public Vector2 TwoDPosition
        {
            get
            {
                return new Vector2(Position.X, Position.Y);
            }
        }

        public Player(Level level, Vector3 position)
        {
            this.level = level;
            this.Position = position;

            this.verticalVelocity = 0;
            this.NumberOfBounces = 0;

            this.Colour = Color.White;
        }

        public void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("player");
            this.crossHairTexture = content.Load<Texture2D>("crosshair");
        }

        public void Update(GameTime gameTime)
        {
            this.verticalVelocity -= (float)(level.Gravity * gameTime.ElapsedGameTime.TotalSeconds);

            this.Position = new Vector3(this.Position.X, this.Position.Y, (float)(this.Position.Z + this.verticalVelocity * gameTime.ElapsedGameTime.TotalSeconds));

            MoveTo(this.TwoDPosition + Level.isoToTwoD(InputManager.Instance.GetMouseDiff()));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.crossHairTexture, Level.TwoDToIso(this.TwoDPosition), null, Color.White, 0.0f, new Vector2(10, 5), 1.0f, SpriteEffects.None, 0.1f);
            spriteBatch.Draw(this.texture, Level.ThreeDToIso(this.Position), null, this.Colour, 0.0f, new Vector2(7, 15), 1.0f, SpriteEffects.None, 0.0f);
        }

        public void MoveTo(Vector2 position)
        {
            if (position.X >= level.Bounds.Left && position.X < level.Bounds.Right)
            {
                this.Position.X = position.X;
            }

            if (position.Y >= level.Bounds.Top && position.Y < level.Bounds.Bottom)
            {
                this.Position.Y = position.Y;
            }
        }

        public void Bounce()
        {
            this.Position = new Vector3(this.Position.X, this.Position.Y, 0);
            this.verticalVelocity = 172;//Math.Max(90 - this.NumberOfBounces, 45);

            ++this.NumberOfBounces;
        }
    }
}
