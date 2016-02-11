using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace DiscoBounce.Screens
{
    class EndGameScreen:State.GameScreen
    {
        private int score;
        private SpriteFont scoreFont;

        private Texture2D replayTex;
        private Texture2D homeTex;

        Vector2 scorePos;

        public EndGameScreen(int score)
            : base()
        {
            this.score = score;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            this.scoreFont = content.Load<SpriteFont>("fonts/end_game_score");
            this.replayTex = content.Load<Texture2D>("click_to_continue");
            this.homeTex = content.Load<Texture2D>("advance_home");

            scorePos = new Vector2((State.ScreenManager.Instance.ScreenWidth - scoreFont.MeasureString(score.ToString()).X) / 2, 150);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.Instance.JustLeftClicked())
            {
                State.ScreenManager.Instance.GoToScreen(new Screens.LevelScreen());
            }

            if (InputManager.Instance.JustRightClicked())
            {
                State.ScreenManager.Instance.GoToScreen(new Screens.MainMenuScreen());
            }
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            spriteBatch.DrawString(scoreFont, score.ToString(), scorePos, Color.White);
            spriteBatch.Draw(homeTex, new Vector2(70.0f, 650.0f), Color.White);
            spriteBatch.Draw(replayTex, new Vector2(820.0f, 650.0f), Color.White);
        }

        public override void Close()
        {
            MediaPlayer.Stop();
            base.Close();
        }
    }
}
