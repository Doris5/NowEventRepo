﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly ILogger<OfferController> _logger;
        private readonly IOfferRepository _offerRepository;
        private readonly IEventRepository _eventRepository;

        public OfferController(ILogger<OfferController> logger, IOfferRepository offerRepository, IEventRepository eventRepository)
        {
            _logger = logger;
            _offerRepository = offerRepository;
            _eventRepository = eventRepository;
        }

        [HttpGet("GetOffersWithInCompleteStatus")]
        [AllowAnonymous]
        public IEnumerable<Event> GetOffersWithInCompleteStatus([FromQuery]string? query)
        {
           return _eventRepository.GetOffersWithInCompleteStatus(query);
        }
        /*[HttpGet]*/
        /*public IEnumerable<Offer> GetAll()
        {
            return _offerRepository.GetAllOffers().ToList();
            //return _eventRepository.GetOffersWithInCompleteStatus().ToList();
        }*/
    }
}
