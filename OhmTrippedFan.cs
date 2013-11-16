using System;
using Microsoft.SPOT;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using Microsoft.SPOT.Hardware;


namespace TempControlDuino
{
    public class OhmTrippedFan
    {
        public OhmTrippedFan(Cpu.AnalogChannel temperatureInput, Cpu.Pin relayOutput, double setpointOhm)
        {
            _temperatureInput = new AnalogInput(temperatureInput);//, (1.0/20.0), 0, 12);
            _relayOutput = new OutputPort(relayOutput, true);
            _setpointOhms = setpointOhm;
        }

#region Fields and Getters

        private AnalogInput _temperatureInput;

        private OutputPort _relayOutput;

        private double _setpointOhms;

        /// <summary>
        /// True if the fan is running.
        /// </summary>
        public bool FanState { get; private set; }

#endregion

        /// <summary>
        /// Returns the current resistance. Sets <see cref="FanState"/>.
        /// </summary>
        /// <returns></returns>
        public double Evaluate()
        {
            //Check the thermister. 
            //If it is LESS than the setpoint (resistance decreases with increasing temperature), turn the fan ON.
            var currentOhms = ReadThermister();
            if (currentOhms < _setpointOhms)
            {
                //The fans are connected to the NORMALLY CLOSED pin on the relay, so set FALSE to turn fan ON
                _relayOutput.Write(false);
                FanState = true;
            }
            else
            {
                _relayOutput.Write(true);
                FanState = false;
            }
            return currentOhms;
        }

        /// <summary>
        /// Returns the resistance, in Ohms, of the thermister
        /// </summary>
        /// <returns></returns>
        private double ReadThermister()
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
            return R1;

        }

        //public double ReadTemp()
        //{
        //    var R1 = ReadThermister();

        //    /* 
        //     * Now map R1 to known temperature values
        //     * 26000 == 0°C 
        //     *  9900 == 24°C 
        //     *  5200 == 38°C 
        //     * 
        //     * And solve the regression to fing polynomial coefficients.
        //     * Plot the above values in Excel in an XY Scatter Plot, add a Trendline (Polynomial; Order=2), configure 
        //     * the Trendline to show the equation on the chart, and possibly configure the number format 
        //     * of the equation (12 decimal places). 
        //     * 
        //     */

        //    //var temp = 45.741304389619 - (0.0018037574648082 * R1);

        //    //y = 0.000000074527x2 - 0.004104074117x + 57.325987841945
        //    var tempC = (0.000000074527 * R1 * R1) - (0.004104074117 * R1) + 57.325987841945;

        //    return tempC;
        //}





    }
}
