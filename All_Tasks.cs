using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1_Vendor
{
    class Program
    {
        static void Main(string[] args)
        {
            string Num, Vendor, NextNumber;
            bool Verification;
            Console.WriteLine("Please enter the number of the card:");
            Num = Console.ReadLine();

            Vendor = GetCreditCardVendor(Num);
            if (Vendor == "Error")
            {
                Console.WriteLine("You enter the wrong number, try again.");
                Console.ReadKey();
                return;
            }
            else
                Console.WriteLine(Vendor);

            Verification = IsCreditCardNumberValid(Num);
            if (Verification)
                Console.WriteLine("The card number is valid.");
            else
            {
                Console.WriteLine("The card number is not valid.");
                Console.ReadKey();
                return;
            }

            NextNumber = GenerateNextCreditCardNumber(Num);
            Console.WriteLine("Next generated card number is: \n" + NextNumber);

            Console.ReadKey();
        }


        static string GetCreditCardVendor(string Num)
        {
            int[] Check_Num = new int[4];
            string Vendor, Error;

            const int AmExpress_JCB = 3, //Перші цифри номеру карток
                      Maestro = 6,
                      Maestro_Master = 5,
                      Visa = 4;

            if (!(Verification(Num))) { return "Error"; } // Перевірка на правильність введеного номеру

            if (!IsCreditCardNumberValid(Num)) 
            {
                Vendor = "Unknown"; return Vendor;
            }

            for (int i = 0; i < Check_Num.Length; i++)
            {
                Check_Num[i] = Num[i] - '0'; //Присвоєння елементам масиву перших 4 цифр номера картки
            }

            switch (Check_Num[0]) //Визначення вендора картки за її першими цифрами
            {
                case AmExpress_JCB:
                    if (Check_Num[1] == 4 || Check_Num[1] == 7)
                    { Vendor = "AmericanExpress"; break; }
                    if (Check_Num[1] == 5)
                        if (Check_Num[2] * 10 + Check_Num[3] >= 28 && Check_Num[2] * 10 + Check_Num[3] <= 89)
                        { Vendor = "JCB"; break; }
                    Vendor = "Unknown";
                    break;
                case Maestro:
                    Vendor = "Maestro"; break;
                case Maestro_Master:
                    if (Check_Num[1] == 0 || Check_Num[1] >= 6)
                        Vendor = "Maestro";
                    else
                        Vendor = "MasterCard";
                    break;
                case Visa: Vendor = "Visa"; break;
                default: Vendor = "Unknown"; break;
            }
            return Vendor;
        }


        static bool IsCreditCardNumberValid(string Num)
        {
            int count, Check_digit, Sum = 0;

            if (!(Verification(Num))) { Console.WriteLine("You entered wrong number"); return false; } 

            if (Num.Length == 19)
                Num = Num.Replace(" ", ""); // Виключаємо пробіли
            int[] Digits = new int[Num.Length];

            for (count = Num.Length - 1; count >= 1; count -= 2) // Масив зі звичайними й подвоєними числами
            {
                Digits[count] = Num[count] - '0';
                Digits[count - 1] = 2 * (Num[count - 1] - '0');
            }
           
            if (count==0)
            {
                Digits[count] = Num[count] - '0';
            }

            for (int i = 0; i < Num.Length - 1; i++)
            {
                Sum += Digits[i] % 10 + Digits[i] / 10; //Деякі подвоєні числа можуть складатися з двох цифр
            }

            if ((Check_digit = (10 - Sum % 10) % 10) == Digits[Num.Length - 1])
                return true; 
            else
                return false; 
        }


        static string GenerateNextCreditCardNumber(string Num)
        {
            int Count = 0, Total;
            string New_num = "";
            if (!(Verification(Num))) { New_num = "You entered the wrong number"; return New_num; }

            if (Num.Length == 19)
                Num = Num.Replace(" ", "");

            int[] Digits = new int[Num.Length];
            for (; Count < Num.Length - 1; Count++) // Остання цифра в наст номері зміниться
            {
                Digits[Count] = Num[Count] - '0';
            }

            for (int i = Count - 1; i >= 0; i--)
            {
                if (++Digits[i] == 10) // Збільшуємо передостанню цифру на 1
                    Digits[i] = 0;
                else
                    break;
            }

            Total = Sum(Digits, Count - 1);
            Digits[Count] = (10 - Total % 10) % 10;

            for (int i = 0; i <= Count; i++)
            {
                New_num += Convert.ToString(Digits[i]);
            }
            return New_num;
        }


        static int Sum(int[] Digits, int Count)
        {
            int i;
            int Sum = Digits[Count] * 2 % 10 + Digits[Count] * 2 / 10;
            for (i = Count - 1; i > 0; i -= 2)
            {
                Sum += Digits[i] + Digits[i - 1] * 2 % 10 + Digits[i - 1] * 2 / 10;
            }
            if (i == 0)
            {
                Sum += Digits[i];
            }
            return Sum;
        }


        static bool Verification(string Num)
        {
            int Prev = 0;
            for (int i = 0; i < Num.Length; i++)
                if (Num[i] - '0' < 0 || Num[i] - '0' > 9) //Чи всі введені символи є цифрами
                    if (Num[i] == ' ' && i - Prev == 4) //Можливо присутні пробіли, що розділяють по 4 цифри
                        Prev = i + 1;
                    else
                        return false;
            return true;
        }

    }
}
