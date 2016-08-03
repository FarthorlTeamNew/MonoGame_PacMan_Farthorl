﻿namespace GameEngine.Handlers
{
    using Globals;
    using Interfaces;
    using Models.LevelObjects;
    using Microsoft.Xna.Framework;
    using System;
    using System.Collections.Generic;
    using Models;
    public class GhostGoodRandomMovement : ObjectMover
    {
        private PacMan pacman;
        private Random random;
        List<Direction> possibleDirections;

        public GhostGoodRandomMovement(Ghost gameObject, GameEngine.Matrix levelMatrix, PacMan pacman)
            : base(gameObject, levelMatrix)
        {
            this.pacman = pacman;
            pixelMoved = Global.DefaultGhostSpeed;
            random = new Random(DateTime.Now.Millisecond);
            possibleDirections = new List<Direction>();
        }

        private Direction SeePackman()
        {
            // watch left and right
            if (gameObject.QuadrantY == pacman.QuadrantY)
            {
                int distanceToSee = 1;
                while (gameObject.QuadrantX - distanceToSee >= 0
                    && obstacles[gameObject.QuadrantY, gameObject.QuadrantX - distanceToSee] != true)
                {
                    if (gameObject.QuadrantX - distanceToSee == pacman.QuadrantX)
                    {
                        return Direction.Left;
                    }
                    distanceToSee++;
                }

                distanceToSee = 1;
                while (gameObject.QuadrantX + distanceToSee <= Global.XMax - 1
                    && obstacles[gameObject.QuadrantY, gameObject.QuadrantX + distanceToSee] != true)
                {
                    if (gameObject.QuadrantX + distanceToSee == pacman.QuadrantX)
                    {
                        return Direction.Right;
                    }
                    distanceToSee++;
                }
            }

            // watch up and down
            if (gameObject.QuadrantX == pacman.QuadrantX)
            {
                int distanceToSee = 1;
                while (gameObject.QuadrantY - distanceToSee >= 0
                    && obstacles[gameObject.QuadrantY - distanceToSee, gameObject.QuadrantX] != true)
                {
                    if (gameObject.QuadrantY - distanceToSee == pacman.QuadrantY)
                    {
                        return Direction.Up;
                    }
                    distanceToSee++;
                }

                distanceToSee = 1;
                while (gameObject.QuadrantY + distanceToSee <= Global.YMax - 1
                    && obstacles[gameObject.QuadrantY + distanceToSee, gameObject.QuadrantX] != true)
                {
                    if (gameObject.QuadrantY + distanceToSee == pacman.QuadrantY)
                    {
                        return Direction.Down;
                    }
                    distanceToSee++;
                }
            }

            return Direction.None;
        }

        protected override void CalculateDirection(Direction bannedDirection)
        {
            this.possibleDirections.Clear();

            // checks if ghost is can randomize direction if not going back
            // preferred left, right, front
            if (currentDir == Direction.Up)
            {
                if (IsMovingLeftPossible() && bannedDirection != Direction.Left)
                {
                    possibleDirections.Add(Direction.Left);
                }
                if (IsMovingRightPossible() && bannedDirection != Direction.Right)
                {
                    possibleDirections.Add(Direction.Right);
                }
                if (IsMovingUpPossible() && bannedDirection != Direction.Up)
                {
                    possibleDirections.Add(Direction.Up);
                }
                if (IsMovingDownPossible() && possibleDirections.Count == 0 && bannedDirection != Direction.Down) // go back
                {
                    currentDir = Direction.Down;
                    return;
                }
                else if (possibleDirections.Count == 0)
                {
                    currentDir = Direction.None;
                }
            }
            else if (currentDir == Direction.Down)
            {
                if (IsMovingLeftPossible() && bannedDirection != Direction.Left)
                {
                    possibleDirections.Add(Direction.Left);
                }
                if (IsMovingRightPossible() && bannedDirection != Direction.Right)
                {
                    possibleDirections.Add(Direction.Right);
                }
                if (IsMovingDownPossible() && bannedDirection != Direction.Down)
                {
                    possibleDirections.Add(Direction.Down);
                }
                if (IsMovingUpPossible() && possibleDirections.Count == 0 && bannedDirection != Direction.Up) // go back
                {
                    currentDir = Direction.Up;
                    return;
                }
                else if (possibleDirections.Count == 0)
                {
                    currentDir = Direction.None;
                    return;
                }
            }
            else if (currentDir == Direction.Left)
            {
                if (IsMovingLeftPossible() && bannedDirection != Direction.Left)
                {
                    possibleDirections.Add(Direction.Left);
                }
                if (IsMovingDownPossible() && bannedDirection != Direction.Down)
                {
                    possibleDirections.Add(Direction.Down);
                }
                if (IsMovingUpPossible() && bannedDirection != Direction.Up)
                {
                    possibleDirections.Add(Direction.Up);
                }
                if (IsMovingRightPossible() && possibleDirections.Count == 0 && bannedDirection != Direction.Right) // go back
                {
                    currentDir = Direction.Right;
                    return;
                }
                else if (possibleDirections.Count == 0)
                {
                    currentDir = Direction.None;
                    return;
                }
            }
            else if (currentDir == Direction.Right)
            {
                if (IsMovingRightPossible() && bannedDirection != Direction.Right)
                {
                    possibleDirections.Add(Direction.Right);
                }
                if (IsMovingDownPossible() && bannedDirection != Direction.Down)
                {
                    possibleDirections.Add(Direction.Down);
                }
                if (IsMovingUpPossible() && bannedDirection != Direction.Up)
                {
                    possibleDirections.Add(Direction.Up);
                }
                if (IsMovingLeftPossible() && possibleDirections.Count == 0 && bannedDirection != Direction.Left) // go back
                {
                    currentDir = Direction.Left;
                    return;
                }
                else if (possibleDirections.Count == 0)
                {
                    currentDir = Direction.None;
                    return;
                }
            }
            else if (currentDir == Direction.None && bannedDirection == Direction.None)
            {
                if (IsMovingLeftPossible())
                {
                    possibleDirections.Add(Direction.Left);
                }
                if (IsMovingDownPossible())
                {
                    possibleDirections.Add(Direction.Down);
                }
                if (IsMovingUpPossible())
                {
                    possibleDirections.Add(Direction.Up);
                }
                if (IsMovingRightPossible())
                {
                    possibleDirections.Add(Direction.Right);
                }
            }

            if (possibleDirections.Count > 0)
            {
                ChooseRandomDirection();
            }
        }

        private void ChooseRandomDirection()
        {
            if (this.possibleDirections.Count == 1)
            {
                currentDir = this.possibleDirections[0];
            }
            else
            {
                Direction randomDir = (Direction)this.possibleDirections.ToArray().GetValue(random.Next(this.possibleDirections.Count));
                currentDir = randomDir;
            }
        }

        protected override Vector2 GetNextPointToMove()
        {
            if (base.IsReadyToChangeQuadrant())
            {
                Direction directionToPacman = SeePackman();
                if (directionToPacman != Direction.None )
                {
                    CalculateDirection(directionToPacman);
                }
                else
                {
                    CalculateDirection(Direction.None);
                }
            }

            Vector2 nextPointToMove = new Vector2();
            switch (this.currentDir)
            {
                case Direction.Up:
                    nextPointToMove.X = 0;
                    nextPointToMove.Y = 0 - base.pixelMoved;
                    break;
                case Direction.Down:
                    nextPointToMove.X = 0;
                    nextPointToMove.Y = base.pixelMoved;
                    break;
                case Direction.Left:
                    nextPointToMove.X = 0 - base.pixelMoved;
                    nextPointToMove.Y = 0;
                    break;
                case Direction.Right:
                    nextPointToMove.X = base.pixelMoved;
                    nextPointToMove.Y = 0;
                    break;
                case Direction.None:
                    nextPointToMove.X = 0;
                    nextPointToMove.Y = 0;
                    break;
            }

            return nextPointToMove;
        }

        public override void DecreaseSpeed()
        {
            this.pixelMoved--;
        }

        public override void GetDrunkThenRehab()
        {
        }
    }
}
