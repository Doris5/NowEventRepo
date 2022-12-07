﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _appDbContext;

        public EventRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public int AddEvent(Event newEvent)
        {
            newEvent.Status = "Incomplete";
            _appDbContext.Events.Add(newEvent);
            _appDbContext.SaveChanges();

            Budget budget = CreateBudget(newEvent.Id);
            _appDbContext.Budget.Add(budget);
            _appDbContext.SaveChanges();
            return newEvent.Id;
        }

        public IEnumerable<Event> GetOffersWithInCompleteStatus()
        {
            return _appDbContext.Events.Where(x => x.Status == "Incomplete");
        }

        public Budget CreateBudget(int eventId)
        {
            Budget budget = new Budget();
            budget.Total = 0;
            budget.RentPrice = 0;
            budget.DecorationPrice = 0;
            budget.FoodPrice = 0;
            budget.EventId = eventId;
            return budget;
        }

        public Event GetEventById(int id)
        {
            return _appDbContext.Events.FirstOrDefault(x => x.Id == id);
        }


        public bool SetEventDateAndTime(int id, Dictionary<string, string>  dateInfo)
        {
            var eventById = GetEventById(id);
            bool isCorrect = DateTime.TryParse(dateInfo["Date"], out var date);
            var startTime = dateInfo["StartHour"] + ":" + dateInfo["StartMinutes"] + " " + dateInfo["TimeOfDayStart"];
            var endTime = dateInfo["EndHour"] + ":" + dateInfo["EndMinutes"] + " " + dateInfo["TimeOfDayEnd"];
            bool correctStartTime = DateTime.TryParse(startTime, out var start);
            bool correctEndTime = DateTime.TryParse(endTime, out var end);

            if (isCorrect && correctStartTime && correctEndTime)
            {
                int result = DateTime.Compare(date, DateTime.Now);
                if (result < 0)
                {
                    return false;
                }
                else
                    eventById.Date = date;

                DateTime newDateTime = date.Date.Add(start.TimeOfDay);
                eventById.EventStart = newDateTime;
                eventById.EventEnd = end;
                _appDbContext.SaveChanges();
                return true;
            }
            else return false;
        }

        public bool CheckDateAndTimeByEventId(int id)
        {
            return GetEventById(id).Date > DateTime.Now;
        }

        public DateTime GetEventStartDate(int id)
        {
            var eventById = GetEventById(id);
            return eventById.EventStart;
        }

        public void SetStatus(int id, string status)
        {
            var eventById = GetEventById(id);
            eventById.Status = status;
            _appDbContext.SaveChanges();
        }

        public string GetStatus(int id)
        {
            var eventById = GetEventById(id);
            return eventById.Status;
        }

        public Dictionary<string, string> GetInfo(int id)
        {
            Dictionary<string, string> info = new Dictionary<string, string>();
            var eventById = GetEventById(id);
            info["Type"] = eventById.Type;
            info["Name"] = eventById.Name;
            info["Status"] = eventById.Status;
            return info;
        }

        public DateTime GetEventStartTime(int id)
        {
            return _appDbContext.Events.Where(x => x.Id == id).Select(y => y.EventStart).FirstOrDefault();
        }

        public DateTime GetEventEndTime(int id)
        {
            return _appDbContext.Events.Where(x => x.Id == id).Select(y => y.EventEnd).FirstOrDefault();
        }

        public bool ManageEventData(int id, string dataToChange, EventData eventDataCol)
        {
            var eventById = _appDbContext.Events.FirstOrDefault(x => x.Id == id);
            bool isCorrect = eventById != null;
            if (isCorrect)
            {
#pragma warning disable CS8604
                SetEventData(eventById, dataToChange, eventDataCol);
#pragma warning restore CS8604
                _appDbContext.SaveChanges();
            }
            return isCorrect;
        }

        public void SetEventData(Event eventById, string dataToChange, EventData eventDataCol)
        {
            switch (eventDataCol)
            {
                case EventData.Size:
                    eventById.Size = dataToChange;
                    break;

                case EventData.SizeRange:
                    eventById.SizeRange = dataToChange;
                    break;

                case EventData.Theme:
                    eventById.Theme = dataToChange;
                    break;
            }
        }
    }
}
