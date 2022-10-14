using CanvasServidor.Models;
using CanvasServidor.Services;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Threading;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace CanvasServidor.ViewModels
{
    public class CanvasViewModel
    {
        public ICommand IniciarCommand { get; set; }
        MainWindow? canvas;
        Dispatcher dispatcher;
        ServidorCanvasServices scs;
        public CanvasViewModel()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
            IniciarCommand = new RelayCommand(IniciarServidor);
        }
        void IniciarServidor()
        {
            scs = new ServidorCanvasServices();
            scs.RectanguloNuevo += Scs_RectanguloNuevo;
            scs.IniciarServer();
        }
        private void Scs_RectanguloNuevo(Rectangulo obj)
        {
            dispatcher.Invoke(() =>
            {
                canvas = (MainWindow?)Application.Current.Windows.OfType<Window>().FirstOrDefault();
                Color fill = (Color)ColorConverter.ConvertFromString(scs.r.Fill);
                Brush brush = new SolidColorBrush(fill);
                if (canvas != null)
                {
                    Rectangle rec = new Rectangle()
                    {
                        Width = scs.r.Ancho,
                        Height = scs.r.Alto,
                        Fill = brush
                    };
                    canvas.canvasserver.Children.Add(rec);
                    Canvas.SetTop(rec, scs.r.CordenadaY);
                    Canvas.SetLeft(rec, scs.r.CordenadaX);
                }
            });
        }
    }
}
