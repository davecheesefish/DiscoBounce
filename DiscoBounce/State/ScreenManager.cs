using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DiscoBounce.State
{
    class ScreenManager
    {
        private static ScreenManager instance;
        public Game1 Game;

        public int ScreenWidth { get; private set; }
        public int ScreenHeight { get; private set; }
        public bool isFullScreen { get; private set; }

        private List<GameScreen> screens;
        private List<GameScreen> deadScreens;
        public ContentManager Content { get; private set; }

        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScreenManager();
                }
                return instance;
            }
        }


        public ScreenManager()
        {
            screens = new List<GameScreen>();
            deadScreens = new List<GameScreen>();

            // TODO: Load these in from a file
            this.ScreenWidth = 1024;
            this.ScreenHeight = 768;
            this.isFullScreen = false;
        }

        public void Initialize(Game1 game)
        {
            this.Game = game;
            this.Game.ChangeScreenResolution(this.ScreenWidth, this.ScreenHeight, this.isFullScreen);
        }

        public void LoadContent(ContentManager content)
        {
            this.Content = content;
        }

        public void UnloadContent()
        {
            foreach (GameScreen screen in screens)
            {
                screen.UnloadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            // Copy to avoid exceptions from adding/removing screens while enumerating
            List<GameScreen> screensCopy = new List<GameScreen>(screens);

            foreach (GameScreen screen in screensCopy)
            {
                screen.Update(gameTime);
                if (screen.Disposing)
                {
                    deadScreens.Add(screen);
                }
            }

            // Remove dead screens (have had Close() called on them)
            if (deadScreens.Count > 0)
            {
                foreach (GameScreen screen in deadScreens)
                {
                    screens.Remove(screen);
                }
                deadScreens.Clear();
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (GameScreen screen in screens)
            {
                screen.Draw(gameTime, spriteBatch);
            }
        }

        public void AddScreen(GameScreen screen)
        {
            screen.LoadContent();
            screens.Add(screen);
        }

        public void GoToScreen(GameScreen screen)
        {
            // Close all currently shown screens
            foreach (GameScreen currentScreen in screens)
            {
                currentScreen.Close();
            }

            // Add the new one
            AddScreen(screen);
        }
    }
}
