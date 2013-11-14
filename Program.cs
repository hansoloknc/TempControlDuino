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
                var sensors = new[] { new TempTrippedRelay(AnalogChannels.ANALOG_PIN_A0, Pins.GPIO_PIN_D0, 65),
                     new TempTrippedRelay(AnalogChannels.ANALOG_PIN_A1, Pins.GPIO_PIN_D1, 65),
                     new TempTrippedRelay(AnalogChannels.ANALOG_PIN_A2, Pins.GPIO_PIN_D2, 65),
                     new TempTrippedRelay(AnalogChannels.ANALOG_PIN_A3, Pins.GPIO_PIN_D3, 65),
                     new TempTrippedRelay(AnalogChannels.ANALOG_PIN_A4, Pins.GPIO_PIN_D4, 65),
                     new TempTrippedRelay(AnalogChannels.ANALOG_PIN_A5, Pins.GPIO_PIN_D5, 65) };

                //int potValue = 0;

                while (true)
                {
                    //read the value of the potentiometer
                    //potValue = (int)pot.ReadRaw();
                    //Debug.Print("ReadRaw analog value: " + pot.ReadRaw());
                    for (int i = 0; i < sensors.Length; i++)
                    {
                        Debug.Print(i.ToString() + " : " + sensors[i].ReadTemp().ToString("F2"));
                    }
                    Debug.Print("==========");

                    Thread.Sleep(1000);

                }
            }

        }

    }
}
