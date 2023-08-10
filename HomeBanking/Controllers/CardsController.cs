using HomeBanking.DTOS;
using HomeBanking.Models;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeBanking.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private ICardRepository _cardRepository;
        private IClientRepository _clientRepository;
        public CardsController(ICardRepository cardRepository, IClientRepository clientRepository)
        {
            _cardRepository = cardRepository;
            _clientRepository = clientRepository;
        }

        [HttpGet("clients/current/cards")]
        public IActionResult GetCurrent()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return StatusCode(403, "Unauthorized client");
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return StatusCode(403, "Client not found");

                }

                var cardsDTO = new List<CardDTO>();

                foreach (Card card in client.Cards)
                {
                    var newCardDTO = new CardDTO()
                    {
                        Id = card.Id,
                        CardHolder = card.CardHolder,
                        Color = card.Color,
                        Cvv = card.Cvv,
                        FromDate = card.FromDate,
                        Number = card.Number,
                        ThruDate = card.ThruDate,
                        Type = card.Type
                    };

                    cardsDTO.Add(newCardDTO);
                }

                return Ok(cardsDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("clients/current/cards")]
        public IActionResult Post([FromBody] Card card)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return StatusCode(403, "Unauthorized client");
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return StatusCode(403, "Client not found");
                }

                if (client.Cards.Count() == 3)
                {
                    return StatusCode(403, "Card limit per client reached");
                }

                Card newCard = new Card()
                {
                    ClientId = client.Id,
                    CardHolder = client.FirstName + " " + client.LastName,
                    Type = card.Type,
                    Color = card.Color,
                    Number = "3325-6745-7876-1234",
                    Cvv = 111,
                    FromDate = DateTime.Now,
                    ThruDate = DateTime.Now.AddYears(4),
                };

                _cardRepository.Save(newCard);

                CardDTO cardDTO = new CardDTO()
                {
                    CardHolder = newCard.CardHolder,
                    Type = newCard.Type,
                    Color = newCard.Color,
                    Number = newCard.Number,
                    Cvv = newCard.Cvv,
                    FromDate = newCard.FromDate,
                    ThruDate = newCard.ThruDate
                };

                return Created("", cardDTO);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
