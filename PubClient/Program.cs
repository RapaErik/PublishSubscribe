using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Text;
using Contracts;
using Contracts.Contracts;
using Newtonsoft.Json;



var url = "https://127.0.0.1:7014/publish";

HttpClientHandler clientHandler = new HttpClientHandler();
clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

using var client = new HttpClient(clientHandler);



var contract = new Contract1 { Value = 4, Message = "Lorem Ipsum" };
var msg = new ContractSend<Contract1>(contract, 100, "topic1", "b2e1579e9502b1a3f80fd6f086044130");

var contract2 = new Contract2 { Number = 4 };
var msg2 = new ContractSend<Contract2>(contract2, 100, "topic2", "29e6cb9b38217df54cc470d4c9690f1f");

var contract3 = new Contract3 { ListOfMessages = new List<string> { "Lorem", "Ipsum", "dolor", "sit", "amet", "consectetur", "adipiscing", "elit" } };
var msg3 = new ContractSend<Contract3>(contract3, 100, "topic3", "5f8db4f1f584db1be484dfbcb2b4eb1b");


var json = JsonConvert.SerializeObject(msg);
var data = new StringContent(json, Encoding.UTF8, "application/json");

var json2 = JsonConvert.SerializeObject(msg2);
var data2 = new StringContent(json2, Encoding.UTF8, "application/json");

var json3 = JsonConvert.SerializeObject(msg3);
var data3 = new StringContent(json3, Encoding.UTF8, "application/json");



while (true)
{

    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add("Token", msg.Token);
    var response = await client.PostAsync(url, data);
    var result = await response.Content.ReadAsStringAsync();
    Console.WriteLine(result);

    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add("Token", msg.Token);
    var response2 = await client.PostAsync(url, data2);
    var result2 = await response2.Content.ReadAsStringAsync();
    Console.WriteLine(result2);

    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add("Token", msg3.Token);
    var response3 = await client.PostAsync(url, data3);
    var result3 = await response3.Content.ReadAsStringAsync();
    Console.WriteLine(result2);

    await Task.Delay(1000);
}


