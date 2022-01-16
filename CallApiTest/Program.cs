// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Text.Json;
try
{
    Console.WriteLine("Hello, World!");

    var httpClient = new HttpClient();

    var stopWatch = Stopwatch.StartNew();
    /*
     * ======================= First Approach ========================================================
     * This is basic approact to call the api when every call is result is depedent on another result.
     * But if result is not dependent on another result then we can call all the api at the same time
     */
    //var firstValue = await GetFirst(httpClient);
    //var secondValue = await GetSecond(httpClient);
    //var thirdValue = await GetThird(httpClient);

    ///*
    // * ======================= Second Approach ========================================================
    // * Now the time has reduced to almost 1 second. But one problem is there. If all the api throw some
    // * exception then we cann not catch all of them. Only first exception we can get.
    // */

    //var firstValueTask = GetFirst(httpClient);
    //var secondValueTask = GetSecond(httpClient);
    //var thirdValueTask = GetThird(httpClient);

    //await Task.WhenAll(firstValueTask, secondValueTask, thirdValueTask);

    //var firstValue = firstValueTask.Result;
    //var secondValue = secondValueTask.Result;
    //var thirdValue = thirdValueTask.Result;

    /*
   * ======================= Third Approach ========================================================
   * Just change Task Call when calling all
   */

    var firstValueTask = GetFirst(httpClient);
    var secondValueTask = GetSecond(httpClient);
    var thirdValueTask = GetThird(httpClient);

    await TaskExt.WhenAll(firstValueTask, secondValueTask, thirdValueTask);

    var firstValue = firstValueTask.Result;
    var secondValue = secondValueTask.Result;
    var thirdValue = thirdValueTask.Result;

    Console.WriteLine($"Done in: {stopWatch.ElapsedMilliseconds}ms");

    Console.WriteLine($"Return value:{firstValue},{secondValue},{thirdValue}");
}
catch(Exception ex)
{ Console.WriteLine(ex); }

async Task<int> GetFirst(HttpClient httpClient)
{
    var firstResponse = await httpClient.GetStringAsync($"https://localhost:7204/WeatherForecast/first");
    var first=JsonSerializer.Deserialize<First>(firstResponse);
    return first!.firstValue;
}

async Task<int> GetSecond(HttpClient httpClient)
{
    var secondResponse = await httpClient.GetStringAsync($"https://localhost:7204/WeatherForecast/second");
    var second = JsonSerializer.Deserialize<Second>(secondResponse);
    return second!.secondValue;
}

async Task<int> GetThird(HttpClient httpClient)
{
    var thirdResponse = await httpClient.GetStringAsync($"https://localhost:7204/WeatherForecast/third");
    var third = JsonSerializer.Deserialize<Third>(thirdResponse);
    return third!.thirdValue;
}
class First
{
    public int firstValue { get; set; }
}
class Second
{
    public int secondValue { get; set; }
}
class Third
{
    public int thirdValue { get; set; }
}
//Create new class or extention method for all task to handle the exception
public class TaskExt
{
    public static async Task<IEnumerable<T>> WhenAll<T>(params Task<T>[] tasks)
    {
        var allTasks = Task.WhenAll(tasks);
        try
        {
            return await allTasks;
        }
        catch (Exception)
        {
            //we can ignore this exception
        }
        throw allTasks.Exception ?? throw new Exception("Task exception is null");
    }
}
