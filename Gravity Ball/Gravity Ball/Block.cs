using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Gravity_Ball
{
    public class Block : Sprite
    {
        private KeyboardState bPreviousKeyboardState;

        public Vector2 bDirection = Vector2.Zero;
        public Vector2 bSpeed = Vector2.Zero;
        //private bool isVisible;

        const string BLOCK_ASSETNAME = "Block";
        const int BLOCK_SIZE = 90;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        const int START_POSITION_Y = 1280;
        const int END_POSITION_Y = -45;

        //TODO: REDO THIS METHOD
        /*
        private void UpdateMovement(KeyboardState currentKeyboardState, GraphicsDeviceManager deGraphics)
        {
            bSpeed = Vector2.Zero;
            bDirection = Vector2.Zero;

            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                bDirection.X = MOVE_LEFT;
                if (Position.X < 0)
                    bSpeed.X = 0;
                else
                    bSpeed.X = SPEED;
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                bDirection.X = MOVE_RIGHT;
                if (Position.X > deGraphics.GraphicsDevice.Viewport.Width - BLOCK_SIZE)
                    bSpeed.X = 0;
                else
                    bSpeed.X = SPEED;
            }


            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                bDirection.Y = MOVE_UP;
                if (Position.Y < 0)
                    bSpeed.Y = 0;
                else
                    bSpeed.Y = SPEED;
            }

            else if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                bDirection.Y = MOVE_DOWN;
                if (Position.Y > deGraphics.GraphicsDevice.Viewport.Height - BLOCK_SIZE)
                    bSpeed.Y = 0;
                else
                    bSpeed.Y = SPEED;
            }

        }
         * */

        public Block(int a, int b)
        {
            this.Position = new Vector2(a,b);
        }

        public void LoadContent(ContentManager bContentMananger)
        {
            base.LoadContent(bContentMananger, BLOCK_ASSETNAME);
        }

        public void Update(GameTime bGameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();

            //TODO: Have game1 do the collision checks, send what values to change
            //to this function!
            //UpdateMovement(currentKeyboardState, gameGraphics);
            bPreviousKeyboardState = currentKeyboardState;
            base.Update(bGameTime, bSpeed, bDirection);
        }
    }
}
