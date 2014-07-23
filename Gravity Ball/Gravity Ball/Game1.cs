// Game Class
// This is the ultimate class, containing all of the game logic and drawing functions. 
// All decisions made by the game, all drawing, all audio playing, are contained in this class. 
// Do note that some comments have a different formatting; they were automatically generated when the
// project was created. 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Blocking_Gravity
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // Enums are special magic values that can be preset. In this game's case, they are used
        // to represent the various states the game can be in. All of the game states should be 
        // fairly self explanatory, based on their names. 
        public enum GameState
        {
            menuScreen,     // Main menu
            Playing,        // Actually playing the game
            EndScreen,      // Game is ending now
            Leaving,        // Player has just died in game
            howToScreen,    // How to play screen
            creditsScreen   // Credits menu screen
        };
        
        // These values are constant, meaning that they will never be changed. Think of values like pi or e,
        // only these values are explicitly made by the game. The names are fairly self explanatory;
        // the size values for BALL_SIZE, BLOCK_WIDTH, and BLOCK_HEIGHT are measured in pixels. 
        private const int MIN_ROW = 5;
        private const int MAX_ROW = 9;
        private const int BALL_SIZE = 90;
        private const int BLOCK_WIDTH = 128;
        private const int BLOCK_HEIGHT = 45;
        private const double FALLING_SPEED = -.1;

        // This vector is the constant value of the movement of the blocks, but certain menu options have the option
        // to alter this value to some extent
        private Vector2 BLOCK_MOVEMENT = new Vector2(0f, -3f);
        
        // All of these values are the numeric and boolean values required for the game. Some are initialized here
        // and some are initialized in the Initialize function; that's partly due to my laziness. :)
        private int screenWidth;
        private int screenHeight;
        private int gameScore;
        private int generationInterval;
        private int tickCount;
        private double speedFactor;
        private int justStartedCounter = 0;
        private bool justStarted = true;
        private bool screenChanged = false;
        private int screenLeavingCounter = 0;
        bool hitBlock = false;

        //These variables are mostly objects, or instances of a class. Note theGameBall and blockCollection
        //as they are vital to the game. 
        protected SpriteFont gameFont;  // A class type that allows a game to display text with a certain font style
        GraphicsDeviceManager graphics; // Exactly what the name says, a Graphics Device Manager. 
        SpriteBatch spriteBatch;
        Sprite theGameBall;
        Sprite bigLogo;
        Sprite background;      
        Random randomGenerator; // This is a class that helps generate random integers. 
        Input gameInput;        
        Menu gameMenu;
        Vector2 ballMovement;
        public static GameState currentGameState; 
        Sprite displayBlock;
        List<Sprite> blockCollection;

        // These variables are the music and sound effects that play throughout the game. They have a similar implementation
        // to sprite initialization, but have different file types that need to be managed. 
        List<Song> menuSongList;
        List<Song> gameSongList;
        SoundEffect menuClick;
        SoundEffect menuSelect;
        SoundEffect gameCheer;
        Song gameOver;

        // This is the main constructor of the Game. It does not need to be lengthy; these two statements will suffice for 
        // this game. 
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// Most (should be all) variables are initialized here, meaning the computer is shown that
        /// these variables exist and have some sort of value. 
        /// </summary>
        protected override void Initialize()
        {
           // TODO: Add your initialization logic here
            screenWidth = 1280;
            screenHeight = 720;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            gameScore = 0;
            generationInterval = 1000;
            speedFactor = 1;
            tickCount = 1000;
            gameFont = Content.Load<SpriteFont>("RobotoFont");
            currentGameState = GameState.menuScreen;

            randomGenerator = new Random();
            theGameBall = new Sprite(600,100);
            bigLogo = new Sprite(700, 150);
            displayBlock = new Sprite(580, 275);
            background = new Sprite();
            blockCollection = new List<Sprite>();
            menuSongList = new List<Song>();
            gameSongList = new List<Song>();
            ballMovement = new Vector2(0, 1f);

            gameMenu = new Menu();
            gameInput = new Input();

            base.Initialize();
        }   

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content. 
        /// This content primarily includes the textures for the sprites and all of the music. 
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            theGameBall.LoadContent(this.Content, "Ball");
            bigLogo.LoadContent(this.Content, "blockGravityLogoLarge");
            displayBlock.LoadContent(this.Content, "Block");
            background.LoadContent(this.Content, "backGround");
            menuSongList.Add(Content.Load<Song>("UsedMusic/2DPolygon - Destination"));
            menuSongList.Add(Content.Load<Song>("UsedMusic/ChronoNomad - Forever Fantasy"));
            menuSongList.Add(Content.Load<Song>("UsedMusic/cornandbeans - Midnight"));
            menuSongList.Add(Content.Load<Song>("UsedMusic/Dimrain47 - Twilight Techno"));
            menuSongList.Add(Content.Load<Song>("UsedMusic/durn - Seasons"));
            menuSongList.Add(Content.Load<Song>("UsedMusic/Wahnsinn - Magdalenian"));

            gameSongList.Add(Content.Load<Song>("UsedMusic/Dimrain47 - A Spectrum Wind"));
            gameSongList.Add(Content.Load<Song>("UsedMusic/Dimrain47 - Jetstream!"));
            gameSongList.Add(Content.Load<Song>("UsedMusic/DimRain47 - Operation Evolution"));
            gameSongList.Add(Content.Load<Song>("UsedMusic/DjBjra - Above the Clouds"));
            gameSongList.Add(Content.Load<Song>("UsedMusic/JEBBAL - Dead Logic"));
            gameOver = Content.Load<Song>("UsedMusic/Sad-Trombone");
            menuClick = Content.Load<SoundEffect>("UsedMusic/Click");
            menuSelect = Content.Load<SoundEffect>("UsedMusic/Pulse");
            gameCheer = Content.Load<SoundEffect>("UsedMusic/Cheer");

            // This little code fragment starts the Media Player, a system used to play media files. 
            // It randomly selects a track from the Menu Songs list. 
            int playPos = randomGenerator.Next(menuSongList.Count);
            while (playPos > menuSongList.Count)
                playPos = randomGenerator.Next(menuSongList.Count);
            MediaPlayer.Play(menuSongList.ElementAt(playPos));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// To fix some bugs, the SoundEffects in the game have to be disposed. 
        /// Generally, Windows will take care of the unloading of content. 
        /// </summary>
        protected override void UnloadContent()
        {
            menuClick.Dispose();
            menuSelect.Dispose();
            gameCheer.Dispose();
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// This function is THE logical backing behind the game. It checks games states, changes them if necessary,
        /// checks for collisions, and more...pretty much everything that is involved in updating the game is here. 
        protected override void Update(GameTime gameTime)
        {
            // We start off by updating the Input class, checking for any updates to the state of the keyboard. 
            gameInput.Update();

            // This fragment ensures that if the Media Player is not playing a song while it's in the menu, then
            // pull a new song from the Menu Songs and start playing it. 
            if ((MediaPlayer.State == MediaState.Stopped) && (currentGameState == GameState.menuScreen))
            {
                MediaPlayer.Play(menuSongList.ElementAt(randomGenerator.Next(menuSongList.Count)));
            }

            // This fragment ensures that if the Media Player is not playing a song while it's in gameplay, then
            // pull a new song from the Game Songs and start playing it. 
            if ((MediaPlayer.State == MediaState.Stopped) && (currentGameState == GameState.Playing))
            {
                MediaPlayer.Play(gameSongList.ElementAt(randomGenerator.Next(gameSongList.Count)));
            }

            //Without this statement, ball moves realistically with acceleration
            //but out of control. With this statement, ball moves more statically 
            //but at a muchh lower speed. Keep or no? 
            //ballMovement = Vector2.Zero;

            // If the game is currently in the menu, run this big code fragment. s
            if (currentGameState == GameState.menuScreen)
            {
                // If the player pressed the keyboard to move the iterator down, then move it down
                // and play a clicking sound effect. 
                if (gameInput.MenuDown)
                {
                    gameMenu.Iterator++;
                    menuClick.Play();
                }

                // If the player pressed the keyboard to move the iterator up, then move it up
                // and play a clicking sound effect. 
                if (gameInput.MenuUp)
                {
                    gameMenu.Iterator--;
                    menuClick.Play();
                }
                // If the player pressed enter on the keyboard, play the Menu Select sound effect and check which menu 
                // entry was selected by the player. 
                if (gameInput.MenuSelect)
                {
                    menuSelect.Play();
                    // A switch statement allows a game to do multiple things based on the value of one item, e.g. 
                    // if iterator is 0, put the game on slow mode; if iterator is 1, put the game on normal, etc. 
                    switch (gameMenu.Iterator)
                    {
                        // First option: slow mode. Reset all of the game variables to default values and modify them
                        // to suit slow mode. The values were the result of me doing trial and error. 
                        case 0:
                            speedFactor = 0.75;
                            BLOCK_MOVEMENT.Y *= (float)speedFactor;
                            generationInterval = 350;
                            tickCount = 125;
                            currentGameState = GameState.Playing;
                            gameScore = 0;
                            screenLeavingCounter = 0;
                            theGameBall.Position = new Vector2(600, 100);
                            ballMovement = Vector2.Zero;
                            MediaPlayer.Stop(); // Stop any music playing
                            Thread.Sleep(2000); // Have the game pause for 2000 ms, or 2 seconds
                            break;
                        // Second option: normal mode. Reset all of the game variables to default values and modify them
                        // to suit normal mode. The values were the result of me doing trial and error. 
                        case 1:
                            speedFactor = 1;
                            BLOCK_MOVEMENT.Y *= (float)speedFactor;
                            currentGameState = GameState.Playing;
                            generationInterval = 300;
                            tickCount = 100;
                            screenLeavingCounter = 0;
                            gameScore = 0;
                            theGameBall.Position = new Vector2(600, 100);
                            ballMovement = Vector2.Zero;
                            MediaPlayer.Stop(); // Stop any music playing
                            Thread.Sleep(2000); // Have the game pause for 2 seconds
                            break;
                        // Third option: slick mode. Reset all of the game variables to default values and modify them
                        // to suit slick mode. The values were the result of me doing trial and error. 
                        // Slick mode is especially slippery, and most of the people who tried it out liked the challenge. 
                        case 2:
                            speedFactor = 1.5;
                            generationInterval = 250;
                            tickCount = 100;
                            BLOCK_MOVEMENT.Y *= (float)speedFactor;
                            currentGameState = GameState.Playing;
                            gameScore = 0;
                            screenLeavingCounter = 0;
                            theGameBall.Position = new Vector2(600, 100);
                            ballMovement = Vector2.Zero;
                            MediaPlayer.Stop(); // Stop any music playing
                            Thread.Sleep(2000); // Have the game pause for 2 seconds
                            break;
                        // Fourth option: Open the how to screen. 
                        case 3:
                            currentGameState = GameState.howToScreen;
                            screenChanged = true;
                            Thread.Sleep(1000); // Have the game pause for 1 second. 
                            break;
                        // Fifth option: Open the credits screen. 
                        case 4:
                            currentGameState = GameState.creditsScreen;
                            Thread.Sleep(1000); // Have the game pause for 1 second. 
                            screenChanged = true;
                            break;
                        // Sixth option: Close the game. :( 
                        case 5:
                            MediaPlayer.Stop();
                            this.Exit();
                            break;
                        // This fragment only occurs if the iterator somehow exceeds its limits.
                        // If this does happen, exit the game immediately before something else breaks.
                        default :
                            this.Exit();
                            break;
                    }
                    gameMenu.Iterator = 0; // Put the iterator back at zero so selected text is back on top of the screen. 
                }
                
            }
            // If the game is currently being played, then go into the most important portion of the game. 
            if (currentGameState == GameState.Playing)
            {
                // These few lines help determine when to next generate a row of blocks. 
                // Generation interval is a value that tickCount counts upwards to. Once tickCount equals or surpasses
                // the generation interval, a block row is generated and tickCount is reset to 0. This repeats forever until the 
                // game ends. Theoretically, this means there is a finite ending to the game, but who is able to reach it?
                if ((tickCount >= generationInterval) && (!justStarted))
                {
                    generateBlockRow();
                    tickCount = 0;
                    generationInterval -= 1;
                    
                }
                else if (!justStarted)
                {
                    tickCount += 2;
                }
                
                // This fragment is necessary so that the game does not instantly start, but rather give the player a few moments
                // notice to get ready. Once the justStarted period is over, justStarted becomes false and remains that way until 
                // a new game is started. 
                if (justStartedCounter < 50 && justStarted) 
                    justStartedCounter++;
                else if (justStartedCounter >= 50)
                {
                    justStarted = false;
                    justStartedCounter = 0;
                }

                // Move each block in the collection up a set amount. 
                if (blockCollection.Count > 0)
                {
                    foreach (Sprite uBlock in blockCollection)
                    {
                        uBlock.Update(gameTime, BLOCK_MOVEMENT);
                    }
                }

                // If the blocks are off the top of the screen, remove them from the collection. 
                // The code goes through each block until all blocks are checked. 
                int checkPos = 0;
                while (checkPos < blockCollection.Count)
                {
                    if (blockCollection.ElementAt(checkPos).Position.Y < (0 - BLOCK_HEIGHT))
                    {
                        blockCollection.RemoveAt(checkPos);
                        if (checkPos > 0)
                            checkPos--;
                        gameScore++;
                    }
                    checkPos++;
                }

                // This next fragment is the physics of the game, the collision detection. 
                hitBlock = false;
                if (blockCollection.Count > 0)
                {
                    // Check each block in the collection of blocks
                    foreach (Sprite cBlock in blockCollection)
                    {
                        // If the ball is currently in contact with a block, then it is on top, and thus
                        // the block must be moved up. This is to make sure that if the ball is on top, then
                        // there is no need to recheck if other blocks are colliding. 
                        if (hitBlock)
                            ballMovement.Y = BLOCK_MOVEMENT.Y;
                        else
                        {
                            // If the BoundingBox of the ball intersects the block being checked
                            if (theGameBall.BoundingBox.Intersects(cBlock.BoundingBox))
                            {
                                hitBlock = true; // A block was hit, so this is true;
                                // Check which side the ball is hitting the block. 
                                switch (collisionCheck(theGameBall, cBlock))
                                {
                                    // The ball is hitting the top of the block
                                    case 1:
                                        {
                                            ballMovement.Y = BLOCK_MOVEMENT.Y;
                                            break;
                                        }
                                    // The ball is hitting the top and the left of the block
                                    case 2:
                                        {
                                            ballMovement.Y = BLOCK_MOVEMENT.Y;
                                            //ballMovement.X = 0;
                                            break;
                                        }
                                    // The ball is hitting the top and the right of the block
                                    case 3:
                                        {
                                            ballMovement.Y = BLOCK_MOVEMENT.Y;
                                            //ballMovement.X = 0;
                                            break;
                                        }
                                    // The ball is hitting something to the left and is on a block
                                    case 4:
                                        {
                                            ballMovement.X = 0;
                                            ballMovement.Y = 0;
                                            break;
                                        }
                                    // The ball is hitting something to the right and is on a block
                                    case 5:
                                        {
                                            ballMovement.X = 0;
                                            ballMovement.Y = 0;
                                            break;
                                        }
                                    // Weird condition? Move the ball up
                                    default:
                                        {
                                            ballMovement.Y -= .00250f;
                                            break;
                                        }
                                }
                            }
                            // Else move the ball down
                            else
                            {
                                ballMovement.Y += .00250f;
                            }
                        }
                    }
                }
                // If the player is pressing the left arrow, make the ball accelerate left. 
                if (gameInput.LeftArrow)
                    ballMovement.X -= .2f * (float)speedFactor;
                // If the player is pressing the right arrow, make the ball accelerate right. 
                if (gameInput.RightArrow)
                    ballMovement.X += .2f * (float)speedFactor;
                //theGameBall.Position= new Vector2((MathHelper.Clamp(theGameBall.Position.X, 0f, screenWidth - BALL_SIZE)), theGameBall.Position.Y);
                // Make a dummy check to see if ball moves off the screen
                // The dummy checks to make sure what the ball is supposed to do next is in check. 
                Vector2 dummy = theGameBall.Position + (ballMovement);
                Sprite dummy2 = new Sprite((int)dummy.X, (int)dummy.Y);
                dummy2.LoadContent(this.Content, "Ball");

                if (blockCollection.Count > 0)
                {
                    // Check each block in the collection of blocks
                    foreach (Sprite cBlock in blockCollection)
                    {
                        // If the ball is currently in contact with a block, then it is on top, and thus
                        // the block must be moved up. This is to make sure that if the ball is on top, then
                        // there is no need to recheck if other blocks are colliding. 
                        if (hitBlock)
                            ballMovement.Y = BLOCK_MOVEMENT.Y;
                        else
                        {
                            if (dummy2.BoundingBox.Intersects(cBlock.BoundingBox))
                            {
                                hitBlock = true;
                                switch (dummyCollisionCheck(dummy2, cBlock))
                                {
                                    //Ball is hitting from the top only
                                    case 1:
                                        {
                                            ballMovement.Y = BLOCK_MOVEMENT.Y;
                                            break;
                                        }

                                    //Ball is hitting the block from the left only
                                    case 2:
                                        {
                                            //ballMovement.Y = BLOCK_MOVEMENT.Y;
                                            if (gameInput.RightArrow && (theGameBall.BoundingBox.Bottom > cBlock.BoundingBox.Top))
                                                ballMovement.X = 0;
                                            break;
                                        }
                                    //Ball is hitting the block from the right only
                                    case 3:
                                        {
                                            //ballMovement.Y = BLOCK_MOVEMENT.Y;
                                            if (gameInput.LeftArrow && (theGameBall.BoundingBox.Bottom > cBlock.BoundingBox.Top))
                                                ballMovement.X = 0;
                                            break;
                                        }
                                    //Ball is hitting the block from the below and from the left
                                    case 4:
                                        {
                                            //if (kState.IsKeyDown(Keys.Right) && (theGameBall.BoundingBox.Bottom > cBlock.BoundingBox.Top))
                                                ballMovement.X = 0;
                                            break;
                                        }
                                    //Ball is hitting the block from the below and from the right
                                    case 5:
                                        {
                                            //if (kState.IsKeyDown(Keys.Left) && (theGameBall.BoundingBox.Bottom > cBlock.BoundingBox.Top))
                                                ballMovement.X = 0;
                                            break;
                                        }
                                    // Weird case? Move the ball down
                                    default:
                                        {
                                            ballMovement.Y += .000250f;
                                            break;
                                        }
                                }
                            }
                            // Move the ball down
                            else
                            {
                                ballMovement.Y += .0250f;
                            }
                        }
                    }
                }
                //Check to make sure the ball does not move off screen
                if ((dummy.X <= 0) || (dummy.X >= (screenWidth - BALL_SIZE)))
                    ballMovement.X = 0;
                if ((dummy.Y >= screenHeight - BALL_SIZE))
                    ballMovement.Y = 0;

                //theGameBall.Position += ballMovement;
                // Check if the ball hit the top
                // If the ball did hit the top, end the game. 
                theGameBall.Update(gameTime, ballMovement);
                if ((theGameBall.BoundingBox.Y <= 0) && (currentGameState == GameState.Playing))
                    currentGameState = GameState.EndScreen;
                
            }

            // If the player just died, the game is now in the leaving state
            if (currentGameState == GameState.Leaving)
            {
                // If the player managed to get a score greater than 1000, the game cheers
                if (gameScore > 1000 && screenLeavingCounter == 0)
                    gameCheer.Play();
                // Else, the game plays a fail sound by the trombone
                else if (screenLeavingCounter == 0)
                    MediaPlayer.Play(gameOver);
                // This is to simulate a delay in the screen. The game does not continue until the leaving counter 
                // Completely fills. 
                if (screenLeavingCounter == 400)
                    currentGameState = GameState.menuScreen;
                else
                    screenLeavingCounter++;
            }

            // If the game is in the how to play screen, simply wait for the player to press enter
            // once they are done reading. 
            if (currentGameState == GameState.howToScreen)
            {
                if (screenChanged)
                    screenChanged = false;
                else
                    // If the player presses enter, go back to the menu
                    if (gameInput.MenuSelect)
                        currentGameState = GameState.menuScreen;
            }
            // If the game is in the credits screen, simply wait for the player to press enter
            // once they are done reading. 
            if (currentGameState == GameState.creditsScreen)
            {
                if (screenChanged)
                    screenChanged = false;
                else
                    // If the player presses enter, go back to the menu
                    if (gameInput.MenuSelect)
                        currentGameState = GameState.menuScreen;
            }
            if (currentGameState == GameState.EndScreen)
            {
                MediaPlayer.Stop();
                Thread.Sleep(3000); // Pause the game for 3 seconds for emphasis
                currentGameState = GameState.Leaving;
                blockCollection.Clear(); // Remove all the blocks from the block collection
                // Reset a few values
                BLOCK_MOVEMENT.Y /= (float)speedFactor;
                theGameBall = new Sprite(600, 100);
                theGameBall.LoadContent(this.Content, "Ball");
            }
            base.Update(gameTime);
        }
             

        /// <summary>
        /// This is called when the game should draw itself, which is each time the game updates. 
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(); // Start the ability to draw multiple objects onto the screen
            // If the game is in the Leaving state, draw the Leaving Screen
            if (currentGameState == GameState.Leaving)
            {
                GraphicsDevice.Clear(Color.Black); // Clear everything off the screen and have a black background.
                //Draw a string that displays the game score
                spriteBatch.DrawString(gameFont, "Final Score: " + gameScore.ToString(), new Vector2(240, 350), Color.White);
                bigLogo.Draw(this.spriteBatch); // Draws a big version of the logo onto the screen.
            }
            // If not leaving, if the game is in the Main Menu, draw the main menu screen. 
            else if (currentGameState == GameState.menuScreen)
            {
                GraphicsDevice.Clear(Color.Black);
                background.Draw(this.spriteBatch); // Draw a background on the screen
                // Call the Menu function to draw the Main menu
                gameMenu.DrawMenu(spriteBatch, screenWidth, gameFont);
                bigLogo.Draw(this.spriteBatch);
            }
            // If neither of the above two, if the game is showing the How to Screen, draw the How to Screen.
            else if (currentGameState == GameState.howToScreen)
            {
                GraphicsDevice.Clear(Color.Black);
                background.Draw(this.spriteBatch);
                //Draw a ball and block onto the screen
                theGameBall.Draw(this.spriteBatch);
                displayBlock.Draw(this.spriteBatch);
                // Draw the text of the how to screen onto the screen
                gameMenu.DrawHowToScreen(spriteBatch, screenWidth, gameFont);
            }
            // If neither of the about three, if the game is showing the Credits Screen, draw the Credits Screen.
            else if (currentGameState == GameState.creditsScreen)
            {
                GraphicsDevice.Clear(Color.Black);
                background.Draw(this.spriteBatch);
                //Draw the text of the credits screen onto the screen.
                gameMenu.DrawCreditsScreen(spriteBatch, screenWidth, gameFont);
            }
            // If none of those, then the game is being played. 
            else if (currentGameState == GameState.Playing)
            {
                GraphicsDevice.Clear(Color.DarkBlue);   // Make the background dark blue.
                theGameBall.Draw(this.spriteBatch);     // Draw the game ball onto the screen 
                if (blockCollection.Count > 0)          // Draw each block onto the screen
                    foreach (Sprite displayBlock in blockCollection)
                        displayBlock.Draw(this.spriteBatch);
                // Draw some text onto the screen displaying the score.
                spriteBatch.DrawString(gameFont, "Score: " + gameScore.ToString(), new Vector2(640, 50), Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        // This function randomly generates a row of blocks. There are 10 possible positions to place blocks, and initally
        // all of them are filled. A random number between 1-4 is chosen; this is the number of blocks to be removed. 
        // Once this is done, a position is randomly chosen for between 1-4, and that position is removed from the list of positions
        // to place blocks. The function then initializes and loads each new block.
        public void generateBlockRow()
        {
            List<int> blockPositions = new List<int>();
            int removePosition = 0;
            int removeProbability = 0;
            int numRemovePositions = 1;
            for (int a = 0; a < 1280; a += BLOCK_WIDTH)
                blockPositions.Add(a);
            numRemovePositions += randomGenerator.Next(4);
            for (int b = 0; b < numRemovePositions; b++)
            {
                removePosition = randomGenerator.Next(10);
                removeProbability = randomGenerator.Next(1000);
                if (removeProbability <= 800)
                    if (removePosition <= blockPositions.Count - 1)
                        blockPositions.RemoveAt(removePosition);
            }
            if (blockPositions.Count == (1280 / BLOCK_WIDTH))
                generateBlockRow();
            else
            {
                foreach (int addIndex in blockPositions)
                {
                    blockCollection.Add(new Sprite(addIndex, screenHeight+1));
                    blockCollection.ElementAt(blockCollection.Count - 1).LoadContent(this.Content, "Block");
                }
            }

        }

        /*
         * Check if there is a collision between two sprites,
         * returns what side the sprites are colliding relative to the first sprite
         *  0 : No collision
         *  1 : Collision from the top
         *  2 : Collision from the bottom
         *  3 : Collision from the left
         *  4 : Collision from the right
         */
        public int collisionCheck(Sprite firstTest, Sprite secondTest)
        {
            if (firstTest.BoundingBox.Bottom == secondTest.BoundingBox.Top)
                return 1;
            else if (firstTest.BoundingBox.Bottom > secondTest.BoundingBox.Top)
            {
                if (firstTest.BoundingBox.Left <= secondTest.BoundingBox.Right)
                    return 2;
                else if (firstTest.BoundingBox.Right >= secondTest.BoundingBox.Left)
                    return 3;
                else
                    return 1;
            }
            return 0;

            // All of this code is from a previous attempt for collision detection.
            //bool collisionFlagBottom = false;
            //bool collisionFlagLeft = false;
            //bool collisionFlagRight = false;

            //if (firstTest.BoundingBox.Bottom >= secondTest.BoundingBox.Top)
            //    return 1;
            //else
            //    return 0;
            /*
                collisionFlagBottom = true;
            //if (firstTest.BoundingBox.Top <= secondTest.BoundingBox.Bottom)
            //    return 2; //This should not occur, but best to have it in case
            if (firstTest.BoundingBox.Right >= secondTest.BoundingBox.Left)
                collisionFlagRight = true;
            if (firstTest.BoundingBox.Left <= secondTest.BoundingBox.Right)
                collisionFlagLeft = true;

            if (collisionFlagBottom)
                if (collisionFlagRight)
                    return 5;
                else if (collisionFlagLeft)
                    return 4;
                else
                    return 1;
            else if (collisionFlagRi            else
                return 0;ght)
                return 3;
            else if (collisionFlagLeft)
                return 2;
            else
                return 0;
            */

        }

        // This function determines what side the two sprites are colliding on. It uses a flag system; if the bottom of the first sprite
        // hit the top of the second sprite, the bottom flag is set to true, etc. 
        // Most of the collision checks are self explanatory. 
        public int dummyCollisionCheck(Sprite firstTest, Sprite secondTest)
        {
            //Rectangle fTestRect = firstTest.BoundingBox;
            //Rectangle sTestRect = secondTest.BoundingBox;
            //Rectangle ballRect = theGameBall.BoundingBox;

            //int xChange = fTestRect.X - ballRect.X;
            //int yChange = fTestRect.Y - ballRect.Y;


            bool bottomFlag = false;
            bool leftFlag = false;
            bool rightFlag = false;
            if (firstTest.BoundingBox.Bottom >= secondTest.BoundingBox.Top)
                bottomFlag = true;
            if ((firstTest.BoundingBox.Left <= secondTest.BoundingBox.Right) && (firstTest.BoundingBox.Left > secondTest.BoundingBox.Left))
                rightFlag = true;
            if ((firstTest.BoundingBox.Right >= secondTest.BoundingBox.Left) && (firstTest.BoundingBox.Right < secondTest.BoundingBox.Right))
                leftFlag = true;
            if (bottomFlag)
            {
                if (rightFlag && leftFlag)
                    return 5;
                if (rightFlag)
                    return 5;
                else if (leftFlag)
                    return 4;
                else
                    return 1;
            }
            else if (rightFlag)
                return 3;
            else if (leftFlag)
                return 2;
            else
                return 0;

            //if ((firstTest.BoundingBox.Bottom >= secondTest.BoundingBox.Top) && (firstTest.BoundingBox.Top < secondTest.BoundingBox.Bottom))
            //{
            //    if (((firstTest.BoundingBox.Bottom) > secondTest.BoundingBox.Top) && (firstTest.BoundingBox.Left > secondTest.BoundingBox.Left) 
            //        && (firstTest.BoundingBox.Left <= secondTest.BoundingBox.Right))
            //        return 2;
            //    else if (((firstTest.BoundingBox.Bottom) > secondTest.BoundingBox.Top) && (firstTest.BoundingBox.Left < secondTest.BoundingBox.Left) 
            //        && (firstTest.BoundingBox.Right >= secondTest.BoundingBox.Left))
            //        return 3;
            //    else
            //        return 1;
            //}
            //return 0;
            
        }

    }
}
