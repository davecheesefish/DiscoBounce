using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DiscoBounce
{
    class InputManager
    {
        private Game game;
        
        private static InputManager instance;

        private KeyboardState currentKeyState;
        private KeyboardState lastKeyState;

        private MouseState currentMouseState;
        private MouseState lastMouseState;

        private bool mouseLocked;
        private Vector2 lockedMousePosition;

        public Vector2 MousePosition
        {
            get
            {
                return new Vector2(currentMouseState.X, currentMouseState.Y);
            }
        }

        /// <summary>
        /// If set to true, all input-checking functions will return FALSE until the next update.
        /// </summary>
        public bool InputHandled;

        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputManager();
                }
                return instance;
            }
        }

        public InputManager()
        {
            currentKeyState = Keyboard.GetState();
            lastKeyState = currentKeyState;

            currentMouseState = Mouse.GetState();
            lastMouseState = currentMouseState;

            mouseLocked = false;
            lockedMousePosition = new Vector2(400, 300);
        }

        public void Initialize(Game game)
        {
            this.game = game;
        }

        public void Update()
        {
            lastKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (mouseLocked)
            {
                Mouse.SetPosition((int)lockedMousePosition.X, (int)lockedMousePosition.Y);
            }

            InputHandled = false;
        }

        public bool KeyJustPressed(Keys key)
        {
            return game.IsActive && !InputHandled && (currentKeyState.IsKeyDown(key) && lastKeyState.IsKeyUp(key));
        }

        public bool KeyJustReleased(Keys key)
        {
            return game.IsActive && !InputHandled && (currentKeyState.IsKeyUp(key) && lastKeyState.IsKeyDown(key));
        }

        public void LockMouse()
        {
            this.mouseLocked = true;
            Mouse.SetPosition((int)lockedMousePosition.X, (int)lockedMousePosition.Y);
        }

        public void UnlockMouse()
        {
            this.mouseLocked = false;
        }

        public Vector2 GetMouseDiff()
        {
            return MousePosition - lockedMousePosition;
        }

        public bool JustLeftClicked()
        {
            return (game.IsActive && lastMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released);
        }

        public bool JustRightClicked()
        {
            return (game.IsActive && lastMouseState.RightButton == ButtonState.Pressed && currentMouseState.RightButton == ButtonState.Released);
        }
    }
}
