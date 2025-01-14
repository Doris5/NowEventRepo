﻿using NowEvent.Models;

namespace NowEvent.Data
{
    public interface IEventRepository
    {
        int AddEvent(Event newEvent);
        Task<Event> GetEventByIdAsync(int id);
        Event GetEventById(int id);
        IQueryable GetEventsByUserId(string id);
        Task<bool> SetEventDateAndTime(int id, Dictionary<string, string> formattedDateInfo);
        Task<bool> CheckDateAndTimeByEventId(int id);
        DateTime GetEventStartDate(int id);
        Task SetStatus(int id, string status);
        Task<string> GetStatus(int id);
        Task<Dictionary<string, string>> GetInfo(int id);
        bool ManageEventData(int id, string dataToChange, EventData eventDataCol);
        void SetEventData(Event eventById, string dataToChange, EventData eventDataCol);
        public Task<PagedResult<Event>> GetAll(OfferQuery query);
        public Task<List<Event>> GetAllOffers();
        bool CheckIfLargeSize(int id);
        DateTime GetEventTimeStage(int id, EventTimeStages eventTimeStage);
    }

}
