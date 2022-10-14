using CanvasServidor.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace CanvasServidor.Services
{
    public class ServidorCanvasServices
    {
        TcpListener server;
        public void IniciarServer()
        {
            if (server == null)
            {
                IPEndPoint IPE = new IPEndPoint(IPAddress.Any, 4999);
                server = new TcpListener(IPE);
                Thread hilo = new Thread(new ThreadStart(Escuchar));
                hilo.IsBackground = true;
                hilo.Start();
            }
        }

        private void Escuchar()
        {
            if (server != null)
            {
                server.Start();
                while (server.Server.IsBound)
                {
                    var clienteNuevo = server.AcceptTcpClient();
                    Thread hiloRecibir = new Thread(new ParameterizedThreadStart(Recibir));
                    hiloRecibir.Start(clienteNuevo);
                }
            }
        }
        public Rectangulo? r;
        public event Action<Rectangulo> RectanguloNuevo;
        private void Recibir(object? tcpClient)
        {
            if (tcpClient != null)
            {
                TcpClient cliente = (TcpClient)tcpClient;
                var steam = cliente.GetStream();
                while (cliente.Connected)
                {
                    if (cliente.Available > 0)
                    {
                        byte[] buffer = new byte[cliente.Available];
                        steam.Read(buffer, 0, buffer.Length);
                        try
                        {
                            var rectangulo = JsonConvert.DeserializeObject<Rectangulo>(System.Text.Encoding.UTF8.GetString(buffer));
                            if (rectangulo != null)
                            {
                                r = new Rectangulo()
                                {
                                    Alto = rectangulo.Alto,
                                    Ancho = rectangulo.Ancho,
                                    CordenadaX = rectangulo.CordenadaX,
                                    CordenadaY = rectangulo.CordenadaY,
                                    Fill = rectangulo.Fill
                                };
                                RectanguloNuevo?.Invoke(r);
                            }
                        }
                        catch (Exception){ }
                    }
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
