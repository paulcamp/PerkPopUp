using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PerkBot
{
    static class Program
    {
        static Random _rnd = new Random();
        static List<string> _variousPerks = new List<string>();

        static void Main(string[] args)
        {
            int perksToRedeem;
            if (args.Length != 1)
            {
                Console.WriteLine("Please supply number of perks to redeem:");
                var userInput = Console.ReadLine();
                perksToRedeem = Convert.ToInt32(userInput);
            }
            else
            {
                perksToRedeem = Convert.ToInt32(args[0]);
            }

            InitPerkList();
            
            IWebDriver driver;
            driver = new ChromeDriver(AssemblyDirectory);

            //for how many times we want to do this:
            for (int i = 0; i < perksToRedeem; i++)
            {
                //open the redeem page and start redeeming!
                driver.Url = "http://localhost:57323/PerkRedeems/Create";

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

                var redeemButton = driver.FindElement(By.XPath("/html/body/div[2]/form/div/div[2]/div/input"));
                var perkBox = driver.FindElement(By.Id("PerkName"));

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

                perkBox.SendKeys(RandomPerkName());

                Thread.Sleep(1000);

                redeemButton.Click();
            }


            driver.Close();

        }

        private static string RandomPerkName()
        {
            var length = _variousPerks.Count;
            return _variousPerks.ElementAt( _rnd.Next(0,length) );
        }

        private static void InitPerkList()
        {
            _variousPerks.Add("Apple Music");
            //_variousPerks.Add("Spotify");
            //_variousPerks.Add("Amazon Prime");
            _variousPerks.Add("Adidas");
            _variousPerks.Add("Currys");
            //_variousPerks.Add("Ann Summers");
        }
        
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

    }
}
