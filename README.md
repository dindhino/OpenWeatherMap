# OpenWeatherMap
Setup Instruction:
1. Create OpenWeatherMap account at http://api.openweathermap.org/
	You will get an email for account confirmation and click on the "Verify your email" button.
2. Subscribe to Free collection for get an API key to call the API
	The key will be sent to your email or you can check at "My API keys" menu.
3. Set OpenWeatherMapSetting for the appsettings
	"OpenWeatherMapSettings": {
		"ApiBaseUrl": "https://api.openweathermap.org",
		"GetWeatherEndpoint": "/data/2.5/weather",
		"ApiKey": "apiKey from OpenWeatherMap"
	}
	
Run Program:
1. Run multiple startup project in Visual Studio
- OpenWeatherMap.API
- OpenWeatherMap.Web