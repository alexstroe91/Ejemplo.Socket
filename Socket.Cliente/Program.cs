using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Linq;
using Calculator.Comun;

namespace Calculator.Cliente
{
    internal class Program
    {

        public static DatosOperacion RecogerDatos(double operando1, double operando2, string operador)
        {
            DatosOperacion operacion = null;

            if (operador == "s")
            {
                operacion = new DatosOperacion
                {
                    Operando1 = operando1,
                    Operando2 = operando2,
                    Operacion = TipoOperacion.Suma
                };
            }else if (operador == "r")
            {
                operacion = new DatosOperacion
                {
                    Operando1 = operando1,
                    Operando2 = operando2,
                    Operacion = TipoOperacion.resta
                };
            }else if (operador == "m")
            {
                operacion = new DatosOperacion
                {
                    Operando1 = operando1,
                    Operando2 = operando2,
                    Operacion = TipoOperacion.multiplicacion
                };
            }else if (operador == "d")
            {
                operacion = new DatosOperacion
                {
                    Operando1 = operando1,
                    Operando2 = operando2,
                    Operacion = TipoOperacion.division
                };
            }

            return operacion;
        }

        static string EnviaMenaje(DatosOperacion operacion)
        {
            try
            {
                // Connect to a Remote server
                // Get Host IP Address that is used to establish a connection
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1
                // If a host has multiple addresses, you will get a list of addresses

                // IPHostEntry host = Dns.GetHostEntry("infc13_profe");
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];

                // IPAddress ipAddress = IPAddress.Parse("ip destino");
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 2800);

                // Create a TCP/IP  socket.
                using Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    // Connect to Remote EndPoint
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    Console.WriteLine("Socket redad for {0}",
                        sender.LocalEndPoint.ToString());

                    // serializa el objeto
                    string jsonString = JsonSerializer.Serialize(operacion);

                    // convierte el objeto serializdo a un array de bytes
                    var cacheEnvio = Encoding.UTF8.GetBytes(jsonString);

                    // Envia los datos
                    int bytesSend = sender.Send(cacheEnvio);

                    // Receive the response from the remote device.
                    byte[] bufferRec = new byte[1024];
                    int bytesRec1 = sender.Receive(bufferRec);

                    var resultado = Encoding.UTF8.GetString(bufferRec, 0, bytesRec1);

                    // deserializarlo
                    var obj = JsonSerializer.Deserialize<Resultado>(resultado);

                    // mostrarlo por pantalla
                    Console.WriteLine("");
                    Console.WriteLine(obj);

                    // Release the socket.
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                    return resultado;

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Calculadora CLIENTE / SERVIDOR C#\r");
            Console.WriteLine("------------------------\n");
            
            // declaro variables con las que voy a trabajar
            double operando1 = 0;
            double operando2 = 0;
            String operador;

            // preguntamos los datos
            Console.WriteLine("Intoruce la operacion que quieres realizar(S-suma, R-resta, M-multiplicacion, D-division");
            operador = Console.ReadLine();

            Console.WriteLine("Intoruce el primer OPERADOR:");
            operando1 = double.Parse(Console.ReadLine());
            Console.WriteLine("Intoruce el segundo OPERADOR:");
            operando2 = double.Parse(Console.ReadLine());

            // creamos el objeto con los operandos y la operacion
            DatosOperacion operacion = RecogerDatos(operando1, operando2, operador);
            
            var resultado = EnviaMenaje(operacion);

            Console.Write("Press any key to close the Calculator console app...");
            Console.ReadKey();
        }

    }
}