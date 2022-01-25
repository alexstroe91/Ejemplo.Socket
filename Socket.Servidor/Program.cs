using Calculator.Comun;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Calculator.Servidor
{
    internal class Program
    {
        public static Resultado calcularResultado(double Operando1, double Operando2, DatosOperacion obj)
        {
            double servidorOperando1 = 0;
            double servidorOperando2 = 0;
            double servidorResultado = 0;
            TipoOperacion servidorOperando = TipoOperacion.Suma;

            if (obj.Operacion == TipoOperacion.Suma)
            {
                servidorOperando1 = obj.Operando1;
                servidorOperando2 = obj.Operando2;
                servidorResultado = servidorOperando1 + servidorOperando2;
                servidorOperando = obj.Operacion;
            }
            else if (obj.Operacion == TipoOperacion.resta)
            {
                servidorOperando1 = obj.Operando1;
                servidorOperando2 = obj.Operando2;
                servidorResultado = servidorOperando1 - servidorOperando2;
                servidorOperando = obj.Operacion;

            }
            else if (obj.Operacion == TipoOperacion.multiplicacion)
            {
                servidorOperando1 = obj.Operando1;
                servidorOperando2 = obj.Operando2;
                servidorResultado = servidorOperando1 * servidorOperando2;
                servidorOperando = obj.Operacion;

            }
            else if (obj.Operacion == TipoOperacion.division)
            {
                servidorOperando1 = obj.Operando1;
                servidorOperando2 = obj.Operando2;
                servidorResultado = servidorOperando1 / servidorOperando2;
                servidorOperando = obj.Operacion;

            }

            Resultado resultadoServidor = new Resultado
            {
                Operando1 = servidorOperando1,
                Operando2 = servidorOperando2,
                Resultados = servidorResultado,
                Operacion = servidorOperando
            };

            return resultadoServidor;

        }

        static void Main(string[] args)
        {

            //resuelve el nombre que se le indique abajo (DNS)
            IPHostEntry host = Dns.GetHostEntry("localhost");
            //si te devuelve mas de una ip te coge la primera
            IPAddress ipAddress = host.AddressList[0];

            //IPAddress ipAddress = IPAddress.Parse("ip escucha");

            //defines un punto final, un destino con el puerto final, lo unico que hace es definirlo
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 2800);

            try
            {
                // Create a Socket that will use Tcp protocol, le dices que es de tipo STREAM y que es TCP
                using Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // A Socket must be associated with an endpoint using the Bind method, ENLAZAS
                listener.Bind(localEndPoint);

                // Specify how many requests a Socket can listen before it gives Server busy response.
                // We will listen 10 requests at a time
                //le dice que escucha
                listener.Listen(10);

                Console.WriteLine("Waiting for a connection ..." + listener.LocalEndPoint.ToString());

                //bucle infinito a la esccha
                while (true)
                {
                    //si llega un mensaje lo acepta y abre el puerto
                    Socket handler = listener.Accept();

                    //
                    Console.WriteLine("Socket connected to {0}",
                        handler.RemoteEndPoint.ToString());

                    //abre un espacio de memoria y le dice que lo que hay en el mensaje lo guarde en ese sitio
                    var cacheMenaje = new byte[4096];
                    int bytesMenaje = handler.Receive(cacheMenaje);

                    //si no esta vacio el mensaje
                    if (bytesMenaje > 0)
                    {
                        //convierte la red de BYTE a texto
                        var mensaje = Encoding.UTF8.GetString(cacheMenaje, 0, bytesMenaje);

                        var respuesta = "Ok: " + mensaje;
                        var obj = JsonSerializer.Deserialize<DatosOperacion>(mensaje);

                        //guardo en la variable, el objeto con el resultado
                        Resultado resultadoServidor = calcularResultado(obj.Operando1, obj.Operando2, obj);

                        // serializa el objeto
                        string jsonString = JsonSerializer.Serialize(resultadoServidor);

                        //lo que hace es coger el texto y volver a meterlo en binario
                        var cacheRespuesta = Encoding.UTF8.GetBytes(jsonString);
                        
                        //envia de vuelta el mensaje en binario
                        handler.Send(cacheRespuesta);

                        //duermes el hilo de ejecucion
                        Thread.Sleep(0);
                    }

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\n Press any key to continue...");
            Console.ReadKey();
        }
    }
}
