using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;

namespace monteCarloSimulatorPercolator
{
    public partial class MainWindow : Window
    {

        private readonly Rectangle _square = new Rectangle {Height = 10, Width = 10, Fill= new SolidColorBrush(Colors.White), RadiusX = 1.5, RadiusY = 1.5};
        private SquareModel[,] _matrix;
        private int[,] _uiMatrix;
        private int _elementsInRow;
        private readonly int _defaultWidth = 100;
        private CancellationTokenSource _cts;
        private readonly Random _rand = new Random();
        private int _repeated;
        private readonly int _emptyElement = -1;

        private UfStrategy _algorithm;
        private Context _context;
        private UFFactory _factory;

        public UnionFindType CurrentOption { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            var rDimension = ParseInput();
            rtBase.Width = rtBase.Height = rDimension;

            _factory = new UFFactory();
            _context = new Context();
            DataContext = this;

            //If you want to TRACE each value
            //MakeVisiblePercolationLabels();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            ClearBoard();
            var rDimension = ParseInput();
            rtBase.Width = rtBase.Height = rDimension;

            await Application.Current.Dispatcher.Invoke<Task>(
                () =>
                {
                    _algorithm = _factory.Create(CurrentOption, _matrix);
                    _context.SetStrategy(_algorithm);
                    return Run(FillBase, new TimeSpan(0, 0, 0, 0, 3), _cts.Token);
                }
            );
        }

        private void ClearBoard()
        {
            _cts = new CancellationTokenSource();
            rtBase.Children.Clear();
            _uiMatrix = null;
        }

        private int ParseInput()
        {
            var dimension = tbDimension.Text;
            var rDimension = _defaultWidth;
            var counter = 0;
            if (int.TryParse(dimension, out rDimension))
            {
                var element = (int)(rDimension / _square.Width);
                _elementsInRow = element;
                _matrix = new SquareModel[element, element];
                _uiMatrix = new int[element,element];

                for (var i = 0; i < element; i++)
                {
                    for (var j = 0; j < element; j++)
                    {
                        _matrix[i, j] = new SquareModel {Value = counter++};
                        _uiMatrix[i, j] = _emptyElement;
                    }
                }
            }
            else
            {
                MessageBox.Show("incorrect data");
            }

            return rDimension;
        }

        private void FillBase()
        {
            var x = _rand.Next(_elementsInRow);
            var y = _rand.Next(_elementsInRow);

            if (_uiMatrix[x, y] > _emptyElement)
            {
                _repeated++;
                return;
            }

            _uiMatrix[x, y] = 1;

            _matrix[x, y].IsSquared = true;

            if (x == 0)
                _matrix[x, y].IsPercolated = true;

            _context.Execute(x, y);

            var percolatedLists = from SquareModel item in _matrix
                group item by item.Value;

            foreach (var squareModel in percolatedLists
                .Where(percolatedList => percolatedList.Any(m => m.IsPercolated))
                .SelectMany(percolatedList => percolatedList))
            {
                squareModel.IsPercolated = true;
            }

            var square = initSquare(_matrix[x, y].IsPercolated);
            rtBase.Children.Add(square);
            Canvas.SetTop(square, x * 10);
            Canvas.SetLeft(square, y * 10);

            UpdateMatrixPercolation();

            //If you want to TRACE each value
            //DisplayPercolationInfo(x, y);

        }

        private void UpdateMatrixPercolation()
        {
            foreach (Rectangle child in rtBase.Children)
            {
                var left = (int) Canvas.GetLeft(child) / 10;
                var top = (int) Canvas.GetTop(child) / 10;

                if (_matrix[top, left].IsPercolated)
                    child.Fill = new SolidColorBrush(Colors.Aqua);
            }
        }

        private Rectangle initSquare(bool isPercolated)
        {
            return new Rectangle
            {
                Height = 10,
                Width = 10,
                Fill = new SolidColorBrush(isPercolated ? Colors.Aqua : Colors.White),
                RadiusX = 1.5,
                RadiusY = 1.5
            };
        }

        private async Task Run(Action action, TimeSpan period, CancellationToken ct)
        {
            try
            {
                while (!ct.IsCancellationRequested)
                {
                    await Task.Delay(period, ct);
                    action();
                }
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Count of collisions: " + _repeated);
                _repeated = 0;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            _cts.Cancel();
        }

#region Trace percolation process

        private void MakeVisiblePercolationLabels()
        {
            lbX.Visibility = Visibility.Visible;
            label1.Visibility = Visibility.Visible;

            lbY.Visibility = Visibility.Visible;
            label3.Visibility = Visibility.Visible;

            lbIsSquared.Visibility = Visibility.Visible;
            label5.Visibility = Visibility.Visible;

            lbIsPercolated.Visibility = Visibility.Visible;
            label7.Visibility = Visibility.Visible;

            lbValue.Visibility = Visibility.Visible;
            label9.Visibility = Visibility.Visible;
        }
        private void DisplayPercolationInfo(int x, int y)
        {
            lbX.Content = x;
            lbY.Content = y;

            lbIsSquared.Content = _matrix[x, y].IsSquared;
            lbIsPercolated.Content = _matrix[x, y].IsPercolated;
            lbValue.Content = _matrix[x, y].Value;
        }
#endregion
    }
}
