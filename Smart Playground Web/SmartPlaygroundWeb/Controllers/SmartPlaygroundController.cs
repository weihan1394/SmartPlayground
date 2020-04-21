using SmartPlaygroundWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmartPlaygroundWeb
{
    public class SmartPlaygroundController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<UserAtZone> Get()
        {
            List<UserAtZone> userAtZones = new List<UserAtZone>();
            for (int index = 1; index <= 5; index++)
            {
                userAtZones.Add(new UserAtZone("name" + index, "image" + index, index));
            }

            return userAtZones;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}