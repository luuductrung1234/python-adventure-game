using System;
using System.Collections.Generic;
using IronPython.Compiler.Ast;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace IronPython.Custom
{
    public class MyPython
    {
        public MyPython(Action<object> logging)
        {
            Engine = Python.CreateEngine(new Dictionary<string, object>() { { "Debug", false } });
            Logging = logging;
        }

        public ScriptEngine Engine { get; }
        public Action<object> Logging { get; }

        public void PlayingAround(string sourceCode)
        {
            //RunScript(sourceCode);
            AnalyzeScript(sourceCode);
        }

        public void AnalyzeScript(string sourceCode)
        {
            try
            {
                var ast = Engine.GenerateAst(sourceCode);
                if (ast == null)
                    throw new InvalidOperationException("Fail to generate IronPython Abstract Syntax Tree (AST)");
                var body = (SuiteStatement)ast.Body;

                var tokens = Engine.Tokenize(sourceCode);
                if (tokens == null)
                    throw new InvalidOperationException("Fail to tokenize source code");
            }
            catch (SyntaxErrorException ex)
            {
                var token = ex.SourceCode.Substring(ex.RawSpan.Start.Index, ex.RawSpan.Length);
                Logging($"ErrorCode[{ex.ErrorCode}] Start[{ex.RawSpan.Start.Line}:{ex.RawSpan.Start.Column}] " +
                        $"- End[{ex.RawSpan.End.Line}:{ex.RawSpan.End.Column}] " +
                        $@"InCode[{token}] Message[{ex.Message}]");
                // TODO: should return instance of ErrorInfo
            }
        }

        public ScriptScope RunScript(string sourceCode)
        {
            var (source, scope) = Engine.Prepare(sourceCode);

            try
            {
                var compiledCode = source.Compile();
                compiledCode.Execute(scope);
            }
            catch (Exception ex)
            {
                var eo = Engine.GetService<ExceptionOperations>();
                Logging($"{eo.FormatException(ex)}");
            }

            TryCallScriptMethods(scope);
            return scope;
        }

        /// <summary>
        /// Try to get Python methods from executed script 
        /// (script must be executed before calling this method)
        /// </summary>
        /// <param name="scope"></param>
        private void TryCallScriptMethods(ScriptScope scope)
        {
            foreach (var item in scope.GetItems())
            {
                Logging($"Found item (in scope): {item.Key}");
            }

            var isExist = scope.TryGetVariable("greetings", out dynamic greetings);
            if (isExist)
            {
                Logging(greetings(""));
                Logging(greetings("World"));
            }

            isExist = scope.TryGetVariable("get_sys_info_list", out dynamic showSysInfoList);
            if (isExist)
            {
                var infoList = (List)showSysInfoList();
                Logging($"{infoList.GetType()}");
                foreach (var info in infoList)
                {
                    Logging(info);
                }
            }

            isExist = scope.TryGetVariable("get_sys_info_dict", out dynamic showSysInfoDict);
            if (!isExist) return;
            var infoDict = (PythonDictionary)showSysInfoDict();
            foreach (var entry in infoDict)
            {
                Logging($"{entry.Key}: {entry.Value}");
            }
        }
    }
}