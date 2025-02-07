﻿/*
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
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        SolidBrush blueBrush = new SolidBrush(Color.Blue);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush greenBrush = new SolidBrush(Color.Green);
        SolidBrush greyBrush = new SolidBrush(Color.SaddleBrown);
        Pen blackPen, thinBlackPen, greyPen, bluePen, redPen, whitePen;
        Font drawFont = new Font("Courier New", 10);

        // Sounds for game
        SoundPlayer scoreSound = new SoundPlayer(Properties.Resources.score);
        SoundPlayer collisionSound = new SoundPlayer(Properties.Resources.collision);
        SoundPlayer deathSound = new SoundPlayer(Properties.Resources.playerDeath);
        SoundPlayer gameEnd = new SoundPlayer(Properties.Resources.GameEnd);
        SoundPlayer shootSound = new SoundPlayer(Properties.Resources.ShootNoise);
        SoundPlayer ropeSound = new SoundPlayer(Properties.Resources.Rope);

        //determines whether a key is being pressed or not
        Boolean wKeyDown, sKeyDown, aKeyDown, dKeyDown;
        Boolean iKeyDown, jKeyDown, kKeyDown, lKeyDown;
        Boolean qKeyDown, eKeyDown, oKeyDown, uKeyDown;
        Boolean cKeyDown, nKeyDown;
        Boolean lastPressWOrS, lastpressUpOrDown;

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
        const int PADDLE_EDGE = 35;  // buffer distance between screen edge and paddle            
        const int PADDLE_WIDTH = 10;
        const int PADDLE_HEIGHT = 40;
        Rectangle player1, player2;

        //Score Values
        Point pScore1, pScore2;

        //player and game scores
        int player1Score = 0;
        int player2Score = 0;
        int gameWinScore = 4;  // number of points needed to win game

        //Check Paralysis Clause or Deactivation
        bool p1Active = true;
        bool p2Active = true;

        bool p1dash, p2dash, wStateC, sStateC, upStateC, downStateC;

        double p1dashA, p2dashA;
        const int TIMER_WAIT = 10;
        int wWait, sWait, upWait, downWait;
        const int ACCELERATOR = 75;

        const int DASH_TIMER = 15;

        int dTrack1, dTrack2;

        int labelSpacer = 150;

        int p1counterTime = 0, p2counterTime = 0, p1countercool = 0, p2countercool = 0;
        Rectangle p1Count, p2Count;
        const int COUNTER_DURATION = 8;
        int p1coolDuration = 25, p2coolDuration = 25;

        const int HEALTH_SETTING = 100;
        int p1Health = HEALTH_SETTING, p2Health = HEALTH_SETTING;
        Point p1HBar, p2HBar;
        const int BALL_DAMAGE = 20;

        const int SPEED_LIMX = 11, SPEED_LIMY = 6;

        int p1JumpT, p2JumpT;
        double p1JumpA, p2JumpA;
        bool p1JF = false, p2JF = false;

        const int JUMP_A = 10, JUMP_T = 15;

        Image[] handLeft = new Image[8];
        Image[] handRight = new Image[8];

        Rectangle handLeftRect, handRightRect;
        int p1handHeight = 50, p2HandHeight = 50;
        int p1HandState = 0, p2HandState = 0;

        string startText;
        #endregion

        public Form1()
        {
            InitializeComponent();
            startText = startLabel.Text;
            blackPen = new Pen(blackBrush, 16);
            thinBlackPen = new Pen(blackBrush, 8);
            greyPen = new Pen(greyBrush, 45);
            bluePen = new Pen(blueBrush, 5);
            redPen = new Pen(redBrush, 5);
            whitePen = new Pen(whiteBrush, 5);

            handLeft[0] = Properties.Resources.HandLeft__1_;
            handLeft[1] = Properties.Resources.HandLeft__2_;
            handLeft[2] = Properties.Resources.HandLeft__3_;
            handLeft[3] = Properties.Resources.HandLeft__4_;
            handLeft[4] = Properties.Resources.HandLeft__5_;
            handLeft[5] = Properties.Resources.HandLeft__6_;
            handLeft[6] = Properties.Resources.HandLeft__7_;
            handLeft[7] = Properties.Resources.HandLeft__8_;

            handRight[0] = Properties.Resources.HandRight__1_;
            handRight[1] = Properties.Resources.HandRight__2_;
            handRight[2] = Properties.Resources.HandRight__3_;
            handRight[3] = Properties.Resources.HandRight__4_;
            handRight[4] = Properties.Resources.HandRight__5_;
            handRight[5] = Properties.Resources.HandRight__6_;
            handRight[6] = Properties.Resources.HandRight__7_;
            handRight[7] = Properties.Resources.HandRight__8_;
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
                dTrack1 = 0;
                p1dashA = ACCELERATOR;
                wTime.Enabled = false;
            }

            if (wWait >= TIMER_WAIT)
            {
                wTime.Enabled = false;
            }
        }

        private void upTime_Tick(object sender, EventArgs e)
        {
            upWait += 1;
            if (iKeyDown == false)
            {
                upStateC = true;
            }

            if (upStateC && iKeyDown)
            {
                p2dash = true;
                dTrack2 = 0;
                p2dashA = ACCELERATOR;
                upTime.Enabled = false;
            }

            if (upWait >= TIMER_WAIT)
            {
                upTime.Enabled = false;
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

        private void downTime_Tick(object sender, EventArgs e)
        {
            downWait += 1;
            if (kKeyDown == false)
            {
                downStateC = true;
            }

            if (downStateC && kKeyDown)
            {
                p2dash = true;
                dTrack2 = 0;
                p2dashA = ACCELERATOR;
                downTime.Enabled = false;
            }

            if (downWait >= TIMER_WAIT)
            {
                downTime.Enabled = false;
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
                    if (p1HandState > 0)
                    {
                        p1HandState++;
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
                case Keys.A:
                    aKeyDown = true;
                    break;
                case Keys.D:
                    dKeyDown = true;
                    break;
                case Keys.I:
                    iKeyDown = true;
                    if (upTime.Enabled == false)
                    {
                        upTime.Enabled = true;
                        upStateC = false;
                        upWait = 0;
                    }
                    if (p2HandState > 0)
                    {
                        p2HandState++;
                    }
                    break;
                case Keys.J:
                    jKeyDown = true;
                    break;
                case Keys.K:
                    kKeyDown = true;
                    if (downTime.Enabled == false)
                    {
                        downTime.Enabled = true;
                        downStateC = false;
                        downWait = 0;
                    }
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
                case Keys.O:
                    oKeyDown = true;
                    break;
                case Keys.E:
                    eKeyDown = true;
                    break;
                case Keys.C:
                    cKeyDown = true;
                    break;
                case Keys.N:
                    nKeyDown = true;
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

        private void SkillCheck()
        {
            if (player1Score > gameWinScore)
            {
                GameOver("PLAYER 1 DOMINATES");
            }
            else if (player2Score > gameWinScore)
            {
                GameOver("PLAYER 2 DOMINATES");
            }
            else
            {
                SetParameters();
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //check to see if a key has been released and set its KeyDown value to false if it has
            switch (e.KeyCode)
            {
                case Keys.W:
                    wKeyDown = false;
                    if (p1HandState < 50)
                    {
                        p1HandState--;
                    }
                    break;
                case Keys.S:
                    sKeyDown = false;
                    break;
                case Keys.A:
                    aKeyDown = false;
                    break;
                case Keys.D:
                    dKeyDown = false;
                    break;
                case Keys.I:
                    iKeyDown = false;
                    if (p2HandState < 50)
                    {
                        p2HandState--;
                    }
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
                case Keys.O:
                    oKeyDown = false;
                    break;
                case Keys.E:
                    eKeyDown = false;
                    break;
                case Keys.C:
                    cKeyDown = false;
                    break;
                case Keys.N:
                    nKeyDown = false;
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
            p2HBar = new Point((this.Width / 2) + labelSpacer - 10, this.Height - 35);
            p1HBar = new Point((this.Width / 2) - labelSpacer, this.Height - 35);
            p1Health = HEALTH_SETTING;
            p2Health = HEALTH_SETTING;
            ballXSpeed = random.Next(1, SPEED_LIMX);
            ballYSpeed = random.Next(1, SPEED_LIMY);
            if (random.Next(1, 3) == 1)
            {
                ballXSpeed *= -1;
            }
            if (random.Next(1, 3) == 1)
            {
                ballYSpeed *= -1;
            }
            p2JumpA = p1JumpA = 0;
            p2JumpT = p1JumpT = 0;
        }

        private void PControls(bool playerNum, bool dash, int playerID)
        {
            if (playerNum)
            {
                if (p1JumpT > 0)
                {
                    p1JumpT--;
                    if (p1JumpT <= 0 && p1JF)
                    {
                        p1JumpT = JUMP_T - 1;
                        p1JumpA = JUMP_A;
                        p1JF = !p1JF;
                    } else if (p1JumpT <= 0)
                    {
                        p1JumpA = 0;
                    }

                    if (p1JF)
                    {
                        player1.X += Convert.ToInt32(p1JumpA);
                        p1JumpA = (-1 / 250) * (Math.Pow(p1JumpA, 2)) + 25;
                    } else if (p1JumpT > 0)
                    {
                        player1.X -= Convert.ToInt32(p1JumpA);
                        p1JumpA = (-1 / 250) * (Math.Pow(p1JumpA, 2)) + 25;
                    }
                }

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
                    if (lastPressWOrS)
                    {
                        player1.Y -= Convert.ToInt16(p1dashA);
                        p1dashA = (-1 / 250) * (Math.Pow(p1dashA, 2)) + 50;
                        dTrack1 += 1;
                        if (dTrack1 > DASH_TIMER)
                        {
                            p1dash = false;
                        }
                        return;
                    }

                    if (!lastPressWOrS)
                    {
                        player1.Y += Convert.ToInt16(p1dashA);
                        p1dashA = (-1 / 250) * (Math.Pow(p1dashA, 2)) + 50;
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
                if (p2JumpT > 0)
                {
                    p2JumpT--;
                    if (p2JumpT <= 0 && p2JF)
                    {
                        p2JumpT = JUMP_T - 1;
                        p2JumpA = JUMP_A;
                        p2JF = !p2JF;
                    }
                    else if (p2JumpT <= 0)
                    {
                        p2JumpA = 0;
                    }

                    if (p2JF)
                    {
                        player2.X -= Convert.ToInt32(p2JumpA);
                        p2JumpA = (-1 / 250) * (Math.Pow(p2JumpA, 2)) + 25;
                    }
                    else if (p2JumpT > 0)
                    {
                        player2.X += Convert.ToInt32(p2JumpA);
                        p2JumpA = (-1 / 250) * (Math.Pow(p2JumpA, 2)) + 25;
                    }
                }

                if (!dash)
                {
                    if (kKeyDown)
                    {
                        player2.Y += PADDLE_SPEED;
                    }
                    if (iKeyDown)
                    {
                        player2.Y -= PADDLE_SPEED;
                    }
                    return;
                }

                if (dash)
                {
                    if (lastpressUpOrDown)
                    {
                        player2.Y -= Convert.ToInt16(p2dashA);
                        p2dashA = (-1 / 250) * (Math.Pow(p2dashA, 2)) + 50;
                        dTrack2 += 1;
                        if (dTrack2 > DASH_TIMER)
                        {
                            p2dash = false;
                        }
                        return;
                    }

                    if (!lastpressUpOrDown)
                    {
                        player2.Y += Convert.ToInt16(p2dashA);
                        p2dashA = (-1 / 250) * (Math.Pow(p2dashA, 2)) + 50;
                        dTrack2 += 1;
                        if (dTrack2 > DASH_TIMER)
                        {
                            p2dash = false;
                        }
                        return;
                    }
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

            if (wKeyDown && !sKeyDown)
            {
                lastPressWOrS = true;
            }
            else if (sKeyDown && !wKeyDown)
            {
                lastPressWOrS = false;
            }

            if (iKeyDown && !kKeyDown)
            {
                lastpressUpOrDown = true;
            }
            else if (iKeyDown && !kKeyDown)
            {
                lastpressUpOrDown = false;
            }

            if (aKeyDown)
            {
                ballXSpeed = Math.Abs((player1.X + (player1.Width / 2)) - (ball.X + (ball.Width / 2))) / 10;
                if (ball.Y + (ball.Height / 2) > player1.Y + (player1.Height / 2))
                {
                    ballYSpeed = -Math.Abs((player1.Y + (player1.Height / 2)) - (ball.Y + (ball.Height / 2))) / 10;
                }
                else
                {
                    ballYSpeed = Math.Abs((ball.Y + (ball.Height / 2)) - (player1.Y + (player1.Height / 2))) / 10;
                }
            }

            if (lKeyDown)
            {
                ballXSpeed = -Math.Abs((player1.X + (player1.Width / 2)) - (ball.X + (ball.Width / 2))) / 10;
                if (ball.Y + (ball.Height / 2) > player1.Y + (player1.Height / 2))
                {
                    ballYSpeed = -Math.Abs((player1.Y + (player1.Height / 2)) - (ball.Y + (ball.Height / 2))) / 10;
                }
                else
                {
                    ballYSpeed = Math.Abs((ball.Y + (ball.Height / 2)) - (player1.Y + (player1.Height / 2))) / 10;
                }
            }

            if (dKeyDown && p1JumpA == 0)
            {
                p1JumpA = JUMP_A;
                p1JumpT = JUMP_T;
                p1JF = true;
                ropeSound.Play();
            }

            if (jKeyDown && p2JumpA == 0)
            {
                p2JumpA = JUMP_A;
                p2JumpT = JUMP_T;
                p2JF = true;
                ropeSound.Play();
            }

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

            if ((player1.Y + player1.Height) > this.Height - 50)
            {
                player1.Y = this.Height - 50 - player1.Height;
            }
            else if (player1.Y < 50)
            {
                player1.Y = 50;
            }

            if (p2Active)
            {
                PControls(false, p2dash, 0);
            }

            if ((player2.Y + player2.Height) > this.Height - 50)
            {
                player2.Y = this.Height - 50 - player2.Height;
            }
            else if (player2.Y < 50)
            {
                player2.Y = 50;
            }

            // TODO create an if statement and code to move player 1 down 

            // TODO create an if statement and code to move player 2 up

            // TODO create an if statement and code to move player 2 down

            #endregion

            #region ball collision with top and bottom lines

            if (ball.Y < 45)
            {
                ballYSpeed *= -1;
                ball.Y = 50;
                collisionSound.Play();
            }
            else if (ball.Y + ball.Height > this.Height - 45)
            {
                ballYSpeed *= -1;
                ball.Y = this.Height - ball.Height - 50;
                collisionSound.Play();
            }

            #endregion

            #region ball collision with paddles
            p1countercool--;
            if (p1countercool < 0)
            {
                p1countercool = 0;
            }
            if (qKeyDown && p1countercool <= 0 && p1Health > 0)
            {
                p1Count = new Rectangle(player1.X + (player1.Width * 2) + 5, player1.Y, player1.Width, player1.Height);
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
                ballXSpeed = random.Next(1, SPEED_LIMX);
                ballYSpeed = random.Next(1, SPEED_LIMY);
                ball.X = p1Count.X + p1Count.Width + 5;
                p1Health += BALL_DAMAGE;
                collisionSound.Play();
            }

            if (ball.IntersectsWith(player1) && p1Health > 0)
            {
                if (p1JumpA > 0)
                {
                    ball.X = player1.X + player1.Width + 5;
                    ballXSpeed = random.Next(1, SPEED_LIMX);
                    ballYSpeed = random.Next(1, SPEED_LIMY);
                }
                else
                {
                    p1Health -= BALL_DAMAGE;
                    ball.Location = new Point((this.Width / 2) - (BALL_WIDTH / 2), (this.Height / 2) - (BALL_HEIGHT / 2));
                    ballXSpeed = random.Next(1, SPEED_LIMX);
                    ballYSpeed = random.Next(1, SPEED_LIMY);
                    if (random.Next(1, 3) == 1)
                    {
                        ballXSpeed *= -1;
                    }
                    if (random.Next(1, 3) == 1)
                    {
                        ballYSpeed *= -1;
                    }
                }
            }

            p2countercool--;
            if (p2countercool < 0)
            {
                p2countercool = 0;
            }
            if (oKeyDown && p2countercool <= 0 && p2Health > 0)
            {
                p2Count = new Rectangle(player2.X - (player2.Width * 2) - 5, player2.Y, player2.Width * 2, player2.Height);
                p2counterTime = COUNTER_DURATION;
                p2countercool = p2coolDuration;
            }
            p2counterTime--;
            if (p2counterTime < 0)
            {
                p2counterTime = 0;
            }
            if (ball.IntersectsWith(p2Count) && p2counterTime > 0)
            {
                ballXSpeed = -random.Next(1, SPEED_LIMX);
                ballYSpeed = random.Next(1, SPEED_LIMY);
                ball.X = p2Count.X - p2Count.Width - 5;
                p2Health += BALL_DAMAGE;
                collisionSound.Play();
            }

            if (ball.IntersectsWith(player2) && p2Health > 0)
            {
                if (p2JumpA > 0)
                {
                    ball.X = player2.X + player2.Width + 5;
                    ballXSpeed = random.Next(1, SPEED_LIMX);
                    ballYSpeed = random.Next(1, SPEED_LIMY);
                }
                else
                {
                    p2Health -= BALL_DAMAGE;
                    ball.Location = new Point((this.Width / 2) - (BALL_WIDTH / 2), (this.Height / 2) - (BALL_HEIGHT / 2));
                    ballXSpeed = random.Next(1, SPEED_LIMX);
                    ballYSpeed = random.Next(1, SPEED_LIMY);
                    if (random.Next(1, 3) == 1)
                    {
                        ballXSpeed *= -1;
                    }
                    if (random.Next(1, 3) == 1)
                    {
                        ballYSpeed *= -1;
                    }
                }
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
                SkillCheck();

            }

            if (ball.X > this.Width)  // ball hits left wall logic
            {
                scoreSound.Play();
                player1Score++;
                SkillCheck();
            }
            // TODO same as above but this time check for collision with the right wall

            if (p1Health <= 0 && p1Health > -55555 )
            {
                deathSound.Play();
                p1Health = -55555;
            }

            if (p2Health <= 0 && p2Health > -55555)
            {
                deathSound.Play();
                p2Health = -55555;
            }

            handLeftRect = new Rectangle(player1.X + (player1.Width / 2) - 50, p1handHeight, 100, 50);

            p1HandState++;
            if (p1HandState > 7)
            {
                p1HandState = 0;
            }

            handRightRect = new Rectangle(player2.X + (player2.Width / 2) - 50, p2HandHeight, 100, 50);

            p2HandState++;
            if (p2HandState > 7)
            {
                p2HandState = 0;
            }

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
            gameEnd.Play();

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
            if (p1Health > 0)
            {
                e.Graphics.DrawLine(bluePen, player1.X + (player1.Width / 2), player1.Y + 5, ball.X + (ball.Width / 2), ball.Y + (ball.Height / 2));
                e.Graphics.DrawLine(bluePen, player1.X + (player1.Width / 2), player1.Y + player1.Height - 5, ball.X + (ball.Width / 2), ball.Y + (ball.Height / 2));
            }
            if (p2Health > 0)
            {
                e.Graphics.DrawLine(redPen, player2.X + (player2.Width / 2), player2.Y + 5, ball.X + (ball.Width / 2), ball.Y + (ball.Height / 2));
                e.Graphics.DrawLine(redPen, player2.X + (player2.Width / 2), player2.Y + player2.Height - 5, ball.X + (ball.Width / 2), ball.Y + (ball.Height / 2));
            }

            e.Graphics.DrawLine(greyPen, 0, 22, this.Width, 22);
            e.Graphics.DrawLine(greyPen, 0, this.Height - 22, this.Width, this.Height - 22);
            // TODO draw player2 using FillRectangle
            if (p1Health > 0)
            {
                e.Graphics.FillRectangle(blueBrush, player1);
            }
            if (p2Health > 0)
            {
                e.Graphics.FillRectangle(redBrush, player2);
            }

            // TODO draw ball using FillRectangle
            ball.Height = ball.Width = ((this.Width / 2) - Math.Abs((this.Width / 2) - ball.X)) / 5;
            e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(random.Next(100, 256), random.Next(1, 256), random.Next(1, 256), random.Next(1, 256))), ball);
            //Draw Player Health
            if (gameUpdateLoop.Enabled)
            {
                e.Graphics.DrawString(Convert.ToString(p1Health), startLabel.Font, whiteBrush, p1HBar);
                e.Graphics.DrawString(Convert.ToString(p2Health), startLabel.Font, whiteBrush, p2HBar);
            }

            e.Graphics.DrawLine(blackPen, 0, 0, 0, this.Height);
            e.Graphics.DrawLine(blackPen, this.Width, 0, this.Width, this.Height);
            e.Graphics.DrawLine(blackPen, 0, 0, this.Width, 0);
            e.Graphics.DrawLine(blackPen, 0, this.Height, this.Width, this.Height);
            e.Graphics.DrawLine(thinBlackPen, 0, 40, this.Width, 40);
            e.Graphics.DrawLine(thinBlackPen, 0, this.Height - 40, this.Width, this.Height - 40);


            if (newGameOk == false)
            {
                e.Graphics.DrawString(Convert.ToString(player1Score), startLabel.Font, whiteBrush, pScore1);
                e.Graphics.DrawString(Convert.ToString(player2Score), startLabel.Font, whiteBrush, pScore2);
            }

            if (p1counterTime > 0)
            {
                e.Graphics.DrawArc(whitePen, player1.X - (player1.Width * 2), player1.Y - 10, p1Count.Width * 5, p1Count.Height + 20, -90, 180 / p1counterTime);
            }

            if (p2counterTime > 0)
            {
                e.Graphics.DrawArc(whitePen, player2.X - (player2.Width * 2), player2.Y - 10, p2Count.Width * 3, p2Count.Height + 20, -90, -180 / p2counterTime);
            }

            e.Graphics.DrawLine(bluePen, player1.X + (player1.Width / 2), handLeftRect.Y  +20, player1.X + (player1.Width / 2), player1.Y + 10);
            e.Graphics.DrawLine(redPen, player2.X + (player2.Width / 2), handRightRect.Y  +20, player2.X + (player2.Width / 2), player2.Y + 10);


            e.Graphics.DrawImage(handLeft[p1HandState], handLeftRect);
            e.Graphics.DrawImage(handRight[p2HandState], handRightRect);
        }

    }
}
