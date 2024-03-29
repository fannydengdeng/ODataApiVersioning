using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using ODataApiVersioning.Models;

namespace ODataApiVersioning.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ODataRoutePrefix("People")]
    public class PeopleController : ODataController
    {
        // GET ~/v1/people
        // GET ~/v2/people
        // GET ~/api/people?api-version=[1.0|2.0]
        [ODataRoute]
        public IActionResult Get(ODataQueryOptions<Person> options) =>
            Ok(new[] { new Person() { Id = 1, FirstName = "Bill", LastName = "Mei", Email = "bill.mei@somewhere.com", Phone = "555-555-5555" } });

        // GET ~/v1/people(1)
        // GET ~/v2/people(1)
        // GET ~/api/people(1)?api-version=[1.0|2.0]
        [ODataRoute("({id})")]
        public IActionResult Get([FromODataUri] int id, ODataQueryOptions<Person> options) =>
            Ok(new Person() { Id = id, FirstName = "Bill", LastName = "Mei", Email = "bill.mei@somewhere.com", Phone = "555-555-5555" });

        // PATCH ~/v2/people(1)
        // PATCH ~/api/people(1)?api-version=2.0
        [MapToApiVersion("2.0")]
        [ODataRoute("({id})")]
        public IActionResult Patch([FromODataUri] int id, Delta<Person> delta, ODataQueryOptions<Person> options)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var person = new Person() { Id = id, FirstName = "Bill", LastName = "Mei", Email = "bill.mei@somewhere.com", Phone = "555-555-5555" };

            delta.Patch(person);

            return Updated(person);
        }
    }
}
