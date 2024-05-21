using System;
using System.Text;
using System.IO;

namespace OneScriptNoConsole
{
    class Program
    {
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
        private static string os = "";
        private static string script = "";
        private static string settings = @"
# Имя exe файла вашего приложения. /outfilename=""MyApp""
#/outfilename=[string]

# Путь до файла oscript.exe. /os=""C:\Program Files\OneScript\bin\oscript.exe""
#/os=[string]

# Путь до запускаемого файла. Обычно это стартовый сценарий.
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

        static void Main(string[] args)
        {
            string pathSet = Environment.CurrentDirectory + "\\settings.cfg";
            string pathEr = Environment.CurrentDirectory + "\\error.log";

            if (args.Length > 0)
            {
                if (args.Length == 2)
                {
                    foreach (var arg in args)
                    {
                        if (arg.Substring(0, 4) == "/os=")
                        {
                            string os1 = arg.Replace("/os=", "");
                            string os2 = os1.Replace(@"\", @"\\");
                            if (!File.Exists(os2.Replace("\u0022", "")))
                            {
                                try
                                {
                                    File.AppendAllText(pathEr, prefix + "Не найден файл " + os1 + " указанный в аргументе /os=", Encoding.UTF8);
                                }
                                catch
                                {
                                    File.WriteAllText(pathEr, prefix + "Не найден файл " + os1 + " указанный в аргументе /os=", Encoding.UTF8);
                                }
                                Environment.Exit(1);
                            }
                            os = os2;
                        }
                        else if (arg.Substring(0, 8) == "/script=")
                        {
                            string script1 = arg.Replace("/script=", "");
                            string script2 = script1.Replace(@"\", @"\\");
                            if (!File.Exists(script2.Replace("\u0022", "")))
                            {
                                try
                                {
                                    File.AppendAllText(pathEr, prefix + "Не найден файл " + script1 + " указанный в в аргументе /script=", Encoding.UTF8);
                                }
                                catch
                                {
                                    File.WriteAllText(pathEr, prefix + "Не найден файл " + script1 + " указанный в в аргументе /script=", Encoding.UTF8);
                                }
                                Environment.Exit(1);
                            }
                            script = script2;
                        }

                        System.Diagnostics.Process process = new System.Diagnostics.Process();
                        process.StartInfo.FileName = os;
                        process.StartInfo.Arguments = script;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.RedirectStandardError = true;
                        process.Start();
                    }
                }
                else
                {
                    try
                    {
                        File.AppendAllText(pathEr, prefix + @"Число аргументов не равно двум. В командной строке должны быть указаны аргументы /os= и /script=", Encoding.UTF8);
                    }
                    catch
                    {
                        File.WriteAllText(pathEr, prefix + @"Число аргументов не равно двум. В командной строке должны быть указаны аргументы /os= и /script=", Encoding.UTF8);
                    }
                    Environment.Exit(1);
                }
            }
            else
            {
                if (!File.Exists(pathSet))
                {
                    string er = prefix + "Не найден файл настроек settings.cfg" + Environment.NewLine +
                        "Поэтому файл настроек settings.cfg был создан программой автоматически по шаблону." + Environment.NewLine +
                        "Отредактируйте его нужными вам данными.";
                    try
                    {
                        File.AppendAllText(pathEr, er, Encoding.UTF8);
                    }
                    catch
                    {
                        File.WriteAllText(pathEr, er, Encoding.UTF8);
                    }
                    File.WriteAllText(pathSet, settings, Encoding.UTF8);
                    return;
                }
                string set = File.ReadAllText(pathSet);
                string aLine = null;
                string[] result1 = set.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < result1.Length; i++)
                {
                    aLine = result1[i].Trim();
                    if (aLine.Substring(0, 1) == @"#")
                    {
                        continue;
                    }

                    if (aLine.Substring(0, 13) == "/outfilename=")
                    {
                        outFileName = aLine.Replace("/outfilename=", "").Replace("\u0022", "");
                    }
                    else if (aLine.Substring(0, 4) == "/os=")
                    {
                        string os1 = aLine.Replace("/os=", "");
                        string os2 = os1.Replace(@"\", @"\\");
                        if (!File.Exists(os2.Replace("\u0022", "")))
                        {
                            try
                            {
                                File.AppendAllText(pathEr, prefix + "Не найден файл " + os1 + " указанный в settings.cfg", Encoding.UTF8);
                            }
                            catch
                            {
                                File.WriteAllText(pathEr, prefix + "Не найден файл " + os1 + " указанный в settings.cfg", Encoding.UTF8);
                            }
                            Environment.Exit(1);
                        }
                        os = os2;
                    }
                    else if (aLine.Substring(0, 8) == "/script=")
                    {
                        string script1 = aLine.Replace("/script=", "");
                        string script2 = script1.Replace(@"\", @"\\");
                        if (!File.Exists(script2.Replace("\u0022", "")))
                        {
                            try
                            {
                                File.AppendAllText(pathEr, prefix + "Не найден файл " + script1 + " указанный в settings.cfg", Encoding.UTF8);
                            }
                            catch
                            {
                                File.WriteAllText(pathEr, prefix + "Не найден файл " + script1 + " указанный в settings.cfg", Encoding.UTF8);
                            }
                            Environment.Exit(1);
                        }
                        script = script2;
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
                @"using System;" + Environment.NewLine +
                @"using System.IO;" + Environment.NewLine +
                @"" + Environment.NewLine +
                @"namespace OneScriptNoConsole" + Environment.NewLine +
                @"{{" + Environment.NewLine +
                @"    class Program" + Environment.NewLine +
                @"    {{" + Environment.NewLine +
                @"        static void Main(string[] args)" + Environment.NewLine +
                @"        {{" + Environment.NewLine +
                @"            if (File.Exists(" + os + ") && File.Exists(" + script + "))" + Environment.NewLine +
                @"            {{" + Environment.NewLine +
                @"                System.Diagnostics.Process process = new System.Diagnostics.Process();" + Environment.NewLine +
                @"                process.StartInfo.FileName = " + os + ";" + Environment.NewLine +
                @"                process.StartInfo.Arguments = " + script + ";" + Environment.NewLine +
                @"                process.StartInfo.UseShellExecute = false;" + Environment.NewLine +
                @"                process.StartInfo.CreateNoWindow = true;" + Environment.NewLine +
                @"                process.StartInfo.RedirectStandardOutput = true;" + Environment.NewLine +
                @"                process.StartInfo.RedirectStandardError = true;" + Environment.NewLine +
                @"                process.Start();" + Environment.NewLine +
                @"            }}" + Environment.NewLine +
                @"        }}" + Environment.NewLine +
                @"    }}" + Environment.NewLine +
                @"}}" + Environment.NewLine +
                @"";

            var formattedCode = String.Format(typeProgram, "Program");
            var formattedCode2 = String.Format(assem, "");

            var CompilerParams = new System.CodeDom.Compiler.CompilerParameters
            {
                GenerateInMemory = true,
                TreatWarningsAsErrors = false,
                GenerateExecutable = true,
                OutputAssembly = Environment.CurrentDirectory + "\\" + outFileName + ".exe",
                CompilerOptions = "/optimize /target:winexe"
            };

            string[] references = { "System.dll", "Microsoft.CSharp.dll"};
            CompilerParams.ReferencedAssemblies.AddRange(references);

            var provider = new Microsoft.CSharp.CSharpCodeProvider();
            System.CodeDom.Compiler.CompilerResults compile = provider.CompileAssemblyFromSource(CompilerParams, new string[] { formattedCode, formattedCode2 });

            if (compile.Errors.HasErrors)
            {
                string er = prefix;
                foreach (var error in compile.Errors)
                {
                    er = er + error.ToString() + Environment.NewLine;
                }

                try
                {
                    File.AppendAllText(Environment.CurrentDirectory + "\\error.log", er, Encoding.UTF8);
                }
                catch
                {
                    File.WriteAllText(Environment.CurrentDirectory + "\\error.log", er, Encoding.UTF8);
                }
            }
        }
    }
}
