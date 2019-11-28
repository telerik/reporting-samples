using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;



namespace MyTestWebApi.Controllers
{

        public class Person
        {
            public string Name { get; set; }
            public string[] Surname { get; set; }
        }
        public class PersonController : ApiController
        {
            [HttpPost]
            public Person Post([FromBody] Person person )
            {
           
            if (null != person && person.Name == "John")
            {
                return person;
            }
            return  new Person() { Name = "Kiki", Surname = new string[] { "Jameson", "Another surname" } };
            }
        }

    

}
