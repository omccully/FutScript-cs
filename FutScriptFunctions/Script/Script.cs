using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using FutScriptFunctions.Helpers;
using FutScriptFunctions.Numbers;
using FutScriptFunctions.Mouse;
using FutScriptFunctions.Keyboard;

namespace FutScriptFunctions.Script
{
    public class Script
    {
        public MouseActionPerformer MouseMover = new MouseActionPerformer();

        /// <summary>
        /// The average time to wait in milliseconds between each 
        /// script line execution
        /// </summary>
        public Number StandardWait = new NormalDistributor(48, 11);

        public Number MouseSpeed = new NormalDistributor(383, 25);

        public Number TypingSpeed = new NormalDistributor(5.2, 2.3);

        /// <summary>
        /// X reference that all coordinates are relative to
        /// </summary>
        public int XRef = 0;

        /// <summary>
        /// Y reference that all coordinates are relative to
        /// </summary>
        public int YRef = 0;

        /// <summary>
        /// Object containing the definitions of all build-in
        /// FutScript functions
        /// </summary>
        FunctionDefinitions ProvidedFunctionDefinitions;

        /// <summary>
        /// Object containing the definitions of all available
        /// functions for this script
        /// </summary>
        Dictionary<string, FunctionParser> Functions = new Dictionary<string, FunctionParser>();

        List<ScriptFile> Files = new List<ScriptFile>();

        Thread ScriptThread { get; set; }

        internal HotKeyManager HotKeyMngr = new HotKeyManager();


        public bool IsRunning
        {
            get
            {
                return ScriptThread != null;
            }
            private set { }
        }

        public delegate void LoopFinishedEventHandler(object sender, LoopFinishedEventArgs e);
        public event LoopFinishedEventHandler LoopFinished;
        protected virtual void OnLoopFinished(LoopFinishedEventArgs e)
        {
            LoopFinished?.Invoke(this, e);
        }

        public delegate void ScriptStoppedEventHandler(object sender, ScriptStoppedEventArgs e);
        public event ScriptStoppedEventHandler ScriptStopped;
        protected virtual void OnScriptStopped(ScriptStoppedEventArgs e)
        {
            ScriptStopped?.Invoke(this, e);
        }

       
        public Script()
        {
            InitializeFunctions();
        }

        public DateTime DynamicTimerStart { get; set; }

        /// <summary>
        /// Start running the script in a loop.
        /// 
        /// </summary>
        /// <param name="script_text"></param>
        public void RunLoop(string script_text, bool blocking = true)
        {
            RunThread(delegate ()
            {
                DateTime start_time = DynamicTimerStart = DateTime.Now;
                int loop_count = 0;

                List<FunctionCall> base_script = ProcessScript(script_text);
                while (true)
                {
                    ExecuteScript(base_script);

                    loop_count++;
                    OnLoopFinished(new LoopFinishedEventArgs(loop_count, DateTime.Now - start_time));
                }
            });

            if (blocking)
            {
                ScriptThread.Join();
            }
        }

        /// <summary>
        /// Run the script once
        /// </summary>
        /// <param name="script_text"></param>
        public void RunOnce(string script_text, bool blocking=true)
        {
            RunThread(delegate ()
            {
                DateTime start_time = DynamicTimerStart = DateTime.Now;

                List<FunctionCall> base_script = ProcessScript(script_text);

                ExecuteScript(base_script);
            });

            if(blocking)
            {
                ScriptThread.Join();
            }
        }

        /// <summary>
        /// Stop the running script
        /// </summary>
        public void Stop()
        {
            // Do NOT call Stop() from the ScriptThread
            // Instead, throw a ScriptStoppedException
            ScriptThread.Abort();
            ScriptThread = null;

            // calling this event is not necessary because thread aborts cause 
            // ThreadAbortException to be thrown on ScriptThread, thus triggering
            // the event
            // OnScriptStopped(new ScriptStoppedEventArgs());
        }

