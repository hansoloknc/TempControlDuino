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
                AnalogInput pot = new AnalogInput(AnalogChannels.ANALOG_PIN_A0);

                int potValue = 0;

                while (true)
                {
                    //read the value of the potentiometer
                    potValue = (int)pot.ReadRaw();
                    Debug.Print("ReadRaw analog value: " + pot.ReadRaw());

                    Thread.Sleep(1000); 
                    
                } 
            }

        }

    }
}
