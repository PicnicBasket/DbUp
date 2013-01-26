using DbUp.Engine;

namespace DbUp.MySql
{
    public class MySqlPreprocessor : IScriptPreprocessor
    {
        public string Process(string contents)
        {
            return contents;
        }
    }
}