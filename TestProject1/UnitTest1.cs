using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using TestProject1.APIDatas;

namespace TestProject1
{
    public class Tests
    {
        //adf
        private static readonly TestHttpClient<UserDatacs> _users = new TestHttpClient<UserDatacs>();
        private readonly HttpClient _client= new HttpClient();
        private const string Url = "https://6507126a3a38daf4803f13f0.mockapi.io/user/";
        
        private string name = _users.NameGenerator();
        private string pic = _users.PicGenerator();
        [Test]
        public async Task PostUserAsync()
        {
            

            await _users.PostUser(name, pic);

            var response = await _client.GetAsync(Url);

            Assert.IsTrue(response.IsSuccessStatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<UserDatacs>>(content);

            // Now 'users' is a list of UserDatacs objects representing all the users in the JSON array

            Assert.IsNotNull(users);
            Assert.IsNotEmpty(users);

            // You may want to find the specific user you posted and assert its properties
            var nameUser = users.Find(u => u.Name == name);
            Assert.IsNotNull(nameUser);

            // Assert properties of the 'johnUser' object
            StringAssert.AreEqualIgnoringCase(nameUser.Name, name);


        }
        [Test]
        public async Task DeleteFirstUserAsync()
        {
            await _users.Delete("1");

            var response = await _client.GetStringAsync(Url);
            var usersForTest = JsonConvert.DeserializeObject<IEnumerable<UserDatacs>>(response);
            foreach (var user in usersForTest)
            {
                Assert.IsTrue(user.id != "1");
            }
        }
        [Test]
        public async Task PutNewAvatar()
        {
            await _users.PutNewData("1","pic: cat");

            var response = await _client.GetStringAsync(Url);

            var users = JsonConvert.DeserializeObject<List<UserDatacs>>(response);

            Assert.IsNotNull(users);
            Assert.IsNotEmpty(users);

            var firstUserById = users.Find(u => u.id == "1");
            Assert.IsNotNull(firstUserById);

            StringAssert.AreEqualIgnoringCase(firstUserById.Avatar, "pic: cat");

        }
    }
}