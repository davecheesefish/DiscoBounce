using System;
using Microsoft.Xna.Framework;

namespace DiscoBounce
{
    enum CameraMotionState
    {
        Fixed, Panning
    }

    class Camera
    {
        private Vector2 position;
        private float zoom;

        private Vector3 translationVector;
        private Matrix translationMatrix;

        private Vector3 scaleVector;
        private Matrix scaleMatrix;

        private Matrix transformMatrix;
        private bool transformMatrixDirty;

        private Vector2 panOrigin;
        private Vector2 panTarget;

        private TimeSpan panDuration;
        private TimeSpan panTimeLeft;

        private CameraMotionState motionState;

        public delegate void CameraEventHandler();
        public event CameraEventHandler PanCompleted;

        /// <summary>
        /// The position of the camera in world space.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                transformMatrixDirty = true;
            }
        }

        public float Zoom
        {
            get { return zoom; }
            set
            {
                zoom = value;
                transformMatrixDirty = true;
            }
        }


        public Camera()
        {
            position = Vector2.Zero;
            zoom = 1.0f;
            transformMatrixDirty = true;
            motionState = CameraMotionState.Fixed;
        }

        /// <summary>
        /// Pans the camera to a specified point in the world with a given duration.
        /// </summary>
        /// <param name="target">The target point in world space.</param>
        /// <param name="duration">How long the pan should take.</param>
        public void PanTo(Vector2 target, TimeSpan duration)
        {
            panOrigin = Position;
            panTarget = target;

            panDuration = duration;
            panTimeLeft = duration;

            motionState = CameraMotionState.Panning;
        }

        /// <summary>
        /// Pans the camera to a specified point in the world with a given average speed.
        /// </summary>
        /// <param name="target">The target point in world space.</param>
        /// <param name="speed">The average speed of the camera, in world units per second.</param>
        public void PanTo(Vector2 target, float speed)
        {
            // Convert to a duration
            float distance = (target - Position).Length();
            TimeSpan time = TimeSpan.FromSeconds(distance / speed);

            // Call the first overload of this function
            this.PanTo(target, time);
        }

        /// <summary>
        /// Moves the camera immediately to the end of the current pan.
        /// </summary>
        public void CancelPan()
        {
            if (motionState == CameraMotionState.Panning)
            {
                Position = panTarget;
                motionState = CameraMotionState.Fixed;
                if (PanCompleted != null)
                {
                    PanCompleted();
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            // Update panning motion
            if (motionState == CameraMotionState.Panning)
            {
                panTimeLeft -= gameTime.ElapsedGameTime;
                if (panTimeLeft < TimeSpan.Zero)
                {
                    panTimeLeft = TimeSpan.Zero;
                    motionState = CameraMotionState.Fixed;
                    Position = panTarget;
                    if (PanCompleted != null)
                    {
                        PanCompleted();
                    }
                }
                else
                {
                    double panPercentage = (panDuration.TotalMilliseconds - panTimeLeft.TotalMilliseconds) / panDuration.TotalMilliseconds;
                    Position = Vector2.SmoothStep(panOrigin, panTarget, (float)panPercentage);
                }
            }
        }

        /// <summary>
        /// Gets the transformation matrix of the camera for use with SpriteBatch.Begin().
        /// </summary>
        /// <returns>This camera's transformation matrix.</returns>
        public Matrix GetTransformMatrix()
        {
            if (transformMatrixDirty)
            {
                Vector2 screenPosition = zoom * Level.TwoDToIso(position);
                translationVector = new Vector3(-(float)Math.Floor(screenPosition.X - (float)State.ScreenManager.Instance.ScreenWidth / 2.0f), -(float)Math.Floor(screenPosition.Y - (float)State.ScreenManager.Instance.ScreenHeight / 2.0f), 0.0f);
                scaleVector = new Vector3(zoom, zoom, 1);

                Matrix.CreateTranslation(ref translationVector, out translationMatrix);
                Matrix.CreateScale(ref scaleVector, out scaleMatrix);

                transformMatrix = scaleMatrix * translationMatrix;
            }

            return transformMatrix;
        }
    }
}
