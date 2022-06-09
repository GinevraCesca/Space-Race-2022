using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Space_Race_2022
{
    public partial class Form1 : Form
    {
        Rectangle p1 = new Rectangle(200, 500, 10, 25);
        Rectangle p2 = new Rectangle(400, 500, 10, 25);
        Rectangle top = new Rectangle(0, 0, 600, 1);
        List<Rectangle> leftAsteroids = new List<Rectangle>();
        List<Rectangle> rightAsteroids = new List<Rectangle>();

        List<int> asteroidsSpeed = new List<int>();
        int asteroidsSize = 5;

        int playerSpeed = 10;
        int p1Score = 0;
        int p2Score = 0;

        bool p1Up = false;
        bool p1Down = false;
        bool p2Up = false;
        bool p2Down = false;
        bool space = false;
        bool esc = false;

        int time = 1000;

        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush blackBrush = new SolidBrush(Color.Black);

        Random randGen = new Random();

        int randValue = 0;
        string gameState = "waiting";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    p1Up = true;
                    break;
                case Keys.S:
                    p1Down = true;
                    break;
                case Keys.Up:
                    p2Up = true;
                    break;
                case Keys.Down:
                    p2Down = true;
                    break;
                case Keys.Space:
                    if (gameState == "waiting" || gameState == "over")
                    {
                        GameInizialize();
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "waiting" || gameState == "over")
                    {
                        Application.Exit();
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    p1Up = false;
                    break;
                case Keys.S:
                    p1Down = false;
                    break;
                case Keys.Up:
                    p2Up = false;
                    break;
                case Keys.Down:
                    p2Down = false;
                    break;
                case Keys.Space:
                    space = false;
                    break;
                case Keys.Escape:
                    esc = false;
                    break;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(blackBrush, top);

            if (gameState == "running")
            {
                e.Graphics.FillRectangle(whiteBrush, p1);
                e.Graphics.FillRectangle(whiteBrush, p2);
            }

            for (int i = 0; i < leftAsteroids.Count; i++)
            {
                e.Graphics.FillRectangle(whiteBrush, leftAsteroids[i]);
            }

            for (int i = 0; i < rightAsteroids.Count; i++)
            {
                e.Graphics.FillRectangle(whiteBrush, rightAsteroids[i]);
            }

            if (gameState == "waiting")
            {
                titleLabel.Text = "SPACE RACE 2022";
                subTitleLabel.Text = "Press Space Bar to Start or Escape to Exit";
            }
        }

        private void gameEngine_Tick(object sender, EventArgs e)
        {
            //move player1
            if (p1Up == true && p1.Y > 0)
            {
                p1.Y -= playerSpeed;
            }

            if (p1Down == true && p1.Y < this.Height - p1.Height)
            {
                p1.Y += playerSpeed;
            }

            //move player2
            if (p2Up == true && p2.Y > 0)
            {
                p2.Y -= playerSpeed;
            }

            if (p2Down == true && p2.Y < this.Height - p2.Height)
            {
                p2.Y += playerSpeed;
            }

            //create asteroids
            randValue = randGen.Next(1, 100);

            if (randValue < 20)
            {
                int y = randGen.Next(10, this.Height - 100);
                leftAsteroids.Add(new Rectangle(0, y, 5, 5));
                asteroidsSpeed.Add(randGen.Next(2, 10));
            }

            else if (randValue < 40)
            {
                int y = randGen.Next(10, this.Height - 100);
                rightAsteroids.Add(new Rectangle(this.Width - 5, y, 5, 5));
                asteroidsSpeed.Add(randGen.Next(2, 10));
            }

            //move asteroids from left to right
            for (int i = 0; i < leftAsteroids.Count(); i++)
            {
                int x = leftAsteroids[i].X + asteroidsSpeed[i];
                leftAsteroids[i] = new Rectangle(x, leftAsteroids[i].Y, asteroidsSize, asteroidsSize);
            }

            for (int i = 0; i < leftAsteroids.Count(); i++)
            {
                if (leftAsteroids[i].Y > this.Width - asteroidsSize)
                {
                    leftAsteroids.RemoveAt(i);
                    asteroidsSpeed.RemoveAt(i); ;
                }
            }

            //move asteroids from right to left
            for (int i = 0; i < rightAsteroids.Count(); i++)
            {
                int x = rightAsteroids[i].X - asteroidsSpeed[i];
                rightAsteroids[i] = new Rectangle(x, rightAsteroids[i].Y, asteroidsSize, asteroidsSize);
            }

            for (int i = 0; i < rightAsteroids.Count(); i++)
            {
                if (rightAsteroids[i].Y < 0 - asteroidsSize)
                {
                    rightAsteroids.RemoveAt(i);
                    asteroidsSpeed.RemoveAt(i); ;
                }
            }

            //get points
            if (p1.IntersectsWith(top))
            {
                p1Score++;
                p1ScoreLabel.Text = $"{p1Score}";
                p1.X = 150;
                p1.Y = this.Height - p1.Height;
            }

            if (p2.IntersectsWith(top))
            {
                p2Score++;
                p2ScoreLabel.Text = $"{p2Score}";
                p2.X = 450;
                p2.Y = this.Height - p2.Height;
            }

            //player 1 touch the asteroids
            for (int i = 0; i < leftAsteroids.Count(); i++)
            {
                if (p1.IntersectsWith(leftAsteroids[i]))
                {
                    p1.X = 150;
                    p1.Y = this.Height - p1.Height;
                }

                else if (p2.IntersectsWith(leftAsteroids[i]))
                {
                    p2.X = 450;
                    p2.Y = this.Height - p2.Height;
                }
            }

            //player 2 touch the asteroids
            for (int i = 0; i < rightAsteroids.Count(); i++)
            {
                if (p1.IntersectsWith(rightAsteroids[i]))
                {
                    p1.X = 150;
                    p1.Y = this.Height - p1.Height;
                }

                else if (p2.IntersectsWith(rightAsteroids[i]))
                {
                    p2.X = 450;
                    p2.Y = this.Height - p2.Height;
                }
            }

            time--;
            timeLabel.Text = $"time left = {time}";

            //game over
            if (time == 0)
            {
                if (p1Score > p2Score)
                {
                    gameState = "over";
                    titleLabel.Text = $"Player 1 won with {p1Score} points. Congratulation!";
                    subTitleLabel.Text = "Press Space Bar to Play Again or Escape to Exit";
                    p1ScoreLabel.Text = "";
                    p2ScoreLabel.Text = "";

                    gameEngine.Enabled = false;
                    time = 1000;
                    p1Score = 0;
                    p2Score = 0;
                    leftAsteroids.Clear();
                    rightAsteroids.Clear();
                    asteroidsSpeed.Clear();
                }

                else if (p2Score > p1Score)
                {
                    gameState = "over";
                    titleLabel.Text = $"Player 2 won with {p2Score} points. Congratulation!";
                    subTitleLabel.Text = "Press Space Bar to Play Again or Escape to Exit";
                    p1ScoreLabel.Text = "";
                    p2ScoreLabel.Text = "";

                    gameEngine.Enabled = false;
                    time = 1000;
                    p1Score = 0;
                    p2Score = 0;
                    leftAsteroids.Clear();
                    rightAsteroids.Clear();
                    asteroidsSpeed.Clear();
                }

                else
                {
                    gameState = "over";
                    titleLabel.Text = $"It's a Tie";
                    subTitleLabel.Text = "Press Space Bar to Play Again or Escape to Exit";
                    p1ScoreLabel.Text = "";
                    p2ScoreLabel.Text = "";

                    gameEngine.Enabled = false;
                    time = 1000;
                    p1Score = 0;
                    p2Score = 0;
                    leftAsteroids.Clear();
                    rightAsteroids.Clear();
                    asteroidsSpeed.Clear();
                }
            }

            Refresh();
        }

        public void GameInizialize()
        {
            titleLabel.Text = "";
            subTitleLabel.Text = "";
            p1ScoreLabel.Text = "0";
            p2ScoreLabel.Text = "0";

            gameEngine.Enabled = true;
            gameState = "running";
            time = 1000;
            p1Score = 0;
            p2Score = 0;
            leftAsteroids.Clear();
            rightAsteroids.Clear();
            asteroidsSpeed.Clear();

            p1.X = 150;
            p1.Y = this.Height - p1.Height;

            p2.X = 450;
            p2.Y = this.Height - p2.Height;
        }
    }
}
