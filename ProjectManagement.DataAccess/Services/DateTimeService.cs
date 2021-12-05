using System;
using ProjectManagement.DataAccess.Interfaces;

namespace ProjectManagement.DataAccess.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
