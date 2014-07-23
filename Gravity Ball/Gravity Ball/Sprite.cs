// Sprite Class
// Sprites are objects in a game that can be interacted with.
// In the case of Blocking Gravity, there are two: the ball and the block
// Both the ball and the block have the same properties; they move in certain directions,
// have positions, a texture representing them, and are moved by the game itself
// Note that good object oriented programming guides that classes should have function that 
// change the values they contain, but should not actually change the values in the class;
// that is for the main game logic.

// Using statements
// These statements tell the game to call for certain libraries in the C# and XNA frameworks
// so that certain game functions can be implemented. 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics; 

namespace Blocking_Gravity
{
    public class Sprite
    {
        // Sprite Position
        // The position of a sprite is represented by a variable type called a vector. 
        // A vector has an X and a Y component, and it can be retrieved and set in and out
        // of the game
        public Vector2 Position { get; set; }

        // Texture Object used when drawing the sprite
        // A Texture2D object is essentiall an image represented on the actual screen seen by the player. 
        // Note that it is 2D, meaning two dimensional. The ball and the block each have two different 
        // textures. 
        private Texture2D mSpriteTexture;

        // BoundingBox
        // This is one of the most crucial elements in the game. A BoundingBox is a rectangle that encompasses
        // the width and height of the sprite. It is measured from the bottom left corner of the sprite, depending
        // on its position to the top right corner of the sprite. This rectangle differs from the ball
        // and the block.
        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    mSpriteTexture.Width,
                    mSpriteTexture.Height);
            }
        }

        // Sprite constructor
        // This function is called when a Sprite object is instantly made, and no paramaters are sent to it.
        // This creates the sprite and sets it to the top left corner of the screen. 
        public Sprite()
        {
            Position = new Vector2(0, 0);
        }

        // Sprite constructor 2
        // This function is called when a Sprite object is instantly made, and two integers are sent to this function.
        // This creates the sprite and sets it to a location based on the value of the integers. The first integer
        // represents the X position on the screen, and the second integer represents the Y position on the screen. 
        public Sprite(int x, int y)
        {
            Position = new Vector2(x, y);
        }

        // Load the texture using the Content Pipeline
        // This is a function special to the XNA framework. It takes in what is called the content manager and a text line
        // or string, and loads an image file to represent the sprite into the texture file of the sprite. 
        // If this function is not called, the sprite will never be fully initialized and an error will occur. 
        public void LoadContent(ContentManager theContentManager, string assetName)
        {
            mSpriteTexture = theContentManager.Load<Texture2D>(assetName);
        }

        // Draw function
        // This is another function special to the XNA framework. It requires a parameter of what is called a Spritebatch,
        // a special class that enables the drawing of multiple sprites to the screen. The function takes the sprite batch
        // and uses it to draw the sprite's texture to the screen at the position the sprite currently has. The color parameter
        // determines the tint that the sprite is drawn; passing white means that no tint is to be applied to the sprite texture. 
        public void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(mSpriteTexture, Position, Color.White);
        }

        // Update function
        // As yet another specialty to XNA, this function takes in a specified time in game, a speed represented by a vector, and a 
        // direction specified by another vector, and multiplies them all together. The resulting vector product is then added to 
        // the current position vector in the Sprite class. 
        public void Update(GameTime gametime, Vector2 speed, Vector2 direction)
        {
            Position += direction * speed * (float)gametime.ElapsedGameTime.TotalSeconds;
        }

        // Update function 2
        // This function differs from the first Update function in that only the Game time and a vector called movement is passed. 
        // The movement function is simply added to the position vector, updating it. This function is more used if a sprite has 
        // a predetermined vector in the main game class that determines its movement. 
        public void Update(GameTime gametime, Vector2 movement)
        {
            Position += movement;
        }
    }
}
