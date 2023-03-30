using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenWeatherMap.Services.Interfaces;
using OpenWeatherMap.Services.Services;

namespace OpenWeatherMap.Test
{
    [TestClass]
    public class TemperatureDataServiceTest
    {
        readonly ITemperatureDataService _temperatureDataService;
        public TemperatureDataServiceTest() 
        { 
            _temperatureDataService = new TemperatureDataService();
        }

        [TestMethod]
        public void KelvinToFahrenheit_0_negative459_67()
        {
            Assert.AreEqual((-459.67f).ToString("0.00"), _temperatureDataService.KelvinToFahrenheit(0f).GetAwaiter().GetResult().ToString("0.00"));
        }

        [TestMethod]
        public void KelvinToFahrenheit_50_negative369_67()
        {
            Assert.AreEqual((-369.67f).ToString("0.00"), _temperatureDataService.KelvinToFahrenheit(50f).GetAwaiter().GetResult().ToString("0.00"));
        }

        [TestMethod]
        public void KelvinToFahrenheit_150_negative189_67()
        {
            Assert.AreEqual((-189.67f).ToString("0.00"), _temperatureDataService.KelvinToFahrenheit(150f).GetAwaiter().GetResult().ToString("0.00"));
        }

        [TestMethod]
        public void KelvinToFahrenheit_300_positive80_33()
        {
            Assert.AreEqual((80.33f).ToString("0.00"), _temperatureDataService.KelvinToFahrenheit(300f).GetAwaiter().GetResult().ToString("0.00"));
        }

        [TestMethod]
        public void KelvinToFahrenheit_1000_positive1340_33()
        {
            Assert.AreEqual((1340.33f).ToString("0.00"), _temperatureDataService.KelvinToFahrenheit(1000f).GetAwaiter().GetResult().ToString("0.00"));
        }

        [TestMethod]
        public void FahrenheitToCelcius_32_positive0()
        {
            Assert.AreEqual((0f).ToString("0.00"), _temperatureDataService.FahrenheitToCelcius(32f).GetAwaiter().GetResult().ToString("0.00"));
        }

        [TestMethod]
        public void FahrenheitToCelcius_50_positive10()
        {
            Assert.AreEqual((10f).ToString("0.00"), _temperatureDataService.FahrenheitToCelcius(50f).GetAwaiter().GetResult().ToString("0.00"));
        }

        [TestMethod]
        public void FahrenheitToCelcius_100_positive37_78()
        {
            Assert.AreEqual((37.78f).ToString("0.00"), _temperatureDataService.FahrenheitToCelcius(100f).GetAwaiter().GetResult().ToString("0.00"));
        }

        [TestMethod]
        public void FahrenheitToCelcius_negative10_negative23_33()
        {
            Assert.AreEqual((-23.33f).ToString("0.00"), _temperatureDataService.FahrenheitToCelcius(-10f).GetAwaiter().GetResult().ToString("0.00"));
        }

        [TestMethod]
        public void FahrenheitToCelcius_negative50_negative45_56()
        {
            Assert.AreEqual((-45.56f).ToString("0.00"), _temperatureDataService.FahrenheitToCelcius(-50f).GetAwaiter().GetResult().ToString("0.00"));
        }

        [TestMethod]
        public void FahrenheitToCelcius_negative459_67_negative273_15()
        {
            Assert.AreEqual((-273.15f).ToString("0.00"), _temperatureDataService.FahrenheitToCelcius(-459.67f).GetAwaiter().GetResult().ToString("0.00"));
        }
    }
}
