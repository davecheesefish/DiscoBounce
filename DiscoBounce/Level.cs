using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace DiscoBounce
{
    class MapRow
    {
        public List<Tile> Columns;
    }

    class Level
    {
        public const int MAP_WIDTH = 8;
        public const int MAP_HEIGHT = 8;

        public List<MapRow> Rows;
        public Rectangle Bounds;
        public Player Player;

        public Camera Camera { get; set; }

        public float Gravity;

        public int Score;
        public int Stage;

        public List<Color> PossibleColours;

        private Song discoSong;
        private SpriteFont hudFont;

        public Level()
        {
            Gravity = 358.0f;
            this.Score = 0;
            this.Stage = 1;

            Player = new Player(this, new Vector3(0.5f * (float)(MAP_WIDTH * Tile.SIDE_LENGTH), 0.5f * (float)(MAP_HEIGHT * Tile.SIDE_LENGTH), 50));
            Camera = new Camera();
            Camera.Position = new Vector2(Tile.SIDE_LENGTH * 4.0f);

            this.PossibleColours = new List<Color>();
            this.PossibleColours.Add(Color.Crimson);
            this.PossibleColours.Add(Color.Orange);
            //this.PossibleColours.Add(Color.Gold); // Too close to orange
            this.PossibleColours.Add(Color.MediumSpringGreen);
            this.PossibleColours.Add(Color.PowderBlue);
            //this.PossibleColours.Add(Color.CornflowerBlue); // Too close to purple
            this.PossibleColours.Add(Color.MediumPurple);
            this.PossibleColours.Add(Color.White);

            Rows = new List<MapRow>();
            for (int y = 0; y < MAP_HEIGHT; ++y)
            {
                MapRow newRow = new MapRow();
                newRow.Columns = new List<Tile>();
                for (int x = 0; x < MAP_WIDTH; ++x)
                {
                    newRow.Columns.Add(new Tile(new Vector2(x * Tile.SIDE_LENGTH, y * Tile.SIDE_LENGTH)));
                }

                Rows.Add(newRow);
            }

            this.Bounds = new Rectangle(0, 0, MAP_WIDTH * Tile.SIDE_LENGTH, MAP_HEIGHT * Tile.SIDE_LENGTH);

            InputManager.Instance.LockMouse();
        }

        public void LoadContent(ContentManager content)
        {
            Player.LoadContent(content);

            foreach (MapRow row in Rows)
            {
                foreach (Tile tile in row.Columns)
                {
                    tile.LoadContent(content);
                }
            }

            discoSong = content.Load<Song>("music/disco");
            MediaPlayer.Play(discoSong);
            MediaPlayer.IsRepeating = true;

            hudFont = content.Load<SpriteFont>("fonts/hud");
        }

        public void Update(GameTime gameTime)
        {
            foreach (MapRow row in Rows)
            {
                foreach (Tile tile in row.Columns)
                {
                    tile.Update(gameTime);
                }
            }

            this.Player.Update(gameTime);
            if (this.Player.Position.Z <= 0)
            {
                OnPlayerHitFloor();
            }

            //this.Camera.Position = this.Player.TwoDPosition;
            this.Camera.Update(gameTime);

            this.Score = this.Player.NumberOfBounces;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (MapRow row in Rows)
            {
                foreach (Tile tile in row.Columns)
                {
                    tile.Draw(spriteBatch);
                }
            }

            this.Player.Draw(spriteBatch);

            Vector2 textOffset = - hudFont.MeasureString(this.Score.ToString()) * new Vector2(0.5f, 0.0f);
            spriteBatch.DrawString(hudFont, this.Score.ToString(), new Vector2(0, -30) + textOffset, Color.White);
            textOffset = -hudFont.MeasureString("Stage " + this.Stage.ToString()) * new Vector2(0.5f, 0.0f);
            spriteBatch.DrawString(hudFont, "Stage " + this.Stage.ToString(), Level.TwoDToIso(new Vector2(Bounds.Right, Bounds.Bottom + 10)) + textOffset, Color.White);
        }

        private void OnPlayerHitFloor()
        {
            Vector2 playerPos = Player.TwoDPosition;

            int playerTileX = (int)Math.Floor((float)(Player.TwoDPosition.X / Tile.SIDE_LENGTH));
            int playerTileY = (int)Math.Floor((float)(Player.TwoDPosition.Y / Tile.SIDE_LENGTH));
            // TODO: Fix player reporting position too far to the right/drawing too far to the left
            Tile tileHit = Rows[playerTileY].Columns[playerTileX];
            tileHit.Flash();

            if (tileHit.Colour == Player.Colour)
            {
                this.Player.Bounce();
                this.Stage = (Player.NumberOfBounces / 15) + 1;
                RandomiseTiles();

                int randomRow = Utilities.Instance.Random.Next(MAP_HEIGHT);
                int randomCol = Utilities.Instance.Random.Next(MAP_WIDTH);

                this.Player.Colour = this.Rows[randomRow].Columns[randomCol].Colour;
            }
            else
            {
                State.ScreenManager.Instance.GoToScreen(new Screens.EndGameScreen(Score));
            }
        }

        public void RandomiseTiles()
        {
            int colourCount = Math.Min(PossibleColours.Count, Stage + 1);
            Console.WriteLine(Player.NumberOfBounces.ToString());

            foreach (MapRow row in Rows)
            {
                foreach (Tile tile in row.Columns)
                {
                    tile.Colour = PossibleColours[Utilities.Instance.Random.Next(colourCount)];
                }
            }
        }

        public static Vector2 TwoDToIso(Vector2 twoDPosition)
        {
            return new Vector2(twoDPosition.X - twoDPosition.Y, (twoDPosition.X + twoDPosition.Y) / 2.0f);
        }

        public static Vector2 isoToTwoD(Vector2 isoPosition)
        {
            return new Vector2((2.0f * isoPosition.Y + isoPosition.X) / 2.0f, (2.0f * isoPosition.Y - isoPosition.X) / 2.0f);
        }

        public static Vector2 ThreeDToIso(Vector3 threeDPosition)
        {
            return new Vector2(threeDPosition.X - threeDPosition.Y, ((threeDPosition.X + threeDPosition.Y) / 2.0f) - threeDPosition.Z);
        }
    }
}
