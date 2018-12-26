﻿using System;

namespace MightyWatt
{
    public class ProgramItem
    {
        private ProgramModes programMode;
        private RunMode mode;
        private string durationString;
        private double duration; // duration in seconds
        private TimeUnits timeUnit;
        private double? value, startingValue;
        private double finalValue;
        private bool skipEnabled;
        private WDandSkipMode skipMode;
        private Comparison skipComparator;
        private double skipValue;
        private string name = "Empty";
        private const string NUMBER_FORMAT = "g";

        // constructor for constant mode
        public ProgramItem(RunMode mode, double? value, string durationString, TimeUnits timeUnit)
        {
            constant(mode, value, durationString, timeUnit);
        }

        // constructor for constant mode with skip
        public ProgramItem(RunMode mode, double? value, string durationString, TimeUnits timeUnit, WDandSkipMode skipMode, Comparison skipComparator, double skipValue)
        {
            constant(mode, value, durationString, timeUnit);
            skip(skipMode, skipComparator, skipValue);
        }

        // constructor for ramp mode
        public ProgramItem(RampMode mode, double? startingValue, double finalValue, string durationString, TimeUnits timeUnit)
        {
            ramp(mode, startingValue, finalValue, durationString, timeUnit);
        }

        // constructor for ramp mode with skip
        public ProgramItem(RampMode mode, double? startingValue, double finalValue, string durationString, TimeUnits timeUnit, WDandSkipMode skipMode, Comparison skipComparator, double skipValue)
        {
            ramp(mode, startingValue, finalValue, durationString, timeUnit);
            skip(skipMode, skipComparator, skipValue);
        }

        // constructor for pin set
        public ProgramItem(byte pin, bool set)
        {
            UserPin(pin, set);
        }

        // creates constant mode program item
        private void constant(RunMode mode, double? value, string durationString, TimeUnits timeUnit)
        {                        
            this.programMode = ProgramModes.Constant;
            this.mode = mode;
            this.durationString = durationString;
            this.timeUnit = timeUnit;
            this.value = value;
            this.skipEnabled = false;

            // compute duration in seconds
            Converters.TimeConverter timeConverter = new Converters.TimeConverter();
            this.duration = (double)(timeConverter.ConvertBack(durationString, typeof(double), timeUnit, System.Globalization.CultureInfo.CurrentCulture));

            // string representation for GUI
            switch (mode)
            {
                case RunMode.Current:
                    {
                        if (value != null)
                        {
                            this.name = "Constant current " + ((double)value).ToString(NUMBER_FORMAT) + " A, " + durationString + " " + timeUnit.ToString();
                        }
                        else
                        {
                            this.name = "Constant current (use previous), " + durationString + " " + timeUnit.ToString();
                        }
                        break;
                    }
                case RunMode.Power_CC:
                    {
                        if (value != null)
                        {
                            this.name = "Constant power (CC) " + ((double)value).ToString(NUMBER_FORMAT) + " W, " + durationString + " " + timeUnit.ToString();
                        }
                        else
                        {
                            this.name = "Constant power (CC, use previous), " + durationString + " " + timeUnit.ToString();
                        }
                        break;
                    }
                case RunMode.Power_CV:
                    {
                        if (value != null)
                        {
                            this.name = "Constant power (CV) " + ((double)value).ToString(NUMBER_FORMAT) + " W, " + durationString + " " + timeUnit.ToString();
                        }
                        else
                        {
                            this.name = "Constant power (CV, use previous), " + durationString + " " + timeUnit.ToString();
                        }
                        break;
                    }
                case RunMode.Resistance_CC:
                    {
                        if (value != null)
                        {
                            this.name = "Constant resistance (CC) " + ((double)value).ToString(NUMBER_FORMAT) + " Ω, " + durationString + " " + timeUnit.ToString();
                        }
                        else
                        {
                            this.name = "Constant resistance (CC, use previous), " + durationString + " " + timeUnit.ToString();
                        }
                        break;
                    }
                case RunMode.Resistance_CV:
                    {
                        if (value != null)
                        {
                            this.name = "Constant resistance (CV) " + ((double)value).ToString(NUMBER_FORMAT) + " Ω, " + durationString + " " + timeUnit.ToString();
                        }
                        else
                        {
                            this.name = "Constant resistance (CV, use previous), " + durationString + " " + timeUnit.ToString();
                        }
                        break;
                    }
                case RunMode.Voltage:
                    {
                        if (value != null)
                        {
                            this.name = "Constant voltage " + ((double)value).ToString(NUMBER_FORMAT) + " V, " + durationString + " " + timeUnit.ToString();
                        }
                        else
                        {
                            this.name = "Constant voltage (use previous), " + durationString + " " + timeUnit.ToString();
                        }
                        break;
                    }
                case RunMode.VoltageSoftware:
                    {
                        if (value != null)
                        {
                            this.name = "Constant SW controlled voltage " + ((double)value).ToString(NUMBER_FORMAT) + " V, " + durationString + " " + timeUnit.ToString();
                        }
                        else
                        {
                            this.name = "Constant SW controlled voltage (use previous), " + durationString + " " + timeUnit.ToString();
                        }
                        break;
                    }
                case RunMode.MPPT:
                    {
                        if (value != null)
                        {
                            //this.name = "Maximum power point tracker from " + ((double)value).ToString(NUMBER_FORMAT) + " A, " + durationString + " " + timeUnit.ToString();
                            this.name = "Maximum power point tracker from " + ((double)value).ToString(NUMBER_FORMAT) + " V, " + durationString + " " + timeUnit.ToString();
                        }
                        else
                        {
                            //this.name = "Maximum power point tracker (use previous current), " + durationString + " " + timeUnit.ToString();
                            this.name = "Maximum power point tracker (use previous voltage), " + durationString + " " + timeUnit.ToString();
                        }

                        break;
                    }
                case RunMode.SimpleAmmeter:
                    {
                        name = "Simple ammeter, " + durationString + " " + timeUnit.ToString();
                        break;
                    }
            }
        }

