using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polly;
using RestSharp;
using System;

namespace FaultHandlingPolly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRequestsController : ControllerBase
    {
        private readonly ILogger<ServiceRequestsController> _logger;

        public ServiceRequestsController(ILogger<ServiceRequestsController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> get()
        {
            //var RetryPolicy = Policy
            //    .Handle<Exception>()
            //    .RetryAsync(5, onRetry: (exception, retryCount) => {
            //        Console.WriteLine("error:" + exception.Message + "....retryCount"+ retryCount);
            //    });
            // await RetryPolicy.ExecuteAsync(async () =>{ await ConnectToApt(); });

            //var amountToPause = TimeSpan.FromSeconds(15);
            //var RetryWaitPolicy = Policy
            //    .Handle<Exception>()
            //    .WaitAndRetryAsync(5,i=>amountToPause , onRetry: (exception, retryCount) => {
            //        Console.WriteLine("error:" + exception.Message + "....retryCount" + retryCount);
            //    });
            //await RetryWaitPolicy.ExecuteAsync(async () => { await ConnectToApt(); });

            var RetryPolicy = Policy
                .Handle<Exception>()
                .Retry(5, onRetry: (exception, retryCount) =>
                {
                    Console.WriteLine("error:" + exception.Message + "....retryCount" + retryCount);
                });

            var circutPolicy = Policy
                .Handle<Exception>()
                .CircuitBreaker(3, TimeSpan.FromSeconds(30));
            var finalPolicy=RetryPolicy.Wrap(circutPolicy);
            
            finalPolicy.Execute( () => 
                { Console.WriteLine("Execute");
                    ConnectToApSync(); }); 

           // await ConnectToApt();
            return Ok();
        }
        private async Task ConnectToApt()
        {
            var url = "https://matchilling-chuck-norris-jokes-v1.p.rapidapi.com/jokes/random";
            var clinet = new RestClient();
            var request = new RestRequest(url, Method.Get);
            request.AddHeader("accept", "application/json");
            request.AddHeader("X-RapidAPI-Key", "SIGN-UP-FOR-KEY");
            request.AddHeader("X-RapidAPI-Host", "matchilling-chuck-norris-jokes-v1.p.rapidapi.com");

            var response=await clinet.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                Console.WriteLine(response.Content);
            }
            else
            {
                Console.WriteLine(response.ErrorMessage);
                throw new Exception("Error cant abel to conncet");
            }
        }
        private  void ConnectToApSync()
        {
            var url = "https://matchilling-chuck-norris-jokes-v1.p.rapidapi.com/jokes/random";
            var clinet = new RestClient();
            var request = new RestRequest(url, Method.Get);
            request.AddHeader("accept", "application/json");
            request.AddHeader("X-RapidAPI-Key", "SIGN-UP-FOR-KEY");
            request.AddHeader("X-RapidAPI-Host", "matchilling-chuck-norris-jokes-v1.p.rapidapi.com");

            var response =  clinet.Execute(request);
            if (response.IsSuccessful)
            {
                Console.WriteLine(response.Content);
            }
            else
            {
                Console.WriteLine(response.ErrorMessage);
                throw new Exception("Error cant abel to conncet");
            }
        }
    }
}
