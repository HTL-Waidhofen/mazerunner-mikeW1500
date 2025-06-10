using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Objektorieniterung
{
    class Rechteck
    {
        public double laenge = 1;
        public double breite = 2;
        public double posX = 1;
        public double posY = 1;


        public double FlaecheBerechnen()
        {


            return laenge * breite;
        }

        public Rechteck(double laenge, double breite, double posX, double posY)

        {
            this.laenge = laenge;
            this.breite = breite;
            this.posX = posX;
            this.posY = posY;
        }

        public override string ToString()
        {
            return $"Rechteck: {laenge}x{breite}";
        }

    }

    class Spieler
    {
        public int x;
        public int y;
        public Image image;
        public MainWindow.Direction direction = MainWindow.Direction.None;
        public List<Rechteck> rechtecke;

        public Spieler(List<Rechteck> rechtecke)
        {
            x = 1;
            y = 1;
            this.rechtecke = rechtecke;
        }

        public void Move()
        {
            int currentX = x;
            int currentY = y;

            if (direction == MainWindow.Direction.Left)
            {
                x--;
            }
            else if (direction == MainWindow.Direction.Right)
            {
                x++;
            }
            else if (direction == MainWindow.Direction.Up)
            {
                y--;

            }
            else if (direction == MainWindow.Direction.Down)
            {
                y++;
            }

            bool collision = false;
            foreach(Rechteck r in rechtecke)
            {
                if (r.posX == x * MainWindow.GRID_SIZE && r.posY == y * MainWindow.GRID_SIZE)
                {
                    collision = true;
                }
            }
            if (collision)
            {
                x = currentX;
                y = currentY;
            }

            Canvas.SetTop(image, y * MainWindow.GRID_SIZE);
            Canvas.SetLeft(image, x * MainWindow.GRID_SIZE);

        }
        public void SetDirection(MainWindow.Direction direction)
        {
            this.direction = direction;


        }

    }

    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right,
            None
        }


        List<Rechteck> rechtecke = new List<Rechteck>();
        Spieler spieler = null;
        public static int GRID_SIZE = 10;

        DispatcherTimer timer = null;

        private void Update(object sender, EventArgs e)
        {
            spieler.Move();
        }


        public MainWindow()
        {
            InitializeComponent();

            spieler = new Spieler(rechtecke);


            StreamReader reader = new StreamReader("wallsList.txt");
            string zeile;
            double laenge = 10;
            double breite = 10;
            double posX = 0;
            double posY = 0;

            while ((zeile = reader.ReadLine()) != null)
            {
                string[] teile = zeile.Split(',');
                posX = double.Parse(teile[0]) * GRID_SIZE;
                posY = double.Parse(teile[1]) * GRID_SIZE;




                if (lstRechtecke.SelectedItem != null)
                {
                    Rechteck r = (Rechteck)lstRechtecke.SelectedItem;
                    r.laenge = laenge;
                    r.breite = breite;


                }
                else
                {
                    Rechteck r = new Rechteck(1, 1, posX, posY);
                    lstRechtecke.Items.Add(r);
                    rechtecke.Add(r);
                }
                Rectangle rect = new Rectangle();

                rect.Width = laenge;
                rect.Height = breite;
                rect.StrokeThickness = 2;
                rect.Stroke = Brushes.Black;
                rect.Fill = Brushes.Black;

                Canvas.SetLeft(rect, posX);
                Canvas.SetTop(rect, posY);


                myCanvas.Children.Add(rect);
            }

            reader.Close();
            spieler.image = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("Unbenannt.jpg", UriKind.Relative);
            bitmap.EndInit();
            spieler.image.Source = bitmap;
            spieler.image.Width = GRID_SIZE;
            spieler.image.Height = GRID_SIZE;
            Canvas.SetTop(spieler.image, spieler.y * GRID_SIZE);
            Canvas.SetLeft(spieler.image, spieler.x * GRID_SIZE);
            myCanvas.Children.Add(spieler.image);
            double dummy = bitmap.Width + 1;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string laengeStr = this.tbxLaenge.Text;
                double laenge = double.Parse(laengeStr);
                string breiteStr = this.tbxBreite.Text;
                double breite = double.Parse(breiteStr);
                double posX = double.Parse(tbxx.Text);
                double posY = double.Parse(tbxy.Text);

                if (lstRechtecke.SelectedItem != null)
                {
                    Rechteck r = (Rechteck)lstRechtecke.SelectedItem;
                    r.laenge = laenge;
                    r.breite = breite;


                }
                else
                {
                    Rechteck r = new Rechteck(laenge, breite, posX, posY);
                    lstRechtecke.Items.Add(r);
                    rechtecke.Add(r);
                }

                tbxLaenge.Clear();
                tbxBreite.Clear();
                tbxx.Clear();
                tbxy.Clear();
                lstRechtecke.SelectedItem = null;

                lstRechtecke.Items.Refresh();

            }
            catch (FormatException)
            {
                MessageBox.Show("Ungültige Eingabe!");
            }
        }


        private void lstRechtecke_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Rechteck r = (Rechteck)this.lstRechtecke.SelectedItem;
            if (r != null)
            {
                tbxLaenge.Text = r.laenge.ToString();
                tbxBreite.Text = r.breite.ToString();
                tbxx.Text = r.posX.ToString();
                tbxy.Text = r.posY.ToString();
            }
        }

        private void Button_Zeichnen_Click(object sender, RoutedEventArgs e)
        {
            string laengeStr = this.tbxLaenge.Text;
            double laenge = double.Parse(laengeStr);
            string breiteStr = this.tbxBreite.Text;
            double breite = double.Parse(breiteStr);
            string posXStr = this.tbxx.Text;
            double posX = double.Parse(tbxx.Text);
            string posYStr = this.tbxy.Text;
            double posY = double.Parse(tbxy.Text);

            Rectangle rect = new Rectangle();

            rect.Width = laenge;
            rect.Height = breite;
            rect.StrokeThickness = 2;
            rect.Stroke = Brushes.Black;

            Canvas.SetLeft(rect, posX);
            Canvas.SetTop(rect, posY);


            myCanvas.Children.Add(rect);
        }

        private void Button_LoeschenAlle_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                spieler.SetDirection(Direction.Left);
            }
            else if (e.Key == Key.Right)
            {
                spieler.SetDirection(Direction.Right);
            }
            else if (e.Key == Key.Up)
            {
                spieler.SetDirection(Direction.Up);
            }
            else if (e.Key == Key.Down)
            {
                spieler.SetDirection(Direction.Down);
            }
            else
            {
                spieler.SetDirection(Direction.None);


            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (stp_sidebar.Visibility == Visibility.Collapsed)
            {
                stp_sidebar.Visibility = Visibility.Visible;
                stp_background.Visibility = Visibility.Visible;
            }
            else
            {
                stp_sidebar.Visibility = Visibility.Collapsed;
                stp_background.Visibility = Visibility.Collapsed;


                timer = new DispatcherTimer(DispatcherPriority.Render);
                timer.Interval = TimeSpan.FromMilliseconds(300);
                timer.Tick += Update;
                timer.Start();
            }


        }


    }
}