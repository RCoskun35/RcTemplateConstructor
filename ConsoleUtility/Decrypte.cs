using Application.StaticServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUtility
{
    public class Decrypte
    {
        readonly HashService _hashService;

        public Decrypte(HashService hashService)
        {
            while (true)
            {
                string input = Console.ReadLine();

                if (input.ToLower() == "rctemplate")
                {
                    _hashService = hashService;

                    RunHash();

                    break;
                }
            }
        }
        public void GetEncryptedStr(string hashStr)
        {
            try
            {
                Console.WriteLine("\nÇözülmüş Metin:\n" + _hashService.Decrypt(hashStr));
            }
            catch (Exception ex)
            {

                Console.WriteLine("\nHATA : Bu metin çözülemez !");
            }
            finally
            {
                RunHash();
            }
        }

        public void GetDecryptedStr(string hashStr)
        {
            try
            {
                Console.WriteLine("\nŞifrelenmiş Metin:\n" + _hashService.Encrypt(hashStr));
            }
            catch (Exception ex)
            {

                Console.WriteLine("\nHATA : Bu metin şifrelenemez !");

            }
            finally
            {
                RunHash();
            }
        }

        public void RunHash()
        {
            Console.WriteLine("\nMetni çözmek için 1'a, şifrelemek için 2'e basınız :");

            string input2 = Console.ReadLine();

            if (input2 == "1")
            {
                Console.WriteLine("\nÇözülecek metni giriniz:\n");
                string input3 = Console.ReadLine();
                GetEncryptedStr(input3);
            }
            else if (input2 == "2")
            {
                Console.WriteLine("\nŞifrelenecek metni giriniz:\n");
                string input3 = Console.ReadLine();
                GetDecryptedStr(input3);
            }

        }
    }
}
