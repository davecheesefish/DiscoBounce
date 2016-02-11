using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DiscoBounce.State
{
    abstract class GameScreen
    {
        protected ContentManager content;
        protected bool isContentLoaded;

        public delegate void ScreenChangeEventHandler(GameScreen screen);
        public event ScreenChangeEventHandler Closed;

        public bool Disposing { get; private set; }

        public GameScreen()
        {
            isContentLoaded = false;
        }

        public virtual void LoadContent()
        {
            if (!isContentLoaded)
            {
                this.content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
                isContentLoaded = true;
            }
        }

        public virtual void UnloadContent()
        {
            if (isContentLoaded)
            {
                content.Unload();
            }
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public virtual void Close()
        {
            UnloadContent();

            // Call Closed event handler
            if (Closed != null)
            {
                Closed(this);
            }
            Disposing = true;
        }
    }
}
