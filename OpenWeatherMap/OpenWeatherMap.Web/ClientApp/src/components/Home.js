import React, { Component } from 'react';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = { weather: FormData, loading: true };
    }

    componentDidMount() {
        this.populateWeatherData();
    }

    static renderWeatherData(weather) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Location</th>
                        <th>Time</th>
                        <th>Wind</th>
                        <th>Visibility</th>
                        <th>Sky Conditions</th>
                        <th>Temperature (In Celsius)</th>
                        <th>Temperature (In Fahrenheit)</th>
                        <th>Dew Point</th>
                        <th>Relative Humidity</th>
                        <th>Pressure</th>
                    </tr>
                </thead>
                <tbody>
                    <tr key={weather.location}>
                        <td>{weather.location}</td>
                        <td>{weather.time}</td>
                        <td>{weather.wind}</td>
                        <td>{weather.visibility}</td>
                        <td>{weather.skyConditions}</td>
                        <td>{weather.temperatureInCelcius}</td>
                        <td>{weather.temperatureInFahrenheit}</td>
                        <td>{weather.dewPoint}</td>
                        <td>{weather.relativeHumidity}</td>
                        <td>{weather.pressure}</td>
                    </tr>
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Home.renderWeatherData(this.state.weather);

        return (
            <div>
                <h1 id="tabelLabel" >Weather data</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contents}
            </div>
        );
    }

    async populateCountryData() {
        const response = await fetch('https://localhost:44376/country',
            {
                method: 'get',
            });
        const result = await response.json();
        this.setState({ country: result, loading: false });
    }

    async populateWeatherData() {
        const response = await fetch('https://localhost:44376/weather',
        {
            method: 'post',
            headers:
            {
                'Accept': 'application/json, text/plain',
                'Content-Type': 'application/json;charset=UTF-8'
            },
            body: JSON.stringify
            ({
                cityName: 'Bandung',
                countryCode: 'ID'
            })
        });
        const result = await response.json();
        this.setState({ weather: result, loading: false });
    }
}
