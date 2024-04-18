using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Web.HTTP
{
    public class HttpResponseTracer
    {
        private StreamWriter _streamWriter;

        public HttpResponseTracer(StreamWriter streamWriter)
        {
            _streamWriter = streamWriter;
        }

        public void WriteLine(string content = "")
        {
            Console.WriteLine(content);
            _streamWriter.WriteLine(content);
        }

        public void Write(string content)
        {
            Console.Write(content);
            _streamWriter.Write(content);
        }
    }
}
