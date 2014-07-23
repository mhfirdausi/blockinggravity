// Menu class
// This class handles all of the menus in the game. This includes the main menu, the how to play menu, 
// and the credits menu. This uses the Graphics library of the XNA framework to draw text on the screen. 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blocking_Gravity
{
    
    class Menu
    {
        //Each menu has a list of strings that need to be presented, and they are seperated according
        //to the three different screens. 
        private List<string> MenuItems;
        private List<string> CreditItems;
        private List<string> HowToItems;
        // The iterator is the integer representing the menu position in the main menu. 
        private int iterator;
        // This special iterator is the base value that controls the menu iterator. It is modifiable, but is set
        // back to a certain value if it goes below zero or above the number of menu items.
        public int Iterator
        {
            get
            {
                return iterator;
            }

            set
            {
                iterator = value;
                if (iterator > MenuItems.Count - 1) iterator = MenuItems.Count - 1;
                if (iterator < 0) iterator = 0;

            }

        }

        // These two text strings contain the title and any informational text. 
        public string InfoText { get; set; }
        public string Title { get; set; }

        // Menu constructor
        // In this lengthy function, the Menu function is initialized by inserting all of the strings that are displayed 
        // in the menus. This is a large amount of strings, and since each are put on a seperate line on the screen, they 
        // are added to the lists of items seperately, line by line. 
        public Menu()
        {
            Title = "Blocking Gravity";
            MenuItems = new List<string>();
            MenuItems.Add("Play: Slow mode");
            MenuItems.Add("Play: Normal mode");
            MenuItems.Add("Play: Slick mode");
            MenuItems.Add("How to play");
            MenuItems.Add("Credits");
            MenuItems.Add("Exit game");

            CreditItems = new List<string>();
            CreditItems.Add("This game is for a Senior Project at Baton Rouge Magnet High School");
            CreditItems.Add("Produced for the 2012 School year");
            CreditItems.Add("Main Game Designer/Programming/Artist: Mohammad Hasan Firdausi");
            CreditItems.Add("Project Mentor: Omer M. Soysal");
            CreditItems.Add("Game Inspiration: Falldown by Nick Monu");
            CreditItems.Add("Ball sprite design: George W. Clingerman from XNA Development");
            CreditItems.Add("Songs used:");
            CreditItems.Add("2DPolygon - Destination");
            CreditItems.Add("ChronoNomad - Forever Fantasy");
            CreditItems.Add("cornandbeans - Midnight");
            CreditItems.Add("Dimrain47 - A Spectrum Wind");
            CreditItems.Add("Dimrain47 - Jetstream!");
            CreditItems.Add("DimRain47 - Operation Evolution");
            CreditItems.Add("Dimrain47 - Twilight Techno");
            CreditItems.Add("DjBjra - Above the Clouds");
            CreditItems.Add("durn - Seasons");
            CreditItems.Add("JEBBAL - Dead Logic");
            CreditItems.Add("Wahnsinn - Magdalenian");
            CreditItems.Add("This game and all music are distributed under the Creative Commons License 3.0");

            HowToItems = new List<string>();
            HowToItems.Add("Meet the ball. He is your new best friend, and he is in a constant state of falling.");
            HowToItems.Add("To control him, press the left arrow to move left and the right arrow to move right.");
            HowToItems.Add("As you are falling, a row of blocks will rise from the bottom of the screen");
            HowToItems.Add("and try to push you to the top of the screen. The object of this game is to stay in");
            HowToItems.Add("a state of free fall for as long as possible before getting crushed.");
            HowToItems.Add("Each row of blocks will have at least one hole. To keep falling, you must move the ball");
            HowToItems.Add("such that the ball falls through these holes. Simple, right? ");
            HowToItems.Add("Don't let those blocks block gravity! Good luck!");
            Iterator = 0;
            InfoText = string.Empty;
        }

        // This function returns the number of options in the main menu as an integer
        public int numOptions()
        {
            return MenuItems.Count;
        }

        // This function returns one of the string lines in the MenuItems list of strings. The parameter index determines the 
        // specific string value to get from the MenuItems list. 
        public string getItem(int index)
        {
            return MenuItems[index];
        }

        // This function draws the main menu onto the screen, using a Spritebatch, the screen width represented by an integer, and 
        // a font type under the class name SpriteFont. 
        // It draws the title of the game in the top center of the screen and draws the items in MenuItems on the left side of the 
        // screen. The string at the position determined by the value of the iterator is colored gray; all the other menu options 
        // are colored white. This helps the person playing the game differentiate which menu option is selected on the game screen. 
        public void DrawMenu(SpriteBatch batch, int screenWidth, SpriteFont roboto)
        {
            batch.DrawString(roboto, Title, new Vector2(screenWidth / 2 - roboto.MeasureString(Title).X / 2, 20), Color.White);
            int yPos = 200;
            for (int i = 0; i < numOptions(); i++)
            {
                Color colour = Color.White;
                if (i == iterator)
                {
                    colour = Color.Gray;
                }
                batch.DrawString(roboto, getItem(i), new Vector2(240 /*screenWidth / 2 - roboto.MeasureString(getItem(i)).X / 2*/, yPos), colour);
                yPos += 50;
            }
        }

        // This function is not currently being implemented, but it draws a screen displaying whatever string is in Infotext and 
        // "Press Enter to Continue" at a lower position on the screen. 
        public void DrawEndScreen(SpriteBatch batch, int screenWidth, SpriteFont roboto)
        {
            batch.DrawString(roboto, InfoText, new Vector2(screenWidth / 2 - roboto.MeasureString(InfoText).X / 2, 300), Color.White);
            string prompt = "Press Enter to Continue";
            batch.DrawString(roboto, prompt, new Vector2(screenWidth / 2 - roboto.MeasureString(prompt).X / 2, 400), Color.White);
        }

        //This screen draws all of the text that goes onto the How to play screen. The drawing of the textures used on the screen are drawn in the draw function 
        //of the main game. All the text strings in the HowToItems list are centered on the screen. 
        public void DrawHowToScreen(SpriteBatch batch, int screenWidth, SpriteFont roboto)
        {
            string howToTitle = "Blocking Gravity: An Introduction";
            batch.DrawString(roboto, howToTitle, new Vector2(screenWidth / 2 - roboto.MeasureString(howToTitle).X / 2, 50), Color.White);
            batch.DrawString(roboto, HowToItems.ElementAt(0), new Vector2(screenWidth / 2 - roboto.MeasureString(HowToItems.ElementAt(0)).X / 2, 200), Color.White);
            batch.DrawString(roboto, HowToItems.ElementAt(1), new Vector2(screenWidth / 2 - roboto.MeasureString(HowToItems.ElementAt(1)).X / 2, 230), Color.White);
            int yPos = 350;
            for (int b = 2; b < HowToItems.Count; b++)
            {
                batch.DrawString(roboto, HowToItems.ElementAt(b), new Vector2(screenWidth / 2 - roboto.MeasureString(HowToItems.ElementAt(b)).X / 2, yPos), Color.White);
                yPos += 30;
            }
            string prompt = "Press Enter to Continue";
            batch.DrawString(roboto, prompt, new Vector2(screenWidth / 2 - roboto.MeasureString(prompt).X / 2, 550), Color.White);
        }

        // This screen draws all of the text that goes onto the Credits screen. 
        // All the text strings in the HowToItems list are centered on the screen. 
        public void DrawCreditsScreen(SpriteBatch batch, int screenWidth, SpriteFont roboto)
        {
            int yPos = 50;
            for (int a = 0; a < CreditItems.Count; a++)
            {
                batch.DrawString(roboto, CreditItems.ElementAt(a), new Vector2(screenWidth / 2 - roboto.MeasureString(CreditItems.ElementAt(a)).X / 2, yPos), Color.White);
                yPos += 30;
            }
            string prompt = "\nPress Enter to Continue";
            batch.DrawString(roboto, prompt, new Vector2(screenWidth / 2 - roboto.MeasureString(prompt).X / 2, yPos), Color.White);
        }

    }
}
