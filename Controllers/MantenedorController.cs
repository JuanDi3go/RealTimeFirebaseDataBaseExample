using BDFireBaseCodigo.Models;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.Contracts;

namespace BDFireBaseCodigo.Controllers
{
    public class MantenedorController : Controller
    {
        IFirebaseClient _client;

        public MantenedorController()
        {
            FirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "YOurDataBaseSecrets",
                BasePath = "YourBasePath"
            };
            _client = new FirebaseClient(config);
        }

        public IActionResult Index()
        {
            Dictionary<string,Contact> list = new Dictionary<string,Contact>();

            FirebaseResponse response = _client.Get("contactos");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                list = JsonConvert.DeserializeObject<Dictionary<string, Contact>>(response.Body);

            List<Contact> contacts = new List<Contact>();

            foreach ( KeyValuePair<string,Contact> element in list)
            {
                contacts.Add(new Contact()
                {
                    IdContact = element.Key,
                    Name = element.Value.Name,
                    Email = element.Value.Email,
                    Phone = element.Value.Phone,
                });


            }


            return View(contacts);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Contact oContact)
        {
            string idGenerado =  Guid.NewGuid().ToString("N");
            SetResponse response = _client.Set("contactos/" + idGenerado, oContact);


            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return View();
            else
                return View();

       
        }
        [HttpGet]
        public IActionResult Update(string idContact)
        {
            FirebaseResponse response = _client.Get("contactos/" + idContact);

            Contact ocontact = response.ResultAs<Contact>();
            ocontact.IdContact = idContact;

            return View(ocontact);
        }

        [HttpPost]
        public IActionResult Update(Contact oContact)
        {
            string idContact = oContact.IdContact;
            oContact.IdContact = null;

            FirebaseResponse response = _client.Update("contactos/" + idContact, oContact);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }


            
        }
        public IActionResult Delete(string idContact)
        {
            FirebaseResponse response = _client.Delete("contactos/" + idContact);
            return RedirectToAction("Index");
        }
    }
}
