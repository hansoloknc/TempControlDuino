using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace TempControlDuino
{
    public class Program
    {
        public static void Main()
        {
            // write your code here
            while (true)
            {
                //0 and 5 are case fans
                var caseFans = new[] { new Fan(Pins.GPIO_PIN_D0), new Fan(Pins.GPIO_PIN_D5) };

                //1 through 4 are CPU fans
                var sensors = new[] { new OhmTrippedFan(AnalogChannels.ANALOG_PIN_A1, Pins.GPIO_PIN_D1, 1000),
                     new OhmTrippedFan(AnalogChannels.ANALOG_PIN_A2, Pins.GPIO_PIN_D2, 1000),
                     new OhmTrippedFan(AnalogChannels.ANALOG_PIN_A3, Pins.GPIO_PIN_D3, 1000),
                     new OhmTrippedFan(AnalogChannels.ANALOG_PIN_A4, Pins.GPIO_PIN_D4, 1000),
                     };

                //int potValue = 0;

                while (true)
                {
                    //read the value of the potentiometer
                    //potValue = (int)pot.ReadRaw();
                    //Debug.Print("ReadRaw analog value: " + pot.ReadRaw());
                    for (int i = 0; i < sensors.Length; i++)
                    {
                        var ohms = sensors[i].Evaluate();
                        Debug.Print((i+1).ToString() + " : " + ohms.ToString("F2"));
                    }

                    //See if all of the CPU fans are on.
                    //If they are, turn on the case fans.
                    var allFans = true;
                    for (int i = 0; i < sensors.Length; i++)
                    {
                        allFans &= sensors[i].FanState;
                    }
                    foreach (var caseFan in caseFans)
                    {
                        caseFan.SetFan(allFans);
                    }

                    Debug.Print("==========");

                    Thread.Sleep(5000);

                }
            }

        }

    }
}
