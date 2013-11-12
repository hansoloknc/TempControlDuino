using System;
using Microsoft.SPOT;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using Microsoft.SPOT.Hardware;


namespace TempControlDuino
{
    public class TempTrippedRelay
    {
        public TempTrippedRelay(Cpu.AnalogChannel temperatureInput, Cpu.Pin relayOutput, double thresholdC)
        {
            _temperatureInput = new AnalogInput(temperatureInput);
            _temperatureInput.Offset = 0;
            _temperatureInput.Scale = (1.0 / 20.0);
            _relayOutput = new OutputPort(relayOutput, true);
            _thresholdC = thresholdC;
        }

#region Fields and Getters

        private AnalogInput _temperatureInput;

        public AnalogInput TemperatureInput
        {
            get { return _temperatureInput; }
        }

        private OutputPort _relayOutput;

        public OutputPort RelayOutput
        {
            get { return _relayOutput; }
        }

        private double _thresholdC;

        public double ThresholdC
        {
            get { return _thresholdC; }
        }

#endregion

        public double ReadTemp()
        {
            return _temperatureInput.Read();
        }



    }
}
