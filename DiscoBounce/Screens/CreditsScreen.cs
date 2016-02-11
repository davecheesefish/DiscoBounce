using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace DiscoBounce.Screens
{
    class CreditsScreen:State.GameScreen
    {
        private Texture2D logoTex;
        private Texture2D continueTex;
        private Texture2D creditsTex;
        private Texture2D websiteTex;

        //Song bgMusic;

        public CreditsScreen()
            : base()
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            this.logoTex = content.Load<Texture2D>("game_logo");
            this.continueTex = content.Load<Texture2D>("click_to_continue");
            this.creditsTex = content.Load<Texture2D>("credits");
            this.websiteTex = content.Load<Texture2D>("open_dcfgames");

            //this.bgMusic = content.Load<Song>("music/overcast");
            //MediaPlayer.Play(this.bgMusic);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.Instance.JustLeftClicked())
            {
                State.ScreenManager.Instance.GoToScreen(new Screens.MainMenuScreen());
            }

            if (InputManager.Instance.JustRightClicked())
            {
                // Open davecheesefish.com
                Process.Start("http://davecheesefish.com");
                MediaPlayer.Stop();
            }
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            spriteBatch.Draw(logoTex, new Vector2(320.0f, 200.0f), Color.White);
            spriteBatch.Draw(websiteTex, new Vector2(70.0f, 650.0f), Color.White);
            spriteBatch.Draw(creditsTex, new Vector2(112.0f, 50.0f), Color.White);
            spriteBatch.Draw(continueTex, new Vector2(820.0f, 650.0f), Color.White);
        }

        public override void Close()
        {
            //MediaPlayer.Stop();
            base.Close();
        }
    }
}
