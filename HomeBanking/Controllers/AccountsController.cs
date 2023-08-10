using HomeBanking.DTOS;
using HomeBanking.Models;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace HomeBanking.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountRepository _accountRepository;
        private IClientRepository _clientRepository;

        public AccountsController(IAccountRepository accountRepository, IClientRepository clientRepository)
        {
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
        }

        [HttpGet("accounts")]
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

        [HttpGet("accounts/{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                var account = _accountRepository.FindById(id);

                if (account == null)
                    return Forbid();

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

        //Metodo GET que devuelva un JSON con los datos de las cuentas del Current Client
        [HttpGet("clients/current/accounts")]
        public IActionResult GetAccounts()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid();
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }

                var accountsDTO = new List<AccountDTO>();

                foreach (Account account in client.Accounts)
                {
                    var newAccountDTO = new AccountDTO()
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

        [HttpPost("clients/current/accounts")]
        public IActionResult Post()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid();
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }

                if (client.Accounts.Count() == 3)
                {
                    return Forbid();
                }

                //En una aplicacion real que esta en produccion esta manera de asignar el Account Number está mal porque se podrían repetir los
                //numeros en algun momento. La manera correcta podría ser hacerlo incremental a partir de la ultima cuenta creada en la base.
                //Para mostrar el funcionamiento sirve.
                //var random = new Random();
                //Number = "VIN-" + random.Next(0, 99999999).ToString().PadLeft(8, '0'),


                //Nueva manera
                string lastAccountNumber = _accountRepository.GetLastAccountNumber();

                int newNumber = 1;
                if (lastAccountNumber != string.Empty)
                    newNumber = Convert.ToInt32(lastAccountNumber.Substring(4)) + 1;

                if (newNumber > 99999999)
                    return Forbid();

                Account newAccount = new Account
                {
                    Number = "VIN-"+newNumber.ToString().PadLeft(8, '0'),
                    CreationDate = DateTime.Now,
                    Balance = 0,
                    ClientId = client.Id
                };

                _accountRepository.Save(newAccount);

                var accountDTO = new AccountDTO
                {
                    Number = newAccount.Number,
                    CreationDate = newAccount.CreationDate,
                    Balance = newAccount.Balance
                };

                return Created("", accountDTO);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
