using Newtonsoft.Json;
using BlogGetUsers.Classes;



ClientClass _clientClass = new();

for (int a = 4; a <= 14; a++)       //от 4^го до 14^го пользователя
{
    GetUsers(_clientClass, a);
}



static async Task<ClientClass> GetUsers(ClientClass classe, int i)    //метод, который берет юзеров с сайта
{

        HttpClient client = new();
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"https://jsonplaceholder.typicode.com/posts/{i}");


        try         //пробуем получить данные и записать их в файл
        {
            HttpResponseMessage httpResponse = client.SendAsync(httpRequest).Result;
            string response = await httpResponse.Content.ReadAsStringAsync();
            classe = (ClientClass)JsonConvert.DeserializeObject(response, typeof(ClientClass));
            await WriteInFile(classe);

            return classe;
        }

        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(ex);
            Console.ResetColor();

            return classe;
    }
}


static async Task<string> WriteInFile(ClientClass classe)   //метод, который записывает пользователей
{

    string path = Directory.GetParent(Directory.GetCurrentDirectory()) + "/result.txt";
    string text = "User Id: " + Convert.ToString(classe.UserId) + "\n" + "Id: " + Convert.ToString(classe.Id) + "\n" + "Title: " + classe.Title + "\n" + "Body: "+ classe.Body;

    using (StreamWriter writer = new StreamWriter(path, true))  //запись
    {
        await writer.WriteAsync(text + "\n" + "\n");
    }

    return "";
}
