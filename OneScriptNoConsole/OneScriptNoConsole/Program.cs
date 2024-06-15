using System;
using System.Text;
using System.IO;
using System.Reflection;

namespace OneScriptNoConsole
{
    class Program
    {
        private static string separator = Path.DirectorySeparatorChar.ToString();
        private static string currentDirectory = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;
        private static string pathSet = currentDirectory + separator + "settings.cfg";
        private static string pathEr = currentDirectory + separator + "error.log";
        private static string pathOscriptcfg = currentDirectory + separator + "oscript.cfg";
        private static string pathLib = currentDirectory + separator + "lib";
        private static string pathPackageloaderos = pathLib + separator + "package-loader.os";
        private static string prefix = "" + Environment.NewLine + DateTime.Now + Environment.NewLine;
        private static string outFileName = "MyApp";
        private static string assemblyCompany = "";
        private static string assemblyCopyright = "";
        private static string assemblyDescription = "";
        private static string assemblyProduct = "";
        private static string assemblyTitle = "";
        private static string assemblyTrademark = "";
        private static string assemblyVersion = "0.0.0.0";
        private static string assemblyFileVersion = "0.0.0.0";
        private static string myEntryScript = "";

        [STAThread]
        static void Main(string[] args)
        {
            if (!File.Exists(pathSet))
            {
                string er = "Не найден файл настроек settings.cfg" + Environment.NewLine +
                    "Поэтому файл настроек settings.cfg был создан программой автоматически по шаблону." + Environment.NewLine +
                    "Отредактируйте его нужными вам данными.";
                WriteError(er);
                File.WriteAllText(pathSet, settings, Encoding.UTF8);
                return;
            }
            string set = File.ReadAllText(pathSet);
            string aLine;
            string[] result1 = set.Split(new string[] { "\u000a", "\u000d" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < result1.Length; i++)
            {
                aLine = result1[i].Trim();
                if (aLine.Length == 0)
                {
                    continue;
                }
                if (aLine.Substring(0, 1) == @"#")
                {
                    continue;
                }
                if (aLine.Substring(0, 13) == "/outfilename=")
                {
                    outFileName = aLine.Replace("/outfilename=", "").Replace("\u0022", "");
                }
                else if (aLine.Substring(0, 8) == "/script=")
                {
                    string script1 = aLine.Replace("/script=", "");
                    string script2 = script1.Replace(separator, separator + separator);
                    if (!File.Exists(script2.Replace("\u0022", "")))
                    {
                        WriteError("Не найден файл " + script1 + " указанный в settings.cfg");
                        Environment.Exit(1);
                    }
                    else
                    {
                        myEntryScript = File.ReadAllText(script2.Replace("\u0022", ""), Encoding.UTF8);
                    }
                }
                else if (aLine.Substring(0, 9) == "/company=")
                {
                    assemblyCompany = aLine.Replace("/company=", "").Replace("\u0022", "");
                }
                else if (aLine.Substring(0, 13) == "/description=")
                {
                    assemblyDescription = aLine.Replace("/description=", "").Replace("\u0022", "");
                }
                else if (aLine.Substring(0, 11) == "/copyright=")
                {
                    assemblyCopyright = aLine.Replace("/copyright=", "").Replace("\u0022", "");
                }
                else if (aLine.Substring(0, 9) == "/product=")
                {
                    assemblyProduct = aLine.Replace("/product=", "").Replace("\u0022", "");
                }
                else if (aLine.Substring(0, 7) == "/title=")
                {
                    assemblyTitle = aLine.Replace("/title=", "").Replace("\u0022", "");
                }
                else if (aLine.Substring(0, 11) == "/trademark=")
                {
                    assemblyTrademark = aLine.Replace("/trademark=", "").Replace("\u0022", "");
                }
                else if (aLine.Substring(0, 9) == "/version=")
                {
                    assemblyVersion = aLine.Replace("/version=", "").Replace("\u0022", "");
                }
                else if (aLine.Substring(0, 13) == "/fileversion=")
                {
                    assemblyFileVersion = aLine.Replace("/fileversion=", "").Replace("\u0022", "");
                }
            }
            MakeAssembly();

            // Запишем oscriptcfg.
            if (!File.Exists(pathOscriptcfg))
            {
                File.WriteAllText(pathOscriptcfg, oscriptcfg, Encoding.UTF8);
            }

            // Создадим каталог lib и запишем туда packageloaderos.
            if (!Directory.Exists(pathLib))
            {
                Directory.CreateDirectory(pathLib);
            }

            if (!File.Exists(pathPackageloaderos))
            {
                File.WriteAllText(pathPackageloaderos, packageloaderos, Encoding.UTF8);
            }
        }

        private static void WriteError(string er)
        {
            try
            {
                File.AppendAllText(pathEr, prefix + er, Encoding.UTF8);
            }
            catch
            {
                File.WriteAllText(pathEr, prefix + er, Encoding.UTF8);
            }
        }

        public static void MakeAssembly()
        {
            string assem = @"" + Environment.NewLine +
                @"using System.Reflection;" + Environment.NewLine +
                @"using System.Runtime.CompilerServices;" + Environment.NewLine +
                @"using System.Runtime.InteropServices;" + Environment.NewLine +
                @"" + Environment.NewLine +
                @"// Общие сведения об этой сборке предоставляются следующим набором" + Environment.NewLine +
                @"// набора атрибутов. Измените значения этих атрибутов для изменения сведений," + Environment.NewLine +
                @"// связанные с этой сборкой." + Environment.NewLine +
                @"[assembly: AssemblyTitle(""" + assemblyTitle + "\u0022)]" + Environment.NewLine +
                @"[assembly: AssemblyDescription(""" + assemblyDescription + "\u0022)]" + Environment.NewLine +
                @"[assembly: AssemblyConfiguration("""")]" + Environment.NewLine +
                @"[assembly: AssemblyCompany(""" + assemblyCompany + "\u0022)]" + Environment.NewLine +
                @"[assembly: AssemblyProduct(""" + assemblyProduct + "\u0022)]" + Environment.NewLine +
                @"[assembly: AssemblyCopyright(""" + assemblyCopyright + "\u0022)]" + Environment.NewLine +
                @"[assembly: AssemblyTrademark(""" + assemblyTrademark + "\u0022)]" + Environment.NewLine +
                @"[assembly: AssemblyCulture("""")]" + Environment.NewLine +
                @"" + Environment.NewLine +
                @"// Установка значения False для параметра ComVisible делает типы в этой сборке невидимыми" + Environment.NewLine +
                @"// для компонентов COM. Если необходимо обратиться к типу в этой сборке через" + Environment.NewLine +
                @"// из модели COM задайте для атрибута ComVisible этого типа значение true." + Environment.NewLine +
                @"[assembly: ComVisible(false)]" + Environment.NewLine +
                @"" + Environment.NewLine +
                @"// Следующий GUID представляет идентификатор typelib, если этот проект доступен из модели COM" + Environment.NewLine +
                @"[assembly: Guid(""28c17770-f162-4a6c-a539-6296f4084ee1"")]" + Environment.NewLine +
                @"" + Environment.NewLine +
                @"// Сведения о версии сборки состоят из указанных ниже четырех значений:" + Environment.NewLine +
                @"//" + Environment.NewLine +
                @"//      Основной номер версии" + Environment.NewLine +
                @"//      Дополнительный номер версии" + Environment.NewLine +
                @"//      Номер сборки" + Environment.NewLine +
                @"//      Номер редакции" + Environment.NewLine +
                @"//" + Environment.NewLine +
                @"// Можно задать все значения или принять номера сборки и редакции по умолчанию " + Environment.NewLine +
                @"// используя ""*"", как показано ниже:" + Environment.NewLine +
                @"// [assembly: AssemblyVersion(""1.0.*"")]" + Environment.NewLine +
                @"[assembly: AssemblyVersion(""" + assemblyVersion + "\u0022)]" + Environment.NewLine +
                @"[assembly: AssemblyFileVersion(""" + assemblyFileVersion + "\u0022)]" + Environment.NewLine +
                @"";
            string typeProgram = @"" + Environment.NewLine +
                @"using System;" + Environment.NewLine +
                @"using System.Text;" + Environment.NewLine +
                @"using System.IO;" + Environment.NewLine +
                @"using System.Reflection;" + Environment.NewLine +
                @"namespace osexe" + Environment.NewLine +
                @"{{" + Environment.NewLine +
                @"    public class Program" + Environment.NewLine +
                @"    {{" + Environment.NewLine +
                @"        private static string separator = Path.DirectorySeparatorChar.ToString();" + Environment.NewLine +
                @"        private static string currentDirectory = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;" + Environment.NewLine +
                @"        private static string pathEr = currentDirectory + separator + ""error.log"";" + Environment.NewLine +
                @"        private static string prefix = """" + Environment.NewLine + DateTime.Now + Environment.NewLine;" + Environment.NewLine +
                @"        public static int Main(string[] args)" + Environment.NewLine +
                @"        {{" + Environment.NewLine +
                @"            // Если стартовый сценарий уже внедрен в exe файл, его можно извлечь." + Environment.NewLine +
                @"            // Код стартового сценария будет записан в "".os"" файл в той же директории." + Environment.NewLine +
                @"            // Ключ /pullout" + Environment.NewLine +
                @"            if (args.Length > 0)" + Environment.NewLine +
                @"            {{" + Environment.NewLine +
                @"                if (args.Length == 1)" + Environment.NewLine +
                @"                {{" + Environment.NewLine +
                @"                    string aLine = args[0].Trim();" + Environment.NewLine +
                @"                    if (aLine.Substring(0, 8) == ""/pullout"")" + Environment.NewLine +
                @"                    {{" + Environment.NewLine +
                @"                        // Извлекаем стартовый сценарий." + Environment.NewLine +
                @"                        string fileName1 = Assembly.GetExecutingAssembly().Location.Replace(@"".exe"", @"".os"");" + Environment.NewLine +
                @"                        string fileName2;" + Environment.NewLine +
                @"                        if (File.Exists(fileName1))" + Environment.NewLine +
                @"                        {{" + Environment.NewLine +
                @"                            int index = 1;" + Environment.NewLine +
                @"                            while (index < 4)" + Environment.NewLine +
                @"                            {{" + Environment.NewLine +
                @"                                fileName2 = Assembly.GetExecutingAssembly().Location.Replace(@"".exe"", ""("" + index + "").os"");" + Environment.NewLine +
                @"                                if (!File.Exists(fileName2))" + Environment.NewLine +
                @"                                {{" + Environment.NewLine +
                @"                                    File.WriteAllText(fileName2, MyEntryScript.strMyEntryScript, Encoding.UTF8);" + Environment.NewLine +
                @"                                    Environment.Exit(0);" + Environment.NewLine +
                @"                                }}" + Environment.NewLine +
                @"                                else" + Environment.NewLine +
                @"                                {{" + Environment.NewLine +
                @"                                    index++;" + Environment.NewLine +
                @"                                }}" + Environment.NewLine +
                @"                            }}" + Environment.NewLine +
                @"                        }}" + Environment.NewLine +
                @"                        else" + Environment.NewLine +
                @"                        {{" + Environment.NewLine +
                @"                            File.WriteAllText(fileName1, MyEntryScript.strMyEntryScript, Encoding.UTF8);" + Environment.NewLine +
                @"                        }}" + Environment.NewLine +
                @"                        Environment.Exit(0);" + Environment.NewLine +
                @"                    }}" + Environment.NewLine +
                @"                }}" + Environment.NewLine +
                @"            }}" + Environment.NewLine +
                @"            try" + Environment.NewLine +
                @"            {{" + Environment.NewLine +
                @"                Starter starter = new Starter();" + Environment.NewLine +
                @"                return starter.Start();" + Environment.NewLine +
                @"            }}" + Environment.NewLine +
                @"            catch (Exception er)" + Environment.NewLine +
                @"            {{" + Environment.NewLine +
                @"                WriteError(er.Message);" + Environment.NewLine +
                @"                return 1;" + Environment.NewLine +
                @"            }}" + Environment.NewLine +
                @"        }}" + Environment.NewLine +
                @"        private static void WriteError(string er)" + Environment.NewLine +
                @"        {{" + Environment.NewLine +
                @"            try" + Environment.NewLine +
                @"            {{" + Environment.NewLine +
                @"                File.AppendAllText(pathEr, prefix + er, Encoding.UTF8);" + Environment.NewLine +
                @"            }}" + Environment.NewLine +
                @"            catch" + Environment.NewLine +
                @"            {{" + Environment.NewLine +
                @"                File.WriteAllText(pathEr, prefix + er, Encoding.UTF8);" + Environment.NewLine +
                @"            }}" + Environment.NewLine +
                @"        }}" + Environment.NewLine +
                @"    }}" + Environment.NewLine +
                @"}}" + Environment.NewLine +
                @"";
            string typeStarter = @"" + Environment.NewLine +
                @"using System;" + Environment.NewLine +
                @"using System.Text;" + Environment.NewLine +
                @"using System.IO;" + Environment.NewLine +
                @"using System.Reflection;" + Environment.NewLine +
                @"using ScriptEngine.HostedScript;" + Environment.NewLine +
                @"using ScriptEngine.HostedScript.Library;" + Environment.NewLine +
                @"using ScriptEngine.Machine;" + Environment.NewLine +
                @"namespace osexe" + Environment.NewLine +
                @"{{" + Environment.NewLine +
                @"    public class Starter" + Environment.NewLine +
                @"    {{" + Environment.NewLine +
                @"        private static string separator = Path.DirectorySeparatorChar.ToString();" + Environment.NewLine +
                @"        private static string currentDirectory = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;" + Environment.NewLine +
                @"        private static string pathEr = currentDirectory + separator + ""error.log"";" + Environment.NewLine +
                @"        private static string prefix = """" + Environment.NewLine + DateTime.Now + Environment.NewLine;" + Environment.NewLine +
                @"        private static ArrayImpl attachByPath = new ArrayImpl();" + Environment.NewLine +
                @"        public int Start()" + Environment.NewLine +
                @"        {{" + Environment.NewLine +
                @"            var hostedScript = new HostedScriptEngine();" + Environment.NewLine +
                @"            hostedScript.CustomConfig = HostedScriptEngine.ConfigFileName;" + Environment.NewLine +
                @"            // Если в сценарии есть директива #Использовать, тогда закомментируем её и соберем имена сценариев на которые она указывает в attachByPath" + Environment.NewLine +
                @"            // Иначе директива не сработает так как мы сценарий загружаем из строки и у него не срабатывает свойство Источник." + Environment.NewLine +
                @"            string aLine;" + Environment.NewLine +
                @"            string[] result1 = MyEntryScript.strMyEntryScript.Split(new string[] {{ ""\u000a"", ""\u000d"" }}, StringSplitOptions.RemoveEmptyEntries);" + Environment.NewLine +
                @"            for (int i = 0; i < result1.Length; i++)" + Environment.NewLine +
                @"            {{" + Environment.NewLine +
                @"                aLine = result1[i].Trim();" + Environment.NewLine +
                @"                try" + Environment.NewLine +
                @"                {{" + Environment.NewLine +
                @"                    bool isWin = System.Environment.OSVersion.VersionString.Contains(""Microsoft"");" + Environment.NewLine +
                @"                    if (isWin && aLine.Contains(@""/"")) {{ continue; }}" + Environment.NewLine +
                @"                    if (!isWin && aLine.Contains(@""\"")) {{ continue; }}" + Environment.NewLine +
                @"                    if (aLine.Substring(0, 13) != @""#Использовать"") {{ continue; }}" + Environment.NewLine +
                @"                    else" + Environment.NewLine +
                @"                    {{" + Environment.NewLine +
                @"                        string str = aLine.Replace(@""#Использовать"", """").Trim();" + Environment.NewLine +
                @"                        if (isWin)" + Environment.NewLine +
                @"                        {{" + Environment.NewLine +
                @"                            if (str == ""\u0022"" + @"".\"" + ""\u0022"") {{ string path = @"".\Классы\""; if (Directory.Exists(path)) {{ string[] files = Directory.GetFiles(path, ""*.os""); for (int i1 = 0; i1 < files.Length; i1++) {{ IValue ivalue = ValueFactory.Create(files[i1]); if (attachByPath.Find(ivalue) == ValueFactory.Create()) {{ attachByPath.Add(ivalue); }} }} }} path = @"".\Модули\""; if (Directory.Exists(path)) {{ string[] files = Directory.GetFiles(path, ""*.os""); for (int i1 = 0; i1 < files.Length; i1++) {{ IValue ivalue = ValueFactory.Create(files[i1]); if (attachByPath.Find(ivalue) == ValueFactory.Create()) {{ attachByPath.Add(ivalue); }} }} }} }}" + Environment.NewLine +
                @"                            if (str == ""\u0022"" + @""..\"" + ""\u0022"") {{ string path = @""..\Классы\""; if (Directory.Exists(path)) {{ string[] files = Directory.GetFiles(path, ""*.os""); for (int i1 = 0; i1 < files.Length; i1++) {{ IValue ivalue = ValueFactory.Create(files[i1]); if (attachByPath.Find(ivalue) == ValueFactory.Create()) {{ attachByPath.Add(ivalue); }} }} }} path = @""..\Модули\""; if (Directory.Exists(path)) {{ string[] files = Directory.GetFiles(path, ""*.os""); for (int i1 = 0; i1 < files.Length; i1++) {{ IValue ivalue = ValueFactory.Create(files[i1]); if (attachByPath.Find(ivalue) == ValueFactory.Create()) {{ attachByPath.Add(ivalue); }} }} }} }}" + Environment.NewLine +
                @"                            if (!(str.Contains(@"".\"") || str.Contains(@""..\""))) {{ string[] result2 = str.Split(new string[] {{ "";"", ""\u000a"", ""\u000d"" }}, StringSplitOptions.RemoveEmptyEntries); string path = """"; for (int i1 = 0; i1 < result2.Length; i1++) {{ path = result2[i1].Replace(""\u0022"", """").Trim(); if (Directory.Exists(path)) {{ string[] files = Directory.GetFiles(path, ""*.os""); for (int i2 = 0; i2 < files.Length; i2++) {{ IValue ivalue = ValueFactory.Create(files[i2]); if (attachByPath.Find(ivalue) == ValueFactory.Create()) {{ attachByPath.Add(ivalue); }} }} }} }} }}" + Environment.NewLine +
                @"                        }}" + Environment.NewLine +
                @"                        else" + Environment.NewLine +
                @"                        {{" + Environment.NewLine +
                @"                            if (str == ""\u0022"" + @""./"" + ""\u0022"") {{ string path = @""./Классы/""; if (Directory.Exists(path)) {{ string[] files = Directory.GetFiles(path, ""*.os""); for (int i1 = 0; i1 < files.Length; i1++) {{ IValue ivalue = ValueFactory.Create(files[i1]); if (attachByPath.Find(ivalue) == ValueFactory.Create()) {{ attachByPath.Add(ivalue); }} }} }} path = @""./Модули/""; if (Directory.Exists(path)) {{ string[] files = Directory.GetFiles(path, ""*.os""); for (int i1 = 0; i1 < files.Length; i1++) {{ IValue ivalue = ValueFactory.Create(files[i1]); if (attachByPath.Find(ivalue) == ValueFactory.Create()) {{ attachByPath.Add(ivalue); }} }} }} }}" + Environment.NewLine +
                @"                            if (str == ""\u0022"" + @""../"" + ""\u0022"") {{ string path = @""../Классы/""; if (Directory.Exists(path)) {{ string[] files = Directory.GetFiles(path, ""*.os""); for (int i1 = 0; i1 < files.Length; i1++) {{ IValue ivalue = ValueFactory.Create(files[i1]); if (attachByPath.Find(ivalue) == ValueFactory.Create()) {{ attachByPath.Add(ivalue); }} }} }} path = @""../Модули/""; if (Directory.Exists(path)) {{ string[] files = Directory.GetFiles(path, ""*.os""); for (int i1 = 0; i1 < files.Length; i1++) {{ IValue ivalue = ValueFactory.Create(files[i1]); if (attachByPath.Find(ivalue) == ValueFactory.Create()) {{ attachByPath.Add(ivalue); }} }} }} }}" + Environment.NewLine +
                @"                            if (!(str.Contains(@""./"") || str.Contains(@""../""))) {{ string[] result2 = str.Split(new string[] {{ "";"", ""\u000a"", ""\u000d"" }}, StringSplitOptions.RemoveEmptyEntries); string path = """"; for (int i1 = 0; i1 < result2.Length; i1++) {{ path = result2[i1].Replace(""\u0022"", """").Trim(); if (Directory.Exists(path)) {{ string[] files = Directory.GetFiles(path, ""*.os""); for (int i2 = 0; i2 < files.Length; i2++) {{ IValue ivalue = ValueFactory.Create(files[i2]); if (attachByPath.Find(ivalue) == ValueFactory.Create()) {{ attachByPath.Add(ivalue); }} }} }} }} }}" + Environment.NewLine +
                @"                        }}" + Environment.NewLine +
                @"                    }}" + Environment.NewLine +
                @"                }}" + Environment.NewLine +
                @"                catch {{ continue; }}" + Environment.NewLine +
                @"            }}" + Environment.NewLine +
                @"            MyEntryScript.strMyEntryScript = MyEntryScript.strMyEntryScript.Replace(@""#Использовать"", @""//gflvvdur#Использовать"");" + Environment.NewLine +
                @"            var source = hostedScript.Loader.FromString(MyEntryScript.strMyEntryScript);" + Environment.NewLine +
                @"            Process process = hostedScript.CreateProcess(new HostConsole(), source);" + Environment.NewLine +
                @"            try" + Environment.NewLine +
                @"            {{" + Environment.NewLine +
                @"                var compiler = hostedScript.EngineInstance.GetCompilerService();" + Environment.NewLine +
                @"                for (int i = 0; i < attachByPath.Count(); i++)" + Environment.NewLine +
                @"                {{" + Environment.NewLine +
                @"                    FileInfo fi = new FileInfo(attachByPath.Get(i).AsString());" + Environment.NewLine +
                @"                    hostedScript.EngineInstance.AttachedScriptsFactory.AttachByPath(compiler, fi.FullName, fi.Name.Replace("".os"", """"));" + Environment.NewLine +
                @"                }}" + Environment.NewLine +
                @"                return process.Start();" + Environment.NewLine +
                @"            }}" + Environment.NewLine +
                @"            catch (Exception er)" + Environment.NewLine +
                @"            {{" + Environment.NewLine +
                @"                WriteError(er.Message);" + Environment.NewLine +
                @"                return 1;" + Environment.NewLine +
                @"            }}" + Environment.NewLine +
                @"        }}" + Environment.NewLine +
                @"        private static void WriteError(string er)" + Environment.NewLine +
                @"        {{" + Environment.NewLine +
                @"            try" + Environment.NewLine +
                @"            {{" + Environment.NewLine +
                @"                File.AppendAllText(pathEr, prefix + er, Encoding.UTF8);" + Environment.NewLine +
                @"            }}" + Environment.NewLine +
                @"            catch" + Environment.NewLine +
                @"            {{" + Environment.NewLine +
                @"                File.WriteAllText(pathEr, prefix + er, Encoding.UTF8);" + Environment.NewLine +
                @"            }}" + Environment.NewLine +
                @"        }}" + Environment.NewLine +
                @"    }}" + Environment.NewLine +
                @"}}" + Environment.NewLine +
                @"";
            string typeHostConsole = @"" + Environment.NewLine +
                @"using System;" + Environment.NewLine +
                @"using System.Text;" + Environment.NewLine +
                @"using System.IO;" + Environment.NewLine +
                @"using System.Reflection;" + Environment.NewLine +
                @"using ScriptEngine.HostedScript;" + Environment.NewLine +
                @"using ScriptEngine.HostedScript.Library;" + Environment.NewLine +
                @"namespace osexe" + Environment.NewLine +
                @"{{" + Environment.NewLine +
                @"    class HostConsole : IHostApplication" + Environment.NewLine +
                @"    {{" + Environment.NewLine +
                @"        private static string separator = Path.DirectorySeparatorChar.ToString();" + Environment.NewLine +
                @"        private static string currentDirectory = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;" + Environment.NewLine +
                @"        private static string pathEr = currentDirectory + separator + ""error.log"";" + Environment.NewLine +
                @"        private static string prefix = """" + Environment.NewLine + DateTime.Now + Environment.NewLine;" + Environment.NewLine +
                @"" + Environment.NewLine +
                @"        public void Echo(string text, MessageStatusEnum status = MessageStatusEnum.Ordinary)" + Environment.NewLine +
                @"        {{" + Environment.NewLine +
                @"            WriteError(text);" + Environment.NewLine +
                @"        }}" + Environment.NewLine +
                @"        public void ShowExceptionInfo(Exception exc)" + Environment.NewLine +
                @"        {{" + Environment.NewLine +
                @"            WriteError(exc.Message);" + Environment.NewLine +
                @"        }}" + Environment.NewLine +
                @"        public bool InputString(out string result, string prompt, int maxLen, bool multiline)" + Environment.NewLine +
                @"        {{" + Environment.NewLine +
                @"            result = System.Console.ReadLine();" + Environment.NewLine +
                @"            return true;" + Environment.NewLine +
                @"        }}" + Environment.NewLine +
                @"        public string[] GetCommandLineArguments()" + Environment.NewLine +
                @"        {{" + Environment.NewLine +
                @"            return System.Environment.GetCommandLineArgs();" + Environment.NewLine +
                @"        }}" + Environment.NewLine +
                @"        private static void WriteError(string er)" + Environment.NewLine +
                @"        {{" + Environment.NewLine +
                @"            try" + Environment.NewLine +
                @"            {{" + Environment.NewLine +
                @"                File.AppendAllText(pathEr, prefix + er, Encoding.UTF8);" + Environment.NewLine +
                @"            }}" + Environment.NewLine +
                @"            catch" + Environment.NewLine +
                @"            {{" + Environment.NewLine +
                @"                File.WriteAllText(pathEr, prefix + er, Encoding.UTF8);" + Environment.NewLine +
                @"            }}" + Environment.NewLine +
                @"        }}" + Environment.NewLine +
                @"    }}" + Environment.NewLine +
                @"}}" + Environment.NewLine +
                @"";
            string typeMyEntryScript = @"namespace osexe {{ public static class MyEntryScript {{ public static string strMyEntryScript = @""" + myEntryScript.Replace("\u0022", "\u0022\u0022") + @"""; }} }}";

            var assemCode = String.Format(assem, "");
            var ProgramCode = String.Format(typeProgram, "Program");
            var StarterCode = String.Format(typeStarter, "Starter");
            var HostConsoleCode = String.Format(typeHostConsole, "HostConsole");
            var MyEntryScriptCode = String.Format(typeMyEntryScript, "MyEntryScript");

            //// Для просмотра компилируемых файлов раскомментируйте строки ниже.
            System.IO.File.WriteAllText(currentDirectory + separator + separator + "code.txt",
                "assemCode======================================" + Environment.NewLine + assemCode + Environment.NewLine +
                "ProgramCode====================================" + Environment.NewLine + ProgramCode + Environment.NewLine +
                "StarterCode====================================" + Environment.NewLine + StarterCode + Environment.NewLine +
                "HostConsoleCode====================================" + Environment.NewLine + HostConsoleCode + Environment.NewLine +
                "MyEntryScriptCode==============================" + Environment.NewLine + MyEntryScriptCode
                );

            var CompilerParams = new System.CodeDom.Compiler.CompilerParameters
            {
                GenerateInMemory = true,
                TreatWarningsAsErrors = false,
                GenerateExecutable = true,
                OutputAssembly = currentDirectory + separator + separator + outFileName + ".exe",
                CompilerOptions = "/optimize /target:winexe"
            };

            string[] references = {
                "System.dll",
                "System.Core.dll",
                currentDirectory + separator + separator + "ScriptEngine.dll",
                currentDirectory + separator + separator + "ScriptEngine.HostedScript.dll",
                currentDirectory + separator + separator + "Newtonsoft.Json.dll",
                currentDirectory + separator + separator + "OneScript.Language.dll"};
            //"Microsoft.CSharp.dll"};
            CompilerParams.ReferencedAssemblies.AddRange(references);
            var provider = new Microsoft.CSharp.CSharpCodeProvider();
            System.CodeDom.Compiler.CompilerResults compile = provider.CompileAssemblyFromSource(
                CompilerParams,
                new string[] {
                    assemCode,
                    ProgramCode,
                    StarterCode,
                    HostConsoleCode,
                    MyEntryScriptCode
                });

            if (compile.Errors.HasErrors)
            {
                string er = prefix;
                foreach (var error in compile.Errors)
                {
                    er = er + error.ToString() + Environment.NewLine;
                }
                WriteError(er);
            }
        }

        private static string settings = @"
# Значения ключей можно указывать в кавычках или без них.

# Имя exe файла вашего приложения. /outfilename=""MyApp""
#/outfilename=[string]

# Путь до запускаемого файла. Обычно это стартовый сценарий.
# Файл будет внедрен в exe файл. Извлечь стартовый сценарий можно будет
# запустив exe файл с ключем /pullout.
# Для Windows это может выглядеть так: /script=""C:\777\Приложение.os""
# Для Linux это может выглядеть так: /script=""/home/vlad/Projects/444/Приложение.os""
#/script=[string]

# Название компании.
#/company=[string]

# Краткое описание сборки.
#/description=[string]

# Авторское право на программу.
#/copyright=[string]

# Информация о продукте.
#/product=[string]

# Название сборки как информационного продукта.
#/title=[string]

# Сведения о торговой марке.
#/trademark=[string]

# Версия сборки. /version=""1.0.0.0""
#/version=[string]

# Номер версии файла. /fileversion=""1.0.0.0""
#/fileversion=[string]
";
        private static string oscriptcfg = @"
#Конфигурационный файл OneScript

# Корневой каталог системных библиотек
lib.system = lib

#Дополнительные каталоги поиска библиотек
#lib.additional = C:\somedir;C:somedir2;

# Настройки кодировок.
# Можно указывать стандартное имя кодировки, либо значение default для выбора системной кодировки

#encoding.script=utf-8

#systemlanguage = ru
";
        private static string packageloaderos = @"
// Пояснения по переменным даны в конце модуля
Перем ПоказатьСообщенияЗагрузки;
Перем ВыдаватьОшибкуПриЗагрузкеУжеСуществующихКлассовМодулей;

Перем КэшМодулей;

Процедура ПриЗагрузкеБиблиотеки(Путь, СтандартнаяОбработка, Отказ)
	Вывести(""
	|ПриЗагрузкеБиблиотеки "" + Путь);	

	ФайлМанифеста = Новый Файл(ОбъединитьПути(Путь, ""lib.config""));
	
	Если ФайлМанифеста.Существует() Тогда
		Вывести(""Обрабатываем по манифесту"");

		СтандартнаяОбработка = Ложь;
		ОбработатьМанифест(ФайлМанифеста.ПолноеИмя, Путь, Отказ);
	Иначе
		Вывести(""Обрабатываем структуру каталогов по соглашению"");
		ОбработатьСтруктуруКаталоговПоСоглашению(Путь, СтандартнаяОбработка, Отказ);
	КонецЕсли;
	
КонецПроцедуры

Процедура ОбработатьМанифест(Знач Файл, Знач Путь, Отказ)
	
	Чтение = Новый ЧтениеXML;
	Чтение.ОткрытьФайл(Файл);
	Чтение.ПерейтиКСодержимому();
	
	Если Чтение.ЛокальноеИмя <> ""package-def"" Тогда
		Отказ = Истина;
		Чтение.Закрыть();
		Возврат;
	КонецЕсли;
	
	Пока Чтение.Прочитать() Цикл
		
		Если Чтение.ТипУзла = ТипУзлаXML.Комментарий Тогда

			Продолжить;

		КонецЕсли;

		Если Чтение.ТипУзла = ТипУзлаXML.НачалоЭлемента Тогда
		
			Если Чтение.ЛокальноеИмя = ""class"" Тогда
				ФайлКласса = Новый Файл(Путь + ""/"" + Чтение.ЗначениеАтрибута(""file""));
				Если ФайлКласса.Существует() и ФайлКласса.ЭтоФайл() Тогда
					Идентификатор = Чтение.ЗначениеАтрибута(""name"");
					Если Не ПустаяСтрока(Идентификатор) Тогда
						Вывести(СтрШаблон(""	класс %1, файл %2"", Идентификатор, ФайлКласса.ПолноеИмя));

						// ДобавитьКласс(ФайлКласса.ПолноеИмя, Идентификатор);
						ДобавитьКлассЕслиРанееНеДобавляли(ФайлКласса.ПолноеИмя, Идентификатор);
					КонецЕсли;
				Иначе
					ВызватьИсключение ""Не найден файл "" + ФайлКласса.ПолноеИмя + "", указанный в манифесте"";
				КонецЕсли;
				
				Чтение.Прочитать(); // в конец элемента
			КонецЕсли;

			Если Чтение.ЛокальноеИмя = ""module"" Тогда
				ФайлКласса = Новый Файл(Путь + ""/"" + Чтение.ЗначениеАтрибута(""file""));
				Если ФайлКласса.Существует() и ФайлКласса.ЭтоФайл() Тогда
					Идентификатор = Чтение.ЗначениеАтрибута(""name"");
					Если Не ПустаяСтрока(Идентификатор) Тогда
						Вывести(СтрШаблон(""	модуль %1, файл %2"", Идентификатор, ФайлКласса.ПолноеИмя));
						Попытка
							ДобавитьМодульЕслиРанееНеДобавляли(ФайлКласса.ПолноеИмя, Идентификатор);
						Исключение
							Если ВыдаватьОшибкуПриЗагрузкеУжеСуществующихКлассовМодулей Тогда
								ВызватьИсключение;
							КонецЕсли;
							Вывести(""Предупреждение:
							|	"" + ПодробноеПредставлениеОшибки(ИнформацияОбОшибке()));
						КонецПопытки;
					КонецЕсли;
				Иначе
					ВызватьИсключение ""Не найден файл "" + ФайлКласса.ПолноеИмя + "", указанный в манифесте"";
				КонецЕсли;
				
				Чтение.Прочитать(); // в конец элемента
			КонецЕсли;
		
		КонецЕсли;
		
	КонецЦикла;
	
	Чтение.Закрыть();
	
КонецПроцедуры

Процедура ОбработатьСтруктуруКаталоговПоСоглашению(Путь, СтандартнаяОбработка, Отказ)
	
	КаталогиКлассов = Новый Массив;
	КаталогиКлассов.Добавить(ОбъединитьПути(Путь, ""Классы""));
	КаталогиКлассов.Добавить(ОбъединитьПути(Путь, ""Classes""));
	КаталогиКлассов.Добавить(ОбъединитьПути(Путь, ""src"", ""Классы""));
	КаталогиКлассов.Добавить(ОбъединитьПути(Путь, ""src"", ""Classes""));

	КаталогиМодулей = Новый Массив;
	КаталогиМодулей.Добавить(ОбъединитьПути(Путь, ""Модули""));
	КаталогиМодулей.Добавить(ОбъединитьПути(Путь, ""Modules""));
	КаталогиМодулей.Добавить(ОбъединитьПути(Путь, ""src"", ""Модули""));
	КаталогиМодулей.Добавить(ОбъединитьПути(Путь, ""src"", ""Modules""));


	Для Каждого мКаталог Из КаталогиКлассов Цикл

		ОбработатьКаталогКлассов(мКаталог, СтандартнаяОбработка, Отказ);

	КонецЦикла;

	Для Каждого мКаталог Из КаталогиМодулей Цикл

		ОбработатьКаталогМодулей(мКаталог, СтандартнаяОбработка, Отказ);

	КонецЦикла;

КонецПроцедуры

Процедура ОбработатьКаталогКлассов(Знач Путь, СтандартнаяОбработка, Отказ)

	КаталогКлассов = Новый Файл(Путь);
	
	Если КаталогКлассов.Существует() Тогда
		Файлы = НайтиФайлы(КаталогКлассов.ПолноеИмя, ""*.os"");
		Для Каждого Файл Из Файлы Цикл
			Вывести(СтрШаблон(""	класс (по соглашению) %1, файл %2"", Файл.ИмяБезРасширения, Файл.ПолноеИмя));
			СтандартнаяОбработка = Ложь;
			// ДобавитьКласс(Файл.ПолноеИмя, Файл.ИмяБезРасширения);
			ДобавитьКлассЕслиРанееНеДобавляли(Файл.ПолноеИмя, Файл.ИмяБезРасширения);
		КонецЦикла;
	КонецЕсли;
	
КонецПроцедуры

Процедура ОбработатьКаталогМодулей(Знач Путь, СтандартнаяОбработка, Отказ)

	КаталогМодулей = Новый Файл(Путь);

	Если КаталогМодулей.Существует() Тогда
		Файлы = НайтиФайлы(КаталогМодулей.ПолноеИмя, ""*.os"");
		Для Каждого Файл Из Файлы Цикл
			Вывести(СтрШаблон(""	модуль (по соглашению) %1, файл %2"", Файл.ИмяБезРасширения, Файл.ПолноеИмя));
			СтандартнаяОбработка = Ложь;
			Попытка
				ДобавитьМодульЕслиРанееНеДобавляли(Файл.ПолноеИмя, Файл.ИмяБезРасширения);				
			Исключение
				Если ВыдаватьОшибкуПриЗагрузкеУжеСуществующихКлассовМодулей Тогда
					ВызватьИсключение;
				КонецЕсли;
				СтандартнаяОбработка = Истина;
				Вывести(""Предупреждение:
				|"" + ПодробноеПредставлениеОшибки(ИнформацияОбОшибке()));
			КонецПопытки;
		КонецЦикла;
	КонецЕсли;
	
КонецПроцедуры

Процедура ДобавитьКлассЕслиРанееНеДобавляли(ПутьФайла, ИмяКласса)
	Вывести(""Добавляю класс, если ранее не добавляли "" + ИмяКласса);
	Если ВыдаватьОшибкуПриЗагрузкеУжеСуществующихКлассовМодулей Тогда
		Вывести(""Добавляю класс "" + ИмяКласса);
		ДобавитьКласс(ПутьФайла, ИмяКласса);
		Возврат;
	КонецЕсли;
	
	КлассУжеЕсть = Ложь;
	Попытка
		Объект = Новый(ИмяКласса);
		КлассУжеЕсть = Истина;
	Исключение
		СообщениеОшибки = ОписаниеОшибки();
		ИскомаяОшибка = СтрШаблон(""Конструктор не найден (%1)"", ИмяКласса);
		КлассУжеЕсть = СтрНайти(СообщениеОшибки, ИскомаяОшибка) = 0;
	КонецПопытки;
	Если Не КлассУжеЕсть Тогда
		
		Вывести(""Добавляю класс, т.к. он не найден - "" + ИмяКласса);
		ДобавитьКласс(ПутьФайла, ИмяКласса);
	
	Иначе
		Вывести(""Пропускаю загрузку класса "" + ИмяКласса);

	КонецЕсли;
КонецПроцедуры

Процедура ДобавитьМодульЕслиРанееНеДобавляли(ПутьФайла, ИмяМодуля)
	Вывести(""Добавляю модуль, если ранее не добавляли "" + ИмяМодуля);
	
	МодульУжеЕсть = КэшМодулей.Найти(ИмяМодуля) <> Неопределено;
	Если Не МодульУжеЕсть Тогда
		
		Вывести(""Добавляю модуль, т.к. он не найден - "" + ИмяМодуля);
		ДобавитьМодуль(ПутьФайла, ИмяМодуля);
		КэшМодулей.Добавить(ИмяМодуля);
	Иначе
		Вывести(""Пропускаю загрузку модуля "" + ИмяМодуля);

	КонецЕсли;
КонецПроцедуры

Процедура Вывести(Знач Сообщение)
	Если ПоказатьСообщенияЗагрузки Тогда
		Сообщить(Сообщение);
	КонецЕсли;
КонецПроцедуры

Функция ПолучитьБулевоИзПеременнойСреды(Знач ИмяПеременнойСреды, Знач ЗначениеПоУмолчанию)
	Рез = ЗначениеПоУмолчанию;
	РезИзСреды = ПолучитьПеременнуюСреды(ИмяПеременнойСреды);
	Если ЗначениеЗаполнено(РезИзСреды) Тогда
		РезИзСреды = СокрЛП(РезИзСреды);
		Попытка
			Рез = Число(РезИзСреды) <> 0 ;
		Исключение
			Рез = ЗначениеПоУмолчанию;
			Сообщить(СтрШаблон(""Неверный формат переменной среды %1. Ожидали 1 или 0, а получили %2"", ИмяПеременнойСреды, РезИзСреды));
		КонецПопытки;
	КонецЕсли;

	Возврат Рез;
КонецФункции

// Если Истина, то выдаются подробные сообщения о порядке загрузке пакетов, классов, модулей, что помогает при анализе проблем
// очень полезно при анализе ошибок загрузки
// Переменная среды может принимать значение 0 (выключено) или 1 (включено)
// Значение флага по умолчанию - Ложь
ПоказатьСообщенияЗагрузки = ПолучитьБулевоИзПеременнойСреды(
		""OSLIB_LOADER_TRACE"", Ложь);
			
// Если Ложь, то пропускаются ошибки повторной загрузки классов/модулей, 
//что важно при разработке/тестировании стандартных библиотек
// Если Истина, то выдается ошибка при повторной загрузке классов библиотек из движка
// Переменная среды может принимать значение 0 (выключено) или 1 (включено)
// Значение флага по умолчанию - Истина
ВыдаватьОшибкуПриЗагрузкеУжеСуществующихКлассовМодулей = ПолучитьБулевоИзПеременнойСреды(
	""OSLIB_LOADER_DUPLICATES"", Ложь);

// для установки других значений переменных среды и запуска скриптов можно юзать следующую командную строку
// (set OSLIB_LOADER_TRACE=1) && (oscript .\tasks\test.os)

КэшМодулей = Новый Массив;
";
    }
}
