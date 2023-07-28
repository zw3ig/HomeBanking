using HomeBanking.DTOS;
using HomeBanking.Models;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountRepository _accountRepository;

        public AccountsController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var accounts = _accountRepository.GetAllAccounts();
                var accountsDTO = new List<AccountDTO>();

                foreach (Account account in accounts) 
                {
                    var newAccountDTO = new AccountDTO
                    {
                        Id = account.Id,
                        Number = account.Number,
                        CreationDate = account.CreationDate,
                        Balance = account.Balance,
                        Transactions = account.Transactions.Select(x => new TransactionDTO
                        {
                            Id = x.Id,
                            Type = x.Type,
                            Amount = x.Amount,
                            Description = x.Description,
                            Date = x.Date
                        }).ToList()
                    };

                    accountsDTO.Add(newAccountDTO);
                }
                return Ok(accountsDTO);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var account = _accountRepository.FindById(id);

                var accountDTO = new AccountDTO()
                {
                    Id = account.Id,
                    Number = account.Number,
                    CreationDate = account.CreationDate,
                    Balance = account.Balance,
                    Transactions = account.Transactions.Select(x => new TransactionDTO
                    {
                        Id = x.Id,
                        Type = x.Type,
                        Amount = x.Amount,
                        Description = x.Description,
                        Date = x.Date
                    }).ToList()
                };

                return Ok(accountDTO);

                
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
