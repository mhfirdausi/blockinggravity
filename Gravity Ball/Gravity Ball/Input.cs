// Input class
// This class handles all of the input in the game. It takes in keyboard states and sends back
// whether certain keys are pressed on the keyboard. Since this is a fairly simple class, not as 
// many libraries need to be called. 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Blocking_Gravity
{
    public class Input 
    {
        // There are two keyboard states; one holds the current keyboard state, and the other holds 
        // the previous state the keyboard was in. This is to prevent the menu iterator moving too fast
        // when the up or down key is depressed. 
        private KeyboardState keyboardState;
        private KeyboardState lastState;

        // This value, when called, checks the current keyboard state and returns true if the right arrow key is pressed. 
        public bool RightArrow
        {
            get
            {
                return keyboardState.IsKeyDown(Keys.Right);
            }
        }

        // This value, when called, checks the current keyboard state and returns true if the left arrow key is pressed.
        public bool LeftArrow
        {
            get
            {
                return keyboardState.IsKeyDown(Keys.Left);
            }
        }

        // This value, when called, checks the current keyboard state and returns true if the D key is pressed.
        // This is specific to the WASD keyboard option, which is often used as a second arrow key option. 
        // I have not implemented this as of yet, but I may in the future. 
        public bool RightWASD
        {
            get
            {
                return keyboardState.IsKeyDown(Keys.D);
            }
        }

        // This value, when called, checks the current keyboard state and returns true if the A key is pressed.
        // This is specific to the WASD keyboard option, which is often used as a second arrow key collection. 
        // I have not implemented this as of yet, but I may in the future.
        public bool LeftWASD
        {
            get
            {
                return keyboardState.IsKeyDown(Keys.A);
            }
        }

        // This value, when called, checks the current keyboard state and returns true if the enter key is pressed
        // AND if it was not previously pressed in the last keyboard state. 
        public bool MenuSelect
        {

            get
            {
                return keyboardState.IsKeyDown(Keys.Enter) && lastState.IsKeyUp(Keys.Enter);
            }

        }

        // This value, when called, checks the current keyboard state and returns true if the enter key is pressed
        // AND if it was not previously pressed in the last keyboard state.
        // This value is usually only called when the game is in a Menu state. 
        public bool MenuUp
        {
            get
            {
                return keyboardState.IsKeyDown(Keys.Up) && lastState.IsKeyUp(Keys.Up);
            }
        }

        // This value, when called, checks the current keyboard state and returns true if the enter key is pressed
        // AND if it was not previously pressed in the last keyboard state.
        // This value is usually only called when the game is in a Menu state. 
        public bool MenuDown
        {
            get
            {
                return keyboardState.IsKeyDown(Keys.Down) && lastState.IsKeyUp(Keys.Down);
            }
        }

        // This constructor function is called when an instance of the input class is first made.
        // It checks for the first keyboard state, and initializes the previous state by setting it 
        // to the same keyboard state. 
        public Input()
        {
            keyboardState = Keyboard.GetState();
            lastState = keyboardState;
        }

        // This XNA function sets the keyboard state gotten and sends it to the previous keyboard state, and then
        // gets the current keyboard state and puts it into the appropriate keyboard state. 
        public void Update()
        {
            lastState = keyboardState;
            keyboardState = Keyboard.GetState();
        }    
    }
}
