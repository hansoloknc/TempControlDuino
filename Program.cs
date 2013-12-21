#define YELLOW

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
        /// <summary>
        /// The minimum number of iterations that case fans should run for
        /// </summary>
        const long CaseFanIterationThreshold = 2;

        public static void Main()
        {
            long iteration = 0;


            //0 and 5 are case fans
            var caseFans = new[] { new Fan(Pins.GPIO_PIN_D0), new Fan(Pins.GPIO_PIN_D5) };

            //1 through 4 are CPU fans
#if BLUE
            var sensors = new[] { new OhmTrippedFan(AnalogChannels.ANALOG_PIN_A1, Pins.GPIO_PIN_D1, 3900),
                    new OhmTrippedFan(AnalogChannels.ANALOG_PIN_A2, Pins.GPIO_PIN_D2, 7300),
                    new OhmTrippedFan(AnalogChannels.ANALOG_PIN_A3, Pins.GPIO_PIN_D3, 3000),
                    new OhmTrippedFan(AnalogChannels.ANALOG_PIN_A4, Pins.GPIO_PIN_D4, 3200)
                    };
#endif

#if YELLOW
            var sensors = new[] { new OhmTrippedFan(AnalogChannels.ANALOG_PIN_A1, Pins.GPIO_PIN_D1, 4500),
                     new OhmTrippedFan(AnalogChannels.ANALOG_PIN_A2, Pins.GPIO_PIN_D2, 2600),
                     new OhmTrippedFan(AnalogChannels.ANALOG_PIN_A3, Pins.GPIO_PIN_D3, 4800),
                     new OhmTrippedFan(AnalogChannels.ANALOG_PIN_A4, Pins.GPIO_PIN_D4, 3500)
                     };
#endif

            var lastCaseFanStart = iteration;
            while (true)
            {
                //Evaluate all of the sensors and set the fans as appropriate.
                for (int i = 0; i < sensors.Length; i++)
                {
                    var ohms = sensors[i].Evaluate();
                    Debug.Print((i + 1).ToString() + " : " + ohms.ToString("F2") + "\t : " + (sensors[i].FanState ? "ON" : "OFF"));
                }

                //If *all* of the CPU fans are on, turn on the case fans.
                var allFans = true;
                for (int i = 0; i < sensors.Length; i++)
                {
                    allFans &= sensors[i].FanState;
                }
                
                //If the case fans are to be triggered this iteration, mark the starting point.
                if (allFans == true)
                    lastCaseFanStart = iteration;

                //Ensure they stay on for at least Threshold extra iterations
                allFans = ((iteration > CaseFanIterationThreshold || allFans) && ((iteration - lastCaseFanStart) < CaseFanIterationThreshold));

                Debug.Print("Case fans: " + (allFans ? "ON" : "OFF"));
                foreach (var caseFan in caseFans)
                {
                    caseFan.SetFan(allFans);
                }

                Debug.Print("==========");

                Thread.Sleep(15000);
                iteration++;
            }
        }


    }
}