        /// <summary>
        /// All script thread starts should be made through this method
        /// for proper handling of ScriptStoppedException and the ScriptThread 
        /// variable
        /// </summary>
        /// <param name="start"></param>
        void RunThread(ThreadStart start)
        {
            Files.Clear();

            if (ScriptThread == null)
            {
                ScriptThread = new Thread(delegate() {
                    try
                    {
                        start();
                    }
                    catch (Exception e)
                    {
                        OnScriptStopped(new ScriptStoppedEventArgs(e));
                        // if thread was aborted, ThreadAbortException
                        // gets automatically rethrown at end of
                        // catch block
                    }

                    ScriptThread = null;
                });
                ScriptThread.Start();
            }
            else
            {
                throw new Exception("A script is already running.");
            }
        }

        /// <summary>
        /// Executes a series of <see cref="FunctionCall"/>s
        /// </summary>
        /// <param name="calls"></param>
        /// <returns>True if script all lines in the script were executed.
        /// False if the script terminated.</returns>
        bool ExecuteScript(IEnumerable<FunctionCall> calls)
        {
            foreach(FunctionCall call in calls)
            {
                FunctionResult result = call();
                BotHelpers.Wait(StandardWait.GetInt());
                if(result == FunctionResult.Break)
                {
                    // don't continue with script
                    return false;
                }
            }
            return true; // script completed
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns>True if script all lines in the script were executed.
        /// False if the script terminated.</returns>
        internal bool ExecuteFile(string path)
        {
            path = path.Trim();

            // check if this file has already been read and parsed
            ScriptFile file = Files.FirstOrDefault(sf => sf.Path == path);
            if(file != null)
            {
                return ExecuteScript(file.FunctionCalls);
            }

            // first time executing this file in this instance, so
            // we read it, parse it, and cache it. 
            if(!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }

            file = new ScriptFile(path, ProcessScript(File.ReadAllText(path)));
            Files.Add(file);
            return ExecuteScript(file.FunctionCalls);
        }

        internal List<FunctionCall> ProcessScript(string script_text)
        {
            List<FunctionCall> script_calls = new List<FunctionCall>();

            foreach(string line in script_text.Split('\n'))
            {
                if(!line.Contains('(') || !line.Contains(')'))
                {
                    // all valid script lines include parenthesis
                    continue;
                }

                string args = line.Substring(line.IndexOf('(') + 1, line.LastIndexOf(')') - line.IndexOf('(') - 1);
                string name = line.Replace(" ", "").Substring(0, line.IndexOf('('));

                script_calls.Add(Functions[name](args));
            }

            return script_calls;
        }

        public void DeclareFunction(string name, FunctionParser parser)
        {
            if (Functions.ContainsKey(name))
                throw new ArgumentException("This function was already declared");
            Functions[name] = parser;
        }

        public FunctionResult CallFunction(string name, string args)
        {
            // functions[name] is FunctionParser
            // (args) parses the args and gets a FunctionCall
            // () calls the FunctionCall
            return Functions[name](args)();
        }

        public static IEnumerable<MethodInfo> GetBuiltInFunctions()
        {
            return typeof(FunctionDefinitions).GetMethods().Where(
                m => m.GetCustomAttributes(typeof(FunctionParserAttribute)).Any());
        }

        public static MethodInfo GetBuiltInFunction(string function_name)
        {
            return typeof(FunctionDefinitions).GetMethods().First(
                m => m.Name == function_name && 
                m.GetCustomAttributes(typeof(FunctionParserAttribute)).Any()
            );
        }

        void InitializeFunctions()
        {
            ProvidedFunctionDefinitions = new FunctionDefinitions(this);
            foreach (MethodInfo method_info in GetBuiltInFunctions())
            {
                DeclareFunction(method_info.Name,
                   (FunctionParser)method_info.CreateDelegate(typeof(FunctionParser), 
                   ProvidedFunctionDefinitions));
            }
        }
    }
}
