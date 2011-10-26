﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace APO
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void otwórzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            Picture picture = new Picture();
            picture.MdiParent = this;
            picture.loadImage(openFileDialog1.FileName);
            picture.Show();
        }

        private void konwertujDoSzarości8BitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Picture activeChild = (Picture)this.ActiveMdiChild;

            // If there is an active child form, find the active control, which
            // in this example should be a RichTextBox.
            if (activeChild != null)
            {
                activeChild.MakeGrayscale3();
            }
        }

        private void duplikujToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Picture activeChild = (Picture)this.ActiveMdiChild;

            if (activeChild != null)
            {
                Picture newChild = new Picture(activeChild);
                newChild.MdiParent = this;
                newChild.Show();
            }
        }

        private void metodaPierwszaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Picture activeChild = (Picture)this.ActiveMdiChild;

            if (activeChild != null)
            {
                activeChild.Metoda1();
            }
        }

        private void metodaDrugaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Picture activeChild = (Picture)this.ActiveMdiChild;

            if (activeChild != null)
            {
                activeChild.Metoda2();
            }
        }

        private void metodaTrzeciaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Picture activeChild = (Picture)this.ActiveMdiChild;

            if (activeChild != null)
            {
                activeChild.Metoda3();
            }
        }

        private void negacjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Picture activeChild = (Picture)this.ActiveMdiChild;

            if (activeChild != null)
            {
                activeChild.negacja();
            }
        }

        private void progowanieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Picture activeChild = (Picture)this.ActiveMdiChild;

            if (activeChild != null)
            {
                activeChild.progowanie();
            }
        }
    }
}