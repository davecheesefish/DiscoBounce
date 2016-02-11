using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace DiscoBounce.Screens
{
    class MainMenuScreen:State.GameScreen
    {
        private Texture2D[] logoTex;
        private Texture2D continueTex;
        private Texture2D creditsTex;
        private Texture2D instructionsTex;

        private TimeSpan logoSwitchInterval;
        private TimeSpan logoSwitchTimer;

        private int activeLogo;

        Song bgMusic;

        public MainMenuScreen()
            : base()
        {
            this.logoSwitchInterval = TimeSpan.FromSeconds(0.9);
            this.logoSwitchTimer = this.logoSwitchInterval;
            this.logoTex = new Texture2D[2];

            this.activeLogo = 0;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            this.logoTex[0] = content.Load<Texture2D>("game_logo");
            this.logoTex[1] = content.Load<Texture2D>("game_logo_2");
            this.continueTex = content.Load<Texture2D>("click_to_continue");
            this.creditsTex = content.Load<Texture2D>("advance_to_credits");
            this.instructionsTex = content.Load<Texture2D>("instructions");

            this.bgMusic = content.Load<Song>("music/ether");
            MediaPlayer.Play(this.bgMusic);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.logoSwitchTimer -= gameTime.ElapsedGameTime;
            if (this.logoSwitchTimer < TimeSpan.Zero)
            {
                if (this.activeLogo == 0)
                {
                    this.activeLogo = 1;
                }
                else
                {
                    this.activeLogo = 0;
                }

                this.logoSwitchTimer = this.logoSwitchInterval;
            }

            if (InputManager.Instance.JustLeftClicked())
            {
                MediaPlayer.Stop();
                State.ScreenManager.Instance.GoToScreen(new LevelScreen());
            }

            if (InputManager.Instance.JustRightClicked())
            {
                State.ScreenManager.Instance.GoToScreen(new CreditsScreen());
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            spriteBatch.Draw(logoTex[activeLogo], new Vector2(337.0f, 100.0f), Color.White);
            spriteBatch.Draw(instructionsTex, new Vector2(387.0f, 400.0f), Color.White);
            spriteBatch.Draw(creditsTex, new Vector2(70.0f, 650.0f), Color.White);
            spriteBatch.Draw(continueTex, new Vector2(820.0f, 650.0f), Color.White);
        }
    }
}
