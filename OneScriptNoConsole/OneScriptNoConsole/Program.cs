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

        private static string scriptEngine = "";
        private static string scriptEngineHostedScript = "";
        private static string oneScriptLanguage = "";
        private static string newtonsoftJson = "";
        private static string myEntryScript = "";

        private static string settings = @"
# Значения ключей можно указывать в кавычках или без них.

# Имя exe файла вашего приложения. /outfilename=""MyApp""
#/outfilename=[string]

# Путь до запускаемого файла. Обычно это стартовый сценарий.
# Файл будет внедрен в exe файл. Извлечь стартовый сценарий можно будет
# запустив exe файл с ключем /pullout.
# Для Windows это может выглядеть так: /script=""C:\888\os\Приложение.os""
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
        [STAThread]
        static void Main(string[] args)
        {
            // Переводим в base64 библиотеки 
            // ScriptEngine.dll
            // ScriptEngine.HostedScript.dll
            // OneScript.Language.dll
            // Newtonsoft.Json.dll

            string pathdll;
            pathdll = currentDirectory + separator + "ScriptEngine.dll";
            if (File.Exists(pathdll))
            {
                scriptEngine = Convert.ToBase64String(File.ReadAllBytes(pathdll));
            }
            else
            {
                WriteError("Не найден файл " + pathdll);
                Environment.Exit(1);
            }

            pathdll = currentDirectory + separator + "ScriptEngine.HostedScript.dll";
            if (File.Exists(pathdll))
            {
                scriptEngineHostedScript = Convert.ToBase64String(File.ReadAllBytes(pathdll));
            }
            else
            {
                WriteError("Не найден файл " + pathdll);
                Environment.Exit(1);
            }

            pathdll = currentDirectory + separator + "OneScript.Language.dll";
            if (File.Exists(pathdll))
            {
                oneScriptLanguage = Convert.ToBase64String(File.ReadAllBytes(pathdll));
            }
            else
            {
                WriteError("Не найден файл " + pathdll);
                Environment.Exit(1);
            }

            pathdll = currentDirectory + separator + "Newtonsoft.Json.dll";
            if (File.Exists(pathdll))
            {
                newtonsoftJson = Convert.ToBase64String(File.ReadAllBytes(pathdll));
            }
            else
            {
                WriteError("Не найден файл " + pathdll);
                Environment.Exit(1);
            }

            //==================================================
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
            string[] result1 = set.Split(new string[] { "\u000a",  "\u000d"}, StringSplitOptions.RemoveEmptyEntries);

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
            // необходимо двойное открытие закрытие во избежание проблем
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
                @"using System; using System.Text; using System.IO; using System.Reflection; using ScriptEngine.HostedScript.Library; namespace osexe" + Environment.NewLine +
                @"{{" + Environment.NewLine + 
                @"	public class Program" + Environment.NewLine + 
                @"	{{" + Environment.NewLine + 
                @"		private static string separator = Path.DirectorySeparatorChar.ToString();" + Environment.NewLine + 
                @"		private static string currentDirectory = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;" + Environment.NewLine + 
                @"		private static string pathEr = currentDirectory + separator + ""error.log"";" + Environment.NewLine + 
                @"		public static int Main(string[] args)" + Environment.NewLine + 
                @"		{{" + Environment.NewLine + 
                @"			// Если стартовый сценарий уже внедрен в exe файл, его можно извлечь." + Environment.NewLine + 
                @"			// Код стартового сценария будет записан в "".os"" файл в той же директории." + Environment.NewLine +
                @"			// Ключ /pullout" + Environment.NewLine + 
                @"			if (args.Length > 0)" + Environment.NewLine + 
                @"			{{" + Environment.NewLine + 
                @"				if (args.Length == 1)" + Environment.NewLine + 
                @"				{{" + Environment.NewLine + 
                @"					string aLine = args[0].Trim();" + Environment.NewLine +
                @"					if (aLine.Substring(0, 8) == ""/pullout"")" + Environment.NewLine + 
                @"					{{" + Environment.NewLine + 
                @"						// Извлекаем стартовый сценарий." + Environment.NewLine + 
                @"						string fileName1 = Assembly.GetExecutingAssembly().Location.Replace(@"".exe"", @"".os"");" + Environment.NewLine + 
                @"						string fileName2;" + Environment.NewLine + 
                @"						if (File.Exists(fileName1))" + Environment.NewLine + 
                @"                        {{" + Environment.NewLine + 
                @"							int index = 1;" + Environment.NewLine + 
                @"                            while (index < 4)" + Environment.NewLine + 
                @"                            {{" + Environment.NewLine + 
                @"								fileName2 = Assembly.GetExecutingAssembly().Location.Replace(@"".exe"", ""("" + index + "").os"");" + Environment.NewLine + 
                @"								if (!File.Exists(fileName2))" + Environment.NewLine + 
                @"								{{" + Environment.NewLine + 
                @"									File.WriteAllText(fileName2, MyEntryScript.strMyEntryScript, Encoding.UTF8);" + Environment.NewLine + 
                @"									Environment.Exit(0);" + Environment.NewLine + 
                @"								}}" + Environment.NewLine + 
                @"								else" + Environment.NewLine + 
                @"								{{" + Environment.NewLine + 
                @"									index++;" + Environment.NewLine + 
                @"								}}" + Environment.NewLine + 
                @"							}}" + Environment.NewLine + 
                @"						}}" + Environment.NewLine + 
                @"                        else" + Environment.NewLine + 
                @"                        {{" + Environment.NewLine + 
                @"							File.WriteAllText(fileName1, MyEntryScript.strMyEntryScript, Encoding.UTF8);" + Environment.NewLine + 
                @"						}}" + Environment.NewLine + 
                @"						Environment.Exit(0);" + Environment.NewLine + 
                @"					}}" + Environment.NewLine + 
                @"				}}" + Environment.NewLine + 
                @"			}}" + Environment.NewLine + 
                @"" + Environment.NewLine + 
                @"			string path;" + Environment.NewLine + 
                @"			path = currentDirectory + separator + ""ScriptEngine.dll""; if (!File.Exists(path)) {{ try {{ FileStream fs = new FileStream(path, FileMode.Create); BinaryWriter br = new BinaryWriter(fs); byte[] bin = Convert.FromBase64String(ScriptEnginedll.strScriptEnginedll); br.Write(bin, 0, bin.Length); fs.Close(); br.Close(); }} catch {{ WriteError(""Не удалось записать файл "" + path); }} }}" + Environment.NewLine + 
                @"			path = currentDirectory + separator + ""ScriptEngine.HostedScript.dll""; if (!File.Exists(path)) {{ try {{ FileStream fs = new FileStream(path, FileMode.Create); BinaryWriter br = new BinaryWriter(fs); byte[] bin = Convert.FromBase64String(ScriptEngineHostedScriptdll.strScriptEngineHostedScriptdll); br.Write(bin, 0, bin.Length); fs.Close(); br.Close(); }} catch {{ WriteError(""Не удалось записать файл "" + path); }} }}" + Environment.NewLine + 
                @"			path = currentDirectory + separator + ""OneScript.Language.dll""; if (!File.Exists(path)) {{ try {{ FileStream fs = new FileStream(path, FileMode.Create); BinaryWriter br = new BinaryWriter(fs); byte[] bin = Convert.FromBase64String(OneScriptLanguagedll.strOneScriptLanguagedll); br.Write(bin, 0, bin.Length); fs.Close(); br.Close(); }} catch {{ WriteError(""Не удалось записать файл "" + path); }} }}" + Environment.NewLine + 
                @"			path = currentDirectory + separator + ""Newtonsoft.Json.dll""; if (!File.Exists(path)) {{ try {{ FileStream fs = new FileStream(path, FileMode.Create); BinaryWriter br = new BinaryWriter(fs); byte[] bin = Convert.FromBase64String(NewtonsoftJsondll.strNewtonsoftJsondll); br.Write(bin, 0, bin.Length); fs.Close(); br.Close(); }} catch {{ WriteError(""Не удалось записать файл "" + path); }} }}" + Environment.NewLine + 
                @"			try {{ Starter starter = new Starter(); return starter.Start(); }} " + Environment.NewLine +
                @"			catch (Exception ex) {{ WriteError("""" + ex.Message); }} return 1;" + Environment.NewLine +
                @"		}}" + Environment.NewLine +
                @"		private static void WriteError(string er) {{ try {{ File.AppendAllText(pathEr, """" + Environment.NewLine + DateTime.Now + Environment.NewLine + er, Encoding.UTF8); }} catch {{ File.WriteAllText(pathEr, """" + Environment.NewLine + DateTime.Now + Environment.NewLine + er, Encoding.UTF8); }} }}" + Environment.NewLine +
                @"	}}" + Environment.NewLine +
                @"}}" + Environment.NewLine +
                @"";
            string typeExecuteScriptBehavior = @"" + Environment.NewLine +
                @"using System; using System.Text; using System.IO; using ScriptEngine; using ScriptEngine.HostedScript; using ScriptEngine.HostedScript.Library; using ScriptEngine.Machine; using System.Reflection;" + Environment.NewLine +
                @"namespace osexe" + Environment.NewLine +
                @"{{" + Environment.NewLine +
                @"    class ExecuteScriptBehavior : AppBehavior, IHostApplication, ISystemLogWriter" + Environment.NewLine +
                @"    {{" + Environment.NewLine +
                @"        private static ArrayImpl attachByPath = new ArrayImpl();" + Environment.NewLine +
                @"        private static string separator = Path.DirectorySeparatorChar.ToString();" + Environment.NewLine +
                @"        private static string currentDirectory = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;" + Environment.NewLine +
                @"        private static string pathEr = currentDirectory + separator + ""error.log"";" + Environment.NewLine +
                @"        string[] _scriptArgs = {{ }}; string _path = Assembly.GetExecutingAssembly().Location;" + Environment.NewLine +
                @"        public ExecuteScriptBehavior(string path, string[] args) {{ _scriptArgs = args; _path = path; }}" + Environment.NewLine +
                @"        public ExecuteScriptBehavior() {{ }}" + Environment.NewLine +
                @"        public IDebugController DebugController {{ get; set; }}" + Environment.NewLine +
                @"        public override int Execute()" + Environment.NewLine +
                @"        {{" + Environment.NewLine +
                @"            if (!System.IO.File.Exists(_path)) {{ Echo(""Script file is not found "" + _path); return 2; }}" + Environment.NewLine +
                @"            SystemLogger.SetWriter(this);" + Environment.NewLine +
                @"            var hostedScript = new HostedScriptEngine();" + Environment.NewLine +
                @"            hostedScript.DebugController = DebugController;" + Environment.NewLine +
                @"            hostedScript.CustomConfig = ScriptFileHelper.CustomConfigPath(_path);" + Environment.NewLine +
                @"            ScriptFileHelper.OnBeforeScriptRead(hostedScript);" + Environment.NewLine +
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
                @"            Process process;" + Environment.NewLine +
                @"            try {{ process = hostedScript.CreateProcess(this, source); }}" + Environment.NewLine +
                @"            catch (Exception e) {{ this.ShowExceptionInfo(e); WriteError(e.Message); return 1; }}" + Environment.NewLine +
                @"            int result; try {{ var compiler = hostedScript.EngineInstance.GetCompilerService(); for (int i = 0; i < attachByPath.Count(); i++) {{ FileInfo fi = new FileInfo(attachByPath.Get(i).AsString()); hostedScript.EngineInstance.AttachedScriptsFactory.AttachByPath(compiler, fi.FullName, fi.Name.Replace("".os"", """")); }} result = process.Start(); }} catch (Exception ex) {{ WriteError(ex.Message); result = 1; }}" + Environment.NewLine +
                @"            hostedScript.Dispose();" + Environment.NewLine +
                @"            ScriptFileHelper.OnAfterScriptExecute(hostedScript);" + Environment.NewLine +
                @"            return result;" + Environment.NewLine +
                @"        }}" + Environment.NewLine +
                @"        private static void WriteError(string er) {{ try {{ File.AppendAllText(pathEr, """" + Environment.NewLine + DateTime.Now + Environment.NewLine + er, Encoding.UTF8); }} catch {{ File.WriteAllText(pathEr, """" + Environment.NewLine + DateTime.Now + Environment.NewLine + er, Encoding.UTF8); }} }}" + Environment.NewLine +
                @"        public void Echo(string text, MessageStatusEnum status = MessageStatusEnum.Ordinary) {{ ConsoleHostImpl.Echo(text, status); }}" + Environment.NewLine +
                @"        public void ShowExceptionInfo(Exception exc) {{ ConsoleHostImpl.ShowExceptionInfo(exc); }}" + Environment.NewLine +
                @"        public bool InputString(out string result, string prompt, int maxLen, bool multiline) {{ return ConsoleHostImpl.InputString(out result, prompt, maxLen, multiline); }}" + Environment.NewLine +
                @"        public string[] GetCommandLineArguments() {{ return _scriptArgs; }}" + Environment.NewLine +
                @"        public void Write(string text) {{ Console.Error.WriteLine(text); WriteError(text); }}" + Environment.NewLine +
                @"    }}" + Environment.NewLine +
                @"}}" + Environment.NewLine +
                @"";
            string typeStarter = @"" + Environment.NewLine +
                @"using System; using System.Text; using System.IO; using System.Reflection;" + Environment.NewLine +
                @"namespace osexe" + Environment.NewLine +
                @"{{" + Environment.NewLine +
                @"    public class Starter" + Environment.NewLine +
                @"    {{" + Environment.NewLine +
                @"        private static string separator = Path.DirectorySeparatorChar.ToString();" + Environment.NewLine +
                @"        private static string currentDirectory = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;" + Environment.NewLine +
                @"        private static string pathEr = currentDirectory + separator + ""error.log"";" + Environment.NewLine +
                @"        public Starter() {{ }}" + Environment.NewLine +
                @"        public int Start()" + Environment.NewLine +
                @"        {{" + Environment.NewLine +
                @"            int returnCode;" + Environment.NewLine +
                @"            ExecuteScriptBehavior esb = new ExecuteScriptBehavior();" + Environment.NewLine +
                @"            try {{ returnCode = esb.Execute(); }}" + Environment.NewLine +
                @"            catch (Exception e) {{ Output.WriteLine(e.ToString()); WriteError(e.Message); returnCode = 1; }}" + Environment.NewLine +
                @"            return returnCode;" + Environment.NewLine +
                @"        }}" + Environment.NewLine +
                @"        private static void WriteError(string er) {{ try {{ File.AppendAllText(pathEr, """" + Environment.NewLine + DateTime.Now + Environment.NewLine + er, Encoding.UTF8); }} catch {{ File.WriteAllText(pathEr, """" + Environment.NewLine + DateTime.Now + Environment.NewLine + er, Encoding.UTF8); }} }}" + Environment.NewLine +
                @"    }}" + Environment.NewLine +
                @"}}" + Environment.NewLine +
                @"";
            string typeAppBehavior = @"" + Environment.NewLine +
                @"namespace osexe {{ internal abstract class AppBehavior {{ public abstract int Execute(); }} }}" + Environment.NewLine +
                @"";
            string typeScriptFileHelper = @"" + Environment.NewLine +
                @"using System; using System.IO; using System.Net.Configuration; using System.Reflection; using System.Text; using ScriptEngine.Environment; using ScriptEngine.HostedScript; namespace osexe {{ internal static class ScriptFileHelper {{ public static bool CodeStatisticsEnabled {{ get; private set; }} public static string CodeStatisticsFileName {{ get; private set; }} public static string CustomConfigPath(string scriptPath) {{ var dir = Path.GetDirectoryName(scriptPath); var cfgPath = Path.Combine(dir, HostedScriptEngine.ConfigFileName); return File.Exists(cfgPath) ? cfgPath : null; }} public static void EnableCodeStatistics(string fileName) {{ CodeStatisticsEnabled = fileName != null; CodeStatisticsFileName = fileName; }} public static bool SetAllowUnsafeHeaderParsing() {{ var aNetAssembly = Assembly.GetAssembly(typeof(SettingsSection)); if (aNetAssembly == null) return false; var aSettingsType = aNetAssembly.GetType(""System.Net.Configuration.SettingsSectionInternal""); if (aSettingsType == null) return false; var anInstance = aSettingsType.InvokeMember(""Section"", BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic, null, null, new object[] {{ }}); if (anInstance == null) return false; var aUseUnsafeHeaderParsing = aSettingsType.GetField(""useUnsafeHeaderParsing"", BindingFlags.NonPublic | BindingFlags.Instance); if (aUseUnsafeHeaderParsing == null) return false; aUseUnsafeHeaderParsing.SetValue(anInstance, true); return true; }} private static bool ConvertSettingValueToBool(string value, bool defaultValue = false) {{ if (value == null) return defaultValue; if (string.Compare(value, ""true"", StringComparison.InvariantCultureIgnoreCase) == 0) return true; if (string.Compare(value, ""1"", StringComparison.InvariantCultureIgnoreCase) == 0) return true; if (string.Compare(value, ""yes"", StringComparison.InvariantCultureIgnoreCase) == 0) return true; if (string.Compare(value, ""false"", StringComparison.InvariantCultureIgnoreCase) == 0) return false; if (string.Compare(value, ""0"", StringComparison.InvariantCultureIgnoreCase) == 0) return false; if (string.Compare(value, ""no"", StringComparison.InvariantCultureIgnoreCase) == 0) return false; return defaultValue; }} public static void OnBeforeScriptRead(HostedScriptEngine engine) {{ var cfg = engine.GetWorkingConfig(); var openerEncoding = cfg[""encoding.script""]; if (!string.IsNullOrWhiteSpace(openerEncoding)) {{ if (StringComparer.InvariantCultureIgnoreCase.Compare(openerEncoding, ""default"") == 0) {{ engine.Loader.ReaderEncoding = FileOpener.SystemSpecificEncoding(); }} else {{ engine.Loader.ReaderEncoding = Encoding.GetEncoding(openerEncoding); }} }} var strictWebRequest = ConvertSettingValueToBool(cfg[""http.strictWebRequest""]); if (!strictWebRequest) {{ SetAllowUnsafeHeaderParsing(); }} if (CodeStatisticsEnabled) {{ engine.EnableCodeStatistics(); }} }} public static void OnAfterScriptExecute(HostedScriptEngine engine) {{ if (CodeStatisticsEnabled) {{ var codeStat = engine.GetCodeStatData(); var statsWriter = new CodeStatWriter(CodeStatisticsFileName, CodeStatWriterType.JSON); statsWriter.Write(codeStat); }} }} }} }}" + Environment.NewLine +
                @"";
            string typeCodeStatWriter = @"" + Environment.NewLine +
                @"using System; using System.IO; using System.Linq; using Newtonsoft.Json; using ScriptEngine.Machine; namespace osexe {{ public class CodeStatWriter {{ private readonly string _outputFileName; private readonly CodeStatWriterType _type; public CodeStatWriter(string fileName, CodeStatWriterType type) {{ _outputFileName = fileName; _type = type; }} public void Write(CodeStatDataCollection codeStatDataCollection) {{ if (_type == CodeStatWriterType.JSON) WriteToJson(codeStatDataCollection); else throw new ArgumentException(""Unsupported type""); }} private void WriteToJson(CodeStatDataCollection codeStatDataCollection) {{ using (var w = new StreamWriter(_outputFileName)) {{ var jwriter = new JsonTextWriter(w) {{ Formatting = Formatting.Indented }}; jwriter.WriteStartObject(); foreach (var source in codeStatDataCollection.GroupBy(arg => arg.Entry.ScriptFileName)) {{ jwriter.WritePropertyName(source.Key, true); jwriter.WriteStartObject(); jwriter.WritePropertyName(""#path""); jwriter.WriteValue(source.Key); foreach (var method in source.GroupBy(arg => arg.Entry.SubName)) {{ jwriter.WritePropertyName(method.Key, true); jwriter.WriteStartObject(); foreach (var entry in method.OrderBy(kv => kv.Entry.LineNumber)) {{ jwriter.WritePropertyName(entry.Entry.LineNumber.ToString()); jwriter.WriteStartObject(); jwriter.WritePropertyName(""count""); jwriter.WriteValue(entry.ExecutionCount); jwriter.WritePropertyName(""time""); jwriter.WriteValue(entry.TimeElapsed); jwriter.WriteEndObject(); }} jwriter.WriteEndObject(); }} jwriter.WriteEndObject(); }} jwriter.WriteEndObject(); jwriter.Flush(); }} }} }} public enum CodeStatWriterType {{ JSON }} }}" + Environment.NewLine +
                @"";
            string typeConsoleHostImpl = @"" + Environment.NewLine +
                @"using System; using OneScript.Language; using ScriptEngine.HostedScript.Library; namespace osexe {{ internal static class ConsoleHostImpl {{ public static void Echo(string text, MessageStatusEnum status = MessageStatusEnum.Ordinary) {{ if (status == MessageStatusEnum.Ordinary) {{ Output.WriteLine(text); }} else {{ var oldColor = Output.TextColor; ConsoleColor newColor; switch (status) {{ case MessageStatusEnum.Information: newColor = ConsoleColor.Green; break; case MessageStatusEnum.Attention: newColor = ConsoleColor.Yellow; break; case MessageStatusEnum.Important: case MessageStatusEnum.VeryImportant: newColor = ConsoleColor.Red; break; default: newColor = oldColor; break; }} try {{ Output.TextColor = newColor; Output.WriteLine(text); }} finally {{ Output.TextColor = oldColor; }} }} }} public static void ShowExceptionInfo(Exception exc) {{ if (exc.GetType() == typeof(ScriptException)) {{ dynamic rte = exc; Echo(rte.MessageWithoutCodeFragment); }} else Echo(exc.ToString()); }} public static bool InputString(out string result, string prompt, int maxLen, bool multiline) {{ if (!String.IsNullOrEmpty(prompt)) Console.Write(prompt); result = multiline ? ReadMultilineString() : Console.ReadLine(); if (result == null) return false; if (maxLen > 0 && maxLen < result.Length) result = result.Substring(0, maxLen); return true; }} private static string ReadMultilineString() {{ string read; System.Text.StringBuilder text = null; while (true) {{ read = Console.ReadLine(); if (read == null) break; if (text == null) {{ text = new System.Text.StringBuilder(read); }} else {{ text.Append(""\n""); text.Append(read); }} }} if (text != null) {{ return text.ToString(); }} return null; }} }} }}" + Environment.NewLine +
                @"";
            string typeOutput = @"" + Environment.NewLine +
                @"using System; using System.Text; namespace osexe {{ internal static class Output {{ private static Encoding _encoding; static Output() {{ Init(); }} public static Action<string> Write {{ get; private set; }} public static ConsoleColor TextColor {{ get {{ return Console.ForegroundColor; }} set {{ Console.ForegroundColor = value; }} }} public static Encoding ConsoleOutputEncoding {{ get {{ return _encoding; }} set {{ _encoding = value; Init(); }} }} private static void Init() {{ if (ConsoleOutputEncoding == null) {{ Write = WriteStandardConsole; }} else {{ Write = WriteEncodedStream; }} }} public static void WriteLine(string text) {{ Write(text); WriteLine(); }} public static void WriteLine() {{ Write(Environment.NewLine); }} private static void WriteStandardConsole(string text) {{ Console.Write(text); }} private static void WriteEncodedStream(string text) {{ using (var stdout = Console.OpenStandardOutput()) {{ var enc = ConsoleOutputEncoding; var bytes = enc.GetBytes(text); stdout.Write(bytes, 0, bytes.Length); }} }} }} }}" + Environment.NewLine +
                @"";
            string typeScriptEnginedll = @"" + Environment.NewLine +
                @"namespace osexe {{ public static class ScriptEnginedll {{ public static string strScriptEnginedll = @""" + scriptEngine + @"""; }} }}" + Environment.NewLine +
                @"";
            string typeScriptEngineHostedScriptdll = @"" + Environment.NewLine +
                @"namespace osexe {{ public static class ScriptEngineHostedScriptdll {{ public static string strScriptEngineHostedScriptdll = @""" + scriptEngineHostedScript + @"""; }} }}" + Environment.NewLine +
                @"";
            string typeOneScriptLanguagedll = @"" + Environment.NewLine +
                @"namespace osexe {{ public static class OneScriptLanguagedll {{ public static string strOneScriptLanguagedll = @""" + oneScriptLanguage + @"""; }} }}" + Environment.NewLine +
                @"";
            string typeNewtonsoftJsondll = @"" + Environment.NewLine +
                @"namespace osexe {{ public static class NewtonsoftJsondll {{ public static string strNewtonsoftJsondll = @""" + newtonsoftJson + @"""; }} }}" + Environment.NewLine +
                @"";
            string typeMyEntryScript = @"" + Environment.NewLine +
                @"namespace osexe {{ public static class MyEntryScript {{ public static string strMyEntryScript = @""" + myEntryScript.Replace("\u0022", "\u0022\u0022") + @"""; }} }}" + Environment.NewLine +
                @"";

            var assemCode = String.Format(assem, "");
            var ProgramCode = String.Format(typeProgram, "Program");
            var StarterCode = String.Format(typeStarter, "Starter");
            var ExecuteScriptBehaviorCode = String.Format(typeExecuteScriptBehavior, "ExecuteScriptBehavior");
            var AppBehaviorCode = String.Format(typeAppBehavior, "AppBehavior");
            var ScriptFileHelperCode = String.Format(typeScriptFileHelper, "ScriptFileHelper");
            var CodeStatWriterCode = String.Format(typeCodeStatWriter, "CodeStatWriter");
            var ConsoleHostImplCode = String.Format(typeConsoleHostImpl, "ConsoleHostImpl");
            var OutputCode = String.Format(typeOutput, "Output");
            var ScriptEnginedllCode = String.Format(typeScriptEnginedll, "ScriptEnginedll");
            var ScriptEngineHostedScriptdllCode = String.Format(typeScriptEngineHostedScriptdll, "ScriptEngineHostedScriptdll");
            var OneScriptLanguagedllCode = String.Format(typeOneScriptLanguagedll, "OneScriptLanguagedll");
            var NewtonsoftJsondllCode = String.Format(typeNewtonsoftJsondll, "NewtonsoftJsondll");
            var MyEntryScriptCode = String.Format(typeMyEntryScript, "MyEntryScript");

            //// Для просмотра компилируемых файлов раскомментируйте строки ниже.
            //System.IO.File.WriteAllText(currentDirectory + separator + separator + "code.txt",
            //    "assemCode===========================================================================" + Environment.NewLine +
            //    assemCode + Environment.NewLine +
            //    "ProgramCode===========================================================================" + Environment.NewLine +
            //    ProgramCode + Environment.NewLine +
            //    "StarterCode===========================================================================" + Environment.NewLine +
            //    StarterCode + Environment.NewLine +
            //    "ExecuteScriptBehaviorCode===========================================================================" + Environment.NewLine +
            //    ExecuteScriptBehaviorCode + Environment.NewLine +
            //    "AppBehaviorCode===========================================================================" + Environment.NewLine +
            //    AppBehaviorCode + Environment.NewLine +
            //    "ScriptFileHelperCode===========================================================================" + Environment.NewLine +
            //    ScriptFileHelperCode + Environment.NewLine +
            //    "CodeStatWriterCode===========================================================================" + Environment.NewLine +
            //    CodeStatWriterCode + Environment.NewLine +
            //    "ConsoleHostImplCode===========================================================================" + Environment.NewLine +
            //    ConsoleHostImplCode + Environment.NewLine +
            //    "OutputCode===========================================================================" + Environment.NewLine +
            //    OutputCode + Environment.NewLine +
            //    "ScriptEnginedllCode===========================================================================" + Environment.NewLine +
            //    ScriptEnginedllCode + Environment.NewLine +
            //    "ScriptEngineHostedScriptdllCode===========================================================================" + Environment.NewLine +
            //    ScriptEngineHostedScriptdllCode + Environment.NewLine +
            //    "OneScriptLanguagedllCode===========================================================================" + Environment.NewLine +
            //    OneScriptLanguagedllCode + Environment.NewLine +
            //    "NewtonsoftJsondllCode===========================================================================" + Environment.NewLine +
            //    NewtonsoftJsondllCode + Environment.NewLine +
            //    "MyEntryScriptCode===========================================================================" + Environment.NewLine +
            //    MyEntryScriptCode
            //    );

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
                "System.Net.Http.dll",
                "System.Configuration.dll",
                currentDirectory + separator + separator + "ScriptEngine.dll",
                currentDirectory + separator + separator + "ScriptEngine.HostedScript.dll",
                currentDirectory + separator + separator + "Newtonsoft.Json.dll",
                currentDirectory + separator + separator + "OneScript.Language.dll",
                "Microsoft.CSharp.dll"};
            CompilerParams.ReferencedAssemblies.AddRange(references);
            var provider = new Microsoft.CSharp.CSharpCodeProvider();
            System.CodeDom.Compiler.CompilerResults compile = provider.CompileAssemblyFromSource(
                CompilerParams, 
                new string[] {
                    assemCode,
                    ProgramCode,
                    StarterCode,
                    ExecuteScriptBehaviorCode,
                    AppBehaviorCode,
                    ScriptFileHelperCode,
                    CodeStatWriterCode,
                    ConsoleHostImplCode,
                    OutputCode,
                    ScriptEnginedllCode,
                    ScriptEngineHostedScriptdllCode,
                    OneScriptLanguagedllCode,
                    NewtonsoftJsondllCode,
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
    }
}
