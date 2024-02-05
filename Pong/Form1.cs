/*
 * Description:     A basic PONG simulator
 * Author:           
 * Date:            
 */

#region libraries

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;

#endregion

namespace Pong
{
    public partial class Form1 : Form
    {
        #region global values

        //graphics objects for drawing
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        Font drawFont = new Font("Courier New", 10);

        // Sounds for game
        SoundPlayer scoreSound = new SoundPlayer(Properties.Resources.score);
        SoundPlayer collisionSound = new SoundPlayer(Properties.Resources.collision);

        //determines whether a key is being pressed or not
        Boolean wKeyDown, sKeyDown, upKeyDown, downKeyDown;
        Boolean iKeyDown, jKeyDown, kKeyDown, lKeyDown;

        // check to see if a new game can be started
        Boolean newGameOk = true;

        //ball values
        Boolean ballMoveRight = true;
        Boolean ballMoveDown = true;
        const int BALL_SPEED = 4;
        const int BALL_WIDTH = 20;
        const int BALL_HEIGHT = 20; 
        Rectangle ball;
        int ballXSpeed, ballYSpeed;

        //player values
        const int PADDLE_SPEED = 4;
        const int PADDLE_EDGE = 20;  // buffer distance between screen edge and paddle            
        const int PADDLE_WIDTH = 10;
        const int PADDLE_HEIGHT = 40;
        Rectangle player1, player2;

        //Score Values
        Point pScore1, pScore2;

        //player and game scores
        int player1Score = 0;
        int player2Score = 0;
        int gameWinScore = 2;  // number of points needed to win game

        int labelSpacer = 150;

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //check to see if a key is pressed and set is KeyDown value to true if it has
            switch (e.KeyCode)
            {
                case Keys.W:
                    wKeyDown = true;
                    break;
                case Keys.S:
                    sKeyDown = true;
                    break;
                case Keys.Up:
                    upKeyDown = true;
                    break;
                case Keys.Down:
                    downKeyDown = true;
                    break;
                case Keys.I:
                    iKeyDown = true;
                    break;
                case Keys.J:
                    jKeyDown = true;
                    break;
                case Keys.K:
                    kKeyDown = true;
                    break;
                case Keys.L:
                    lKeyDown = true;
                    break;
                case Keys.Y:
                case Keys.Space:
                    if (newGameOk)
                    {
                        SetParameters();
                    }
                    break;
                case Keys.Escape:
                    if (newGameOk)
                    {
                        Close();
                    }
                    break;
            }
        }
        
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //check to see if a key has been released and set its KeyDown value to false if it has
            switch (e.KeyCode)
            {
                case Keys.W:
                    wKeyDown = false;
                    break;
                case Keys.S:
                    sKeyDown = false;
                    break;
                case Keys.Up:
                    upKeyDown = false;
                    break;
                case Keys.Down:
                    downKeyDown = false;
                    break;
                case Keys.I:
                    iKeyDown = false;
                    break;
                case Keys.J:
                    jKeyDown = false;
                    break;
                case Keys.K:
                    kKeyDown = false;
                    break;
                case Keys.L:
                    lKeyDown = false;
                    break;
            }
        }

        /// <summary>
        /// sets the ball and paddle positions for game start
        /// </summary>
        private void SetParameters()
        {
            if (newGameOk)
            {
                player1Score = player2Score = 0;
                newGameOk = false;
                startLabel.Visible = false;
                gameUpdateLoop.Start();
            }

            //player start positions
            player1 = new Rectangle(PADDLE_EDGE, this.Height / 2 - PADDLE_HEIGHT / 2, PADDLE_WIDTH, PADDLE_HEIGHT);
            player2 = new Rectangle(this.Width - PADDLE_EDGE - PADDLE_WIDTH, this.Height / 2 - PADDLE_HEIGHT / 2, PADDLE_WIDTH, PADDLE_HEIGHT);

            // TODO create a ball rectangle in the middle of screen
            ball = new Rectangle((this.Width / 2) - (BALL_WIDTH / 2), (this.Height / 2) - (BALL_HEIGHT / 2), BALL_WIDTH, BALL_HEIGHT);

            //Set Score Locations
            pScore1 = new Point((this.Width / 2) - labelSpacer, 10);
            pScore2 = new Point((this.Width / 2) + labelSpacer - 10, 10);
        }

        /// <summary>
        /// This method is the game engine loop that updates the position of all elements
        /// and checks for collisions.
        /// </summary>
        private void gameUpdateLoop_Tick(object sender, EventArgs e)
        {
            #region update ball position

            // TODO create code to move ball either left or right based on ballMoveRight and using BALL_SPEED
            

            // TODO create code move ball either down or up based on ballMoveDown and using BALL_SPEED

            #endregion

            #region update paddle positions

            if (wKeyDown == true && player1.Y > 0)
            {
                // TODO create code to move player 1 up
                player1.Y += PADDLE_SPEED;
            }

            // TODO create an if statement and code to move player 1 down 

            // TODO create an if statement and code to move player 2 up

            // TODO create an if statement and code to move player 2 down

            #endregion

            #region ball collision with top and bottom lines

            if (ball.Y < 0) // if ball hits top line
            {
                // TODO use ballMoveDown boolean to change direction
                // TODO play a collision sound
            }
            // TODO In an else if statement check for collision with bottom line
            // If true use ballMoveDown boolean to change direction

            #endregion

            #region ball collision with paddles

            // TODO create if statment that checks if player1 collides with ball and if it does
                 // --- play a "paddle hit" sound and
                 // --- use ballMoveRight boolean to change direction

            // TODO create if statment that checks if player2 collides with ball and if it does
                // --- play a "paddle hit" sound and
                // --- use ballMoveRight boolean to change direction
            
            /*  ENRICHMENT
             *  Instead of using two if statments as noted above see if you can create one
             *  if statement with multiple conditions to play a sound and change direction
             */

            #endregion

            #region ball collision with side walls (point scored)

            if (ball.X < 0)  // ball hits left wall logic
            {
                // TODO
                // --- play score sound
                // --- update player 2 score and display it to the label

                // TODO use if statement to check to see if player 2 has won the game. If true run 
                // GameOver() method. Else change direction of ball and call SetParameters() method.

            }

            // TODO same as above but this time check for collision with the right wall

            #endregion
            
            //refresh the screen, which causes the Form1_Paint method to run
            this.Refresh();
        }
        
        /// <summary>
        /// Displays a message for the winner when the game is over and allows the user to either select
        /// to play again or end the program
        /// </summary>
        /// <param name="winner">The player name to be shown as the winner</param>
        private void GameOver(string winner)
        {
            newGameOk = true;

            // TODO create game over logic
            // --- stop the gameUpdateLoop
            // --- show a message on the startLabel to indicate a winner, (may need to Refresh).
            // --- use the startLabel to ask the user if they want to play again

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // TODO draw player2 using FillRectangle
            e.Graphics.FillRectangle(whiteBrush, player1);
            e.Graphics.FillRectangle(whiteBrush, player2);

            // TODO draw ball using FillRectangle
            e.Graphics.FillRectangle(whiteBrush, ball);

            if (newGameOk == false)
            {
                e.Graphics.DrawString(Convert.ToString(player1Score), startLabel.Font, whiteBrush, pScore1);
                e.Graphics.DrawString(Convert.ToString(player2Score), startLabel.Font, whiteBrush, pScore2);
            }
        }

    }
}
