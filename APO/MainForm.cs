﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using APO.Operacje;

namespace APO
{
    public partial class MainForm : Form
    {
        private Int32 formCounter = 0;

        public MainForm()
        {
            InitializeComponent();
        }

        public void ustawStatusTekst(string text){
            this.toolStripStatusLabel1.Text = text;
        }

        private void useFilter(IFilter filter)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            filter.setImage(activeChild.bitmap);

            if (filter.hasDialog)
                if (!filter.showDialog())
                    return;

            filter.Convert();

            activeChild.refresh();
        }

        private void otwórzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            PictureForm picture = new PictureForm(this
);
            picture.MdiParent = this;
            picture.Text = new StringBuilder("Obraz ").Append(++formCounter).ToString();
            picture.loadImage(openFileDialog1.FileName);
            picture.Show();
        }

        private void konwertujDoSzarości8BitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            useFilter(new GrayScale());
        }

        private void duplikujToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild != null)
            {
                PictureForm newChild = new PictureForm(activeChild);
                newChild.Text = new StringBuilder("Obraz ").Append(++formCounter).ToString();
                newChild.MdiParent = this;
                newChild.Show();
            }
        }

        private void metodaPierwszaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            useFilter(new HistogramEqualization());
        }

        private void metodaDrugaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            useFilter(new HistogramEqualization(
                HistogramEqualization.EqualizationMethod.RandomLevel));
        }

        private void metodaTrzeciaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            useFilter(new HistogramEqualization(
                HistogramEqualization.EqualizationMethod.AdjacencyLevel));
        }

        private void negacjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild != null)
            {
                activeChild.negacja();
            }
        }

        private void progowanieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;

            myCustomDialog dialog = new myCustomDialog("Progowanie", "Podaj wartość do progowania");

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            activeChild.progowanie(Convert.ToInt32(dialog.value));
        }

        private void redukcjaPoziomówSzarościToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;

            myCustomDialog dialog = new myCustomDialog("Redukcja poziomów szarości", "Podaj ilość poziomów");

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            activeChild.redukcjaPoziomowSzarosci(Convert.ToInt32(dialog.value));
        }

        private void rozciaganieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;

            myCustomDialog dialog = new myCustomDialog("Rozciąganie", 
                "Podaj początkowy poziom", "Podaj końcowy poziom");

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            activeChild.rozciaganie(Convert.ToInt32(dialog.value), Convert.ToInt32(dialog.value2));
        }

        private void kontrastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;

            myCustomDialog dialog = new myCustomDialog("Jasność", "Podaj o ile procent zwiększyć jasność");

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            activeChild.jasnosc(Convert.ToInt32(dialog.value));
        }

        private void kontrastToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;

            myCustomDialog dialog = new myCustomDialog("Kontrast", "Podaj o ile procent zwiększyć kontrast");

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            activeChild.kontrast(Convert.ToInt32(dialog.value));
        }

        private void gammaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;

            myCustomDialog dialog = new myCustomDialog("Gamma", "Podaj o ile procent zwiększyć gamme");

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            activeChild.gamma(Convert.ToDouble(dialog.value));
        }

        private void dodawanieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;

            myCustomDialog dialog = new myCustomDialog("Dodawanie", 
                "Wybierz obraz który chcesz dodać", this.MdiChildren);

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            activeChild.add(((PictureForm)this.MdiChildren[dialog.combovalue]).bitmap);
        }

        private void odejmowanieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;

            myCustomDialog dialog = new myCustomDialog("Odejmowanie",
                "Wybierz obraz który chcesz odjąć", this.MdiChildren);

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            activeChild.sub(((PictureForm)this.MdiChildren[dialog.combovalue]).bitmap);
        }

        private void aNDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;

            myCustomDialog dialog = new myCustomDialog("Operacja logiczna AND",
                "Wybierz obraz", this.MdiChildren);

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            activeChild.and(((PictureForm)this.MdiChildren[dialog.combovalue]).bitmap);
        }

        private void oRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;

            myCustomDialog dialog = new myCustomDialog("Operacja logiczna OR",
                "Wybierz obraz", this.MdiChildren);

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            activeChild.or(((PictureForm)this.MdiChildren[dialog.combovalue]).bitmap);
        }

        private void xORToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;

            myCustomDialog dialog = new myCustomDialog("Operacja logiczna XOR",
                "Wybierz obraz", this.MdiChildren);

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

			activeChild.xor(((PictureForm)this.MdiChildren[dialog.combovalue]).bitmap);
        }

        private void wyosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;

            Mask3x3Form mask3x3Form = new Mask3x3Form();

            if (mask3x3Form.ShowDialog() == DialogResult.Cancel)
                return;
            activeChild.ApplyMask(mask3x3Form.Mask, mask3x3Form.Divisor);
        }

        private void filtracjaMedianowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;

            myCustomDialog dialog = new myCustomDialog("Filtracja Medianowa", "Podaj rozmiar otoczenia (jedna cyfra)");

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;
            activeChild.FiltracjaMedianowa(Convert.ToInt32(dialog.value));
        }

        private void szkiletyzacjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;

            activeChild.Szkieletyzacja();
        }

        private void spójnaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;
            activeChild.Erozja(4);
        }

        private void spójnaToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;
            activeChild.Erozja(8);
        }

        private void spójnaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;
            activeChild.Dylatacja(4);
        }

        private void spójnaToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;
            activeChild.Dylatacja(8);
        }

        private void spójnaToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;
            activeChild.Erozja(4);
            activeChild.Dylatacja(4);
        }

        private void spójneToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;
            activeChild.Erozja(8);
            activeChild.Dylatacja(8);
        }

        private void spójnaToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;
            activeChild.Dylatacja(4);
            activeChild.Erozja(4);
        }

        private void spójneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;
            activeChild.Dylatacja(8);
            activeChild.Erozja(8);
        }

        private void uniwersalnyOperatorPunktowyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;
            if (activeChild != null)
                useFilter(new UOP());
        }

        private void histogramówRóżnicPoziomówJasnościToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild != null)
            {
                HistDiffForm newChild = new HistDiffForm(activeChild);
                newChild.Text = "Histogram różnic poziomów jasności";
                newChild.MdiParent = this;
                newChild.Show();
            }
        }

        private void segmentacjaPrzezProgowanieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;

            myCustomDialog dialog = new myCustomDialog("Progowanie", "Podaj wartość progowania od:", "Podaj wartość progowania do (lub pozostaw to pole puste):");

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            if(dialog.value2.Equals(String.Empty))
                activeChild.progowanie(Convert.ToInt32(dialog.value));
            else
                activeChild.progowanie(Convert.ToInt32(dialog.value), Convert.ToInt32(dialog.value2));
        }

        private void segmentacjaPrzezRozszerzanieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;
            if (activeChild != null)
                useFilter(new Operacje.Segmentation.RegionGrowing());
        }

        private void algorytmŻółwiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;
            activeChild.zolw();
        }

        private void segmentacjaProbalistycznaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;

            myCustomDialog dialog = new myCustomDialog("Segmaentacja probalistyczna", "Podaj wartość progu:", "Podaj wartość maksymalnej liczby regionów");

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;
            activeChild.segmProba(Convert.ToInt32(dialog.value), Convert.ToInt32(dialog.value2));
        }

        private void mozaikaVoronoiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;

            myCustomDialog dialog = new myCustomDialog("Mozaika Voronoi", "Liczba miejsc centalnych (100:10000):", "Kryterium wyróżnienie (1-min,2-max)");

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;
            activeChild.Voronoi(Convert.ToInt32(dialog.value), Convert.ToInt32(dialog.value2));
        }

        private void segmentacjaWododziałowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;

            if (activeChild == null)
                return;

            myCustomDialog dialog = new myCustomDialog("Segmentacja Wododziałowa", "Ilość kroków wygładzania (1:50):", "Wypełnienie regionów (1-z obr,2-śre. regi.)");

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;
            activeChild.wododzial(Convert.ToInt32(dialog.value), Convert.ToInt32(dialog.value2));
        }

        private void odchylenieStandardoweToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;
            if (activeChild != null)
                useFilter(new Operacje.Segmentation.Split(false));
        }

        private void amplitudaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;
            if (activeChild != null)
                useFilter(new Operacje.Segmentation.Split(true));
        }

        private void segmentacjaPrzezDzielenieToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void odcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;
            if (activeChild != null)
                useFilter(new Operacje.Segmentation.Split(false));
        }

        private void amplitudaToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            PictureForm activeChild = (PictureForm)this.ActiveMdiChild;
            if (activeChild != null)
                useFilter(new Operacje.Segmentation.Split(true));
        }
    }
}
