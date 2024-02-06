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

        Random random = new Random();

        //graphics objects for drawing
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        Font drawFont = new Font("Courier New", 10);

        // Sounds for game
        SoundPlayer scoreSound = new SoundPlayer(Properties.Resources.score);
        SoundPlayer collisionSound = new SoundPlayer(Properties.Resources.collision);

        //determines whether a key is being pressed or not
        Boolean wKeyDown, sKeyDown, upKeyDown, downKeyDown;
        Boolean iKeyDown, jKeyDown, kKeyDown, lKeyDown;
        Boolean qKeyDown, uKeyDown;

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

        //Check Paralysis Clause or Deactivation
        bool p1Active = true;
        bool p2Active = true;

        bool p1dash, p2dash, wStateC, sStateC, upStateC, downStateC;

        float p1dashA, p2dashA;
        const int TIMER_WAIT = 10;
        int wWait, sWait, upWait, downWait;
        const int ACCELERATOR = 100;
        const int DASH_TIMER = 15;
        int dTrack1, dTrack2;

        int labelSpacer = 150;

        int p1counterTime, p2counterTime, p1countercool, p2countercool = 0;
        Rectangle p1Count, p2Count;
        const int COUNTER_DURATION = 5;
        int p1coolDuration, p2coolDuration = 25;


        string startText;
        #endregion

        public Form1()
        {
            InitializeComponent();
            startText = startLabel.Text;
        }

        private void wTime_Tick(object sender, EventArgs e)
        {
            wWait += 1;
            if (wKeyDown == false)
            {
                wStateC = true;
            }

            if (wStateC && wKeyDown)
            {
                p1dash = true;
                dTrack2 = 0;
                p1dashA = ACCELERATOR;
                wTime.Enabled = false;
            }

            if (wWait >= TIMER_WAIT)
            {
                wTime.Enabled = false;
            }
        }

        private void sTime_Tick(object sender, EventArgs e)
        {
            sWait += 1;
            if (sKeyDown == false)
            {
                sStateC = true;
            }

            if (sStateC && sKeyDown)
            {
                p1dash = true;
                dTrack1 = 0;
                p1dashA = ACCELERATOR;
                sTime.Enabled = false;
            }

            if (sWait >= TIMER_WAIT)
            {
                sTime.Enabled = false;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //check to see if a key is pressed and set is KeyDown value to true if it has
            switch (e.KeyCode)
            {
                case Keys.W:
                    wKeyDown = true;
                    if (wTime.Enabled == false)
                    {
                        wTime.Enabled = true;
                        wStateC = false;
                        wWait = 0;
                    }
                    break;
                case Keys.S:
                    sKeyDown = true;
                    if (sTime.Enabled == false)
                    {
                        sTime.Enabled = true;
                        sStateC = false;
                        sWait = 0;
                    }
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
                case Keys.Q:
                    qKeyDown = true;
                    break;
                case Keys.U:
                    uKeyDown = true;
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
                case Keys.U:
                    uKeyDown = false;
                    break;
                case Keys.Q:
                    qKeyDown = false;
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
            ballXSpeed = random.Next(1, 6);
            ballYSpeed = random.Next(1, 6);
            if (random.Next(1, 3) == 1)
            {
                ballXSpeed *= -1;
            }
            if (random.Next(1, 3) == 1)
            {
                ballYSpeed *= -1;
            }
        }

        private void PControls(bool playerNum, bool dash, int playerID)
        {
            if (playerNum)
            {
                if (!dash)
                {
                    if (sKeyDown)
                    {
                        player1.Y += PADDLE_SPEED;
                    }
                    if (wKeyDown)
                    {
                        player1.Y -= PADDLE_SPEED;
                    }
                    return;
                }

                if (dash)
                {
                    if (wKeyDown && !sKeyDown)
                    {
                        player1.Y -= Convert.ToInt16(p1dashA);
                        p1dashA /= 2;
                        dTrack1 += 1;
                        if (dTrack1 > DASH_TIMER)
                        {
                            p1dash = false;
                        }
                        return;
                    }

                    if (!wKeyDown && sKeyDown)
                    {
                        player1.Y += Convert.ToInt16(p1dashA);
                        p1dashA /= 2;
                        dTrack1 += 1;
                        if (dTrack1 > DASH_TIMER)
                        {
                            p1dash = false;
                        }
                        return;
                    }
                }
            }



            else
            {
                if (downKeyDown)
                {
                    player1.Y += PADDLE_SPEED;
                }
                if (upKeyDown)
                {
                    player1.Y -= PADDLE_SPEED;
                }
            }
        }

        /// <summary>
        /// This method is the game engine loop that updates the position of all elements
        /// and checks for collisions.
        /// </summary>
        private void gameUpdateLoop_Tick(object sender, EventArgs e)
        {
            #region update ball position

            // TODO create code to move ball either left or right based on ballMoveRight and using BALL_SPEED
            ball.X += ballXSpeed;
            ball.Y += ballYSpeed;

            // TODO create code move ball either down or up based on ballMoveDown and using BALL_SPEED

            #endregion

            #region update paddle positions

            if (p1Active)
            {
                PControls(true, p1dash, 0);
            }

            if (p2Active)
            {
                PControls(false, p2dash, 0);
            }

            // TODO create an if statement and code to move player 1 down 

            // TODO create an if statement and code to move player 2 up

            // TODO create an if statement and code to move player 2 down

            #endregion

            #region ball collision with top and bottom lines

            if (ball.Y < 0)
            {
                ballYSpeed *= -1;
                ball.Y = 10;
                collisionSound.Play();
            }
            else if (ball.Y + ball.Height > this.Height)
            {
                ballYSpeed *= -1;
                ball.Y = this.Height - ball.Height - 10;
                collisionSound.Play();
            }

            #endregion

            #region ball collision with paddles
            p1countercool--;
            if (p1countercool < 0)
            {
                p1countercool = 0;
            }
            if (qKeyDown && p1countercool <= 0)
            {
                p1Count = new Rectangle(player1.X + player1.Width + 5, player1.Y, player1.Width, player1.Height);
                p1counterTime = COUNTER_DURATION;
                p1countercool = p1coolDuration;
            }
            p1counterTime--;
            if (p1counterTime < 0)
            {
                p1counterTime = 0;
            }
            if (ball.IntersectsWith(p1Count) && p1counterTime > 0)
            {
                ballXSpeed = random.Next(1, 10);
                ballYSpeed = random.Next(1, 10);
                ball.X = p1Count.X + p1Count.Width + 5;
                collisionSound.Play();
            }



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
                scoreSound.Play();
                player2Score++;
                if (player2Score > gameWinScore)
                {
                    GameOver("PLAYER 1 DOMINATES");
                }
                else
                {
                    SetParameters();
                }
                // TODO
                // --- play score sound
                // --- update player 2 score and display it to the label

                // TODO use if statement to check to see if player 2 has won the game. If true run 
                // GameOver() method. Else change direction of ball and call SetParameters() method.

            }

            if (ball.X > this.Width)  // ball hits left wall logic
            {
                scoreSound.Play();
                player1Score++;
                if (player1Score > gameWinScore)
                {
                    GameOver("PLAYER 2 DOMINATES");
                }
                else
                {
                    SetParameters();
                }
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

            gameUpdateLoop.Enabled = false;
            startLabel.Text = $"{winner} \n {startText}";
            startLabel.Visible = true;
            Refresh();

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

            if (p1counterTime > 0)
            {
                e.Graphics.FillRectangle(whiteBrush, p1Count);
            }
        }

    }
}
