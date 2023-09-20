using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using NUnit.Framework.Internal;
using RestSharp;

namespace TestProject1.APIDatas
{
    public class TestHttpClient<T>
    {
        public HttpClient _client = new HttpClient();
        private static string Url = "https://6507126a3a38daf4803f13f0.mockapi.io/api/user/";

        public async Task PostUser(string name, string avatar)
        {
            var user = new UserDatacs() {Name = name, Avatar = avatar, CreatedAt = DateAndTime.Now};
            var json = JsonConvert.SerializeObject(user);

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(Url, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task Delete(string id)
        {
            var response = await _client.DeleteAsync(Url + id);
            response.EnsureSuccessStatusCode();
        }

        public string NameGenerator()
        {
            // Define the character set from which we will select random letters
            string characters = "abcdefghijklmnopqrstuvwxyz";

            // Initialize a random number generator
            Random random = new Random();

            // Create a variable to store the generated random string
            string randomString = "";

            // Generate 5 random letters and add them to the string
            for (int i = 0; i < 5; i++)
            {
                // Generate a random index to select a character from the character set
                int index = random.Next(0, characters.Length);

                // Add the selected character to the random string
                randomString += characters[index];
            }

            return randomString;
        }

        public string PicGenerator()
        {
            // Define the character set from which we will select random letters
            string characters = "abcdefghijklmnopqrstuvwxyz";

            // Initialize a random number generator
            Random random = new Random();

            // Create a variable to store the generated random string
            string randomString = "pic: ";

            // Generate 5 random letters and add them to the string
            for (int i = 0; i < 5; i++)
            {
                // Generate a random index to select a character from the character set
                int index = random.Next(0, characters.Length);

                // Add the selected character to the random string
                randomString += characters[index];
            }

            return randomString;
        }

        public async Task PutNewData(string id, string avatar)
        {
            var response = await _client.GetAsync(Url + id);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<T>(content);
            if (user is UserDatacs type)
            {
                type.Avatar = avatar;
                var objAsJson = JsonConvert.SerializeObject(type);
                var stringContent = new StringContent(objAsJson, Encoding.UTF8, "application/json");
                await _client.PutAsync(Url + id, stringContent);
            }
        }
    }
}