        // creates ramp mode program item
        private void ramp(RampMode mode, double? startingValue, double finalValue, string durationString, TimeUnits timeUnit)
        {

            this.programMode = ProgramModes.Ramp;
            this.mode = mode.ToRunMode();
            this.durationString = durationString;
            this.timeUnit = timeUnit;
            this.startingValue = startingValue;
            this.finalValue = finalValue;

            // compute duration in seconds
            Converters.TimeConverter timeConverter = new Converters.TimeConverter();
            this.duration = (double)(timeConverter.ConvertBack(durationString, typeof(double), timeUnit, System.Globalization.CultureInfo.CurrentCulture));

            // string representation for GUI
            switch (mode)
            {
                case RampMode.Current:
                    {
                        if (startingValue != null)
                        {
                            this.name = "Ramp current " + ((double)startingValue).ToString(NUMBER_FORMAT) + " –> " + finalValue.ToString(NUMBER_FORMAT) + " A, " + durationString + " " + timeUnit.ToString();
                        }
                        else
                        {
                            this.name = "Ramp current (use previous) –> " + finalValue.ToString(NUMBER_FORMAT) + " A, " + durationString + " " + timeUnit.ToString();
                        }
                        break;
                    }
                case RampMode.Power_CC:
                    {
                        if (startingValue != null)
                        {
                            this.name = "Ramp power (CC) " + ((double)startingValue).ToString(NUMBER_FORMAT) + " –> " + finalValue.ToString(NUMBER_FORMAT) + " W, " + durationString + " " + timeUnit.ToString();
                        }
                        else
                        {
                            this.name = "Ramp power (CC, use previous) –> " + finalValue.ToString(NUMBER_FORMAT) + " W, " + durationString + " " + timeUnit.ToString();
                        }
                        break;
                    }
                case RampMode.Power_CV:
                    {
                        if (startingValue != null)
                        {
                            this.name = "Ramp power (CV) " + ((double)startingValue).ToString(NUMBER_FORMAT) + " –> " + finalValue.ToString(NUMBER_FORMAT) + " W, " + durationString + " " + timeUnit.ToString();
                        }
                        else
                        {
                            this.name = "Ramp power (CV, use previous) –> " + finalValue.ToString(NUMBER_FORMAT) + " W, " + durationString + " " + timeUnit.ToString();
                        }
                        break;
                    }
                case RampMode.Resistance_CC:
                    {
                        if (startingValue != null)
                        {
                            this.name = "Ramp resistance (CC) " + ((double)startingValue).ToString(NUMBER_FORMAT) + " –> " + finalValue.ToString(NUMBER_FORMAT) + " Ω, " + durationString + " " + timeUnit.ToString();
                        }
                        else
                        {
                            this.name = "Ramp resistance (CC, use previous) –> " + finalValue.ToString(NUMBER_FORMAT) + " Ω, " + durationString + " " + timeUnit.ToString();
                        }
                        break;
                    }
                case RampMode.Resistance_CV:
                    {
                        if (startingValue != null)
                        {
                            this.name = "Ramp resistance (CV) " + ((double)startingValue).ToString(NUMBER_FORMAT) + " –> " + finalValue.ToString(NUMBER_FORMAT) + " Ω, " + durationString + " " + timeUnit.ToString();
                        }
                        else
                        {
                            this.name = "Ramp resistance (CV, use previous) –> " + finalValue.ToString(NUMBER_FORMAT) + " Ω, " + durationString + " " + timeUnit.ToString();
                        }
                        break;
                    }
                case RampMode.Voltage:
                    {
                        if (startingValue != null)
                        {
                            this.name = "Ramp voltage " + ((double)startingValue).ToString(NUMBER_FORMAT) + " –> " + finalValue.ToString(NUMBER_FORMAT) + " V, " + durationString + " " + timeUnit.ToString();
                        }
                        else
                        {
                            this.name = "Ramp voltage (use previous) –> " + finalValue.ToString(NUMBER_FORMAT) + " V, " + durationString + " " + timeUnit.ToString();
                        }
                        break;
                    }
                case RampMode.VoltageSoftware:
                    {
                        if (startingValue != null)
                        {
                            this.name = "Ramp SW controlled voltage " + ((double)startingValue).ToString(NUMBER_FORMAT) + " –> " + finalValue.ToString(NUMBER_FORMAT) + " V, " + durationString + " " + timeUnit.ToString();
                        }
                        else
                        {
                            this.name = "Ramp SW controlled voltage (use previous) –> " + finalValue.ToString(NUMBER_FORMAT) + " V, " + durationString + " " + timeUnit.ToString();
                        }
                        break;
                    }
            }
        }

