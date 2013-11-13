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
            _temperatureInput = new AnalogInput(temperatureInput);//, (1.0/20.0), 0, 12);
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
            /* The thermister is configured as a voltage divider 
             * http://en.wikipedia.org/wiki/Thermister
             * http://en.wikipedia.org/wiki/Voltage_divider
             * 
             * 3V3
             * Vin --+
             *       |
             *       R1 (thermister)
             *       | 
             *       +--- Vout (analog input)
             *       |
             *       R2 (2K2)
             *       |
             * GND --+
             * 
             * Vout = (R2 / ( R1 + R2)) * Vin
             * 
             * The Netduino 2 gives us Vout via its 12-bit ADC, as a percent of 3V3, so let's solve for R1
             * 
             * R1 = (( R2 * Vin ) / Vout ) - R2
             * 
             */


            var Vout = _temperatureInput.Read() * 3.3;
            var R2 = 2200.0;
            var Vin = 3.3;

            var R1 = ((R2 * Vin) / Vout) - R2;

            /* Now map R1 to known temperature values
             *  0°C == 26000
             * 24°C == 9900
             * 38°C == 5200
             * 
             * And solve to a polynomial regression.
             * Plot the above values in Excel in an XY Scatter Plot, add a Polynomial Trendline, and configure 
             * the Trendline to show the equation on the chart. 
             * 
             */

            //var temp = 45.741304389619 - (0.0018037574648082 * R1);

            //y = 0.000000074527x2 - 0.004104074117x + 57.325987841945
            var tempC = (0.000000074527 * R1 * R1) - (0.004104074117 * R1) + 57.325987841945;

            return tempC;
        }



    }
}
