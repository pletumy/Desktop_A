﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game02
{
    public partial class lvl_d : Form
    {
        bool right, left, up, down, gameOver;
        string facing = "up";
        int playerHealth = 100;
        int SelectChar;
        int ammo = 10;
        int enSpeed = 3;
        Random random = new Random();
        int score;
        List<PictureBox> enList = new List<PictureBox>();
        Char player;
        Char p1 = new Char();
        private int scoreFromPreviousLevel;
        public lvl_d(int choice)
        {
            InitializeComponent();
            SelectChar = choice;
            RestartGame();
        }

        private void lvl_d_Load(object sender, EventArgs e)
        {
            if (SelectChar == 1)
            {
                player = p1;
                picPlayer.Image = Properties.Resources.p1_a;
            }
            else if (SelectChar == 2)
            {
                player = p1;
                picPlayer.Image = Properties.Resources.p2_a;
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (SelectChar == 1)
            {
                picPlayer.BringToFront();
                switch (e.KeyCode)
                {
                    case Keys.Right:
                        right = true;
                        facing = "right";
                        picPlayer.Image = Properties.Resources.p1_D;
                        break;
                    case Keys.Left:
                        left = true;
                        facing = "left";
                        picPlayer.Image = Properties.Resources.p1_c;
                        break;
                    case Keys.Up:
                        up = true;
                        facing = "up";
                        picPlayer.Image = Properties.Resources.p1_b;
                        break;
                    case Keys.Down:
                        down = true;
                        facing = "down";
                        picPlayer.Image = Properties.Resources.p1_a;
                        break;
                }
            }
            else
            {
                picPlayer.BringToFront();
                switch (e.KeyCode)
                {
                    case Keys.Right:
                        right = true;
                        facing = "right";
                        picPlayer.Image = Properties.Resources.p2_d;
                        break;
                    case Keys.Left:
                        left = true;
                        facing = "left";
                        picPlayer.Image = Properties.Resources.p2_c;
                        break;
                    case Keys.Up:
                        up = true;
                        facing = "up";
                        picPlayer.Image = Properties.Resources.p2_b;
                        break;
                    case Keys.Down:
                        down = true;
                        facing = "down";
                        picPlayer.Image = Properties.Resources.p2_a;
                        break;
                }
            }

        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    right = false;
                    break;
                case Keys.Left:
                    left = false;
                    break;
                case Keys.Up:
                    up = false;
                    break;
                case Keys.Down:
                    down = false;
                    break;
            }
            if (e.KeyCode == Keys.Space && ammo > 0 && gameOver == false)
            {
                ammo--;
                ShootBullet(facing);
                if (ammo < 1)
                {
                    DropAmmo();
                }
            }
            if (a1.Bounds.IntersectsWith(picPlayer.Bounds))
            {
                lvl_e newlv = new lvl_e(SelectChar,score + scoreFromPreviousLevel);
                this.Hide();
                GameTimer.Stop();
                newlv.Show();
                this.Close();
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (playerHealth > 1)
            {
                HPbar.Value = playerHealth;
            }
            else
            {
                gameOver = true;
                GameTimer.Stop();
                this.Hide();
                GameOver go = new GameOver();
                go.ShowDialog();
                this.Close();
            }
            txtAmmo.Text = "Ammo: " + ammo;
            txtScore.Text = "Score: " + score;
            playerMove();
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "ammo")
                {
                    if (picPlayer.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        ammo += 5;
                    }
                }
                if (x is PictureBox && (string)x.Tag == "block")
                {
                    // Kiểm tra nếu vị trí của người chơi giao nhau với vị trí của PictureBox "block"
                    if (picPlayer.Bounds.IntersectsWith(x.Bounds))
                    {
                        if (right)
                        {
                            // Move player back to avoid collision
                            picPlayer.Left -= 10;
                        }
                        else if (left)
                        {
                            picPlayer.Left += 10;
                        }
                        if (up)
                        {
                            picPlayer.Top += 10;
                        }
                        else if (down)
                        {
                            picPlayer.Top -= 10;
                        }
                    }
                }

                if (x is PictureBox && (string)x.Tag == "en")
                {
                    if (picPlayer.Bounds.IntersectsWith(x.Bounds))
                    {
                        playerHealth -= 1;
                    }
                    if (x.Left > picPlayer.Left)
                    {
                        x.Left -= enSpeed;
                        ((PictureBox)x).Image = Properties.Resources.enLeft;
                    }
                    if (x.Left < picPlayer.Left)
                    {
                        x.Left += enSpeed;
                        ((PictureBox)x).Image = Properties.Resources.enRight;
                    }
                    if (x.Top > picPlayer.Top)
                    {
                        x.Top -= enSpeed;
                        ((PictureBox)x).Image = Properties.Resources.enBack;
                    }
                    if (x.Top < picPlayer.Top)
                    {
                        x.Top += enSpeed;
                        ((PictureBox)x).Image = Properties.Resources.enFont;
                    }
                }
                foreach (Control j in this.Controls)
                {
                    if (j is PictureBox && (string)j.Tag == "bullet" && x is PictureBox && (string)x.Tag == "en")
                    {
                        if (x.Bounds.IntersectsWith(j.Bounds))
                        {
                            IncreaseScore(10); // Tăng điểm số khi tiêu diệt một kẻ địch
                            this.Controls.Remove(j);
                            ((PictureBox)j).Dispose();
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                            enList.Remove(((PictureBox)x));
                            SpawnEnemy();
                        }
                    }
                }
            }
        }
        public void IncreaseScore(int points)
        {
            score += points;
        }

        private void ShootBullet(string direction)
        {
            Bullet shoot = new Bullet();
            shoot.direction = direction;
            shoot.bulletLeft = picPlayer.Left + (picPlayer.Width / 2);
            shoot.bulletTop = picPlayer.Top + (picPlayer.Height / 2);
            shoot.MakeBullet(this);
        }

        public void playerMove()
        {
            if (right == true)
            {
                if (picPlayer.Left < 500)
                {
                    picPlayer.Left += 10;
                }
            }
            if (left == true)
            {
                if (picPlayer.Left > 20)
                {
                    picPlayer.Left -= 10;
                }
            }
            if (up == true)
            {
                if (picPlayer.Top > 20)
                {
                    picPlayer.Top -= 10;
                }
            }
            if (down == true)
            {
                if (picPlayer.Top < 260)
                {
                    picPlayer.Top += 10;
                }
            }
        }

        public void DropAmmo()
        {
            PictureBox ammo = new PictureBox();
            ammo.Image = Properties.Resources.Shuriken;
            ammo.SizeMode = PictureBoxSizeMode.AutoSize;
            ammo.BackColor = Color.Transparent;
            ammo.Left = random.Next(100, this.ClientSize.Width - ammo.Width - 100);
            ammo.Top = random.Next(50, 210);
            ammo.Tag = "ammo";

            this.Controls.Add(ammo);
            ammo.BringToFront();
            picPlayer.BringToFront();
        }

        public void SpawnEnemy()
        {
            PictureBox en = new PictureBox();
            en.Tag = "en";
            en.Image = Properties.Resources.enFont;
            en.Left = random.Next(0, 700);
            en.Top = random.Next(0, 215);
            en.SizeMode = PictureBoxSizeMode.AutoSize;
            en.BackColor = Color.Transparent;
            enList.Add(en);
            this.Controls.Add(en);
            picPlayer.BringToFront();
        }

        public void RestartGame()
        {
            foreach (PictureBox i in enList)
            {
                this.Controls.Remove(i);
            }
            enList.Clear();
            for (int i = 0; i < 10; i++)
            {
                SpawnEnemy();
            }
            up = false;
            down = false;
            left = false;
            right = false;
            playerHealth = 100;
            score = 0;
            ammo = 10;
            GameTimer.Start();
        }


    }
}
