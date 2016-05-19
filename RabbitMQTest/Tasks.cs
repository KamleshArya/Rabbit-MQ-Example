using System;

namespace RabbitMQExample
{
    public class Tasks
    {
        public static string Foo(string test, Int64 id)
        {
            return string.Format("Kamlesh {0} {1}",test, id);
        }

        public static Avail ReturnAvail() {
            var a = new Avail();
            return a;
        }
    }
    public class TaskParams
    {
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        //public Type MethodReturnType { get; set; }
        public object[] Parameters { get; set; }
    }

    public class Avail{
        public string Test1{get;set;}
        public string Test2{get;set;}
    }
}
