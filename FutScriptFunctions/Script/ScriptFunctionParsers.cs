using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using FutScriptFunctions.Helpers;
using FutScriptFunctions.Numbers;
using FutScriptFunctions.Screen;
using FutScriptFunctions.Keyboard;

namespace FutScriptFunctions.Script
{
    public enum FunctionResult
    {
        Continue,
        Break
    }

    public delegate FunctionResult FunctionCall();

    // should return delegate for parsing string to a FunctionCall
    public delegate FunctionCall FunctionParser(string args);

    public class FunctionDefinitions
    {
        const bool BREAK = false;

        Script script { get; set; }

        public FunctionDefinitions(Script script)
        {
            // the script object contains additional parameters for the script
            // that are needed for script execution
            this.script = script;
        }

        Keys ParseKey(string KeyText)
        {
            KeyText = KeyText.Trim().ToLower();
            foreach (Keys TestKey in Enum.GetValues(typeof(Keys)))
            {
                string KeyName = Enum.GetName(typeof(Keys), TestKey);

                if (KeyName.ToLower() == KeyText)
                {
                    return TestKey;
                }
            }

            return (Keys)Convert.ToByte(KeyText, 16);
        }

        /// <summary>
        /// Executes file path with an optional break symbol
        /// "" or null -> No file executed, continue;
        /// "@" -> No file executed, break;
        /// "file.txt" -> file.txt executed, continue;
        /// "@file.txt" -> file.txt executed, break
        /// </summary>
        /// <param name="file_path_arg"></param>
        /// <returns></returns>
        public FunctionResult ExecFileBreakable(string file_path_arg)
        {
            if (file_path_arg == null) return FunctionResult.Continue;

            file_path_arg = file_path_arg.Trim();

            bool Break = file_path_arg[0] == '@';

            file_path_arg = file_path_arg.Replace("@", "");
            script.ExecuteFile(file_path_arg);

            return Break ? FunctionResult.Break : FunctionResult.Continue;


        }

