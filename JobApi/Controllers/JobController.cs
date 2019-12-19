using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobApi.Models;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using System;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
//---
namespace JobApi.Controllers
{
    [Route("api/Job")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly JobContext _context;

        public JobController(JobContext context)
        {
            _context = context;

            if (_context.JobItems.Count() == 0)
            {
                //Hämtar data från AF begränsad till 100 träffar
                string apiUrl = "https://jobsearch.api.jobtechdev.se/search?q=Hudiksvall&offset=0&limit=100";

                HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
                getRequest.Method = "GET"; // Använder get metod för att hämta data
                getRequest.Headers.Add("api-key", "YiJcXFx4YWJRXHgwZSxuPlx4OGZceGQ5XHgwOEFDXHg4M3NdXHg5MSMnXHhiZnYi"); //Sätt in egen nyckel för api
                HttpWebResponse responseGet = (HttpWebResponse)getRequest.GetResponse();

                //Om allt är OK kommer den att fortsätta läsa in data
                if (responseGet.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = responseGet.GetResponseStream();
                    StreamReader readStream = null;

                    if (responseGet.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    }
                    var data = readStream.ReadToEnd();

                    var jobObj = JObject.Parse(data);
                    for (int y = 0; y < 100; y++)
                    {
                        var title = jobObj["hits"][y]["headline"].ToString();
                        var dateEnd = DateTime.Parse(jobObj["hits"][y]["application_deadline"].ToString());
                        var place = jobObj["hits"][y]["workplace_address"]["municipality"].ToString();
                        var region = jobObj["hits"][y]["workplace_address"]["region"].ToString();
                        var text = jobObj["hits"][y]["description"]["text"].ToString();
                        dateEnd.ToString("yyyy-MM-dd");

                        _context.JobItems.Add(new JobItem { Title = title, Region = region, Place = place, DateEnd = dateEnd, Text = text });
                    }
                    _context.SaveChanges();//Sparar data

                    //Stänger anslutningen mot AFs api
                    responseGet.Close();
                    readStream.Close();
                }
            }
        }
        // GET: api/Job
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobItem>>> GetJobItems()
        {
            return await _context.JobItems.ToListAsync();
        }
    }
}