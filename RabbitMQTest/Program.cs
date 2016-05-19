
namespace RabbitMQExample
{
    public class Program
    {
        static void Main()
        {
            var obj = new TaskParams()
            {
                MethodName = "Foo",
                ClassName = "RabbitMqTest1.Tasks",
                Parameters = new object[] { "kamlesh", 2 }, 
                //MethodReturnType = typeof(string)
            };

            new Producer(obj).InsertTaskIntoQueue();
            new Consumer().ProcessTaskFromQueue();
        }
    }
}
