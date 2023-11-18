using System;
using System.Linq;
using Azure.Data.Tables;
using Azure_Tables.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Azure_Tables.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly string _tableName;

        public ContactsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("SAConnectionString");
            _tableName = configuration.GetValue<string>("AzureTableName");
        }

        private TableClient GetTableClient()
        {
            var serviceClient = new TableServiceClient(_connectionString);
            var TableClient = serviceClient.GetTableClient(_tableName);

            TableClient.CreateIfNotExists();
            return TableClient;
        }

        [HttpPost]
        public IActionResult Create(Contact contact)
        {
            var tableClient = GetTableClient();

            contact.RowKey = Guid.NewGuid().ToString();
            contact.PartitionKey = contact.RowKey;

            tableClient.UpsertEntity(contact);
            return Ok(contact);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, Contact contact)
        {
            var tableClient = GetTableClient();
            var contactTable = tableClient.GetEntity<Contact>(id, id).Value;

            contactTable.Name = contact.Name;
            contactTable.Phone = contact.Phone;
            contactTable.Email = contact.Email;

            tableClient.UpsertEntity(contactTable);
            return Ok();
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var tableClient = GetTableClient();
            var contacts = tableClient.Query<Contact>().ToList();

            return Ok(contacts);
        }

        [HttpGet("GetByName/{name}")]
        public IActionResult GetByName(string name)
        {
            var tableClient = GetTableClient();
            var contacts = tableClient.Query<Contact>(x => x.Name.Equals(name)).ToList();
            
            return Ok(contacts);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var tableClient = GetTableClient();
            tableClient.DeleteEntity(id, id);

            return NoContent();
        }
    }
}