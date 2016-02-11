using Microsoft.Xna.Framework.Graphics;

namespace DiscoBounce.Screens
{
    class LevelScreen:State.GameScreen
    {
        private Level level;
        private SpriteBatch levelSpriteBatch;

        public LevelScreen()
            : base()
        {
            this.level = new Level();
            this.levelSpriteBatch = new SpriteBatch(State.ScreenManager.Instance.Game.GraphicsDevice);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            level.LoadContent(content);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            level.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            levelSpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, level.Camera.GetTransformMatrix());
            level.Draw(levelSpriteBatch);
            levelSpriteBatch.End();
        }

        public override void Close()
        {
            InputManager.Instance.UnlockMouse();
            base.Close();
        }
    }
}
