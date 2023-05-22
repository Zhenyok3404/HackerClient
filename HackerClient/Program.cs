using System.IO;
using System;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Net.Http;

using TcpClient tcpClient = new TcpClient();
await tcpClient.ConnectAsync("127.0.0.1", 8888);
Stream stream = tcpClient.GetStream();
while (true)
{
    Console.Clear();
    Console.WriteLine("1. Під'єднатися;");
    Console.WriteLine("2. Брут Форс;");
    Console.WriteLine("3. Закрити з'єднання;");
    Console.WriteLine("Вибір:");
    int choose = int.Parse(Console.ReadLine());
    switch (choose)
    {
        case 1:
            Console.Clear();
            CheckConnect();
            break;
        case 2:
            Console.Clear();
            AttackByBruteForce();
            break;
        case 3:
            ConnectClose();
            break;
        default:
            break;
    }
}
async void AttackByBruteForce()
{
    StreamReader reader;
    Console.WriteLine("Enter username: ");
    string? username = Console.ReadLine();
    while (true)
    {
        try
        {
            List<char> symbols = new List<char>() { 'p','a','s'};
            List<string> variants = new List<string>();
            int count = 0;
            int index = 0;
            BruteForce(variants , symbols, new char[4]);
            for (int i = 0; i < variants.Count(); i++)
            {
                string result = await Auth(username, variants[i]);
                if (result == "Success")
                {
                    index = i;
                    count++;
                    break;
                }
            }
            if (count == 1)
            {
                Console.WriteLine($"Password for {username} is {variants[index]}");
            }
            else
            {
                Console.WriteLine($"The password could not be selected for {username}");
            }
            Console.ReadKey();
            break;
            
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (IOException ex)
        { 
            Console.WriteLine(ex.Message);
        }
    }
}
void BruteForce(List<string> variants ,List<char> symbols, char[] symbolsChar, int index = 0)
{
    if (index == symbolsChar.Length)
    {
        variants.Add(new string(symbolsChar));
    }
    else
    {
        symbols.ForEach((el) =>
        {
            symbolsChar[index] = el;
            BruteForce(variants,symbols, symbolsChar, index + 1);
        });
    }

}
async void CheckConnect()
{
    while (true)
    {
        Console.WriteLine("Enter username: ");
        string? username = Console.ReadLine();
        Console.WriteLine("Enter password: ");
        string? password = Console.ReadLine();

        string result = await Auth(username, password);
        Console.WriteLine(result);

        if (result == "Success")
        {
            Console.ReadKey();
            break;
        }
    }
}

async void ConnectClose()
{
    byte[] request = Encoding.UTF8.GetBytes("Close" + "\n");
    await stream.WriteAsync(request);
}

async Task<String> Auth(string username, string password)
{
    List<byte> buffer = new List<byte>();
    int bytesRead = 10;
    byte[] requestData = Encoding.UTF8.GetBytes(username + " " + password + "\n");
    await stream.WriteAsync(requestData);
    Console.WriteLine("Data is sended");

    while ((bytesRead = stream.ReadByte()) != '\n')
    {
        buffer.Add((byte)bytesRead);
    }

    string result = Encoding.UTF8.GetString(buffer.ToArray());
    return result;
}

async void BruteForse()
{

}
