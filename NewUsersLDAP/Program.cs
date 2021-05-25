using System;
using System.IO;
using System.Text.RegularExpressions;

namespace NewUsersLDAP
{
    class Program
    {
        /*
            dn: cn=michael.martinez,cn=Usuarios,dc=ldap,dc=gobart,dc=gob,dc=cu
            cn:  michael.martinez
            gidnumber: 500
            homedirectory: /home/users/michael.martinez
            loginshell: /bin/sh
            objectclass: inetOrgPerson
            objectclass: posixAccount
            objectclass: top
            sn: michael.martinez
            uid: michael.martinez
            uidnumber: 1000
            userpassword: Reivaj:9101
       */
        static string newLineFormat = "dn: cn={0},cn=Usuarios,dc=ldap,dc=gobart,dc=gob,dc=cu\ncn:  {1}\ngidnumber: 500\nhomedirectory: /home/users/{2}\nloginshell: /bin/sh\nobjectclass: inetOrgPerson\nobjectclass: posixAccount\nobjectclass: top\nsn: {3}\nuid: {4}\nuidnumber: {5}\nuserpassword: {6}";
        static void Main(string[] args)
        {
            string newUsersPath = "newUsersLDAP.ldif";
            Regex userRegExp = new Regex(@"(\w+\.\w+)");
            bool repeat;
            do
            {
                int uidNew;
                do
                {
                    Console.Write("Id del Primer Usuario (uidnumber): ");
                    repeat = !int.TryParse(Console.ReadLine(), out uidNew);
                    if (repeat)
                        Console.WriteLine("El Id debe ser numérico < {0}", int.MaxValue);
                } while (repeat);
                Console.Write("Fichero Nombre de Usuarios: ");
                string userPath = Console.ReadLine();
                if (File.Exists(userPath))
                {
                    using (StreamReader reader = new StreamReader(userPath))
                    using (StreamWriter writer = new StreamWriter(newUsersPath))
                    {
                        string line;
                        int countLines = 0;
                        repeat = false;
                        while ((line = reader.ReadLine()) != null)
                        {
                            countLines++;
                            string[] lineSplit = line.Split(new char[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);
                            string userName = null;
                            if (lineSplit.Length > 0)
                                userName = lineSplit[0];
                            if (userName != null && userRegExp.IsMatch(userName))
                                writer.WriteLine(String.Format(newLineFormat, userName, userName, userName, userName, userName, uidNew++, String.Format("{0}{1}{2}**\n",
                                        userName[0].ToString().ToUpper(), userName.Split('.')[1], DateTime.Now.Year)));
                            else
                                Console.WriteLine("Sintáxis Inválida en la línea {0}", countLines);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("El Fichero de los usuarios no existe, intentelo de nuevo.");
                    repeat = true;
                }
            } while (repeat);
        }
    }
}
