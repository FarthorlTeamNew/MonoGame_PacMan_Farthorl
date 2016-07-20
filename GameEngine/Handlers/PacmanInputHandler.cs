﻿using GameEngine.Globals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Handlers
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        None
    }

    public class PacmanInputHandler
    {
        private PacMan pacman;
        private Direction currentDir;
        private Direction desiredDir;
        private bool[,] obsticals; 

        public PacmanInputHandler(PacMan pacMan, Matrix levelMatrix)
        {
            this.pacman = pacMan;
            currentDir = Direction.None;
            desiredDir = Direction.None;
            obsticals = new bool[Global.YMax, Global.XMax];

            for (int i = 0; i < Global.YMax; i++)
            {
                for (int j = 0; j < Global.XMax; j++)
                {
                    string obstical = levelMatrix.PathsMatrix[i, j].Trim().Split(',')[0];
                    obsticals[i, j] = obstical == "1";
                }
            }
        }

        public void Reset()
        {
            currentDir = Direction.None;
            desiredDir = Direction.None;
        }

        private void GetInput()
        {
            KeyboardState state = Keyboard.GetState();
            //Point point = new Point();

            if (state.GetPressedKeys().Length == 1)
            {
                if (state.IsKeyDown(Keys.Down))
                {
                    //point.Y += 15;
                    desiredDir = Direction.Down;
                }
                else if (state.IsKeyDown(Keys.Up))
                {
                    //point.Y -= 15;
                    desiredDir = Direction.Up;
                }
                else if (state.IsKeyDown(Keys.Left))
                {
                    //point.X -= 15;
                    desiredDir = Direction.Left;
                }
                else if (state.IsKeyDown(Keys.Right))
                {
                    //point.X += 15;
                    desiredDir = Direction.Right;
                }
            }
        }

        private void CalculateDirection()
        {
            if (desiredDir == currentDir)
            {
                if (currentDir == Direction.Up
                    && (pacman.QuadrantY == 0
                    || obsticals[pacman.QuadrantY - 1, pacman.QuadrantX] == true))
                {
                    currentDir = Direction.None;
                }
                else if (currentDir == Direction.Down
                    && (pacman.QuadrantY == (Global.YMax - 1)
                    || obsticals[pacman.QuadrantY + 1, pacman.QuadrantX] == true))
                { 
                    currentDir = Direction.None;
                }
                else if (currentDir == Direction.Left
                    && (pacman.QuadrantX == 0
                    || obsticals[pacman.QuadrantY, pacman.QuadrantX - 1] == true))
                {
                    currentDir = Direction.None;
                }
                else if (currentDir == Direction.Right
                   && (pacman.QuadrantX == (Global.XMax - 1)
                   || obsticals[pacman.QuadrantY, pacman.QuadrantX + 1] == true))
                {
                    currentDir = Direction.None;
                }
            }
            else
            {
                if (desiredDir == Direction.Up
                    && pacman.QuadrantY > 0)
                {
                    if (obsticals[pacman.QuadrantY - 1, pacman.QuadrantX] == false)
                        currentDir = desiredDir;
                }
                else if (desiredDir == Direction.Down
                    && pacman.QuadrantY < (Global.YMax - 1))
                {
                    if (obsticals[pacman.QuadrantY + 1, pacman.QuadrantX] == false)
                        currentDir = desiredDir;
                }
                else if (desiredDir == Direction.Left
                    && pacman.QuadrantX > 0)
                {
                    if (obsticals[pacman.QuadrantY, pacman.QuadrantX - 1] == false)
                        currentDir = desiredDir;
                }
                else if (desiredDir == Direction.Right
                    && pacman.QuadrantX < (Global.XMax - 1))
                {
                    if (obsticals[pacman.QuadrantY, pacman.QuadrantX + 1] == false)
                        currentDir = desiredDir;
                }
            }
        }

        private bool IsReadyToChangePackmanQuadrant()
        {
            if (pacman.X % Global.quad_Width == 0
                && pacman.Y % Global.quad_Height == 0)
            {
                pacman.QuadrantX = (int)pacman.X / 32;
                pacman.QuadrantY = (int)pacman.Y / 32;
                return true;
            }

            return false;
        }

        private Vector2 GetDesiredVelocityFromInput()
        {
            //var newPoint = this.GetInput();
            Vector2 desiredVelocity = new Vector2();
            this.GetInput(); // listens for key pressed

            if (IsReadyToChangePackmanQuadrant())
            {
                CalculateDirection();
            }
            //if ((this.pacman.X + newPoint.X) + 17 < Global.GLOBAL_WIDTH && (this.pacman.X + newPoint.X) + 13 >= 0 &&
            //    (this.pacman.Y + newPoint.Y) + 17 < Global.GLOBAL_HEIGHT && (this.pacman.Y + newPoint.Y) + 13 >= 0)
            //{

            switch (currentDir)
            {
                case Direction.Up:
                    desiredVelocity.X = 0;
                    desiredVelocity.Y = -4; // this magic number is velocity(pixels per gameTime) and he must devide 32(Global.quad_Width) with reminder 0
                    break;
                case Direction.Down:
                    desiredVelocity.X = 0;
                    desiredVelocity.Y = 4;
                    break;
                case Direction.Left:
                    desiredVelocity.X = -4;
                    desiredVelocity.Y = 0;
                    break;
                case Direction.Right:
                    desiredVelocity.X = 4;
                    desiredVelocity.Y = 0;
                    break;
                case Direction.None:
                    desiredVelocity.X = 0;
                    desiredVelocity.Y = 0;
                    break;
            }

            //desiredVelocity.X = newPoint.X;
            //    desiredVelocity.Y = newPoint.Y;

            //    if (desiredVelocity.X != 0 || desiredVelocity.Y != 0)
            //    {
            //        desiredVelocity.Normalize();
            //        float desiredSpeed = Global.PacmanSpeed;
            //        desiredVelocity *= desiredSpeed;
            //    }
            //}

            return desiredVelocity;
        }

        public Vector2 Move(GameTime gameTime)
        {
            var velocity = GetDesiredVelocityFromInput();

            this.pacman.X += velocity.X /** (float)gameTime.ElapsedGameTime.TotalSeconds*/;
            this.pacman.Y += velocity.Y /** (float)gameTime.ElapsedGameTime.TotalSeconds*/;
            this.pacman.UpdateBoundingBox();

            return velocity;
        }
    }
}