        private void UserPin(byte pin, bool set)
        {
            programMode = ProgramModes.Pin;
            Pin = pin;
            SetUserPin = set;
            if (set)
            {
                if (pin < Load.Pins.Length)
                {
                    name = string.Format("Set {0}", Load.Pins[pin]);
                }
                else
                {
                    name = "Set all user pins";
                }
            }
            else
            {
                if (pin < Load.Pins.Length)
                {
                    name = string.Format("Reset {0}", Load.Pins[pin]);
                }
                else
                {
                    name = "Reset all user pins";
                }
            }
        }

        // adds skip condition to program item
        private void skip(WDandSkipMode skipMode, Comparison skipComparator, double skipValue)
        {
            this.skipMode = skipMode;
            this.skipComparator = skipComparator;
            this.skipValue = skipValue;
            this.name += "; skip if ";
            this.skipEnabled = true;

            switch (this.skipMode)
            {
                case WDandSkipMode.Current:
                    {
                        if (this.skipComparator == Comparison.LessThan)
                        {
                            this.name += "current < ";
                        }
                        else
                        {
                            this.name += "current > ";
                        }
                        this.name += skipValue.ToString(NUMBER_FORMAT) + " A";
                        break;
                    }
                case WDandSkipMode.Power:
                    {
                        if (this.skipComparator == Comparison.LessThan)
                        {
                            this.name += "power < ";
                        }
                        else
                        {
                            this.name += "power > ";
                        }
                        this.name += skipValue.ToString(NUMBER_FORMAT) + " W";
                        break;
                    }
                case WDandSkipMode.Resistance:
                    {
                        if (this.skipComparator == Comparison.LessThan)
                        {
                            this.name += "resistance < ";
                        }
                        else
                        {
                            this.name += "resistance > ";
                        }
                        this.name += skipValue.ToString(NUMBER_FORMAT) + " Ω";
                        break;
                    }
                case WDandSkipMode.Voltage:
                    {
                        if (this.skipComparator == Comparison.LessThan)
                        {
                            this.name += "voltage < ";
                        }
                        else
                        {
                            this.name += "voltage > ";
                        }
                        this.name += skipValue.ToString(NUMBER_FORMAT) + " V";
                        break;
                    }
                case WDandSkipMode.Temperature:
                    {
                        if (this.skipComparator == Comparison.LessThan)
                        {
                            this.name += "temperature < ";
                        }
                        else
                        {
                            this.name += "temperature > ";
                        }
                        this.name += skipValue.ToString(NUMBER_FORMAT) + " °C";
                        break;
                    }
            }
        }

        public override string ToString()
        {
            return this.name;
        }

        public ProgramModes ProgramMode
        {
            get
            {
                return this.programMode;
            }
        }

        public RunMode Mode
        {
            get
            {
                return this.mode;
            }
        }

        public string DurationString // duration represented as a string entered by user
        {
            get
            {
                return this.durationString;
            }
        }

        public double Duration
        {
            get
            {
                return this.duration;
            }
        }

        public TimeUnits TimeUnit
        {
            get
            {
                return this.timeUnit;
            }
        }

        public double? Value
        {
            get
            {
                return this.value;
            }
        }

        public double? StartingValue
        {
            get
            {
                return this.startingValue;
            }
        }

        public double FinalValue
        {
            get
            {
                return this.finalValue;
            }
        }

        public bool SkipEnabled
        {
            get
            {
                return this.skipEnabled;
            }
        }

        public WDandSkipMode SkipMode
        {
            get
            {
                return this.skipMode;
            }
        }

        public Comparison SkipComparator
        {
            get
            {
                return this.skipComparator;
            }
        }

        public double SkipValue
        {
            get
            {
                return this.skipValue;
            }
        }

        public byte Pin { get; private set; }
        public bool SetUserPin { get; private set; }
    }
}
