using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeatherForcastApi.Models;

namespace WeatherForcastApi.Hubs
{
    public class TemperatureHub : Hub
    {
        public void brodcastMessage(Temperature temperature)
        {
            Clients.All.reciveTemperature(temperature);
        }
    }
}