using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace TempControlDuino
{
    public class Fan
    {
        public Fan(Cpu.Pin relayOutput)
        {
            _relayOutput = new OutputPort(relayOutput, true);
        }

        private OutputPort _relayOutput;

        public void SetFan(bool state)
        {
            //The fans are connected to the NORMALLY CLOSED pin on the relay, so set FALSE to turn fan ON
            _relayOutput.Write(!state);
        }

    }
}
