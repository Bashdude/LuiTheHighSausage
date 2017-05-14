using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace High_Lui
{
    public partial class window : Form
    {
        bool goright;
        bool goleft;
        int speed = 5;
        int score = 0;
        bool isPressed;
        int totalEnemies = 52;
        int playerSpeed = 5;

        public window()
        {
            InitializeComponent();
        }
        // Tastdruck erkennen
        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = true;
            }
            if (e.KeyCode == Keys.Space && !isPressed)
            {
                isPressed = true;
                makejoint();
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = false;
            }
            if (isPressed)
            {
                isPressed = false;
            }
        }
        // Zustandsänderungsprüfung per Timer 
        private void timer1_Tick(object sender, EventArgs e)
        {   // Spieler Bewegung
            if (goleft)
            {
                lui.Left -= playerSpeed;
            }
            if (goright)
            {
                lui.Left += playerSpeed;
            }
            foreach (Control x in this.Controls)
            {   // Enemies Bewegung und Kolisionsprüfung
                if (x is PictureBox && x.Tag == "Cops")
                {
                    if (((PictureBox)x).Bounds.IntersectsWith(lui.Bounds))
                    {
                        gameOver();
                    }
                    ((PictureBox)x).Left += speed;
                    
                    if (((PictureBox)x).Left > 1000)
                    {
                        ((PictureBox)x).Top += ((PictureBox)x).Height + 10;
                        ((PictureBox)x).Left = -50;
                    }
                }
            }
            // joint movement und entfernung am Bildschirmrand
            foreach (Control y in this.Controls)
            {
                if (y is PictureBox && y.Tag == "joint")
                {  
                    
                    y.Top -= 4;
                    
                    if (((PictureBox)y).Top < this.Height - 670)
                    {
                        this.Controls.Remove(y);
                    }
                }
            }
            // Trefferprüfung Entfernen Gegner und Score änderung 
            foreach (Control i in this.Controls)
            {
                foreach (Control j in this.Controls)
                {
                    if (i is PictureBox && i.Tag == "Cops")
                    {
                        if (j is PictureBox && j.Tag == "joint")
                        {
                            if (i.Bounds.IntersectsWith(j.Bounds))
                            {
                                score++;
                                this.Controls.Remove(i);
                                this.Controls.Remove(j);
                            }
                        }
                    }
                }
            }
            // Score prüfung
        
            punkte.Text = "Score : " + score;

            if (score > totalEnemies - 1)
            {
                gameOver();
                MessageBox.Show("You succesfully saved your Stash!" +
                    "Happy 420 BRO XD");
            }
        }
        private void makejoint()
        {
            PictureBox joint = new PictureBox();
            joint.Image = Properties.Resources.joint;
            joint.Size = new Size(5, 20);
            joint.Tag = "joint";
            joint.Left = lui.Left + lui.Width / 2;
            joint.Top = lui.Top - 20;
            this.Controls.Add(joint);
            joint.BringToFront();
        }

        private void gameOver()
        {
            timer1.Stop();
            punkte.Text += "Game Over";
        }
    }
}