        [FunctionParserAttribute("execfile(file_path)")]
        public FunctionCall execfile(string arg_txt)
        {
            return delegate ()
            {
                script.ExecuteFile(arg_txt);
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("hotkey(key,file_path)", "Keyboard")]
        public FunctionCall hotkey(string arg_txt)
        {
            string[] args = arg_txt.Split(',');
            Keys key = ParseKey(args[0]);
            string file_path = args[1];

            return delegate ()
            {
                if (script.HotKeyMngr.HotKeyIsDefined(key)) return FunctionResult.Continue;
                script.HotKeyMngr.CreateHotKey(key, delegate (KeyEventArgs kea)
                {
                    script.ExecuteFile(file_path);
                    return false;
                });
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("ifcolor(x,y,color_rule,file_path)", "Screen")]
        public FunctionCall ifcolor(string arg_txt)
        {
            string[] args = arg_txt.Split(',');
            int X = Int32.Parse(args[0]);
            int Y = Int32.Parse(args[1]);
            ColorChecker color_checker = ColorRule.Parse(args[2]);
            string file_path = (args.Length > 3 ? args[3] : null);

            return delegate ()
            {
                if(color_checker(ScreenCapture.GetColorOfPx(X + script.XRef, 
                    Y + script.YRef)))
                {
                    return ExecFileBreakable(file_path);
                }
                return FunctionResult.Continue;
            };
        }

        // ifarea(200:400, 400:600, 0ff0000, 

        [FunctionParserAttribute("ifarea(x1,y1,x2,y2,color_rule,file_path)", "Screen")]
        public FunctionCall ifarea(string arg_txt)
        {
            string[] args = arg_txt.Split(',');
            int x1 = Int32.Parse(args[0]);
            int y1 = Int32.Parse(args[1]);
            int x2 = Int32.Parse(args[2]);
            int y2 = Int32.Parse(args[3]);
            ColorChecker color_checker = ColorRule.Parse(args[4]);
            string file_path = args.Length > 5 ? args[5] : null;
           
            return delegate ()
            {
                Rectangle screen_area = Rectangle.FromLTRB(x1 + script.XRef, 
                    script.YRef + y1, script.XRef + x2, script.YRef + y2);
                if (ColorDetection.ScreenAreaIncludesColor(screen_area, color_checker))
                {
                    return ExecFileBreakable(file_path);
                }
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("iftimer(time_seconds,file_path)")]
        public FunctionCall iftimer(string arg_txt)
        {
            string[] args = arg_txt.Split(',');
            Number TimeSeconds = Number.Parse(args[0]);
            string file_path = (args.Length > 1 ? args[1] : null);

            return delegate ()
            {
                if((DateTime.Now - script.DynamicTimerStart).TotalSeconds > TimeSeconds.GetInt())
                {
                    return ExecFileBreakable(file_path);
                }
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("keydown(key_code)", "Keyboard")]
        public FunctionCall keydown(string arg_txt)
        {
            Keys key = ParseKey(arg_txt);
            return delegate ()
            {
                KeyboardActionPerformer.KeyDown(key);
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("keyup(key_code)", "Keyboard")]
        public FunctionCall keyup(string arg_txt)
        {
            Keys key = ParseKey(arg_txt);
            return delegate ()
            {
                KeyboardActionPerformer.KeyUp(key);
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("leftclick(time_ms)", "Mouse")]
        public FunctionCall leftclick(string arg_txt)
        {
            Number rn = Number.Parse(arg_txt);
            return delegate ()
            {
                script.MouseMover.LeftButton.Click(rn.GetInt());
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("leftclickat(x,y)", "Mouse")]
        public FunctionCall leftclickat(string arg_txt)
        {
            string[] args = arg_txt.Split(',');
            Number x = Number.Parse(args[0]);
            Number y = Number.Parse(args[1]);
            return delegate ()
            {
                script.MouseMover.MoveToAndClick(x.GetInt(),
                    y.GetInt(), script.MouseMover.LeftButton, script.MouseSpeed.GetDouble());
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("loop(number_of_repeats,file_path)")]
        public FunctionCall loop(string arg_txt)
        {
            string[] args = arg_txt.Split(',');
            Number repeats = Number.Parse(args[0]);
            string file_path = args[1];

            return delegate ()
            {
                int loops = repeats.GetInt();
                for (int i = 0; i < loops; i++)
                {
                    script.ExecuteFile(file_path);
                }
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("middleclick(time_ms)", "Mouse")]
        public FunctionCall middleclick(string arg_txt)
        {
            Number rn = Number.Parse(arg_txt);
            return delegate ()
            {
                script.MouseMover.MiddleButton.Click(rn.GetInt());
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("move(x,y)", "Mouse")]
        public FunctionCall move(string arg_txt)
        {
            string[] args = arg_txt.Split(',');
            Number X = Number.Parse(args[0]);
            Number Y = Number.Parse(args[1]);

            return delegate ()
            {
                script.MouseMover.MoveTo(X.GetInt() + script.XRef,
                    Y.GetInt() + script.YRef, script.MouseSpeed.GetDouble());
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("mover(x1,y1,x2,y2)", "Mouse")]
        public FunctionCall mover(string arg_txt)
        {
            string[] args = arg_txt.Split(',');
            Number X = Number.Parse(args[0]);
            Number Y = Number.Parse(args[1]);
            Number X2 = Number.Parse(args[2]);
            Number Y2 = Number.Parse(args[3]);

            return delegate ()
            {
                script.MouseMover.MoveTo(X.GetInt() + script.XRef,
                    Y.GetInt() + script.YRef, 
                    X2.GetInt() + script.XRef,
                    Y2.GetInt() + script.YRef, script.MouseSpeed.GetDouble());
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("movefrom(x,y)", "Mouse")]
        public FunctionCall movefrom(string arg_txt)
        {
            string[] args = arg_txt.Split(',');
            Number x = Number.Parse(args[0]);
            Number y = Number.Parse(args[1]);

            return delegate ()
            {
                script.MouseMover.MoveFrom(x.GetInt(), y.GetInt());
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("movetocolor(x1,y1,x2,y2,color_rule[,timeout_ms[,file_path]])", "Mouse")]
        public FunctionCall movetocolor(string arg_txt)
        {
            string[] args = arg_txt.Split(',');
            Number x1 = Number.Parse(args[0]);
            Number y1 = Number.Parse(args[1]);
            Number x2 = Number.Parse(args[2]);
            Number y2 = Number.Parse(args[3]);
            ColorChecker color_checker = ColorRule.Parse(args[4]);
            Number timeout = new StaticNumber(0);
            
            string file_path = null;
            if(args.Length > 5)
            {
                timeout = Number.Parse(args[5]);
                if(args.Length > 6)
                {
                    file_path = args[6];
                }
            }

            return delegate ()
            {
                if(!script.MouseMover.MoveToColor(
                    Rectangle.FromLTRB(x1.GetInt() + script.XRef, 
                    y1.GetInt() + script.YRef, 
                    x2.GetInt() + script.XRef, 
                    y2.GetInt() + script.YRef),
                    color_checker, timeout.GetInt(), script.MouseSpeed.GetDouble()))
                {
                    // timed out
                    return ExecFileBreakable(file_path);
                }

                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("process_start(file_path)")]
        public FunctionCall process_start(string arg_txt)
        {
            return delegate ()
            {
                BotHelpers.StartProcess(arg_txt);
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("process_stop(process)")]
        public FunctionCall process_stop(string arg_txt)
        {
            return delegate ()
            {
                BotHelpers.EndProcess(arg_txt);
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("playsound(sound_file)")]
        public FunctionCall playsound(string arg_txt)
        {
            return delegate ()
            {
                BotHelpers.PlaySound(arg_txt);
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("sendkeys(keys)", "Keyboard")]
        public FunctionCall sendkeys(string arg_txt)
        {
            return delegate ()
            {
                SendKeys.SendWait(arg_txt);
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("randomexec(percent,file_path)")]
        public FunctionCall randomexec(string arg_txt)
        {
            string[] args = arg_txt.Split(',');
            Number percent = Number.Parse(args[0]);
            string file_path = args[1];

            return delegate ()
            {
                if(RandomGenerator.RandomBool(percent.GetDouble()))
                {
                    return ExecFileBreakable(file_path);
                }
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("resettimer()")]
        public FunctionCall resettimer(string arg_txt)
        {
            return delegate ()
            {
                script.DynamicTimerStart = DateTime.Now;
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("rightclick(time_ms)", "Mouse")]
        public FunctionCall rightclick(string arg_txt)
        {
            Number rn = Number.Parse(arg_txt);
            return delegate ()
            {
                script.MouseMover.RightButton.Click(rn.GetInt());
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("rightclickat(x,y)", "Mouse")]
        public FunctionCall rightclickat(string arg_txt)
        {
            string[] args = arg_txt.Split(',');
            Number x = Number.Parse(args[0]);
            Number y = Number.Parse(args[1]);
            return delegate ()
            {
                script.MouseMover.MoveToAndClick(x.GetInt(),
                    y.GetInt(), script.MouseMover.RightButton, script.MouseSpeed.GetDouble());
                return FunctionResult.Continue;
            };
        }


        [FunctionParserAttribute("savescreen()", "Screen")]
        public FunctionCall savescreen(string arg_txt)
        {
            return delegate ()
            {
                Bitmap bmp = ScreenCapture.CaptureAllScreens();
                bmp.Save(arg_txt);
                bmp.Dispose();
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("savewindow()", "Screen")]
        public FunctionCall savewindow(string arg_txt)
        {
            return delegate ()
            {
                Bitmap bmp = ScreenCapture.CaptureForegroundWindow();
                bmp.Save(arg_txt);
                bmp.Dispose();
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("stopscript()")]
        public FunctionCall stopscript(string arg_txt)
        {
            return delegate ()
            {
                throw new ScriptStoppedException();
            };
        }

        [FunctionParserAttribute("type()", "Keyboard")]
        public FunctionCall type(string arg_txt)
        {
            return delegate ()
            {
                KeyboardActionPerformer.Type(arg_txt, script.TypingSpeed.GetDouble());
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("wait(time_ms)")]
        public FunctionCall wait(string arg_txt)
        {
            Number rn = Number.Parse(arg_txt);
            return delegate ()
            {
                BotHelpers.Wait(rn.GetInt());
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("waitforarea(x1,y1,x2,y2,tolerance,pixelreq[,timeout_ms[,file_path]])", "Screen")]
        public FunctionCall waitforarea(string arg_txt)
        {
            string[] args = arg_txt.Split(',');
            int x1 = Int32.Parse(args[0]);
            int y1 = Int32.Parse(args[1]);
            int x2 = Int32.Parse(args[2]);
            int y2 = Int32.Parse(args[3]);
            byte tolerance = byte.Parse(args[4]);
            int pixel_requirement = Int32.Parse(args[5]);
            Number timeout_ms = (args.Length > 6 ? Number.Parse(args[6]) : new StaticNumber(0));
            string file_path = (args.Length > 7 ? args[7] : null);

            return delegate ()
            {
               if(!ColorDetection.WaitForAreaChange(Rectangle.FromLTRB(
                   x1 + script.XRef, y1 + script.YRef,
                   x2 + script.XRef, y2 + script.YRef
                   ), pixel_requirement, timeout_ms.GetInt(),
                   ColorCompare.TolerantWithin(tolerance)))
                {
                    // timed out
                    return ExecFileBreakable(file_path);
                }
                return FunctionResult.Continue;
            };
        }

        [FunctionParserAttribute("waitforpx(x,y,color_rule[,timeout_ms[,file_path]])", "Screen")]
        public FunctionCall waitforpx(string arg_txt)
        {
            string[] args = arg_txt.Split(',');
            int x = Int32.Parse(args[0]);
            int y = Int32.Parse(args[1]);
            ColorChecker color_checker = ColorRule.Parse(args[2]);
            Number timeout_ms = (args.Length > 3 ? Number.Parse(args[3]) : new StaticNumber(0));
            string file_path = (args.Length > 4 ? args[4] : null);

            return delegate ()
            {
                if(!ColorDetection.WaitForPx(new Point(x + script.XRef, y + script.YRef),
                    color_checker, timeout_ms.GetInt()))
                {
                    // timed out
                    return ExecFileBreakable(file_path);
                }
                return FunctionResult.Continue;
            };
        }
    }
}
